<%@ Page language="C#"   Inherits="Microsoft.SharePoint.Publishing.PublishingLayoutPage,Microsoft.SharePoint.Publishing,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" meta:progid="SharePoint.WebPartPage.Document" %>
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
	<asp:Literal runat="server" Id="litTitle" />
</asp:Content>

<asp:Content ContentPlaceholderID="PlaceHolderPageTitleInTitleArea" runat="server">
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
            <PublishingWebControls:RichHtmlField FieldName="PublishingPageContent" HasInitialFocus="True" MinimumEditHeight="400px" runat="server"></PublishingWebControls:RichHtmlField>	        
        </div>
        <div class="clear"></div>
    </div>
</asp:Content>

<asp:Content ContentPlaceholderID="PlaceHolderAdditionalPageHead" runat="server"> 
	<CLFCustomControls:PageMetadata runat="server"></CLFCustomControls:PageMetadata>
</asp:Content>

<asp:Content ContentPlaceholderID="PlaceHolderLastModifiedDate" runat="server"><CLFCustomControls:LastModifiedDate runat="server"></CLFCustomControls:LastModifiedDate></asp:Content>

<script runat="server">
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        string itemUrl = Page.Request.QueryString["ItemUrl"];
        if (itemUrl != null)
        {
            // Nik20121107 - the current item comes from an External List (BCS)
            int start = itemUrl.ToLower().IndexOf("listid={") + 8;
            int end = itemUrl.ToLower().IndexOf("}", start);
            string listId = itemUrl.Substring(start, end - start);

            start = itemUrl.ToLower().IndexOf("&id=", 0) + 4;
            end = itemUrl.ToLower().IndexOf("&", start);

            if (end <= 0)
                end = itemUrl.Length;
            string itemId = itemUrl.Substring(start, end - start);
            Microsoft.SharePoint.SPListItem item = null;
            using (Microsoft.SharePoint.SPSite site = new Microsoft.SharePoint.SPSite(itemUrl))
            {
                using (Microsoft.SharePoint.SPWeb web = site.OpenWeb())
                {
                    Microsoft.SharePoint.SPList curList = web.Lists[new Guid(listId)];
                    foreach (Microsoft.SharePoint.SPListItem curItem in curList.Items)
                    {
                        if (curItem.Fields.TryGetFieldByStaticName("BdcIdentity") != null && curItem["BdcIdentity"].ToString() == itemId)
                            item = curItem;
                        else if (curItem.ID.ToString() == itemId)
                            item = curItem;
                    }
                    litTitle.Text = item.Title;
                }
            }
        }
    }

    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);
        
    }
    
    
</script>
