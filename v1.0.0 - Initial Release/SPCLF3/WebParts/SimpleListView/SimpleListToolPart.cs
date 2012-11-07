using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;

namespace SPCLF3.WebParts.SimpleListView
{
    class SimpleListToolPart:Microsoft.SharePoint.WebPartPages.ToolPart
    {
        DropDownList ddlList;
        Panel toolPartPanel;
        RadioButtonList rdButtons;

        protected override void CreateChildControls()
        {
            toolPartPanel = new Panel();
            ddlList = new DropDownList();
            ddlList.ID = "ddlList";

            rdButtons = new RadioButtonList();
            rdButtons.Items.Add(new ListItem("Yes"));
            rdButtons.Items.Add(new ListItem("No"));

            SPListCollection lists = SPContext.Current.Web.Lists;
            foreach (SPList list in lists)
            {
                ddlList.Items.Add(new ListItem(list.Title, list.ID.ToString()));
            }

            toolPartPanel.Controls.Add(ddlList);
            toolPartPanel.Controls.Add(rdButtons);
            Controls.Add(toolPartPanel);
            base.CreateChildControls();
        }

        public override void ApplyChanges()
        {
            SimpleListView simpleListView = (SimpleListView)this.ParentToolPane.SelectedWebPart;
            simpleListView.ListToDisplay = ddlList.SelectedValue;

            if (rdButtons.SelectedValue == "Yes")
                simpleListView.IsExternalList = true;
            else
                simpleListView.IsExternalList = false;
        }
    }
}
