using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;

namespace SPCLF3.Objects
{
    [Serializable]
    public class MetaTag
    {

        public string Name { get; set; }
        public List<MetaTagAttribute> Attributes { get; set; }
        public string ColumnTitle { get; set; }
        public SPFieldType ColumnType { get; set; }
        public bool Hidden { get; set; }
        public string Group { get; set; }
        public string DefaultContent { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MetaTag()
        {
            this.Attributes = new List<MetaTagAttribute>();
        }


        /// <summary>
        /// Additional constructor.
        /// </summary>
        /// <param name="name">The name of the metatag.</param>
        public MetaTag(string name)
        {
            this.Name = name;
            this.Attributes = new List<MetaTagAttribute>();
        }

        /// <summary>
        /// Outputs the HTML attributes of every attribute.
        /// </summary>
        /// <returns></returns>
        public string GenerateAllAttributes()
        {
            StringBuilder strCode = new StringBuilder();

            foreach (MetaTagAttribute attribute in this.Attributes)
            {
                strCode.Append(attribute.GenerateAttribute());
            }

            return strCode.ToString();
        }


        /// <summary>
        /// Renders the metatag HTML code.
        /// </summary>
        /// <param name="writer">HtmlTextWriter to write to.</param>
        public void Render(HtmlTextWriter writer, PublishingPage publishingPage)
        {
            string code = String.Empty;
            string value = String.Empty;

            if (publishingPage.Fields.ContainsField(this.ColumnTitle))
            {
                if (publishingPage.ListItem[this.ColumnTitle] != null)
                {

                    // Format DateTime fields
                    if (this.ColumnType == SPFieldType.DateTime)
                        value = this.FormatDate((DateTime)publishingPage.ListItem[this.ColumnTitle]);
                    else
                        value = System.Web.HttpUtility.HtmlEncode(publishingPage.ListItem[this.ColumnTitle].ToString());

                    code = String.Format("<meta name=\"{0}\" {1} content=\"{2}\" />", this.Name, this.GenerateAllAttributes(), value);
                }
            }
            else
            {
                if (this.DefaultContent != null)
                {
                    value = this.ReplaceDefaultContent(this.DefaultContent);
                    code = String.Format("<meta name=\"{0}\" {1} content=\"{2}\" />", this.Name, this.GenerateAllAttributes(), value);
                }
            }

            writer.WriteLine(code);
        }


        /// <summary>
        /// Formats a DateTime object (YYYY-MM-DD).
        /// </summary>
        /// <param name="dateTime">DateTime object to format.</param>
        /// <returns>A formatted date string.</returns>
        private string FormatDate(DateTime dateTime)
        {
            return String.Format("{0:0000}-{1:00}-{2:00}", dateTime.Year, dateTime.Month, dateTime.Day);
        }


        /// <summary>
        /// Replaces the metatag's default content.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private string ReplaceDefaultContent(string parameter)
        {
            string defaultContent = String.Empty;

            switch (parameter)
            {
                case "@language":
                    string currentLanguageName = SPContext.Current.Web.Locale.TwoLetterISOLanguageName;
                    defaultContent = currentLanguageName;// == "fr" ? "fra" : "eng";
                    break;
                case "@servername":
                    defaultContent = Microsoft.SharePoint.Administration.SPServer.Local.DisplayName;
                    break;
                case "@SiteTitle":
                    defaultContent = HttpContext.GetGlobalResourceObject("CLF3", "SiteTitleText", SPContext.Current.Web.Locale).ToString();
                    break;
                default:
                    defaultContent = parameter;
                    break;
            }

            return defaultContent;
        }

    }
}
