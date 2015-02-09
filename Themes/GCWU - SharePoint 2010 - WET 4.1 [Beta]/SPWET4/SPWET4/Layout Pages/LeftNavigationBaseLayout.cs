using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;

namespace SPWET4.Layout_Pages
{
    public class LeftNavigationBaseLayout : Microsoft.SharePoint.Publishing.PublishingLayoutPage
    {

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            SPWET4.Master_Pages.WET4PublishingMaster masterPage = (SPWET4.Master_Pages.WET4PublishingMaster)this.Page.Master;

            HtmlGenericControl main = (HtmlGenericControl)masterPage.FindChildControlRecursive("main", this.Page);
            if (main != null)
            {
                if (main.Attributes["class"] != null)
                    main.Attributes.Remove("class");

                main.Attributes.Add("class", "col-md-9 col-md-push-3");
            }
        }

    }
}
