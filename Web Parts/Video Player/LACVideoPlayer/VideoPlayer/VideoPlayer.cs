using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace LACVideoPlayer.VideoPlayer
{
    [ToolboxItemAttribute(false)]
    public class VideoPlayer : WebPart
    {
        private string _VideoUrlMP4;
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(false)]
        [Description("Video Url MP4")]
        public string VideoUrlMP4
        {
            get
            {
                return _VideoUrlMP4;
            }
            set
            {
                _VideoUrlMP4 = value;
            }
        }
        private string _VideoUrlWebM;
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(false)]
        [Description("Video Url WebM")]
        public string VideoUrlWebM
        {
            get
            {
                return _VideoUrlWebM;
            }
            set
            {
                _VideoUrlWebM = value;
            }
        }
        private string _subUrl;
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(false)]
        [Description("Subtitle Url")]
        public string subUrl
        {
            get
            {
                if (_subUrl == null) return "";
                else return _subUrl;
            }
            set
            {
                _subUrl = value;
            }
        }
                
        private string _width;
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(false)]
        [Description("Width")]
        public string width
        {
            get
            {
                if (_width == null) return "640";
                else return _width;
            }
            set
            {
                _width = value;
            }
        }
        private string _height;
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(false)]
        [Description("Height")]
        public string height
        {
            get
            {
                if (_height == null) return "360";
                else return _height;
            }
            set
            {
                _height = value;
            }
        }
        private string _altimg;
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(false)]
        [Description("Alternate Image")]
        public string altimage
        {
            get
            {
                if (_altimg == null) return "";
                else return _altimg;
            }
            set
            {
                _altimg = value;
            }
        }
        Panel editPanel;
        TextBox videourl, suburl, subtype, videourlwebm, txtwidth, txtheight, txtaltimg, suburlFR;
        Button submit;

        protected override void CreateChildControls()
        {
            if (SPContext.Current.FormContext.FormMode == SPControlMode.Display)
            {
                ScriptLink sl = new ScriptLink();
                sl.Name = "jquery-1.7.min.js";
                Controls.Add(sl);
                ScriptLink sl2 = new ScriptLink();
                sl2.Name = "mediaelement-and-player.min.js";
                Controls.Add(sl2);
                CssRegistration css = new CssRegistration();
                css.Name = "mediaelementplayer.min.css";
                Controls.Add(css);

                Controls.Add(new LiteralControl("<video width=\"" + width + "\" height=\"" + height + "\" controls=\"controls\" preload=\"none\" poster=\"" + altimage + "\">"));
                Controls.Add(new LiteralControl("<source type=\"video/mp4\" src=\"" + VideoUrlMP4 + "\" />"));
                Controls.Add(new LiteralControl("<source type=\"video/webm\" src=\"" + VideoUrlWebM + "\" />"));
                if (subUrl.Length > 2)
                {
                    Controls.Add(new LiteralControl("<track kind=\"subtitles\" label=\"English\" srclang=\"en\" src=\"" + subUrl + "\"></track>"));
                }
                //<!-- Fallback flash player for no-HTML5 browsers with JavaScript turned off -->
                Controls.Add(new LiteralControl("<object width=\"" + width + "\" height=\"" + height + "\" type=\"application/x-shockwave-flash\" data=\"/_layouts/1033/flashmediaelement.swf\"> 	"));
                Controls.Add(new LiteralControl("<param name=\"movie\" value=\"/_layouts/1033/flashmediaelement.swf\" /> "));
                Controls.Add(new LiteralControl("<param name=\"flashvars\" value=\"controls=true&amp;file=" + VideoUrlMP4 + "\" /> 	"));
                //<!-- Image fall back for non-HTML5 browser with JavaScript turned off and no Flash player installed -->
                Controls.Add(new LiteralControl("<img src=\"" + altimage + "\" width=\"" + width + "\" height=\"" + height + "\" alt=\"Here we are\" title=\"No video playback capabilities\" />"));
                Controls.Add(new LiteralControl("</object> 	"));
                //FallBack to Silverlight video player for wmv playback 
                Controls.Add(new LiteralControl("<object width=\"" + width + "\" height=\"" + height + "\" type=\"application/x-silverlight-2\" data=\"/_layouts/1033/silverlightmediaelement.xap\"> 	"));
                Controls.Add(new LiteralControl("<param name=\"movie\" value=\"/_layouts/1033/silverlightmediaelement.xap\" /> "));
                Controls.Add(new LiteralControl("<param name=\"flashvars\" value=\"controls=true&amp;file=" + VideoUrlMP4 + "\" /> 	"));
                //<!-- Image fall back for non-HTML5 browser with JavaScript turned off and no Flash player installed -->
                Controls.Add(new LiteralControl("<img src=\"" + altimage + "\" width=\"" + width + "\" height=\"" + height + "\" alt=\"Here we are\" title=\"No video playback capabilities\" />"));
                Controls.Add(new LiteralControl("</object> 	"));
                Controls.Add(new LiteralControl("</video>"));

                Controls.Add(new LiteralControl(@"
                    <script>
                        $('video').mediaelementplayer({
                            // initial volume when the player starts
                            startVolume: 0.8,
                            // useful for <audio> player loops
                            loop: false,
                            // enables Flash and Silverlight to resize to content size
                            enableAutosize: true,
                            // the order of controls you want on the control bar (and other plugins below)
                            features: ['playpause','progress','current','duration','tracks','volume','fullscreen'],
                            // Hide controls when playing and mouse is not over the video
                            alwaysShowControls: false,
                            // force iPad's native controls
                            iPadUseNativeControls: false,
                            // force iPhone's native controls
                            iPhoneUseNativeControls: false,
                            // force Android's native controls
                            AndroidUseNativeControls: false
                        });
                    </script>"));

            }
            else // SPContext.Current.FormContext.FormMode = SPControlMode.Edit    
            {
                editPanel = new Panel();
                editPanel.Width = Unit.Pixel(460);
                editPanel.Controls.Add(new LiteralControl("WebPart Configuration"));
                editPanel.Controls.Add(new LiteralControl("<br/>Please define the MP4 video url<br/>"));
                videourl = new TextBox();
                if (!string.IsNullOrEmpty(VideoUrlMP4))
                {
                    videourl.Text = VideoUrlMP4;
                }
                editPanel.Controls.Add(videourl);

                editPanel.Controls.Add(new LiteralControl("<br/>"));
                editPanel.Controls.Add(new LiteralControl("<br/>Please define the WebM video url<br/>"));
                videourlwebm = new TextBox();
                if (!string.IsNullOrEmpty(VideoUrlWebM))
                {
                    videourlwebm.Text = VideoUrlWebM;
                }
                editPanel.Controls.Add(videourlwebm);

                editPanel.Controls.Add(new LiteralControl("<br/>"));
                editPanel.Controls.Add(new LiteralControl("<br/>Please define the subtitle url<br/>"));
                suburl = new TextBox();
                if (!string.IsNullOrEmpty(subUrl))
                {
                    suburl.Text = subUrl;
                }
                editPanel.Controls.Add(suburl);
                
                editPanel.Controls.Add(new LiteralControl("<br/>"));
                editPanel.Controls.Add(new LiteralControl("<br/>Please define the width<br/>"));
                txtwidth = new TextBox();
                if (!string.IsNullOrEmpty(width))
                {
                    txtwidth.Text = width;
                }
                editPanel.Controls.Add(txtwidth);

                editPanel.Controls.Add(new LiteralControl("<br/>"));
                editPanel.Controls.Add(new LiteralControl("<br/>Please define the height<br/>"));
                txtheight = new TextBox();
                if (!string.IsNullOrEmpty(height))
                {
                    txtheight.Text = height;
                }
                editPanel.Controls.Add(txtheight);

                editPanel.Controls.Add(new LiteralControl("<br/>"));
                editPanel.Controls.Add(new LiteralControl("<br/>Please define the alternate image (no playback fallback) url<br/>"));
                txtaltimg = new TextBox();
                if (!string.IsNullOrEmpty(altimage))
                {
                    txtaltimg.Text = altimage;
                }
                editPanel.Controls.Add(txtaltimg);

                editPanel.Controls.Add(new LiteralControl("<br/>"));
                submit = new Button();
                submit.Text = "Save";
                submit.Click += new EventHandler(submit_Click);
                editPanel.Controls.Add(submit);
                Controls.Add(editPanel);
            }


        }
        void submit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(videourl.Text.Trim()))
            {
                Controls.Remove(editPanel);
                VideoUrlMP4 = videourl.Text;
                subUrl = suburl.Text;
                VideoUrlWebM = videourlwebm.Text;
                width = txtwidth.Text;
                height = txtheight.Text;
                altimage = txtaltimg.Text;
                this.SetPersonalizationDirty(); //This is what saves the property        
                Controls.Add(new LiteralControl("WebPart Configured."));
            }
            else
            {
                Controls.Add(new LiteralControl("Missing value."));
            }
        }
    }
}
