<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SliderWebPartUserControl.ascx.cs" Inherits="LAC.SharePoint.Slider.SliderWebPart.SliderWebPartUserControl" %>

<SharePoint:CssRegistration ID="CssRegistration1" name="/Style Library/CSS Style Sheets/SliderWebPart.css" After="corev4.css" runat="server"/>
<div class="span-4" style="width: 99%;">		
    <div id="sliderWPTabs" class="wet-boew-tabbedinterface tabs-style-3 cycle swpTabs" runat="server">
		<ul class="tabs swpTabsList">
            <asp:Literal ID="litTabs" runat="server" />
		</ul>
		<div class="tabs-panel swpTabsPanel">
            <asp:Literal ID="litPanels" runat="server" />
		</div>
    </div>
    <div class="clear"></div>
</div>
