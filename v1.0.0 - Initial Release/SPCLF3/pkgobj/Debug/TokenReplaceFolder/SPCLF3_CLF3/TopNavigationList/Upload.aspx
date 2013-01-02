<%@ Page language="C#" MasterPageFile="~masterurl/default.master"    Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c"  %> <%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Import Namespace="Microsoft.SharePoint" %> <%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ContentPlaceHolderId="PlaceHolderLeftNavBar" runat="server" >
<SharePoint:UIVersionedContent UIVersion="4" runat="server">
	<ContentTemplate>
				<div class="ms-quicklaunchouter">
				<div class="ms-quickLaunch">
				<SharePoint:VersionedPlaceHolder UIVersion="3" runat="server">
					<h3 class="ms-standardheader"><label class="ms-hidden"><SharePoint:EncodedLiteral runat="server" text="<%$Resources:wss,quiklnch_pagetitle%>" EncodeMethod="HtmlEncode"/></label>
					<Sharepoint:SPSecurityTrimmedControl runat="server" PermissionsString="ViewFormPages">
						<div class="ms-quicklaunchheader"><SharePoint:SPLinkButton id="idNavLinkViewAll" runat="server" NavigateUrl="~site/_layouts/viewlsts.aspx" Text="<%$Resources:wss,quiklnch_allcontent%>" accesskey="<%$Resources:wss,quiklnch_allcontent_AK%>"/></div>
					</SharePoint:SPSecurityTrimmedControl>
					</h3>
				</SharePoint:VersionedPlaceHolder>
				<Sharepoint:SPNavigationManager
				id="QuickLaunchNavigationManager"
				runat="server"
				QuickLaunchControlId="QuickLaunchMenu"
				ContainedControl="QuickLaunch"
				EnableViewState="false"
				CssClass="ms-quicklaunch-navmgr"
				>
				<div>
					<SharePoint:DelegateControl runat="server"
						ControlId="QuickLaunchDataSource">
					 <Template_Controls>
						<asp:SiteMapDataSource
						SiteMapProvider="SPNavigationProvider"
						ShowStartingNode="False"
						id="QuickLaunchSiteMap"
						StartingNodeUrl="sid:1025"
						runat="server"
						/>
					 </Template_Controls>
					</SharePoint:DelegateControl>
			<SharePoint:VersionedPlaceHolder UIVersion="3" runat="server">
				<SharePoint:AspMenu
					id="QuickLaunchMenu"
					runat="server"
					DataSourceId="QuickLaunchSiteMap"
					Orientation="Vertical"
					StaticDisplayLevels="2"
					ItemWrap="true"
					MaximumDynamicDisplayLevels="0"
					StaticSubMenuIndent="0"
					SkipLinkText=""
					CssClass="s4-die"
					>
					<LevelMenuItemStyles>
						<asp:MenuItemStyle CssClass="ms-navheader"/>
						<asp:MenuItemStyle CssClass="ms-navitem"/>
					</LevelMenuItemStyles>
					<LevelSubMenuStyles>
						<asp:SubMenuStyle CssClass="ms-navSubMenu1"/>
						<asp:SubMenuStyle CssClass="ms-navSubMenu2"/>
					</LevelSubMenuStyles>
					<LevelSelectedStyles>
						<asp:MenuItemStyle CssClass="ms-selectednavheader"/>
						<asp:MenuItemStyle CssClass="ms-selectednav"/>
					</LevelSelectedStyles>
				</SharePoint:AspMenu>
			</SharePoint:VersionedPlaceHolder>
			<SharePoint:VersionedPlaceHolder UIVersion="4" runat="server">
			  <SharePoint:AspMenu
				  id="V4QuickLaunchMenu"
				  runat="server"
				  EnableViewState="false"
				  DataSourceId="QuickLaunchSiteMap"
				  UseSimpleRendering="true"
				  Orientation="Vertical"
				  StaticDisplayLevels="2"
				  MaximumDynamicDisplayLevels="0"
				  SkipLinkText=""
				  CssClass="s4-ql" />
			</SharePoint:VersionedPlaceHolder>
				</div>
				</Sharepoint:SPNavigationManager>
			<Sharepoint:VersionedPlaceHolder runat="server" UIVersion="3">
				<Sharepoint:SPNavigationManager
				id="TreeViewNavigationManager"
				runat="server"
				ContainedControl="TreeView"
				>
				  <table class="ms-navSubMenu1" cellpadding="0" cellspacing="0" border="0">
					<tr>
					  <td>
						<table class="ms-navheader" width="100%" cellpadding="0" cellspacing="0" border="0">
						  <tr>
							<td nowrap="nowrap" id="idSiteHierarchy">
							  <SharePoint:SPLinkButton runat="server" NavigateUrl="~site/_layouts/viewlsts.aspx" id="idNavLinkSiteHierarchy" Text="<%$Resources:wss,treeview_header%>" accesskey="<%$Resources:wss,quiklnch_allcontent_AK%>"/>
							</td>
						  </tr>
						</table>
					  </td>
					</tr>
				  </table>
				  <div class="ms-treeviewouter">
					<SharePoint:DelegateControl runat="server" ControlId="TreeViewAndDataSource">
					  <Template_Controls>
						<SharePoint:SPHierarchyDataSourceControl
						 runat="server"
						 id="TreeViewDataSource"
						 RootContextObject="Web"
						 IncludeDiscussionFolders="true"
						/>
						<SharePoint:SPRememberScroll runat="server" id="TreeViewRememberScroll" onscroll="javascript:_spRecordScrollPositions(this);" style="overflow: auto;height: 400px;width: 150px; ">
						  <Sharepoint:SPTreeView
							id="WebTreeView"
							runat="server"
							ShowLines="false"
							DataSourceId="TreeViewDataSource"
							ExpandDepth="0"
							SelectedNodeStyle-CssClass="ms-tvselected"
							NodeStyle-CssClass="ms-navitem"
							NodeStyle-HorizontalPadding="2"
							SkipLinkText=""
							NodeIndent="12"
							ExpandImageUrl="/_layouts/images/tvplus.gif"
							CollapseImageUrl="/_layouts/images/tvminus.gif"
							NoExpandImageUrl="/_layouts/images/tvblank.gif"
						  >
						  </Sharepoint:SPTreeView>
						</Sharepoint:SPRememberScroll>
					  </Template_Controls>
					</SharePoint:DelegateControl>
				  </div>
				</Sharepoint:SPNavigationManager>
			</SharePoint:VersionedPlaceHolder>
			<Sharepoint:VersionedPlaceHolder runat="server" UIVersion="4">
				<Sharepoint:SPNavigationManager
				id="TreeViewNavigationManagerV4"
				runat="server"
				ContainedControl="TreeView"
				CssClass="s4-treeView"
				>
				  <SharePoint:SPLinkButton runat="server" NavigateUrl="~site/_layouts/viewlsts.aspx" id="idNavLinkSiteHierarchyV4" Text="<%$Resources:wss,treeview_header%>" accesskey="<%$Resources:wss,quiklnch_allcontent_AK%>" CssClass="s4-qlheader" />
					  <div class="ms-treeviewouter">
						<SharePoint:DelegateControl runat="server" ControlId="TreeViewAndDataSource">
						  <Template_Controls>
							<SharePoint:SPHierarchyDataSourceControl
							 runat="server"
							 id="TreeViewDataSourceV4"
							 RootContextObject="Web"
							 IncludeDiscussionFolders="true"
							/>
							<SharePoint:SPRememberScroll runat="server" id="TreeViewRememberScrollV4" onscroll="javascript:_spRecordScrollPositions(this);" style="overflow: auto;height: 400px;width: 155px; ">
							  <Sharepoint:SPTreeView
								id="WebTreeViewV4"
								runat="server"
								ShowLines="false"
								DataSourceId="TreeViewDataSourceV4"
								ExpandDepth="0"
								SelectedNodeStyle-CssClass="ms-tvselected"
								NodeStyle-CssClass="ms-navitem"
								SkipLinkText=""
								NodeIndent="12"
								ExpandImageUrl="/_layouts/images/tvclosed.png"
								ExpandImageUrlRtl="/_layouts/images/tvclosedrtl.png"
								CollapseImageUrl="/_layouts/images/tvopen.png"
								CollapseImageUrlRtl="/_layouts/images/tvopenrtl.png"
								NoExpandImageUrl="/_layouts/images/tvblank.gif"
							  >
							  </Sharepoint:SPTreeView>
							</Sharepoint:SPRememberScroll>
						  </Template_Controls>
						</SharePoint:DelegateControl>
					  </div>
				</Sharepoint:SPNavigationManager>
			</SharePoint:VersionedPlaceHolder>
				<SharePoint:VersionedPlaceHolder UIVersion="3" runat="server" id="PlaceHolderQuickLaunchBottomV3">
					<table width="100%" cellpadding="0" cellspacing="0" border="0" class="s4-die">
					<tr><td>
					<table class="ms-recyclebin" width="100%" cellpadding="0" cellspacing="0" border="0">
					<tr><td nowrap="nowrap">
					<SharePoint:SPLinkButton runat="server" NavigateUrl="~site/_layouts/recyclebin.aspx" id="v3idNavLinkRecycleBin" ImageUrl="/_layouts/images/recycbin.gif" Text="<%$Resources:wss,StsDefault_RecycleBin%>" PermissionsString="DeleteListItems" />
					</td></tr>
					</table>
					</td></tr>
					</table>
				</SharePoint:VersionedPlaceHolder>
				<SharePoint:VersionedPlaceHolder UIVersion="4" runat="server" id="PlaceHolderQuickLaunchBottomV4">
					<ul class="s4-specialNavLinkList">
						<li>
							<SharePoint:ClusteredSPLinkButton
								runat="server"
								NavigateUrl="~site/_layouts/recyclebin.aspx"
								ImageClass="s4-specialNavIcon"
								ImageUrl="/_layouts/images/fgimg.png"
								ImageWidth=16
								ImageHeight=16
								OffsetX=0
								OffsetY=405
								id="idNavLinkRecycleBin"
								Text="<%$Resources:wss,StsDefault_RecycleBin%>"
								CssClass="s4-rcycl"
								PermissionsString="DeleteListItems" />
						</li>
						<li>
							<SharePoint:ClusteredSPLinkButton
								id="idNavLinkViewAllV4"
								runat="server"
								PermissionsString="ViewFormPages"
								NavigateUrl="~site/_layouts/viewlsts.aspx"
								ImageClass="s4-specialNavIcon"
								ImageUrl="/_layouts/images/fgimg.png"
								ImageWidth=16
								ImageHeight=16
								OffsetX=0
								OffsetY=0
								Text="<%$Resources:wss,quiklnch_allcontent_short%>"
								accesskey="<%$Resources:wss,quiklnch_allcontent_AK%>"/>
						</li>
					</ul>
				</SharePoint:VersionedPlaceHolder>
				</div>
				</div>
	</ContentTemplate>
</SharePoint:UIVersionedContent>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
	<SharePoint:UIVersionedContent UIVersion="4" runat="server"><ContentTemplate>
		<SharePoint:CssRegistration Name="forms.css" runat="server"/>
	</ContentTemplate></SharePoint:UIVersionedContent>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderTitleLeftBorder" runat="server">
<table cellpadding="0" height="100%" width="100%" cellspacing="0">
 <tr><td class="ms-areaseparatorleft"><img src="/_layouts/images/blank.gif" width='1' height='1' alt="" /></td></tr>
</table>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderTitleAreaClass" runat="server">
<script type="text/javascript" id="onetidPageTitleAreaFrameScript">
	if (document.getElementById("onetidPageTitleAreaFrame") != null)
	{
		document.getElementById("onetidPageTitleAreaFrame").className="ms-areaseparator";
	}
</script>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderBodyAreaClass" runat="server">
<style type="text/css">
.ms-bodyareaframe {
	padding: 8px;
	border: none;
}
</style>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderBodyLeftBorder" runat="server">
<div class='ms-areaseparatorleft'><img src="/_layouts/images/blank.gif" width='8' height='100%' alt="" /></div>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderTitleRightMargin" runat="server">
<div class='ms-areaseparatorright'><img src="/_layouts/images/blank.gif" width='8' height='100%' alt="" /></div>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderBodyRightMargin" runat="server">
<div class='ms-areaseparatorright'><img src="/_layouts/images/blank.gif" width='8' height='100%' alt="" /></div>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderTitleAreaSeparator" runat="server"/>
<asp:Content ContentPlaceHolderId="PlaceHolderPageImage" runat="server">
	<img src="/_layouts/images/blank.gif" width='1' height='1' alt="" />
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderUtilityContent" runat="server">
<script language=javascript>
function ULSk4d(){var o=new Object;o.ULSTeamName="Windows SharePoint Services 4";o.ULSFileName="Upload.aspx";return o;}
var fCtl=false;
function EnsureUploadCtl()
{ULSk4d:;
	return browseris.ie5up && !browseris.mac &&
		null != document.getElementById("idUploadCtl");
}
function MultipleUploadView()
{ULSk4d:;
	if (EnsureUploadCtl())
	{
		treeColor = GetTreeColor();
		document.all.idUploadCtl.SetTreeViewColor(treeColor);
		if(!fCtl)
		{
			rowsArr = document.all.formTbl.rows;
			for(i=0; i < rowsArr.length; i++)
			{
				if ((rowsArr[i].id != "OverwriteField") &&
					(rowsArr[i].id != "trUploadCtl"))
				{
					rowsArr[i].removeNode(true);
					i=i-1;
				}
			}
			document.all.reqdFldTxt.removeNode(true);
			newCell = document.all.OverwriteField.insertCell();
			newCell.innerHTML = "ONET_NBSP";
			newCell.style.width="60%";
			document.all("dividMultipleView").style.display="inline";
			fCtl = true;
		}
	}
}
function RemoveMultipleUploadItems()
{ULSk4d:;
	if(browseris.nav || browseris.mac ||
		!EnsureUploadCtl()
	)
	{
		formTblObj = document.getElementById("formTbl");
		if(formTblObj)
		{
			rowsArr = formTblObj.rows;
			for(i=0; i < rowsArr.length; i++)
			{
				if (rowsArr[i].id == "trUploadCtl" || rowsArr[i].id == "diidIOUploadMultipleLink")
				{
					formTblObj.deleteRow(i);
				}
			}
		}
	}
}
function DocumentUpload()
{ULSk4d:;
	if (fCtl)
	{
		document.all.idUploadCtl.MultipleUpload();
	}
	else
	{
		ClickOnce();
	}
}
function GetTreeColor()
{ULSk4d:;
	var bkColor="";
	if(null != document.all("onetidNavBar"))
		bkColor = document.all.onetidNavBar.currentStyle.backgroundColor;
	if(bkColor=="")
	{
		numStyleSheets = document.styleSheets.length;
		for(i=numStyleSheets-1; i>=0; i--)
		{
			numRules = document.styleSheets(i).rules.length;
			for(ruleIndex=numRules-1; ruleIndex>=0; ruleIndex--)
			{
				if(document.styleSheets[i].rules.item(ruleIndex).selectorText==".ms-uploadcontrol")
					uploadRule = document.styleSheets[i].rules.item(ruleIndex);
			}
		}
		if(uploadRule)
			bkColor = uploadRule.style.backgroundColor;
	}
	return(bkColor);
}
</script>
<script type="text/javascript">
// <![CDATA[
	function _spBodyOnLoad()
	{ULSk4d:;
		var frm = document.forms[MSOWebPartPageFormName];
		frm.encoding="multipart/form-data";
	}
// ]]>
</script>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">
		<WebPartPages:WebPartZone runat="server" FrameType="None" ID="Main" Title="loc:Main" />
	<input type="hidden" name="VTI-GROUP" value="0"/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="<%$Resources:wss,upload_pagetitle_form%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	<SharePoint:ListProperty Property="LinkTitle" runat="server"/> : <SharePoint:EncodedLiteral runat="server" text="<%$Resources:wss,upload_pagetitle_form%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
