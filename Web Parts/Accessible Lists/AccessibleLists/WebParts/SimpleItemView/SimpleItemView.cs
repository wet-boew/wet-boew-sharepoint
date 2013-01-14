using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using Microsoft.SharePoint.WebControls;

namespace SPCLF3.WebParts.SimpleItemView
{
    [ToolboxItemAttribute(false)]
    public class SimpleItemView : Microsoft.SharePoint.WebPartPages.WebPart
    {
        private string _itemUrl, _fieldsToDisplay, _selectedFieldsOrder;

        public string ItemUrl
        {
            get { return this._itemUrl; }
            set { this._itemUrl = value; }
        }

        public string SelectedFieldsOrder
        {
            get { return this._selectedFieldsOrder; }
            set { this._selectedFieldsOrder = value; }
        }

        public string FieldsToDisplay
        {
            get { return this._fieldsToDisplay; }
            set { this._fieldsToDisplay = value; }
        }

        protected override void CreateChildControls()
        {
            if (Page.Request.QueryString["ItemUrl"] != null)
            {
                this.ItemUrl = Page.Request.QueryString["ItemUrl"];
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            if (this.ItemUrl != null)
            {
                using (SPSite site = new SPSite(this.ItemUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPFile file = web.GetFile(this.ItemUrl);
                        SPListItem item = null;
                        try
                        {
                            item = file.Item;
                        }
                        catch
                        {
                            // Nik20121107 - the current item comes from an External List (BCS)
                            int start = this.ItemUrl.ToLower().IndexOf("listid={") + 8;
                            int end = this.ItemUrl.ToLower().IndexOf("}", start);
                            string listId = this.ItemUrl.Substring(start, end - start);

                            start = this.ItemUrl.ToLower().IndexOf("&id=", 0) + 4;
                            end = this.ItemUrl.ToLower().IndexOf("&", start);

                            if (end <= 0)
                                end = this.ItemUrl.Length;
                            string itemId = this.ItemUrl.Substring(start, end - start);

                            SPList curList = web.Lists[new Guid(listId)];
                            foreach (SPListItem curItem in curList.Items)
                            {
                                if (curItem.Fields.TryGetFieldByStaticName("BdcIdentity") != null && curItem["BdcIdentity"].ToString() == itemId)
                                    item = curItem;
                                else if (curItem.ID.ToString() == itemId)
                                    item = curItem;

                            }
                        }

                        AccessibleListItem accListItem = new AccessibleListItem(item, this.FieldsToDisplay, this.SelectedFieldsOrder);
                        string renderedContent = accListItem.RenderAsHtml();
                        writer.WriteLine("<h1 id=\"wb-cont\">" + item.Title + "</h1>");
                        writer.WriteLine(renderedContent);
                    }
                }
            }
        }

        public override ToolPart[] GetToolParts()
        {
            ToolPart[] allToolsParts = new ToolPart[3];
            WebPartToolPart standardToolParts = new WebPartToolPart();
            CustomPropertyToolPart customToolParts = new CustomPropertyToolPart();

            allToolsParts[0] = standardToolParts;
            allToolsParts[1] = customToolParts;
            allToolsParts[2] = new SimpleListItemToolPart();

            return allToolsParts;
        }

        /*[ConnectionConsumer("ItemUrl", "ITransformableFilterValues", AllowsMultipleConnections = true)]
        public void SetItemUrl(ITransformableFilterValues itemUrl)
        {
            this.ItemUrl = itemUrl.ParameterValues[0].ToString();
        }*/
    }
}
