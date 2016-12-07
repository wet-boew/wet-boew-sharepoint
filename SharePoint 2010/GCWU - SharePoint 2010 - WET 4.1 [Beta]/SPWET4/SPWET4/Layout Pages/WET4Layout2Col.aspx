<%@ Page Language="C#" Inherits="SPWET4.Layout_Pages.WET4Layout2Col,SPWET4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e515e573c8cc4469" meta:progid="SharePoint.WebPartPage.Document" %>

<%@ Register TagPrefix="SharePointWebControls" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="PublishingNavigation" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WETCustomControls" Namespace="SPWET4.WebControls" Assembly="SPWET4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e515e573c8cc4469" %>

<%--
<WET4Changes>
    2014-11-26 The HTML tags in the "PlaceHolderLeftNavBar" and "PlaceHolderMain" sections were adjusted for WET4 - BARIBF
               PlaceHolderCLFCSS1 was deleted.  It will be managed in the Masterpage.
               PlaceHolderBodyLeftBorder was striped from it's empty DIV tag.
</WET4Changes>
--%>

<asp:content contentplaceholderid="PlaceHolderPageTitle" runat="server">
	<SharePointWebControls:FieldValue id="PageTitle" FieldName="Title" runat="server"/>
</asp:content>

<asp:content contentplaceholderid="PlaceHolderPageTitleInTitleArea" runat="server">
	<SharePointWebControls:FieldValue id="PageTitleInTitleArea" FieldName="Title" runat="server"/>
</asp:content>

<asp:content contentplaceholderid="PlaceHolderBodyLeftBorder" runat="server">
</asp:content>

<asp:content contentplaceholderid="PlaceHolderLeftNavBar" runat="server">
    <nav id="wb-sec" class="col-md-3 col-md-pull-9 visible-md visible-lg" typeof="SiteNavigationElement" role="navigation">
        <h2 id="wb-nav"><asp:Literal Text="<%$Resources:WET4, PrimaryNavigationHeaderText%>" runat="server" /></h2>
                    
        <WETCustomControls:WETLeftNavigation runat="server" />
    </nav>
 
</asp:content>

<asp:content contentplaceholderid="PlaceHolderMain" runat="server">
    <section>
        <WebPartPages:SPProxyWebPartManager runat="server" id="ProxyWebPartManager"></WebPartPages:SPProxyWebPartManager>
        <PublishingWebControls:RichHtmlField FieldName="PublishingPageContent" HasInitialFocus="True" MinimumEditHeight="400px" runat="server" PrefixStyleSheet="WET"></PublishingWebControls:RichHtmlField>
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
    </section>
</asp:content>

<asp:content contentplaceholderid="PlaceHolderAdditionalPageHead" runat="server"> 
	<WETCustomControls:PageMetadata runat="server"></WETCustomControls:PageMetadata>
    <style type="text/css">
        html.mediumview body header+.container,
        html.largeview body header+.container,
        html.xlargeview body header+.container {
            background-color: #fff;
            border-left: 1px solid #ccc;
            border-right: 1px solid #ccc;
        }
    </style>
</asp:content>

<asp:content contentplaceholderid="PlaceHolderLastModifiedDate" runat="server"><WETCustomControls:LastModifiedDate runat="server"></WETCustomControls:LastModifiedDate>
</asp:content>
