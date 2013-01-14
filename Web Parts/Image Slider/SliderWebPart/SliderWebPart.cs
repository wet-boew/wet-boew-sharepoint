using System;
using System.ComponentModel;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.WebPartPages;

namespace LAC.SharePoint.Slider.SliderWebPart
{
    [ToolboxItemAttribute(false)]
    public class SliderWebPart : Microsoft.SharePoint.WebPartPages.WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/LAC.SharePoint.Slider/SliderWebPart/SliderWebPartUserControl.ascx";

        [Browsable(false)]
        [Personalizable(PersonalizationScope.Shared)]
        public Guid ListID { get; set; }

        [Browsable(false)]
        [Personalizable(PersonalizationScope.Shared)]
        public Guid ListWebID { get; set; }

        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]
        [ResourcesAttribute("Slider_ItemLimit", "Slider_ItemLimit_Category", "Slider_ItemLimit_Description")]
        public int ItemLimit { get { return this._ItemLimit; } set { this._ItemLimit = value; } }
        private int _ItemLimit = 10;

        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]
        [ResourcesAttribute("Slider_Speed", "Slider_Speed_Category", "Slider_Speed_Description")]
        public int Speed { get { return this._Speed; } set { this._Speed = value; } }
        private int _Speed = 6000;

        public SliderWebPartUserControl UserControl;

        protected override void CreateChildControls()
        {
            SliderWebPartUserControl control = (SliderWebPartUserControl)Page.LoadControl(_ascxPath);
            this.UserControl = control;
            Controls.Add(control);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (this.ListID.Equals(Guid.Empty))
            {
                // Show error message
                Label lblMessage = new Label();
                lblMessage.Text = LoadResource("Slider_Error_NotConfigured");
                lblMessage.ForeColor = Color.Red;
                lblMessage.ID = "sliderWPStatus";
                UserControl.Controls.AddAt(0, lblMessage);
            }
            base.OnPreRender(e);
        }

        /// <summary>
        /// Adds custom PickerEditorPart.
        /// </summary>
        /// <returns></returns>
        public override EditorPartCollection CreateEditorParts()
        {
            return new EditorPartCollection(base.CreateEditorParts(), new[] { new SliderEditorPart { ID = "EditorPart_" + this.ID } });
        }

        /// <summary>
        /// Retrieve the cache key used to store query results.
        /// </summary>
        /// <returns></returns>
        public string GetCacheKey()
        {
            return "CacheKey_" + this.ID.ToString();
        }

        public override string LoadResource(string id)
        {
            return (string)HttpContext.GetGlobalResourceObject("SliderWebPart", id);
        }

    }
}
