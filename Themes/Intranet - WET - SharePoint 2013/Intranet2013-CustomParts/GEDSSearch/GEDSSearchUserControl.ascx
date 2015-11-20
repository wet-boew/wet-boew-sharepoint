<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GEDSSearchUserControl.ascx.cs" Inherits="Intranet2013_CustomParts.GEDSSearch.GEDSSearchUserControl" %>
<link rel="stylesheet" type="text/css" href="/Style Library/SideImageLink/SideImageLink.css"/>

<div id="gedstitle" class="GEDSTitleRow" runat="server" onclick="location.href='http://geds20-sage20.ssc-spc.gc.ca/en/GEDS20/'" style="cursor:pointer;"><h3 runat="server" id="GEDSh3" class="background-accent margin-bottom-medium\" style="width:100% !important; vertical-align:middle;"> replacethis</h3></div>
<br /> <br />
    <div class="gedsGrid"  >
        <div class="gedsDataCol"  >
            <asp:TextBox ID="Name" runat="server" OnTextChanged="Name_TextChanged" Height="30"></asp:TextBox ><br />
            <asp:RadioButton ID="Internal" runat="server" value="Internal" OnCheckedChanged="GEDSRadio_CheckedChanged" GroupName="GEDSRadio" Text="" Checked="True" /><br />
            <asp:RadioButton ID="External" runat="server" value="Exteral" OnCheckedChanged="GEDSRadio_CheckedChanged" GroupName="GEDSRadio" text=""/><br />
        </div><div id="go" class="gedsButtonCol"  ><asp:Button ID="btnGEDS" runat="server" Text="Button" OnClick="btnGEDS_Click" height="30"/> </div>
    </div>
    <!--<div class="GedsUpdate" >
        <a runat="server" id="updateLink" href="#"> Test </a>
    </div>-->
