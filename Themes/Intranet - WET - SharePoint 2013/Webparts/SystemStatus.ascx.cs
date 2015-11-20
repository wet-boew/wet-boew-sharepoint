using System;
using System.Text;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace webparts.SystemStatus
{
    [ToolboxItemAttribute(false)]
    public partial class SystemStatus : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public SystemStatus()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string listName = "Service Desk Status";
            int licdtmp = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;

            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    try
                    {
                        int LCID = (HttpContext.Current.Request.Url.ToString().Contains("fra/") ? 1036 : 1033);
                        StringBuilder sb = new StringBuilder();
                        SPList list = web.Lists.TryGetList(listName);

                       

                        Literal1.Text = sb.ToString();
                        
                        /*TableRow headerRow = new TableRow();
                        TableCell headerCell = new TableCell();

                        headerCell.BackColor = System.Drawing.Color.Black;
                        headerCell.ForeColor = System.Drawing.Color.White;
                        headerCell.Text = (LCID == 1036 ? "<h3>État du système</h3>" : " <h3> System Status</h3>");
                        headerRow.BackColor = System.Drawing.Color.Black;
                        headerRow.ForeColor = System.Drawing.Color.White;
                        headerRow.Cells.Add(headerCell);
                        Table1.Rows.Add(headerRow);
                         *Dont need to do this just changing the Title so Pascal doesnot have to modify it from GUI
                         * */

                        string wptitle; 
                        string html; 
                        


                        
                        string Fieldname = (LCID == 1036 ? "Status-FR" : "Status");
                        if (LCID==1036)
                        {
                            this.Title = "État du système";
                            wptitle = "État du système";
                            
                        }
                        else
                        {
                            this.Title = "System Status";
                            wptitle = "System Status";
                        }
                        //SPList geturl = web.Lists.[listName];
                        String strUrlbase = web.Url + "/" +list.RootFolder.Url;
                        int i = 0;
                        //only write html if there is something to write
                        html = "";
                        if (list.ItemCount > 0 )
                            html = "<div id=\"SystemStatustitle\" class=\"SystemStatusTitleRow\" runat=\"server\"><h3 runat=\"server\" id=\"SystemStatush3\" class=\"background-accent margin-bottom-medium\" style=\"width:100% !important; vertical-align:middle;\">" + wptitle + "</h3></div>";
                        //html = html + "<br /> <br />";
                        foreach (SPListItem item in list.Items)
                        {
                            if (i < 3)
                            {

                                String strUrl = strUrlbase + "/DispForm.aspx?ID=" + item.ID;
                                String tableField = " " +
                                    "<a href=\"" + strUrl + "\"" + " class =\"taglink\" >" + item[Fieldname].ToString() + "</a> ";
                                //SystemStatusGrid.InnerText = "<div> " + tableField + " </div>" ;
                                
                                html=html + "<div class=\"SystemStatusGrid\" id=\"SystemStatusGrid\" runat=\"server\" >" ;
                                html = html + tableField.ToString();
                                html=html + "</div>";
                                
                                i++;
                            }
                        }
                        Literal1.Text = html.ToString();
                        
                        
                        
                    }
                    catch (Exception ex)
                    {
                        Literal1.Text = ex.ToString();
                    }
                }
            }

        }

    }
}
