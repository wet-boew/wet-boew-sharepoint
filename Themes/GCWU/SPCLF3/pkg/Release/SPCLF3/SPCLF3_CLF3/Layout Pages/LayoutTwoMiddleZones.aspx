<%@ Page language="C#"   Inherits="Microsoft.SharePoint.Publishing.PublishingLayoutPage,Microsoft.SharePoint.Publishing,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" meta:progid="SharePoint.WebPartPage.Document" meta:webpartpageexpansion="full" %>
<%@ Register Tagprefix="SharePointWebControls" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="PublishingNavigation" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="CLFCustomControls" Namespace="SPCLF3.WebControls" Assembly="SPCLF3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=04a860f987069351" %>
<asp:Content ContentPlaceholderID="PlaceHolderCLFCSS1" runat="server"> 
	<SharePointWebControls:UIVersionedContent ID="UIVersionedContent2" UIVersion="4" runat="server">
		<ContentTemplate>
			<SharePointWebControls:CssRegistration name="<% $SPUrl:~sitecollection/Style Library/~language/Core Styles/page-layouts-21.css %>" runat="server"/>
			<PublishingWebControls:EditModePanel ID="EditModePanel1" runat="server">
				<!-- Styles for edit mode only-->
				<SharePointWebControls:CssRegistration ID="CssRegistration2" name="<% $SPUrl:~sitecollection/Style Library/~language/Core Styles/edit-mode-21.css %>"
					After="<% $SPUrl:~sitecollection/Style Library/~language/Core Styles/page-layouts-21.css %>" runat="server"/>
			</PublishingWebControls:EditModePanel>
		</ContentTemplate>
	</SharePointWebControls:UIVersionedContent>
</asp:Content>

<asp:Content ContentPlaceholderID="PlaceHolderPageTitle" runat="server">
	<SharePointWebControls:FieldValue id="PageTitle" FieldName="Title" runat="server"/>
</asp:Content>

<asp:Content ContentPlaceholderID="PlaceHolderPageTitleInTitleArea" runat="server">
	<SharePointWebControls:FieldValue id="PageTitleInTitleArea" FieldName="Title" runat="server"/>
</asp:Content>

<asp:Content ContentPlaceholderID="PlaceHolderBodyLeftBorder" runat="server">
	<div id="wb-body-sec">
</asp:Content>

<asp:Content ContentPlaceholderID="PlaceHolderLeftNavBar" runat="server">
	<div id="wb-sec">
        <div id="wb-sec-in">
            <nav role="navigation" >
                <h2 id="wb-nav"><asp:Literal Text="<%$Resources:CLF3, PrimaryNavigationHeaderText%>" runat="server" /></h2>
                <div class="wb-sec-def">
                    <!-- GC Web Usability theme begins / Début du thème de la facilité d'emploi GC -->
                    
                    <CLFCustomControls:CLFLeftNavigation runat="server" />
                    
                    <!-- GC Web Usability theme ends / Fin du thème de la facilité d'emploi GC -->
                </div>
            </nav>
        </div>
    </div>
</asp:Content>

<asp:Content ContentPlaceholderID="PlaceHolderMain" runat="server">
	<div id="cont" class="center">
        <div class="span-6">
            <WebPartPages:SPProxyWebPartManager runat="server" id="ProxyWebPartManager"></WebPartPages:SPProxyWebPartManager>
            
            <WebPartPages:WebPartZone runat="server" ID="TopZone"
	                Title="Top Zone"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone>

            <div class="span-3 row-start">
            	<WebPartPages:WebPartZone runat="server" ID="MiddleSidebarZone"
	                Title="Middle Left Zone"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone>
            </div>
            
            <div class="span-3 row-end">
            	<WebPartPages:WebPartZone runat="server" ID="MiddleContentZone"
	                Title="Middle Right Zone"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone>
            </div>

            <div class="clear"></div>
            
            <PublishingWebControls:RichHtmlField FieldName="PublishingPageContent" HasInitialFocus="True" MinimumEditHeight="400px" runat="server"></PublishingWebControls:RichHtmlField>
	        <PublishingWebControls:editmodepanel runat="server" id="editmodepanel1">
			    <!-- Add field controls here to bind custom metadata viewable and editable in edit mode only.-->
			    <table width="100%" cellpadding="10" cellspacing="0" class="editModePanel">
				    <tr>
					    <td>
						    <SharePointWebControls:TextField runat="server" id="TitleField" FieldName="Title" DisplaySize="100" />
					    </td>
				    </tr>
				    <tr>
				        <td>
				            <SharePointWebControls:NoteField FieldName="dc.description" DisplaySize="100" runat="server"></SharePointWebControls:NoteField>
				        </td>
			        </tr>
			        <tr>
				        <td>
				            <SharePointWebControls:NoteField FieldName="meta_keywords" DisplaySize="100" runat="server"></SharePointWebControls:NoteField>
				        </td>
			        </tr>
			        <tr>
				        <td>
				            <SharePointWebControls:NoteField FieldName="dc.subject" DisplaySize="100" runat="server"></SharePointWebControls:NoteField>
				        </td>
			        </tr>
                    <!-- BEGIN - Custom Metadata Fields Below here -->
				
                    <!-- END - Custom Metadata Fields Below here -->
			    </table>
		    </PublishingWebControls:editmodepanel>
        </div>
        <div class="clear"></div>
    </div>
</asp:Content>

<asp:Content ContentPlaceholderID="PlaceHolderAdditionalPageHead" runat="server"> 
	<CLFCustomControls:PageMetadata runat="server"></CLFCustomControls:PageMetadata>
</asp:Content>

<asp:Content ContentPlaceholderID="PlaceHolderLastModifiedDate" runat="server">
	<CLFCustomControls:LastModifiedDate runat="server"></CLFCustomControls:LastModifiedDate></asp:Content>
