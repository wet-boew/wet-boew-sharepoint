<%@ Page language="C#"   Inherits="Microsoft.SharePoint.Publishing.PublishingLayoutPage,Microsoft.SharePoint.Publishing,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" meta:progid="SharePoint.WebPartPage.Document" %>
<%@ Register Tagprefix="SharePointWebControls" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="PublishingNavigation" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WETCustomControls" Namespace="WET.Theme.Intranet.WebControls" Assembly="WET.Theme.Intranet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=04a860f987069351" %>
<asp:Content ContentPlaceholderID="PlaceHolderCSS1" runat="server"> 
    <SharePointWebControls:UIVersionedContent UIVersion="4" runat="server">
		<ContentTemplate>
			<SharePointWebControls:CssRegistration name="<% $SPUrl:~sitecollection/Style Library/~language/Core Styles/page-layouts-21.css %>" runat="server"/>
			<PublishingWebControls:EditModePanel runat="server">
				<!-- Styles for edit mode only-->
				<SharePointWebControls:CssRegistration name="<% $SPUrl:~sitecollection/Style Library/~language/Core Styles/edit-mode-21.css %>"
					After="<% $SPUrl:~sitecollection/Style Library/~language/Core Styles/page-layouts-21.css %>" runat="server"/>
			</PublishingWebControls:EditModePanel>
		</ContentTemplate>
	</SharePointWebControls:UIVersionedContent>
	<SharePointWebControls:CssRegistration name="<% $SPUrl:~sitecollection/Style Library/~language/Core Styles/rca.css %>" runat="server"/>
	<SharePointWebControls:FieldValue id="PageStylesField" FieldName="HeaderStyleDefinitions" runat="server"/>
</asp:Content>

<asp:Content ContentPlaceholderID="PlaceHolderPageTitle" runat="server">
	<SharePointWebControls:FieldValue id="PageTitle" FieldName="Title" runat="server"/>
</asp:Content>

<asp:Content ContentPlaceholderID="PlaceHolderPageTitleInTitleArea" runat="server">
	<SharePointWebControls:UIVersionedContent UIVersion="4" runat="server">
		<ContentTemplate>
			<SharePointWebControls:FieldValue FieldName="Title" runat="server"/>
		</ContentTemplate>
	</SharePointWebControls:UIVersionedContent>
</asp:Content>

<asp:Content ContentPlaceholderID="PlaceHolderBodyLeftBorder" runat="server">
    <div id="wb-body-sec">
</asp:Content>

<asp:Content ContentPlaceholderID="PlaceHolderLeftNavBar" runat="server">
    <div id="wb-sec">
        <script type="text/javascript">
            $(document).ready(function () {
                $("#wb-sec").hide();
                var element = document.getElementById("s4-workspace");
                element.setAttribute("style", "height: auto !important;");
            });
     </script>
        <div id="wb-sec-in">
            <nav role="navigation" >
                <h2 id="wb-nav"></h2>
                <div class="wb-sec-def">
                    <!-- GC Web Usability theme begins / Début du thème de la facilité d'emploi GC -->
                    
                    <WETCustomControls:LeftNavigation runat="server" />
                    
                    <!-- GC Web Usability theme ends / Fin du thème de la facilité d'emploi GC -->
                </div>
            </nav>
        </div>
    </div>
</asp:Content>

<asp:Content ContentPlaceholderID="PlaceHolderMain" runat="server">
    <div id="cont" class="center">
        <div class="span-6">
            <PublishingWebControls:RichHtmlField FieldName="PublishingPageContent" HasInitialFocus="True" MinimumEditHeight="400px" runat="server" PrefixStyleSheet="wet-"></PublishingWebControls:RichHtmlField>
	        <PublishingWebControls:EditModePanel runat="server" CssClass="editmode-panel roll-up">
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
		    </PublishingWebControls:EditModePanel>
        </div>
        <div class="clear"></div>
    </div>
</asp:Content>

<asp:Content ContentPlaceholderID="PlaceHolderAdditionalPageHead" runat="server"> 
	<WETCustomControls:PageMetadata runat="server"></WETCustomControls:PageMetadata>
</asp:Content>

<asp:Content ContentPlaceholderID="PlaceHolderLastModifiedDate" runat="server"><WETCustomControls:LastModifiedDate runat="server"></WETCustomControls:LastModifiedDate>
</asp:Content>
