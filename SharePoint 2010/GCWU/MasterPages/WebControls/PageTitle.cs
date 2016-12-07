using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Security;
using WET.Theme.GCWU.Objects;

namespace WET.Theme.GCWU.WebControls
{
    /// <summary>
    /// 
    /// </summary>
    [ToolboxData("<{0}:PageTitle runat=\"server\" />")]
    public class PageTitle : Control
    {

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            // allow third party applications to override the title of the current node in the breadcrumb
            WET.Theme.GCWU.Master_Pages.WETPublishingMaster masterPage = (WET.Theme.GCWU.Master_Pages.WETPublishingMaster)this.Page.Master;
            if (String.IsNullOrEmpty(masterPage.PageTitle))
            {
                if (SPContext.Current.ListItem != null && PublishingPage.IsPublishingPage(SPContext.Current.ListItem))
                {
                    PublishingPage publishingPage = PublishingPage.GetPublishingPage(SPContext.Current.ListItem);
                    writer.WriteLine(publishingPage.Title);
                }
                else if (SPContext.Current.ListItem != null)
                {
                    try
                    {

                        writer.Write(SPContext.Current.ListItem.Title);
                    }
                    catch {
                        try
                        {
                            writer.Write(SPContext.Current.ListItem.DisplayName);
                        }
                        catch (Exception ex)
                        {
                            WET.Theme.GCWU.Objects.Logger.WriteLog("Page Title:" + ex.Message);
                        }
                    }
                }
                else if (SPContext.Current.List != null)
                {
                    writer.Write(SPContext.Current.List.Title);
                }
                else if (HttpContext.Current.Request != null)
                {
                    string curUrl = HttpContext.Current.Request.Url.ToString();
                    string fileNameNoExtension = curUrl.Split('/')[curUrl.Split('/').Length - 1].Split('.')[0];
                    string fakeTitle = char.ToUpper(fileNameNoExtension[0]) + fileNameNoExtension.ToLower().Substring(1);
                    writer.Write(fakeTitle);
                }
                else
                    writer.Write("Administrative Page");
            }
            else
                writer.WriteLine(masterPage.PageTitle);
        }

    }
}
