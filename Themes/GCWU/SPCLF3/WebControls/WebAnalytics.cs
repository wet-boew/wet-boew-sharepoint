using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Web;
using System.Web.UI;

namespace WET.Theme.GCWU.WebControls
{
    /// <summary>
    /// Contains all the methods required to render the tracking code from Google Analytics and WebTrends.
    /// This web control will only render the tracking code when the solution configuration is set to
    /// </summary>
    [ToolboxData("<{0}:WebAnalytics runat=\"server\" />")]
    public class WebAnalytics : Control
    {

#if DEBUG
        private bool RenderTrackingCode = false;
#else
        private bool RenderTrackingCode = true;
#endif

        public TrackingCodeLocation Location
        {
            get { return this._Location; }
            set { this._Location = value; }
        }
        protected TrackingCodeLocation _Location = TrackingCodeLocation.Head;
        public enum TrackingCodeLocation { Head = 0, Body = 1 };


        protected override void Render(HtmlTextWriter writer)
        {
            if (this.RenderTrackingCode)
            {
                if (this.Location == TrackingCodeLocation.Head)
                {
                    writer.WriteLine("<!-- Google Analytics Tracking Code Begin -->");
                    writer.WriteLine("<script type=\"text/javascript\">");
                    writer.WriteLine("var _gaq = _gaq || [];");
                    writer.WriteLine("_gaq.push(['_setAccount', 'UA-27341782-1']);");
                    writer.WriteLine("_gaq.push(['_setDomainName', 'bac-lac.gc.ca']);");
                    writer.WriteLine("_gaq.push(['_setAllowLinker', true]);");
                    writer.WriteLine("_gaq.push(['_trackPageview']);");
                    writer.WriteLine("(function() {");
                    writer.WriteLine("var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;");
                    writer.WriteLine("ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';");
                    writer.WriteLine("var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);");
                    writer.WriteLine("})();");
                    writer.WriteLine("</script>");
                    writer.WriteLine("<!-- Google Analytics Tracking Code End -->");
                }
                else if (this.Location == TrackingCodeLocation.Body)
                {
                    writer.WriteLine("<!-- START OF SmartSource Data Collector TAG -->");
                    writer.WriteLine("<!-- Copyright (c) 1996-2012 Webtrends Inc.  All rights reserved. -->");
                    writer.WriteLine("<!-- Version: 9.4.0 -->");
                    writer.WriteLine("<!-- Tag Builder Version: 4.1  -->");
                    writer.WriteLine("<!-- Created: 8/30/2012 3:33:50 PM -->");
                    writer.WriteLine("<script src=\"/_layouts/WET.Theme.GCWU/webtrends/js/webtrends.js\" type=\"text/javascript\"></script>");

                    writer.WriteLine("<!-- Warning: The two script blocks below must remain inline. Moving them to an external -->");
                    writer.WriteLine("<!-- JavaScript include file can cause serious problems with cross-domain tracking.      -->");

                    writer.WriteLine("<script type=\"text/javascript\">");
                    writer.WriteLine("//<![CDATA[");
                    writer.WriteLine("var _tag=new WebTrends();");
                    writer.WriteLine("_tag.dcsGetId();");
                    writer.WriteLine("//]]>");
                    writer.WriteLine("</script>");
                    writer.WriteLine("<script type=\"text/javascript\">");
                    writer.WriteLine("//<![CDATA[");
                    writer.WriteLine("_tag.dcsCustom=function(){");
                    writer.WriteLine("// Add custom parameters here.");
                    writer.WriteLine("//_tag.DCSext.param_name=param_value;");
                    writer.WriteLine("}");
                    writer.WriteLine("_tag.dcsCollect();");
                    writer.WriteLine("//]]>");
                    writer.WriteLine("</script>");
                    writer.WriteLine("<noscript>");
                    writer.WriteLine("<div><img alt=\"DCSIMG\" id=\"DCSIMG\" width=\"1\" height=\"1\" src=\"//webtrends2.collectionscanada.gc.ca/dcsbicp4b100004n4y0dqb5li_6e9v/njs.gif?dcsuri=/nojavascript&amp;WT.js=No&amp;WT.tv=9.4.0&amp;dcssip=www.bac-lac.gc.ca\"/></div>");
                    writer.WriteLine("</noscript>");
                    writer.WriteLine("<!-- END OF SmartSource Data Collector TAG -->");
                }
            }
        }


    }
}
