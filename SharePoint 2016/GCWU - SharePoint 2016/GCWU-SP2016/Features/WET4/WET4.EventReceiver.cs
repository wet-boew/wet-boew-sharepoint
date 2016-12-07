using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System.Globalization;
using System.Xml;
using System.Collections;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint.Publishing;
using SPWET4.Objects;

namespace SPWET4.Features.WET4
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("1f5a4573-9b99-45df-ae03-02a22458eeab")]
    public class WET4EventReceiver : SPFeatureReceiver
    {

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            if (properties != null)
            {
                using (SPSite site = (SPSite)properties.Feature.Parent)
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        var ElementDefinitions = properties.Definition.GetElementDefinitions(CultureInfo.CurrentCulture);

                        foreach (SPElementDefinition ElementDefinition in ElementDefinitions)
                        {
                            if (ElementDefinition.ElementType == "Module")
                            {
                                Helper.UpdateFilesInModule(ElementDefinition, web);
                            }
                        }
                    }

                }
            }
        }
    }

    internal static class Helper
    {
        internal static void UpdateFilesInModule(SPElementDefinition elementDefinition, SPWeb web)
        {
            XElement xml = elementDefinition.XmlDefinition.ToXElement();
            XNamespace xmlns = "http://schemas.microsoft.com/sharepoint/";
            string featureDir = elementDefinition.FeatureDefinition.RootDirectory;
            Module module = (from m in xml.DescendantsAndSelf()
                             select new Module
                             {
                                 Name = (m.Attribute("Name") != null) ? m.Attribute("Name").Value : null,
                                 ProvisioningUrl = (m.Attribute("Url") != null) ? m.Attribute("Url").Value : null,
                                 PhysicalPath = featureDir,
                                 Files = (from f in m.Elements(xmlns.GetName("File"))
                                          select new Module.File
                                          {

                                              FilePath = (m.Attribute("Path") == null) ? string.Empty : Path.Combine(featureDir, m.Attribute("Path").Value),
                                              Name = (f.Attribute("Url") != null) ? f.Attribute("Url").Value : null,
                                              Properties = (from p in f.Elements(xmlns.GetName("Property"))
                                                            select p).ToDictionary(
                                                              n => n.Attribute("Name").Value,
                                                              v => v.Attribute("Value").Value)
                                          }).ToArray()
                             }).First();

            if (module == null)
            {
                return;
            }

            if (module.Name == "Layout Pages")
            {
                foreach (Module.File file in module.Files)
                {

                    string filename = file.Name.Contains("/") ? file.Name.Substring(file.Name.LastIndexOf("/") + 1) : file.Name;
                    string physicalPath = string.IsNullOrEmpty(file.FilePath) ? Path.Combine(module.PhysicalPath, filename) : Path.Combine(file.FilePath, filename);
                    string virtualPath = string.Concat(web.Url, "/", module.ProvisioningUrl, "/", file.Name);

                    if (File.Exists(physicalPath))
                    {
                        using (StreamReader sreader = new StreamReader(physicalPath))
                        {
                            if (!CheckOutStatus(web.GetFile(virtualPath)))
                            {
                                web.GetFile(virtualPath).CheckOut();
                            }
                            SPFile spFile = web.Files.Add(virtualPath, sreader.BaseStream, new Hashtable(file.Properties), true);
                            spFile.CheckIn("Updated", SPCheckinType.MajorCheckIn);
                            if (CheckContentApproval(spFile.Item))
                            {
                                spFile.Approve("Updated");
                            }

                            spFile.Update();
                        }
                    }
                }
            }
        }

        private static bool CheckOutStatus(SPFile file)
        {
            if (file.CheckOutType != SPFile.SPCheckOutType.None)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CheckContentApproval(SPListItem listitem)
        {
            bool isContentApprovalEnabled = listitem.ParentList.EnableModeration;

            return isContentApprovalEnabled;
        }

        public static XElement ToXElement(this XmlNode node)
        {
            XDocument xDoc = new XDocument();

            using (XmlWriter xmlWriter = xDoc.CreateWriter())

                node.WriteTo(xmlWriter);

            return xDoc.Root;

        }
    }

    public class Module
    {
        public string Name { get; set; }
        public string ProvisioningUrl { get; set; }
        public string PhysicalPath { get; set; }
        public Module.File[] Files { get; set; }

        public class File
        {
            public string FilePath { get; set; }
            public string Name { get; set; }
            public Dictionary<string, string> Properties { get; set; }
        }
    }

}
