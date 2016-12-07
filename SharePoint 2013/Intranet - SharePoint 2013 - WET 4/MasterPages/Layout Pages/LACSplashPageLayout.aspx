<%@ Page language="C#"   Inherits="Microsoft.SharePoint.Publishing.PublishingLayoutPage,Microsoft.SharePoint.Publishing,Version=15.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" meta:webpartpageexpansion="full" meta:progid="SharePoint.WebPartPage.Document" %>
<%@ Register Tagprefix="SharePointWebControls" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="PublishingNavigation" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ContentPlaceholderID="PlaceHolderAdditionalPageHead" runat="server">
	<SharePointWebControls:CssRegistration ID="CssRegistration1" name="<% $SPUrl:~sitecollection/Style Library/~language/Themable/Core Styles/pagelayouts15.css %>" runat="server"/>
	<PublishingWebControls:EditModePanel ID="EditModePanel1" runat="server">
		<!-- Styles for edit mode only-->
		<SharePointWebControls:CssRegistration ID="CssRegistration2" name="<% $SPUrl:~sitecollection/Style Library/~language/Themable/Core Styles/editmode15.css %>"
			After="<% $SPUrl:~sitecollection/Style Library/~language/Themable/Core Styles/pagelayouts15.css %>" runat="server"/>
	</PublishingWebControls:EditModePanel>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">



<PublishingWebControls:EditModePanel ID="EditModePanel2" runat=server PageDisplayMode="Display">
    <div class="row mrgn-tp-lg"> 
	<div class="col-md-12"> 
		<section class="col-md-6"> 
			<h2 class="h3 text-center"><SharePointWebControls:TextField ID="EnglishTitle" FieldName="f5520ac9-9ff4-4f00-9280-642519919e6e" runat="server"></SharePointWebControls:TextField></h2> 
			<div class="btn btn-lg btn-primary btn-block lacButtonEn"><SharePointWebControls:UrlField ID="EnglishHyperlink" FieldName="88340333-972A-4473-8217-CA1FDD10E0F4" runat="server"></SharePointWebControls:UrlField></div> 
			
		</section> 
		<section class="col-md-6" lang="fr"> 
			<h2 class="h3 text-center"><SharePointWebControls:TextField ID="FrenchTitle" FieldName="1AA2AAFB-B7BF-4C9B-AC30-5C818DFC4002" runat="server"></SharePointWebControls:TextField></h2> 
			<div class="btn btn-lg btn-primary btn-block lacButtonFr"><SharePointWebControls:UrlField ID="FrenchHyperlink" FieldName="1840147B-C4C6-407B-917E-F04036804AE0" runat="server"></SharePointWebControls:UrlField></div> 
		</section> 
	</div> 
</div> 

</PublishingWebControls:EditModePanel>

<PublishingWebControls:EditModePanel ID="EditModePanel3" runat=server PageDisplayMode="Edit">
  <SharePointWebControls:TextField ID="TextField2" FieldName="f5520ac9-9ff4-4f00-9280-642519919e6e" runat="server"></SharePointWebControls:TextField></br>
  <SharePointWebControls:UrlField ID="UrlField1" FieldName="88340333-972A-4473-8217-CA1FDD10E0F4" runat="server"></SharePointWebControls:UrlField></br>
  <SharePointWebControls:TextField ID="TextField1" FieldName="1AA2AAFB-B7BF-4C9B-AC30-5C818DFC4002" runat="server"></SharePointWebControls:TextField></br>
  <SharePointWebControls:UrlField ID="UrlField2" FieldName="1840147B-C4C6-407B-917E-F04036804AE0" runat="server"></SharePointWebControls:UrlField></br>

</PublishingWebControls:EditModePanel>



</asp:Content>
