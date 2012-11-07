using System;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Publishing.Navigation;
using Microsoft.SharePoint.WebControls;
using SPCLF3.Objects;

namespace SPCLF3.WebControls
{
    [ToolboxData("<{0}:CLFProgressiveEnhancement runat=server></{0}:CLFProgressiveEnhancement>")]
    public class CLFProgressiveEnhancement : WebControl
    {

        protected override void  OnPreRender(EventArgs e)
        {
 	        base.OnPreRender(e);

            SPCLF3.Master_Pages.CLF3PublishingMaster masterPage = (SPCLF3.Master_Pages.CLF3PublishingMaster)this.Page.Master;
            if (masterPage != null)
            {
                LiteralControl script = new LiteralControl();
                script.Text = "";
                script.Text += "<script>";
                script.Text += "/* <![CDATA[ */";
                script.Text += "var params = {";
                script.Text += "menubar : \"\"";
                script.Text += masterPage.RenderFeatures();
                script.Text += "};";
                script.Text += "PE.progress(params);";
                script.Text += "/* ]]> */";
                script.Text += "</script>";

                this.Controls.Add(script);
            }
        }

    }
}
