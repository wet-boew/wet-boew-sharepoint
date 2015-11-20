<%@ Page language="C#"   Inherits="Microsoft.SharePoint.Publishing.PublishingLayoutPage,Microsoft.SharePoint.Publishing,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" meta:progid="SharePoint.WebPartPage.Document" %>
<%@ Register Tagprefix="SharePointWebControls" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="PublishingNavigation" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>  


<asp:Content ContentPlaceholderID="PlaceHolderPageTitle" runat="server">

    <SharePointWebControls:FieldValue id="PageTitle" FieldName="Title" runat="server"/>
</asp:Content>
<asp:Content ContentPlaceholderID="PlaceHolderMain" runat="server">
    <div id="cont" class="span-8">
        <script type="text/javascript">
            $(document).ready(function () {
                $("#wb-sec").hide();
                var elem = document.getElementById("wb-main-in");
                elem.setAttribute("style", "margin-left: -300px;");
                
            });
  </script>


        <div class="span-5">
            <WebPartPages:WebPartZone runat="server" id="WebPartZoneTopLeft" Title="Top Left">
            </WebPartPages:WebPartZone>
        </div>
        <div class="span-2 float-right ">
            <WebPartPages:WebPartZone runat="server" id="WebPartZoneTopRight" Title="Top Right">
            </WebPartPages:WebPartZone>
        </div>
        <div class="span-5">
            <WebPartPages:WebPartZone runat="server" id="WebPartZoneLeftColumn" Title="Left Column">
            </WebPartPages:WebPartZone>
        </div>
        <div class="span-2 float-right">
            <WebPartPages:WebPartZone runat="server" id="WebPartZoneCenterColumn" Title="Center Column">
            </WebPartPages:WebPartZone>
             <WebPartPages:WebPartZone runat="server" id="WebPartZoneRightColumn" Title="Right Column">
            </WebPartPages:WebPartZone>
        </div>
    <div class="clear"></div>
    </div></asp:Content> 
