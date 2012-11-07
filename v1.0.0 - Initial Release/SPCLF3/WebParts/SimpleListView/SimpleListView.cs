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
        private string _listToDisplay;
        private bool _isExternalList;
        protected override void CreateChildControls()
        {
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            try
            {
               
                if (this.ListToDisplay != null)
                {
                    SPList selectedList = SPContext.Current.Web.Lists[new Guid(this._listToDisplay)];
                    SPView defaultView = selectedList.DefaultView;
                    if (this.IsExternalList)
                    {
                        SPViewFieldCollection fields = defaultView.ViewFields;
                        System.Collections.Specialized.StringCollection stringCol = fields.ToStringCollection();
                        // Nik20121105 - Write the table headers;
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.AppendLine("<table class=\"wet-boew-zebra\">");
                        sb.Append("<tr>");
                        foreach (string field in stringCol)
                        {
                            sb.Append("<th>" + field + "</th>");
                        }
                        sb.Append("</tr>");

                        foreach (SPListItem item in selectedList.Items)
                        {
                            sb.AppendLine("<tr>");
                            bool firstCol = true;
                            foreach (string field in stringCol)
                            {
                                if (firstCol)
                                {
                                    firstCol = false;
                                    sb.AppendLine("<td><a href=\"" + SPContext.Current.Web.Url + "/_layouts/listform.aspx?PageType=4&ListId={" + this._listToDisplay + "}&ID=" + item["BdcIdentity"].ToString() + "\">" + item[field].ToString() + "</a></td>");
                                }
                                else
                                {
                                    sb.AppendLine("<td>" + item[field].ToString() + "</td>");
                                }
                            }
                            sb.AppendLine("</tr>");
                        }

                        sb.AppendLine("</table>");
                        writer.Write(sb.ToString());
                    }
                    else
                    {
                        writer.Write(defaultView.RenderAsHtml());
                    }
                }
            }
            catch (Exception ex)
            {
                writer.Write(ex.ToString());
            }
        }

        public override ToolPart[] GetToolParts()
        {
            ToolPart[] allToolsParts = new ToolPart[3];
            WebPartToolPart standardToolParts = new WebPartToolPart();
            CustomPropertyToolPart customToolParts = new CustomPropertyToolPart();

            allToolsParts[0] = standardToolParts;
            allToolsParts[1] = customToolParts;
            allToolsParts[2] = new SimpleListToolPart();

            return allToolsParts;
        }

        public string ListToDisplay
        {
            get { return this._listToDisplay; }
            set { this._listToDisplay = value; }
        }

        public bool IsExternalList
        {
            get { return this._isExternalList; }
            set { this._isExternalList = value; }
        }
    }
}
