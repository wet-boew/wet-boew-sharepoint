<%@ Page language="C#"   Inherits="Microsoft.SharePoint.Publishing.PublishingLayoutPage,Microsoft.SharePoint.Publishing,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" meta:progid="SharePoint.WebPartPage.Document" %>
<%@ Register Tagprefix="SharePointWebControls" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="PublishingNavigation" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="CustomControls" Namespace="WET.Theme.Intranet.WebControls" Assembly="WET.Theme.Intranet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=04a860f987069351" %>
<asp:Content ContentPlaceholderID="PlaceHolderPageTitle" runat="server">
    <SharePointWebControls:FieldValue id="PageTitle" FieldName="Title" runat="server"/>
</asp:Content>
<asp:Content ContentPlaceholderID="PlaceHolderMain" runat="server">
	<div>
        <div class="span-4">
        	<WebPartPages:WebPartZone runat="server" Title="Top Left" ID="TopLeft"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone>
        </div>
        <div class="span-2">
        	<WebPartPages:WebPartZone runat="server" Title="Top Right" ID="TopRight"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone>
        </div>
    </div>
    <div>
        <div class="span-4">
        	<WebPartPages:WebPartZone runat="server" Title="Mid Left" ID="MidLeft"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone>
        </div>
        <div class="span-2">
        	<WebPartPages:WebPartZone runat="server" Title="Mid Right" ID="MidRight"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone>
        </div>
    </div>

    <div>
        <div class="span-4">

        </div>
        <div class="span-2">
        	<WebPartPages:WebPartZone runat="server" Title="Bot Right" ID="BotRight"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone>
        </div>
    </div>
</asp:Content>
