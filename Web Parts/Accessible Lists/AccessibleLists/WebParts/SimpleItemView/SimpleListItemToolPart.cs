using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;

namespace SPCLF3.WebParts.SimpleItemView
{
    class SimpleListItemToolPart : Microsoft.SharePoint.WebPartPages.ToolPart
    {
        Panel toolPartPanel;
        CheckBox[] chckList;
        Label lblFieldsTodisplay;
        DropDownList[] ddlOrder;

        private string ItemUrl
        {
            get { return Page.Request.QueryString["ItemUrl"]; }
        }

        protected override void CreateChildControls()
        {
            lblFieldsTodisplay = new Label();

            if (SPContext.Current.Web.Url.ToLower().Contains("/eng/"))
                lblFieldsTodisplay.Text = "Fields to Display:";
            else
                lblFieldsTodisplay.Text = "Champs à afficher:";
            lblFieldsTodisplay.Font.Bold = true;
            int count = 0;
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

                        SimpleItemView simpleItemView = (SimpleItemView)this.ParentToolPane.SelectedWebPart;

                        ddlOrder = new DropDownList[item.Fields.Count];
                        chckList = new CheckBox[item.Fields.Count];
                        
                        foreach (SPField field in item.Fields)
                        {
                            ddlOrder[count] = new DropDownList();
                            chckList[count] = new CheckBox();
                            for (int i = 1; i < item.Fields.Count + 1; i++)
                            {
                                ddlOrder[count].Items.Add(new ListItem(i.ToString()));
                                if (i-1 == count)
                                    ddlOrder[count].Items[i-1].Selected = true;
                            }

                            chckList[count].Text = field.Title;      

                            /*if (simpleItemView.FieldsToDisplay != null && simpleItemView.FieldsToDisplay.Contains(field.InternalName + ";"))
                            {
                                chckList.Items[chckList.Items.Count - 1].Selected = true;
                            }*/
                            count++;

                        }

                    }
                }
            }
            toolPartPanel = new Panel();
            toolPartPanel.GroupingText = "Fields to Display";
            toolPartPanel.Controls.Add(lblFieldsTodisplay);

            Table tblFields = new Table();

            for (int i = 0; i < count; i++)
            {
                TableRow row = new TableRow();
                TableCell cellField = new TableCell();
                TableCell cellOrder = new TableCell();
                cellField.Controls.Add(chckList[i]);
                cellOrder.Controls.Add(ddlOrder[i]);
                row.Cells.Add(cellField);
                row.Cells.Add(cellOrder);
                tblFields.Rows.Add(row);
            }
            Controls.Add(tblFields);
            Controls.Add(toolPartPanel);
            base.CreateChildControls();
        }

        public override void ApplyChanges()
        {
            SimpleItemView simpleItemView = (SimpleItemView)this.ParentToolPane.SelectedWebPart;

            System.Text.StringBuilder fieldsSelected = new StringBuilder();
            System.Text.StringBuilder selectedFieldsOrder = new StringBuilder();

            int count = 0;
            foreach (CheckBox chckBox in chckList)
            {
                if (chckBox.Checked)
                {
                    fieldsSelected.Append(chckBox.Text + ";");
                    selectedFieldsOrder.Append(ddlOrder[count].SelectedValue + ";");
                }
                count++;
            }

            simpleItemView.FieldsToDisplay = fieldsSelected.ToString();
            simpleItemView.SelectedFieldsOrder = selectedFieldsOrder.ToString();
        }
    }
}
