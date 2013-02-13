using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Security;
using WET.Theme.GCWU.Objects;

namespace WET.Theme.GCWU.WebControls
{
    [DefaultProperty("Text"),
    ToolboxData("<{0}:PageMetadata runat=server></{0}:PageMetadata>")]
    public class PageMetadata : WebControl
    {

        protected override void Render(HtmlTextWriter writer)
        {
            if (SPContext.Current.ListItem != null && PublishingPage.IsPublishingPage(SPContext.Current.ListItem))
            {
                // Generate each metatag
                PublishingPage publishingPage = PublishingPage.GetPublishingPage(SPContext.Current.ListItem);
                foreach (MetaTag metaTag in PageMetadata.GetCustomMetaTags())
                {
                    metaTag.Render(writer, publishingPage);
                }
            }
        }

        /// <summary>
        /// Retrieves the custom metatags as defined in the resource metatags XML file.
        /// </summary>
        /// <returns></returns>
        public static List<MetaTag> GetCustomMetaTags()
        {
            List<MetaTag> metaTags = new List<MetaTag>();
            MetaTag aMeta;
            //foreach (SPField aField in publishingPage.ContentType.Fields)
            //{

            //    if (aField.Group.ToLower() == "meta")
            //    {
            //        // description
            //        aMeta = new MetaTag(aField.StaticName);
            //        aMeta.ColumnTitle = aField.StaticName;
            //        //test for dc.description and rename to dcterms.description
            //        aMeta.ColumnType = aField.Type;
            //        aMeta.Hidden = false;
            //        aMeta.Group = "Meta tags columns";
            //        metaTags.Add(aMeta);
            //    }
            //}

            // MUST PRODUCE THIS
            // <meta name="dcterms.description" content="French description / Description en français" />
            //<meta name="description" content="French description / Description en français" />
            //<meta name="keywords" content="French keywords / Mots-clés en français" />
            //<meta name="dcterms.creator" content="French name of the content author / Nom en français de l'auteur du contenu" />
            //<meta name="dcterms.title" content="French title / Titre en français" />
            //<meta name="dcterms.issued" title="W3CDTF" content="Date published (YYYY-MM-DD) / Date de publication (AAAA-MM-JJ)" />
            //<meta name="dcterms.modified" title="W3CDTF" content="Date modified (YYYY-MM-DD) / Date de modification (AAAA-MM-JJ)" />
            //<meta name="dcterms.subject" title="scheme" content="French subject terms / Termes de sujet en français" />
            //<meta name="dcterms.language" title="ISO639-2" content="fra" />
            // dcterms.description
            aMeta = new MetaTag("dcterms.description");
            aMeta.ColumnTitle = "dc.description";
            aMeta.ColumnType = SPFieldType.Text;
            aMeta.Hidden = false;
            aMeta.Group = "Meta tags columns";
            metaTags.Add(aMeta);

            //description
            aMeta = new MetaTag("description");
            aMeta.ColumnTitle = "dc.description";
            aMeta.ColumnType = SPFieldType.Text;
            aMeta.Hidden = false;
            aMeta.Group = "Meta tags columns";
            metaTags.Add(aMeta);

            // keywords
            aMeta = new MetaTag("keywords");
            aMeta.ColumnTitle = "meta_keywords";
            aMeta.ColumnType = SPFieldType.Text;
            aMeta.Hidden = false;
            aMeta.Group = "Meta tags columns";
            metaTags.Add(aMeta);

            // Dublic Core: creator
            aMeta = new MetaTag("dcterms.creator");
            aMeta.ColumnTitle = "dcterms.creator";
            //aMeta.ColumnTitle = "Author"; // uncomment this line and comment the previous line to have the Author displayed
            aMeta.ColumnType = SPFieldType.Text;
            aMeta.Hidden = false;
            aMeta.Group = "Dublin core columns";
            // comment out this line to have the user name appear based on the Author field of the Pages list
            aMeta.DefaultContent = "@SiteTitle";
            metaTags.Add(aMeta);

            // Dublic Core: Title
            aMeta = new MetaTag("dcterms.title");
            aMeta.ColumnTitle = "Title";
            aMeta.ColumnType = SPFieldType.Text;
            aMeta.Hidden = false;
            aMeta.Group = "Dublin core columns";
            metaTags.Add(aMeta);

            // Dublic Core: issued
            aMeta = new MetaTag("dcterms.issued");
            aMeta.Attributes.Add(new MetaTagAttribute("title", "W3CDTF"));
            aMeta.ColumnTitle = "Created";
            aMeta.ColumnType = SPFieldType.DateTime;
            aMeta.Hidden = false;
            aMeta.Group = "Dublin core columns";
            metaTags.Add(aMeta);

            // Dublic Core: Modified
            aMeta = new MetaTag("dcterms.modified");
            aMeta.Attributes.Add(new MetaTagAttribute("title", "W3CDTF"));
            aMeta.ColumnTitle = "Modified";
            aMeta.ColumnType = SPFieldType.DateTime;
            aMeta.Hidden = false;
            aMeta.Group = "Dublin core columns";
            metaTags.Add(aMeta);

            // Dublic Core: subject
            aMeta = new MetaTag("dcterms.subject");
            aMeta.Attributes.Add(new MetaTagAttribute("title", "scheme"));
            aMeta.ColumnTitle = "dc.subject";
            aMeta.ColumnType = SPFieldType.Text;
            aMeta.Hidden = false;
            aMeta.Group = "Dublin core columns";
            metaTags.Add(aMeta);

            //// Dublic Core: language
            aMeta = new MetaTag("dc.language");
            aMeta.Attributes.Add(new MetaTagAttribute("title", "ISO639-2"));
            aMeta.ColumnTitle = "Dublin Core: Language";
            aMeta.ColumnType = SPFieldType.Text;
            aMeta.Hidden = true;
            aMeta.Group = "Dublin core columns";
            aMeta.DefaultContent = "@language";
            metaTags.Add(aMeta);

            //// Dublic Core: language
            aMeta = new MetaTag("dcterms.language");
            aMeta.Attributes.Add(new MetaTagAttribute("title", "ISO639-2"));
            aMeta.ColumnTitle = "Dublin Core: Language";
            aMeta.ColumnType = SPFieldType.Text;
            aMeta.Hidden = true;
            aMeta.Group = "Dublin core columns";
            aMeta.DefaultContent = "@language";
            metaTags.Add(aMeta);

            //// CUSTOM FIELDS NOT PART OF CLF 3.0

            //// Webtrends: Program
            //aMeta = new MetaTag("DCSext.LAC-PL_prgrm");
            //aMeta.ColumnTitle = "Webtrends: Program";
            //aMeta.ColumnType = SPFieldType.Choice;
            //aMeta.Hidden = false;
            //aMeta.Group = "WebTrends columns";
            //aMeta.DefaultContent = "Program 1\r\nProgram2";
            //metaTags.Add(aMeta);

            //// Webtrends: Funding
            //aMeta = new MetaTag("DCSext.LAC-PL_funding");
            //aMeta.ColumnTitle = "Webtrends: Funding";
            //aMeta.ColumnType = SPFieldType.Text;
            //aMeta.Hidden = false;
            //aMeta.Group = "WebTrends columns";
            //metaTags.Add(aMeta);

            //// Webtrends: Project number
            //aMeta = new MetaTag("DCSext.LAC-PL_prj_nmb");
            //aMeta.ColumnTitle = "Webtrends: Project number";
            //aMeta.ColumnType = SPFieldType.Text;
            //aMeta.Hidden = false;
            //aMeta.Group = "WebTrends columns";
            //metaTags.Add(aMeta);

            //// Webtrends: Topic
            //aMeta = new MetaTag("DCSext.LAC-OL_topic");
            //aMeta.ColumnTitle = "Webtrends: Topic";
            //aMeta.ColumnType = SPFieldType.Text;
            //aMeta.Hidden = false;
            //aMeta.Group = "WebTrends columns";
            //metaTags.Add(aMeta);

            //// Webtrends: Subtopic
            //aMeta = new MetaTag("DCSext.LAC-OL_sb_topic");
            //aMeta.ColumnTitle = "Webtrends: Subtopic";
            //aMeta.ColumnType = SPFieldType.Text;
            //aMeta.Hidden = false;
            //aMeta.Group = "WebTrends columns";
            //metaTags.Add(aMeta);

            //// Webtrends: Page type
            //aMeta = new MetaTag("DCSext.LAC-OL_pge_type");
            //aMeta.ColumnTitle = "Webtrends: Page type";
            //aMeta.ColumnType = SPFieldType.Text;
            //aMeta.Hidden = false;
            //aMeta.Group = "WebTrends columns";
            //metaTags.Add(aMeta);

            //// Webtrends: Subpage type
            //aMeta = new MetaTag("DCSext.LAC-OL_sb_pge_type");
            //aMeta.ColumnTitle = "Webtrends: Subpage typee";
            //aMeta.ColumnType = SPFieldType.Text;
            //aMeta.Hidden = false;
            //aMeta.Group = "WebTrends columns";
            //metaTags.Add(aMeta);

            //// Webtrends: Subpage type
            //aMeta = new MetaTag("WT.sv");
            //aMeta.ColumnType = SPFieldType.Text;
            //aMeta.Hidden = true;
            //aMeta.Group = "WebTrends columns";
            //aMeta.DefaultContent = "@servername";
            //metaTags.Add(aMeta);

            return metaTags;
        }

    }
}
