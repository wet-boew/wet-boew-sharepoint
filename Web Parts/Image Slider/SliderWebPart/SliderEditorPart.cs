using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using System.Web;
using System.Web.Caching;
using Microsoft.SharePoint;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LAC.SharePoint.Slider.SliderWebPart
{
    class SliderEditorPart : EditorPart
    {
        protected SliderWebPart WebPart { get; set; }

        protected Label lblList { get; set; }
        protected HiddenField hdnListIdentifier { get; set; }
        protected Image imgListIcon { get; set; }
        protected Label lblListTitle { get; set; }

        private string SliderItemIdentifier = null;
        private SPList SliderItemList = null;

        public SliderEditorPart()
        {
            this.lblList = new Label();
            this.hdnListIdentifier = new HiddenField();
            this.imgListIcon = new Image();
            this.lblListTitle = new Label();
        }


        protected override void CreateChildControls()
        {
            this.WebPart = (SliderWebPart)this.WebPartToEdit;

            this.Title = WebPart.LoadResource("Slider_Source_List");

            Panel pnlToolPart = new Panel();
            pnlToolPart.CssClass = "UserSectionHead";

            // List label 
            Panel pnlListField = new Panel();
            pnlListField.CssClass = "UserSectionHead";
            this.lblList.ID = "lblList";
            this.lblList.AssociatedControlID = "txtEntityPath";
            this.lblList.Text = " "; // WebPart.LoadResource("Slider_Source_List");
            pnlListField.Controls.Add(this.lblList);
            pnlToolPart.Controls.Add(pnlListField);

            // List icon and list name panel 
            Panel pnlList = new Panel();
            pnlList.Style.Add("padding", "2px 5px");
            pnlList.Style.Add("width", "145px");
            pnlList.Style.Add("float", "left");
            pnlList.Style.Add("overflow", "hidden");
            pnlList.Style.Add("margin", "0 5px 5px 0");
            pnlList.Style.Add("border", "#ccc 1px solid");

            // List icon 
            this.imgListIcon.Style.Add("float", "left");
            this.imgListIcon.Style.Add("margin", "0 5px 0 0");
            pnlList.Controls.Add(this.imgListIcon);

            // Entity name textbox 
            this.lblListTitle.ID = "txtEntityPath";
            this.lblListTitle.CssClass = "UserInput";
            this.lblListTitle.Style.Add("margin", "4px 0 0 0");
            pnlList.Controls.Add(this.lblListTitle);

            pnlToolPart.Controls.Add(pnlList);

            // Entity ID hidden field 
            pnlToolPart.Controls.Add(this.hdnListIdentifier);

            // Browse button 
            Button btnBrowse = new Button();
            btnBrowse.CssClass = "UserButton";
            btnBrowse.Style.Add("float", "left");
            btnBrowse.OnClientClick = String.Format("LaunchPickerTreeDialog('CbqPickerSelectListTitle','CbqPickerSelectListText','{0}','', '{1}', null,'','','/_layouts/images/smt_icon.gif','', callback, ''); return false;", "listsOnly", SPContext.Current.Web.Url);
            btnBrowse.Text = "...";
            pnlToolPart.Controls.Add(btnBrowse);

            this.Controls.Add(pnlToolPart);

            base.CreateChildControls();
        }

        protected override void OnPreRender(EventArgs e)
        {
            EnsureChildControls();

            // Retrieve the list identifier based on postback value or saved GUID 
            if (!String.IsNullOrEmpty(this.hdnListIdentifier.Value))
                this.SliderItemIdentifier = this.hdnListIdentifier.Value;
            else if (!this.WebPart.ListID.Equals(Guid.Empty))
                this.SliderItemIdentifier = String.Format("SPList:{0}?SPWeb:{1}:", this.WebPart.ListID.ToString(), this.WebPart.ListWebID.ToString());

            // Retrieve SPList if list identifier exists 
            if (!String.IsNullOrEmpty(this.SliderItemIdentifier))
                this.SliderItemList = this.GetList(this.SliderItemIdentifier);

            // Set default field values 
            if (this.SliderItemList != null)
                this.lblListTitle.Text = this.SliderItemList.Title;
            this.hdnListIdentifier.Value = this.SliderItemIdentifier;

            // Load fields 
            if (this.SliderItemList != null)
            {
                this.imgListIcon.Visible = true;

                // Show list icon 
                this.imgListIcon.ImageUrl = this.SliderItemList.ImageUrl;
            }
            else
            {
                this.imgListIcon.Visible = false;
                this.lblListTitle.Text = "None";
            }

            // Load PickerTreeDialog.js
            this.Controls.Add(new LiteralControl("<script type=\"text/javascript\" src=\"/_layouts/1033/PickerTreeDialog.js\"></script>"));

            // Render the JavaScript PickerTreeDialog callback
            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\">");
            script.Append("callback = function(arr){");
            script.Append("if(arr != null && arr != undefined){");
            script.Append("document.getElementById('" + this.hdnListIdentifier.ClientID + "').value=arr[0];");
            script.Append("document.getElementById('" + this.lblListTitle.ClientID + "').value=arr[2];");
            script.Append("__doPostBack('" + this.ClientID + "', arr[0]);");
            script.Append("}");
            script.Append("};");
            script.Append("</script>");
            this.Controls.Add(new LiteralControl(script.ToString()));

            base.OnPreRender(e);
        }


        /// 
        /// Retrieves the SPList object from the specified identifier. 
        /// 
        /// The identifier returned by the PickerTreeDialog. 
        /// A SPList object if list is found, otherwise null. 
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


        public override bool ApplyChanges()
        {
            if (this.WebPart != null)
            {
                this.SliderItemList = this.GetList(this.hdnListIdentifier.Value);
                if (this.SliderItemList != null)
                {
                    // Save custom properties 
                    this.WebPart.ListID = this.SliderItemList.ID;
                    this.WebPart.ListWebID = this.SliderItemList.ParentWeb.ID;

                    // Clear cache
                    string cacheKey = this.WebPart.GetCacheKey();
                    Cache cache = HttpContext.Current.Cache;
                    if (cache[cacheKey] != null)
                        cache.Remove(cacheKey);

                    return true;
                }
            }

            return false;
        }


        public override void SyncChanges()
        {
            if (this.WebPart != null)
            {
                this.SliderItemList = this.GetList(this.hdnListIdentifier.Value);
                if (this.SliderItemList != null)
                {
                    this.hdnListIdentifier.Value = this.SliderItemIdentifier;
                    this.lblListTitle.Text = this.SliderItemList.Title;
                }
            }
        }

    }
}
