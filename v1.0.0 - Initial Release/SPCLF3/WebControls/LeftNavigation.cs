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
    [ToolboxData("<{0}:LeftNavigation runat=\"server\" />")]
    public class LeftNavigation : SPControl, INamingContainer
    {
        /// <summary>
        /// Ceates content in this manner - all outer divs are in the layout.
        /// Non selected section
        /// <section>
        ///    <h3><a href="#">Section 1</a></h3>
        ///    <ul>
        ///        <li><a href="#">Item 1a</a>
        ///            <ul>
        ///                <li><a href="#">Item 1ai</a></li>
        ///                <li><a href="#">Item 1aii</a></li>
        ///            </ul>
        ///        </li>
        ///        <li><a href="#">Item 1b</a></li>
        ///        <li><a href="#">Item 1c</a></li>
        ///    </ul>
        /// </section>
        /// Selected Section
        /// <section>
        ///    <h3><a href="#" class="nav-current">Current section (4) example </a></h3>
        ///    <ul>
        ///        <li><a href="#">Item 4a</a></li>
        ///        <li><a href="#" class="nav-current">Current item (4b) example</a>
        ///            <ul>
        ///                <li><a href="#">Item 4bi</a></li>
        ///                <li><a href="#" class="nav-current">Current sub (4bii) item example</a></li>
        ///                <li><a href="#">Item 4biii</a></li>
        ///            </ul>
        ///        </li>
        ///        <li><a href="#">Item 4c</a></li>
        ///    </ul>
        /// </section>
        /// </summary>
        #region Properties

        private const string defaultSiteMapProvider = "CurrentNavigation";
        private string _siteMapProvider = defaultSiteMapProvider;
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(defaultSiteMapProvider)]
        [Description("SiteMapProvider for navigation control")]
        public string SiteMapProvider
        {
            get
            {
                return _siteMapProvider;
            }
            set
            {
                _siteMapProvider = value;
            }
        }

        private string _startNodeKey;
        [Browsable(true)]
        [Category("Appearance")]
        [Description("StartNodeKey")]
        public string StartNodeKey
        {
            get
            {
                return _startNodeKey;
            }
            set
            {
                _startNodeKey = value;
            }
        }


        private string _includeSubSites = String.Empty;
        [Browsable(true)]
        [Category("Appearance")]
        [Description("IncludeSubSites bool value")]
        public string IncludeSubSites
        {
            get
            {
                return _includeSubSites;
            }
            set
            {
                _includeSubSites = value;
            }
        }

        private string _includePages = String.Empty;
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("IncludePages bool value")]
        public string IncludePages
        {
            get
            {
                return _includePages;
            }
            set
            {
                _includePages = value;
            }
        }

        private const bool defaultIncludeHeadings = true;
        private bool _includeHeadings = defaultIncludeHeadings;
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(defaultIncludeHeadings)]
        [Description("IncludeHeadings bool value")]
        public bool IncludeHeadings
        {
            get
            {
                return _includeHeadings;
            }
            set
            {
                _includeHeadings = value;
            }
        }

        private const bool defaultIncludeAuthoredLinks = true;
        private bool _includeAuthoredLinks = defaultIncludeAuthoredLinks;
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(defaultIncludeAuthoredLinks)]
        [Description("IncludeAuthoredLinks bool value")]
        public bool IncludeAuthoredLinks
        {
            get
            {
                return _includeAuthoredLinks;
            }
            set
            {
                _includeAuthoredLinks = value;
            }
        }

        // This value is 0 based so maxLevels of 2 means it is showing 3 levels
        private const int defaultMaxLevels = 2;
        private int _maxLevels = defaultMaxLevels;
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(defaultMaxLevels)]
        [Description("Maximum levels to be displayed")]
        public int MaxLevels
        {
            get
            {
                return _maxLevels;
            }
            set
            {
                _maxLevels = value;
            }
        }

        private const string defaulCurrentNodeCssClass = "nav-current";
        private string _currentNodeCssClass = defaulCurrentNodeCssClass;
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(defaulCurrentNodeCssClass)]
        [Description("CurrentNodeCssClass")]
        public string CurrentNodeCssClass
        {
            get
            {
                return _currentNodeCssClass;
            }
            set
            {
                _currentNodeCssClass = value;
            }
        }

        private int _staticDisplayLevels = 2;
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("StaticDisplayLevels")]
        public int StaticDisplayLevels
        {
            get
            {
                return _staticDisplayLevels;
            }
            set
            {
                _staticDisplayLevels = value;
            }
        }

        private const string SectionOpenHTML = "\n<section>";
        private const string SectionCloseHTML = "\n</section>";
        private const string SectionHeaderOpenHtml = "\n<h3>";
        private const string SectionHeaderCloseHtml = "</h3>";
        private const string ListOpenHtml = "\n<ul>";
        private const string ListCloseHtml = "\n</ul>";
        private const string ListItemOpenHtml = "\n<li>";
        private const string ListItemCloseHtml = "</li>";

        private bool displaySubNodesOnly = false;
        private bool showSiblingsChildren = true;
        private SiteMapNode sNode = null;
        private SiteMapNode currNode = null;

        #endregion

        private PortalSiteMapProvider _provider;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SiteMapProvider siteMapProvider = SiteMap.Providers[_siteMapProvider];
            if (siteMapProvider == null)
            {
                return;
            }

            InitPortalSiteMapProvider(siteMapProvider);
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            try
            {
                if (_provider == null)
                {
                    throw new HttpException("Invalid SiteMapProvder:" + _siteMapProvider);
                }

                currNode = _provider.FindSiteMapNodeFromKey(Microsoft.SharePoint.SPContext.Current.Web.ServerRelativeUrl);

                sNode = GetStartingNode();
                SiteMapNodeCollection nodes = _provider.GetChildNodes(sNode);

                // if we are only displaying the subnodes or if we are at /eng or /fra then only show children
                if ((!displaySubNodesOnly) || (Microsoft.SharePoint.SPContext.Current.Web.ParentWeb.IsRootWeb))
                {
                    // displaying children as sections
                    PlaceHolder placeHolder = new PlaceHolder();

                    foreach (SiteMapNode childrenNode in nodes)
                    {
                        RenderNode(placeHolder, childrenNode, 0);
                    }

                    Controls.Add(placeHolder);
                }
                else
                {
                    // displaying top level as only section and everything else as children
                    PlaceHolder placeHolder = new PlaceHolder();
                    RenderNode(placeHolder, GetStartingNode(), 0);
                    Controls.Add(placeHolder);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message + " " + ex.StackTrace);
            }
        }


        private void InitPortalSiteMapProvider(SiteMapProvider siteMapProvider)
        {

            if (siteMapProvider is PortalSiteMapProvider)
            {
                _provider = siteMapProvider as PortalSiteMapProvider;
                _provider.DynamicChildLimit = 0;
                _provider.EncodeOutput = true;
                _provider.IncludeAuthoredLinks = _includeAuthoredLinks;
                _provider.IncludeHeadings = _includeHeadings;
                _provider.IncludePages = GetIncludeOption(_includePages);
                _provider.IncludeSubSites = GetIncludeOption(_includeSubSites);
            }
        }

        private PortalSiteMapProvider.IncludeOption GetIncludeOption(string value)
        {
            switch (value.ToLower())
            {
                case "true":
                    return PortalSiteMapProvider.IncludeOption.Always;
                case "false":
                    return PortalSiteMapProvider.IncludeOption.Never;
                default:
                    return PortalSiteMapProvider.IncludeOption.PerWeb;
            }
        }

        private SiteMapNode GetStartingNode()
        {
            SiteMapNode startingNode = null;

            if (!String.IsNullOrEmpty(_startNodeKey))
            {
                startingNode = _provider.FindSiteMapNodeFromKey(_startNodeKey);
            }

            if (startingNode == null)
            {
                PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(_provider.CurrentWeb);

                if (!pubWeb.Navigation.InheritCurrent && pubWeb.ParentPublishingWeb.IsRoot)
                {
                    // then we are on the /eng or /fra just off the root so use the current node
                    return currNode;
                }
                else if (!pubWeb.Navigation.InheritCurrent && pubWeb.Navigation.ShowSiblings)
                {
                    startingNode = _provider.FindSiteMapNodeFromKey(Microsoft.SharePoint.SPContext.Current.Web.ServerRelativeUrl);

                    startingNode = startingNode.ParentNode;
                    showSiblingsChildren = false;
                }
                else if (!pubWeb.Navigation.InheritCurrent && !pubWeb.Navigation.ShowSiblings)
                {
                    startingNode = _provider.FindSiteMapNodeFromKey(Microsoft.SharePoint.SPContext.Current.Web.ServerRelativeUrl);
                    if (startingNode.Url.ToLower().IndexOf("/eng/pages") == -1 && startingNode.Url.ToLower().IndexOf("/fra/pages") == -1 && startingNode.ParentNode != null)
                        displaySubNodesOnly = true;
                }
                else if (pubWeb.Navigation.InheritCurrent)
                {
                    startingNode = _provider.FindSiteMapNodeFromKey(Microsoft.SharePoint.SPContext.Current.Web.ServerRelativeUrl);
                    PublishingWeb pWeb = null;
                    SPWeb web = null;
                    int count = 1;
                    do
                    {
                        startingNode = startingNode.ParentNode;
                        try
                        {
                            web = _provider.CurrentSite.OpenWeb(startingNode.Key);
                            pWeb = PublishingWeb.GetPublishingWeb(web);
                            if ((!pWeb.Navigation.InheritCurrent && pWeb.Navigation.ShowSiblings) || (!pWeb.Navigation.InheritCurrent && !pWeb.Navigation.ShowSiblings))
                            {
                                if (startingNode.Url.ToLower().IndexOf("/eng/pages") == -1 && startingNode.Url.ToLower().IndexOf("/fra/pages") == -1 && startingNode.ParentNode != null)
                                {
                                    displaySubNodesOnly = true;
                                }
                            }
                            count++;
                        }
                        finally
                        {
                            if (web != null)
                            {
                                web.Dispose();
                            }
                        }
                    } while (pWeb.Navigation.InheritCurrent);
                }
                else
                {
                    string currenturl = HttpContext.Current.Request.Path.ToString().ToLower();
                    if (currenturl.IndexOf("eng") != -1)
                        startingNode = _provider.FindSiteMapNode("/eng");
                    else if (currenturl.IndexOf("fra") != -1)
                        startingNode = _provider.FindSiteMapNode("/fra");
                    else
                        startingNode = _provider.FindSiteMapNode("/");
                }
            }

            return startingNode;
        }

        private void RenderNode(PlaceHolder placeHolder, SiteMapNode node, int level)
        {
            //<section>
            //   <h3><a href="#" class="nav-current">Current section (4) example </a></h3>
            //   <ul>
            //       <li><a href="#">Item 4a</a></li>
            //       <li><a href="#" class="nav-current">Current item (4b) example</a>
            //           <ul>
            //               <li><a href="#">Item 4bi</a></li>
            //               <li><a href="#" class="nav-current">Current sub (4bii) item example</a></li>
            //               <li><a href="#">Item 4biii</a></li>
            //           </ul>
            //       </li>
            //       <li><a href="#">Item 4c</a></li>
            //   </ul>
            //</section>
            if (!ShowNode(node, level))
            {
                return;
            }

            string cssClass = GetCssClass(node, level);
            String cssClassHtml = String.Empty;
            if (!String.IsNullOrEmpty(cssClass))
            {
                cssClassHtml = String.Format(" class=\"{0}\"", cssClass);
            }

            // figure out the wrapper that you want to put around the Node depending on the level
            // handle the section headers
            if (level == 0)
            {
                // add the "<section><h3>"
                placeHolder.Controls.Add(new LiteralControl(SectionOpenHTML + SectionHeaderOpenHtml));
            }
            else
            {
                // render a "<LI>" for the item
                placeHolder.Controls.Add(new LiteralControl(String.Format(ListItemOpenHtml, cssClassHtml)));
            }

            // Render the link for the item or title
            RenderNodeItem(placeHolder, node, cssClass);

            // render any closing elements for the level
            if (level == 0)
            {
                placeHolder.Controls.Add(new LiteralControl(SectionHeaderCloseHtml));
            }

            // render any expandable child items
            if (IsNodeExpandable(node, level))
            {
                SiteMapNodeCollection nodes = _provider.GetChildNodes(node);
                PlaceHolder childrenPlaceHolder = new PlaceHolder();

                foreach (SiteMapNode childrenNode in nodes)
                {
                    RenderNode(childrenPlaceHolder, childrenNode, level + 1);
                }

                if (childrenPlaceHolder.Controls.Count > 0)
                {
                    String subListCssClass = String.Empty;
                    placeHolder.Controls.Add(new LiteralControl(ListOpenHtml));
                    placeHolder.Controls.Add(childrenPlaceHolder);
                    placeHolder.Controls.Add(new LiteralControl(ListCloseHtml));
                }
                else
                {
                    childrenPlaceHolder.Dispose();
                }
            }

            // render any closing elements for the 0 level section or the child list item
            if (level == 0)
            {
                placeHolder.Controls.Add(new LiteralControl(SectionCloseHTML));
            }
            else
            {
                placeHolder.Controls.Add(new LiteralControl(ListItemCloseHtml));
            }
        }

        private bool ShowNode(SiteMapNode node, int level)
        {
            // if it is too deep then NO
            if (level > this.MaxLevels)
            {
                return false;
            }

            if (level <= 1 || _provider.CurrentNode.IsDescendantOf(node) || node.Key == _provider.CurrentNode.Key)
            {
                return true;
            }

            SiteMapNode parentNode = node.ParentNode;
            if (parentNode == null || _provider.CurrentNode.ParentNode == null)
            {
                return false;
            }

            if (parentNode.Key == _provider.CurrentNode.Key ||
               (parentNode.Key == _provider.CurrentNode.ParentNode.Key && !IsNodeExpandable(_provider.CurrentNode, level)) ||
               (_provider.CurrentNode.IsDescendantOf(parentNode)))
            {
                return true;
            }

            if (StaticDisplayLevels > 1)
            {
                return true;
            }
            return false;
        }

        // indicates if the node can expand
        private bool IsNodeExpandable(SiteMapNode node, int level)
        {
            SiteMapNodeCollection nodes = _provider.GetChildNodes(node);
            SiteMapNodeCollection StartingNodeSiblings = currNode.ParentNode.ChildNodes;
            if (!showSiblingsChildren && StartingNodeSiblings.Contains(node) && node != currNode)
            {
                return false;
            }
            if (nodes.Count == 0)
            {
                return false;
            }
            if (node.Key == _provider.CurrentNode.Key && level >= _maxLevels && _maxLevels != 0)
            {
                return true;
            }
            if (level >= _maxLevels && _maxLevels != 0)
            {
                return false;
            }

            return true;
        }

        // figures out of the node is on the path and adds the _currentNodeCssClass if it is
        private string GetCssClass(SiteMapNode node, int level)
        {
            if (Page.Request.Url.AbsolutePath.ToLower().StartsWith(node.Key.ToLower()))
            {
                //// if it is the current node or one of its parents
                //if (Page.Request.Url.AbsolutePath.StartsWith(node.Url))
                ////if (node.Url.StartsWith(Page.Request.Url.AbsolutePath))
                //{
                //    if (node.ParentNode.Url.ToLower().IndexOf("/eng/pages") != -1 || node.ParentNode.Url.ToLower().IndexOf("/fra/pages") != -1)
                //    {
                //        // not on the path... no CSS
                //        //return string.Empty;
                //        return _currentNodeCssClass;
                //    }
                //    else
                //    {
                //        // is on the path
                //        //return _currentNodeCssClass;
                //        return string.Empty;
                //    }
                //}
                //else
                //{
                return _currentNodeCssClass;
                //}
            }
            else
            {
                return string.Empty;
            }

            //else if (String.IsNullOrEmpty(node.Url))
            //{
            //    return string.Empty;
            //}
            //else
            //{
            //    if (_provider.CurrentNode.IsDescendantOf(node) && level == _maxLevels)
            //    {
            //        return _currentNodeCssClass;
            //    }
            //    else
            //    {
            //        return string.Empty;
            //    }
            //}
        }

        // renders the text of the Node as a link or plain text
        private void RenderNodeItem(PlaceHolder placeHolder, SiteMapNode node, string cssClass)
        {
            // only render if it is not the root
            if (node.Url.ToLower().IndexOf("/eng/pages") == -1 && node.Url.ToLower().IndexOf("/fra/pages") == -1 && node.ParentNode != null)
            {
                if (String.IsNullOrEmpty(node.Url))
                {
                    placeHolder.Controls.Add(new LiteralControl(node.Title));
                }
                else
                {
                    HyperLink hyperlink = new HyperLink();
                    hyperlink.NavigateUrl = node.Url;
                    hyperlink.CssClass = cssClass;
                    if (node is PortalSiteMapNode)
                    {
                        PortalSiteMapNode portalNode = node as PortalSiteMapNode;
                        if (portalNode.Target != null)
                        {
                            hyperlink.Target = portalNode.Target;
                        }
                    }
                    hyperlink.Text = node.Title;
                    placeHolder.Controls.Add(hyperlink);
                }
            }
        }
    }
}
