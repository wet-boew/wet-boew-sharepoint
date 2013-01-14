using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using Microsoft.SharePoint.WebControls;

namespace SPCLF3.WebParts.SimpleListView
{
    [ToolboxItemAttribute(false)]
    public class SimpleListView : Microsoft.SharePoint.WebPartPages.WebPart
    {
        private Guid _listId;
        private Guid _listWeb;
        private string _viewName;
        private string _itemViewerUrl;protected override void CreateChildControls()
        {
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string CAMLQuery = null;
            

            try
            {
                if (this._listId != null && this._viewName != null)
                {
                     using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                    {
                        using (SPWeb web = site.OpenWeb(this.ListWebId))                        
                        {
                            SPList selectedList = web.Lists[this._listId];
                            SPListItemCollection items = selectedList.Items;
                            SPView defaultView = selectedList.Views[this._viewName];
                            uint itemsPerPage = defaultView.RowLimit;
                            
                            SPViewFieldCollection fields = defaultView.ViewFields;
                            System.Collections.Specialized.StringCollection stringCol = fields.ToStringCollection();
                            
                            if (Page.Request.QueryString != null && Page.Request.QueryString.Count > 0)
                            {
                                FilterEngine filterEngine = new FilterEngine(Page.Request.QueryString, selectedList);
                                CAMLQuery = filterEngine.CAMLQuery;                                
                            }

                            // Nik20121105 - Write the table headers;
                            sb.AppendLine("<table class=\"wet-boew-zebra\">");
                            sb.Append("<tr>");
                            foreach (string field in stringCol)
                            {
                                sb.Append("<th>" + selectedList.Fields.GetFieldByInternalName(field).Title + "</th>");
                            }
                            sb.Append("</tr>");

                            if (CAMLQuery == null)
                            {
                                CAMLQuery = "";
                            }

                            SPQuery query = new SPQuery();
                            query.Query = CAMLQuery;
                            query.RowLimit = itemsPerPage;
                            items = selectedList.GetItems(query);

                            if (Page.Request.QueryString["p_ID"] != null)
                            {
                                string prev = "";
                                if (Page.Request.QueryString["PagedPrev"] == "TRUE")
                                    prev = "&PagedPrev=TRUE";
                                SPListItemCollectionPosition position = new SPListItemCollectionPosition("Paged=TRUE&p_ID=" + Page.Request.QueryString["p_ID"] + prev);
                                query.ListItemCollectionPosition = position;
                            }

                            string lastId = "";
                            foreach (SPListItem item in items)
                            {
                                sb.AppendLine("<tr>");
                                bool firstCol = true;
                                foreach (string field in stringCol)
                                {
                                    if (firstCol)
                                    {
                                        firstCol = false;

                                        string itemUrl = string.Empty;                                            
                                        SPField test = item.Fields.TryGetFieldByStaticName("BdcIdentity");                                            

                                        if(test != null)
                                            itemUrl = HttpUtility.UrlEncode(this._listWeb + "/_layouts/listform.aspx?PageType=4&ListId={" + this._listId.ToString() + "}&ID=" + item["BdcIdentity"].ToString());
                                        else
                                            itemUrl = HttpUtility.UrlEncode(this._listWeb + "/_layouts/listform.aspx?PageType=4&ListId={" + this._listId.ToString() + "}&ID=" + item.ID.ToString());

                                        string renderedField = string.Empty;
                                        if (item[field] != null)
                                            renderedField = item[field].ToString();

                                        sb.AppendLine("<td><a href=\"" + this.ItemViewerUrl + "?ItemUrl=" + itemUrl + "\">" + renderedField.Replace("string;#", "").Replace("datetime;#", "").Replace("number;#", "") + "</a></td>");
                                    }
                                    else
                                    {
                                        string renderedField = string.Empty;
                                        if(item[field] != null)
                                            renderedField = FieldRenderer.RenderField(item[field].ToString(), item.Fields.GetFieldByInternalName(field));
                                        sb.AppendLine("<td>" + renderedField + "</td>");
                                    }
                                }
                                sb.AppendLine("</tr>");
                                lastId = item.ID.ToString();
                            }

                            sb.AppendLine("</table>");

                            string curUrl = Page.Request.Url.OriginalString;
                            string forwardUrl = curUrl.Replace("p_ID=" + Page.Request.QueryString["p_ID"], "p_ID=" + lastId).Replace("&PagedPrev=TRUE", "");
                            string prevUrl = curUrl.Replace("p_ID=" + Page.Request.QueryString["p_ID"], "p_ID=" + items[0].ID.ToString());
                            
                            if(forwardUrl.IndexOf("p_ID") < 0)
                            {
                                if(!forwardUrl.Contains("?"))
                                    forwardUrl += "?p_ID=" + lastId;
                                else
                                    forwardUrl += "&p_ID=" + lastId;
                            }

                            if (prevUrl.IndexOf("p_ID") < 0)
                            {
                                if (!prevUrl.Contains("?"))
                                    prevUrl += "?p_ID=" + items[0].ID.ToString();
                                else
                                    prevUrl += "&p_ID=" + items[0].ID.ToString();
                            }

                            if (!prevUrl.Contains("PagedPrev"))
                                prevUrl += "&PagedPrev=TRUE";
                            
                            if (CAMLQuery != null)
                            {                                
                                SPQuery newQuery = new SPQuery();
                                newQuery.Query = CAMLQuery;
                                if(items[0].ID != selectedList.GetItems(newQuery)[0].ID)
                                    sb.AppendLine("<a href=\"" + prevUrl + "\">< Previous</a>&nbsp;&nbsp;");
                            }
                            else if(items[0].ID != selectedList.Items[0].ID)
                                sb.AppendLine("<a href=\"" + prevUrl + "\">< Previous</a>&nbsp;&nbsp;");
                            
                            if((int.Parse(lastId) + itemsPerPage) < selectedList.Items.Count)
                                sb.AppendLine("<a href=\"" + forwardUrl + "\">Next ></a>");
                            writer.Write(sb.ToString());                    
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogEngine.Log(ex, "Accessible Lists");
            }
        }

        public override ToolPart[] GetToolParts()
        {
            ToolPart[] allToolsParts = new ToolPart[3];
            try
            {                
                WebPartToolPart standardToolParts = new WebPartToolPart();
                CustomPropertyToolPart customToolParts = new CustomPropertyToolPart();

                allToolsParts[0] = standardToolParts;
                allToolsParts[1] = customToolParts;
                allToolsParts[2] = new SimpleListToolPart();
            }
            catch (Exception ex)
            {
                LogEngine.Log(ex, "Accessible Lists");
            }

            return allToolsParts;
        }

        public string ViewName
        {
            get { return this._viewName; }
            set { this._viewName = value; }
        }

        public Guid ListId
        {
            get { return this._listId; }
            set { this._listId = value; }
        }

        public Guid ListWebId
        {
            get { return this._listWeb; }
            set { this._listWeb = value; }
        }

        public string ItemViewerUrl
        {
            get { return this._itemViewerUrl; }
            set { this._itemViewerUrl = value; }
        }
    }
}
