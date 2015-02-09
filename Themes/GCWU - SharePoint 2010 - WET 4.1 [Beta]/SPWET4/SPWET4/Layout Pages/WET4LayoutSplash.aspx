<%@ Page language="C#"   Inherits="Microsoft.SharePoint.Publishing.PublishingLayoutPage,Microsoft.SharePoint.Publishing,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" meta:progid="SharePoint.WebPartPage.Document" %>
<%@ Register Tagprefix="SharePointWebControls" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="PublishingNavigation" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WETCustomControls" Namespace="SPWET4.WebControls" Assembly="SPWET4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e515e573c8cc4469" %>

<%--
<WET4Changes>
    2014-11-26 The HTML tags in the "PlaceHolderMain" section were adjusted for WET4;
               PlaceHolderWETCSS1 was deleted.  It will be managed in the Masterpage.
               PlaceHolderBodyLeftBorder was striped from it's empty DIV tag.
               - BARIBF
</WET4Changes>
--%>

<asp:Content ContentPlaceholderID="PlaceHolderPageTitle" runat="server">
	<SharePointWebControls:FieldValue id="PageTitle" FieldName="Title" runat="server"/>
</asp:Content>

<asp:Content ContentPlaceholderID="PlaceHolderPageTitleInTitleArea" runat="server">
	<h1 id="wb-cont"><SharePointWebControls:FieldValue id="PageTitleInTitleArea" FieldName="Title" runat="server"/></h1>
</asp:Content>

<asp:Content ContentPlaceholderID="PlaceHolderBodyLeftBorder" runat="server">
</asp:Content>

<asp:Content ContentPlaceholderID="PlaceHolderLeftNavBar" runat="server" />

<asp:Content ContentPlaceholderID="PlaceHolderMain" runat="server">
    <section>
	    <PublishingWebControls:editmodepanel runat="server" id="editmodepanel1">
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
			</table>
		</PublishingWebControls:editmodepanel>
    </section>
</asp:Content>

<asp:Content ContentPlaceholderID="PlaceHolderAdditionalPageHead" runat="server"> 
	<WETCustomControls:PageMetadata runat="server"></WETCustomControls:PageMetadata>
</asp:Content>
