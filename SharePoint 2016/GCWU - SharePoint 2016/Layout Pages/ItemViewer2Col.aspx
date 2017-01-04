<%@ Page Language="C#" Inherits="Microsoft.SharePoint.Publishing.PublishingLayoutPage,Microsoft.SharePoint.Publishing,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" meta:progid="SharePoint.WebPartPage.Document" %>
<%@ Register TagPrefix="SharePointWebControls" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="PublishingNavigation" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WETCustomControls" Namespace="SPWET4.WebControls" Assembly="SPWET4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e515e573c8cc4469" %>

<%--
<WET4Changes>
    2014-11-26 The HTML tags in the "PlaceHolderLeftNavBar" and "PlaceHolderMain" sections were adjusted for WET4
               PlaceHolderCLFCSS1 was deleted.  It will be managed in the Masterpage.
               PlaceHolderBodyLeftBorder was striped from it's empty DIV tag.
               - BARIBF
</WET4Changes>
--%>

<asp:Content ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    <asp:Literal runat="server" ID="litTitle" />
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderBodyLeftBorder" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderLeftNavBar" runat="server">
    <nav id="wb-sec" class="col-md-3 col-md-pull-9 visible-md visible-lg" typeof="SiteNavigationElement" role="navigation">
        <h2><asp:Literal Text="<%$Resources:WET4, PrimaryNavigationHeaderText%>" runat="server" /></h2>
                    
        <WETCustomControls:WETLeftNavigation runat="server" />
    </nav>
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <section>
            <WebPartPages:SPProxyWebPartManager runat="server" ID="ProxyWebPartManager"></WebPartPages:SPProxyWebPartManager>
            <PublishingWebControls:RichHtmlField FieldName="PublishingPageContent" HasInitialFocus="True" MinimumEditHeight="400px" runat="server"></PublishingWebControls:RichHtmlField>
    </section>
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <WETCustomControls:PageMetadata runat="server"></WETCustomControls:PageMetadata>
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderLastModifiedDate" runat="server">
    <WETCustomControls:LastModifiedDate runat="server"></WETCustomControls:LastModifiedDate>
</asp:Content>

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