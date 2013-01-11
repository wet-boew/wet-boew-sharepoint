using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Publishing.Navigation;
using SPCLF3.Objects;

namespace SPCLF3.WebControls
{

    [DefaultProperty("Text"),
    ToolboxData("<{0}:CLFSiteMap runat=server></{0}:CLFSiteMap>")]
    public class CLFSiteMap : WebControl
    {
        // creates links in this format... no images or styles as they are taken care of in style sheet of CLF:
        // links style includes separators (is why last item is not a link)
        //<ol itemprop="breadcrumb">
        //  <li><a href....></li>
        //  <li>last item is not a link</li>
        //</ol>
        # region Properties

        ///
        /// A string with comma delimited values which will be ignored when the breadcrumb is rendered.  This is especially useful
        /// for trimming the variation labels, or if any level of the hierarchy in the navigation needs to be skipped.
        ///
        private const string defaultIgnoreNodeName = "";
        private string ignoreNodeName = defaultIgnoreNodeName;
        [Category("Appearance")]
        [DefaultValue(defaultIgnoreNodeName)]
        [Description("The substrings (name of navigation nodes) in this comma delimited string will not be rendered on the breadcrumb.")]
        public string IgnoreNodeName
        {
            get { return ignoreNodeName; }
            set { ignoreNodeName = value; }
        }

        private const string defaultSiteMapProvider = "AspnetXmlSiteMapProvider";
        private string siteMapProvider = defaultSiteMapProvider;
        [Category("Appearance")]
        [DefaultValue(defaultSiteMapProvider)]
        [Description("Specify the name of the current site map provider")]
        public string SiteMapProvider
        {
            get { return siteMapProvider; }
            set { siteMapProvider = value; }
        }

        #endregion

        Stack<HyperLink> LinkStack = new Stack<HyperLink>();

        ///
        /// Create HyperLink objects and pop to the stack, recursively going up the hierarchy
        ///
        ///
        ///
        private void TraverseUp(SiteMapNode currentNode, Stack<HyperLink> linkStack)
        {
            try
            {
                if (currentNode != null)
                {
                    if (currentNode.ParentNode != null)
                    {
                        HyperLink currentLink = new HyperLink();
                        if (currentNode.Title.ToLower() == "english" || currentNode.Title.ToLower() == "Anglais" || currentNode.Title.ToLower() == "eng")
                        {
                            currentLink.Text = "Home";
                        }
                        else if (currentNode.Title.ToLower() == "français" || currentNode.Title.ToLower() == "french" || currentNode.Title.ToLower() == "fra")
                        {
                            currentLink.Text = "Accueil";
                        }
                        else
                        {
                            currentLink.Text = currentNode.Title;
                        }
                        currentLink.NavigateUrl = currentNode.Url;
                        linkStack.Push(currentLink);
                        if (currentNode.ParentNode != null)
                        {
                            TraverseUp(currentNode.ParentNode, linkStack);
                        }
                    }
                }
            }
            catch { }
        }

        protected override void Render(HtmlTextWriter output)
        {
            PortalSiteMapProvider map = PortalSiteMapProvider.GlobalNavSiteMapProvider;
            string contentOutput = string.Empty;
            try
            {
                TraverseUp(map.CurrentNode, LinkStack);
                while (LinkStack.Count > 0)
                {
                    HyperLink nodeLink = (HyperLink)LinkStack.Pop(); // use a stack (FILO) to reverse the order of traversing up the hierarchy
                    if (!IsIgnore(nodeLink.Text))
                    {
                        // last link is always rendered as a text
                        if (LinkStack.Count == 0)
                        {
                            // allow third party applications to override the title of the current node in the breadcrumb
                            SPCLF3.Master_Pages.CLF3PublishingMaster masterPage = (SPCLF3.Master_Pages.CLF3PublishingMaster)this.Page.Master;
                            if (String.IsNullOrEmpty(masterPage.BreadcrumbPageNodeTitle))
                                contentOutput += "<li>" + nodeLink.Text + "</li>\r\n";
                            else
                                contentOutput += "<li>" + masterPage.BreadcrumbPageNodeTitle + "</li>";
                        }
                        else
                        {
                            contentOutput += "<li><a href=\"" + nodeLink.NavigateUrl + "\">" + nodeLink.Text + "</a></li>\r\n";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message + " " + ex.StackTrace);
            }
            if (contentOutput != string.Empty)
            {
                contentOutput = "<ol>" + contentOutput + "</ol>";
            }
            output.Write(contentOutput);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {

        }

        ///
        /// Determine if the node should be ignored.
        ///
        ///
        ///
        private bool IsIgnore(string nodeName)
        {
            bool isIgnore = false;
            try
            {
                string[] ignoreNodeNames;
                if (IgnoreNodeName != String.Empty)
                {
                    char[] splitChar = new char[1];
                    splitChar[0] = ',';
                    ignoreNodeNames = IgnoreNodeName.Split(splitChar);

                    foreach (string ignoreString in ignoreNodeNames)
                    {
                        if (Regex.IsMatch(nodeName, ignoreString, RegexOptions.IgnoreCase))
                        {
                            isIgnore = true;
                            break;
                        }
                    }
                }
            }
            catch { }
            return isIgnore;
        }
    }
}
