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
    /// Ensures anonymous users of a SharePoint 2010 site do not receive unnecessary large JavaScript files (slows down first page load). Files to suppress are specified
    /// in the FilesToSuppress property (a semi-colon separated list). This control *must* be placed before the main OOTB ScriptLink control (Microsoft.SharePoint.WebControls.ScriptLink) in the
    /// markup for the master page.
    /// </summary>
    /// <remarks>
    /// This control works by manipulating the HttpContext.Current.Items key which contains the script links added by various server-side registrations. Since SharePoint uses sealed/internal 
    /// code to manage this list, some minor reflection is required to read values. However, this is preferable to end-users downloading huge JS files which they do not need.
    /// </remarks>
    [ToolboxData("<{0}:SuppressJSForAnonymous runat=\"server\" />")]
    public class SuppressJSForAnonymous : Control
    {
        private const string HTTPCONTEXT_SCRIPTLINKS = "sp-scriptlinks";
        private List<string> files = new List<string>();
        private List<int> indiciesOfFilesToBeRemoved = new List<int>();
        public string FilesToSuppress
        {
            get;
            set;
        }

        protected override void OnInit(EventArgs e)
        {
            files.AddRange(FilesToSuppress.Split(';'));
            base.OnInit(e);
        }
        protected override void OnPreRender(EventArgs e)
        {
            // only process if user is anonymous..
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                // get list of registered script files which will be loaded..
                object oFiles = HttpContext.Current.Items[HTTPCONTEXT_SCRIPTLINKS];
                IList registeredFiles = (IList)oFiles;
                int i = 0;
                foreach (var file in registeredFiles)
                {
                    // use reflection to get the ScriptLinkInfo.Filename property, then check if in FilesToSuppress list and remove from collection if so..
                    Type t = file.GetType();
                    PropertyInfo prop = t.GetProperty("Filename");
                    if (prop != null)
                    {
                        string filename = prop.GetValue(file, null).ToString();
                        if (!string.IsNullOrEmpty(files.Find(delegate(string sFound)
                        {
                            return filename.ToLower().Contains(sFound.ToLower());
                        })))
                        {
                            indiciesOfFilesToBeRemoved.Add(i);
                        }
                    }
                    i++;
                }
                int iRemoved = 0;
                foreach (int j in indiciesOfFilesToBeRemoved)
                {
                    registeredFiles.RemoveAt(j - iRemoved);
                    iRemoved++;
                }
                // overwrite cached value with amended collection..
                HttpContext.Current.Items[HTTPCONTEXT_SCRIPTLINKS] = registeredFiles;
            }

            base.OnPreRender(e);
        }
    }
}

