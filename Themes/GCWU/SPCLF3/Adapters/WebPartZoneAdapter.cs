using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebPartPages;
using System.Web.UI.WebControls.WebParts;

namespace SPCLF3.Adapters
{
    public class WebPartZoneAdapter:System.Web.UI.Adapters.ControlAdapter
    {
        public WebPartZoneAdapter() { }
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            bool inEditMode = false;
            var webpartZone = Control as Microsoft.SharePoint.WebPartPages.WebPartZone;
            if (webpartZone != null)
            {
                var wpManager = (SPWebPartManager)WebPartManager.GetCurrentWebPartManager(webpartZone.Page);
                if (wpManager != null)
                {
                    inEditMode = wpManager.GetDisplayMode().AllowPageDesign;
                }
            }

            if (!inEditMode)
            {
                if (webpartZone.WebParts.Count > 0)
                {
                    WebPartCollection wpColl = new WebPartCollection(webpartZone.WebParts);
                    foreach (System.Web.UI.WebControls.WebParts.WebPart wp in wpColl)
                    {
                        wp.RenderControl(writer);
                    }
                }
            }
            else
            {
                base.Render(writer);
            }
        }
    }
}
