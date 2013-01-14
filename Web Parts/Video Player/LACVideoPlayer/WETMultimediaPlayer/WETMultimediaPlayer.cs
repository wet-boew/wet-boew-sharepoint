using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace LACVideoPlayer.WETMultimediaPlayer
{
    [ToolboxItemAttribute(false)]
    public class WETMultimediaPlayer : WebPart
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

        private string _Title;
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(false)]
        [Description("Video's Title")]
        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                this._Title = value;
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
        [Description("Caption Url")]
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
        TextBox videourl, suburl, videourlwebm, txtwidth, txtheight, txtaltimg, titleTxt;
        Button submit;

        protected override void CreateChildControls()
        {
            if (SPContext.Current.FormContext.FormMode == SPControlMode.Display)
            {
                Controls.Add(new LiteralControl("<div class=\"wet-boew-multimedia\">"));
                Controls.Add(new LiteralControl("<video width=\"" + width + "\" height=\"" + height + "\" poster=\"" + altimage + "\" title=\"" + Title + "\">"));

                if (VideoUrlWebM != string.Empty)
                    Controls.Add(new LiteralControl("<source src=\"" + VideoUrlWebM + "\" type=\"video/webm\" />"));

                if (VideoUrlMP4 != string.Empty)
                    Controls.Add(new LiteralControl("<source src=\"" + VideoUrlMP4 + "\" type=\"video/mp4\" />"));

                if (subUrl.Length > 2)
                {
                    Controls.Add(new LiteralControl("<track kind=\"captions\" data-type=\"application/ttml+xml\" src=\"" + subUrl + "\"></track>"));
                }
                Controls.Add(new LiteralControl("</video>"));
                Controls.Add(new LiteralControl("</div>"));
            }
            else // SPContext.Current.FormContext.FormMode = SPControlMode.Edit    
            {
                editPanel = new Panel();
                editPanel.Width = Unit.Pixel(460);
                editPanel.Controls.Add(new LiteralControl("WebPart Configuration"));
                editPanel.Controls.Add(new LiteralControl("<br/>Video's Title<br/>"));
                titleTxt = new TextBox();
                if (!string.IsNullOrEmpty(Title))
                {
                    titleTxt.Text = Title;
                }
                editPanel.Controls.Add(titleTxt);

                editPanel.Controls.Add(new LiteralControl("<br />"));
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
                Title = titleTxt.Text;
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
