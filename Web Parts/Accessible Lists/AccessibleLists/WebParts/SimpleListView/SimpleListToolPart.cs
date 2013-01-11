using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;

namespace SPCLF3.WebParts.SimpleListView
{
    class SimpleListToolPart : Microsoft.SharePoint.WebPartPages.ToolPart
    {
        Button btnList;
        Panel toolPartPanel;
        TextBox txtItemViewerUrl;
        DropDownList ddlViews;
        Label lblListToDisplay, lblViewerPageUrl;
        protected TextBox txtListTitle { get; set; }
        protected HiddenField hdnListIdentifier { get; set; }
        private string strID;

        private SimpleListView webPart;

        private SPList itemList = null;

        public SimpleListToolPart()
        {
            this.txtListTitle = new TextBox();
            this.hdnListIdentifier = new HiddenField();
        }

        protected override void OnLoad(EventArgs e)
        {
            string test = hdnListIdentifier.Value;
            base.OnLoad(e);
        }
        protected override void CreateChildControls()
        {
            try
            {
                Table table = new Table();

                this.webPart = (SimpleListView)this.ParentToolPane.SelectedWebPart;
                lblListToDisplay = new Label();
                lblViewerPageUrl = new Label();

                if (SPContext.Current.Web.Url.ToLower().Contains("/eng/"))
                {
                    lblListToDisplay.Text = "List to Display:";
                    lblViewerPageUrl.Text = "Url of the Item Viewer Page:";
                }
                else
                {
                    lblListToDisplay.Text = "Liste à afficher:";
                    lblViewerPageUrl.Text = "Url de la page de visionneur d'item:";
                }

                try
                {
                    if (webPart.ListId != null)
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                        {
                            using (SPWeb web = site.OpenWeb(webPart.ListWebId))
                            {
                                SPList selList = web.Lists[webPart.ListId];
                                txtListTitle.Text = selList.Title;

                                ddlViews = new DropDownList();
                                foreach (SPView view in selList.Views)
                                {
                                    ListItem item = new ListItem(view.Title);

                                    if (webPart.ViewName != string.Empty && view.Title.Equals(webPart.ViewName))
                                        item.Selected = true;
                                    ddlViews.Items.Add(item);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogEngine.Log(ex, "Accessible Lists");
                }

                lblListToDisplay.Font.Bold = true;
                lblViewerPageUrl.Font.Bold = true;

                toolPartPanel = new Panel();

                this.txtListTitle.ID = "txtEntityPath";
                this.txtListTitle.CssClass = "UserInput";
                this.txtListTitle.Style.Add("margin", "4px 0 0 0");

                btnList = new Button();
                btnList.CssClass = "UserButton";
                btnList.OnClientClick = String.Format("LaunchPickerTreeDialog('CbqPickerSelectListTitle','CbqPickerSelectListText','{0}','', '{1}', null,'','','/_layouts/images/smt_icon.gif','', callback, ''); return false;", "listsOnly", SPContext.Current.Web.Url);
                btnList.Text = "...";

                txtItemViewerUrl = new TextBox();
                if (webPart.ItemViewerUrl != null)
                    txtItemViewerUrl.Text = webPart.ItemViewerUrl;

                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                cell.Controls.Add(lblListToDisplay);
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Controls.Add(txtListTitle);
                cell.Controls.Add(btnList);
                row.Cells.Add(cell);
                table.Rows.Add(row);
                
                if (ddlViews != null)
                {
                    row = new TableRow();
                    cell = new TableCell();
                    row.Cells.Add(cell);
                    cell = new TableCell();
                    cell.Controls.Add(ddlViews);
                    row.Cells.Add(cell);
                    table.Rows.Add(row);
                }

                row = new TableRow();
                cell = new TableCell();
                cell.Controls.Add(lblViewerPageUrl);
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Controls.Add(txtItemViewerUrl);
                row.Cells.Add(cell);
                table.Rows.Add(row);
                toolPartPanel.Controls.Add(hdnListIdentifier);
                toolPartPanel.Controls.Add(table);
                Controls.Add(toolPartPanel);
                base.CreateChildControls();
            }
            catch (Exception ex)
            {
                LogEngine.Log(ex, "Accessible Lists");
            }
        }

        protected SPList GetList(string identifier)
        {
            if (identifier.StartsWith("SPList"))
            {
                try
                {
                    string[] segments = identifier.TrimEnd(':').Split('?');
                    string listID = segments[0].Substring(7);
                    string webID = segments[1].Substring(6);
                    using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                    {
                        using (SPWeb web = site.OpenWeb(new Guid(webID)))
                        {
                            // Search for list with specified URL 
                            foreach (SPList list in web.Lists)
                            {
                                if (list.ID == new Guid(listID))
                                    return list;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;
        }

        public override void ApplyChanges()
        {
            try
            {
                itemList = this.GetList(this.hdnListIdentifier.Value);

                // Save custom properties 
                this.webPart.ListId = itemList.ID;
                this.webPart.ListWebId = itemList.ParentWeb.ID;
                this.webPart.ViewName = ddlViews.SelectedItem.Text;

            }
            catch (Exception ex)
            {
                LogEngine.Log(ex, "Accessible Lists");
            }
        }

        public override void SyncChanges()
        {
            if (this.webPart != null)
            {
                this.itemList = this.GetList(this.hdnListIdentifier.Value);
                if (this.itemList != null)
                {
                    this.hdnListIdentifier.Value = this.strID;
                    this.txtListTitle.Text = this.itemList.Title;
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            EnsureChildControls();

            if (!String.IsNullOrEmpty(this.hdnListIdentifier.Value))
                this.strID = this.hdnListIdentifier.Value;
            else if (!this.webPart.ListId.Equals(Guid.Empty))
                this.strID = String.Format("SPList:{0}?SPWeb:{1}:", this.webPart.ListId.ToString(), this.webPart.ListWebId.ToString());

            // Retrieve SPList if list identifier exists 
            if (!String.IsNullOrEmpty(this.strID))
                this.itemList = this.GetList(this.strID);

            // Set default field values 
            if (this.itemList != null)
                this.txtListTitle.Text = this.itemList.Title;
            this.hdnListIdentifier.Value = this.strID;
            
            // Load PickerTreeDialog.js
            this.Controls.Add(new LiteralControl("<script type=\"text/javascript\" src=\"/_layouts/1033/PickerTreeDialog.js\"></script>"));

            // Render the JavaScript PickerTreeDialog callback
            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\">");
            script.Append("callback = function(arr){");
            script.Append("if(arr != null && arr != undefined){");
            script.Append("document.getElementById('" + this.hdnListIdentifier.ClientID + "').value=arr[0];");
            script.Append("document.getElementById('" + this.txtListTitle.ClientID + "').value=arr[2];");
            script.Append("__doPostBack('" + this.ClientID + "', arr[0]);");
            script.Append("}");
            script.Append("};");
            script.Append("</script>");
            this.Controls.Add(new LiteralControl(script.ToString()));
            base.OnPreRender(e);
        }
    }
}
