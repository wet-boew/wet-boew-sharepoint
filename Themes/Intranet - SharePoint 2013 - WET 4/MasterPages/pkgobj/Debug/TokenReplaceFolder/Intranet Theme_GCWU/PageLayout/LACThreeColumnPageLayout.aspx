<%@ Page language="C#"   Inherits="Microsoft.SharePoint.Publishing.PublishingLayoutPage,Microsoft.SharePoint.Publishing,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" meta:progid="SharePoint.WebPartPage.Document" %>
<%@ Register Tagprefix="SharePointWebControls" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="PublishingNavigation" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>  
<%@ Register Tagprefix="otechCustomControls" Namespace="WET.Theme.Intranet.WebControls" Assembly="WET.Theme.Intranet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=04a860f987069351" %>
  

<asp:Content ContentPlaceholderID="PlaceHolderPageTitleInTitleArea" runat="server">  
              
                          <div id="wb-main" role="main"> 
                            <div id="wb-main-in">
                                <!-- Content title begins / Début du titre du contenu -->
                                <div id="wb-cont">
                                               
                                               <h1><SharePointWebControls:FieldValue id="FieldValue1" FieldName="Title" runat="server"/></h1>
                                   
                                </div>
                                <!-- Content Title ends / Fin du titre du contenu -->
</asp:Content>                          
                                
 <asp:Content ContentPlaceholderID="PlaceHolderMain" runat="server"> 
       <PublishingWebControls:EditModePanel runat="server" PageDisplayMode="Display">                              
                                <!-- s4-ca is the main body div -->
                                <!-- GC Web Usability theme begins / Début du thème de la facilité d'emploi GC -->
                                <div class="LACContentPage span-6">
	                                    <div class="LACThreeMid">
		                                    <PublishingWebControls:RichHtmlField ID="RichHtmlField5" FieldName="LACPageContent" HasInitialFocus="True" MinimumEditHeight="400px" runat="server"/>
	                                    </div>

	                                <div class="LACThreeRight span-2">
		                                <div class="LACThreeRightBox"><PublishingWebControls:RichHtmlField ID="RichHtmlField6" FieldName="PublishingPageContent" HasInitialFocus="True" MinimumEditHeight="400px" runat="server"/></div>
	                                </div>

                                </div>
                                <!-- Date Modified begins / Début de la date de modification -->
                                <div style="float:right;"><%#SPUtility.GetLocalizedString("WET", "TopOfPage", 1033)%>&nbsp;&nbsp;</div>
                                
                                <!-- Print this page script. If JS is disable, then it won't show up. -->                                
                                <script type="text/javascript">
                                    var curUrl = document.URL;
                                    var lblPrint = "Print";
                                    if (curUrl.indexOf("/fra/") != -1)
                                        lblPrint = "Imprimer";
                                    var contentToShow = "<ul><li style='margin-left:10px;bottom:0px;left:0px;position:absolute;float:left; list-style-image:none; list-style-position:inside;list-style-type:none;margin-bottom:0px; margin-right:0px; margin-top:0px;line-height:1em; !important;'><a title='" + lblPrint + "' onclick='javascript:window.print();return false;' href='#' rel='alternate' style='padding-left:17px;text-decoration:none; color:#666 !important;line-height 1em !important;background-image: url(\"/_layouts/15/WET.Theme.Intranet/GC-SharePoint/Images/printbutton.gif\");background-position-x:left; background-position-y:1px;background-repeat:no-repeat;display:block;'>" + lblPrint + "</a></li></ul>";
                                    document.write(contentToShow);
                                </script>                                

                                <dl id="gcwu-date-mod" role="contentinfo">
                                    <dt><%#SPUtility.GetLocalizedString("WET", "DateModifiedText",1033)%></dt>
                                    <dd>
                                        <span><time><otechCustomControls:LastModifiedDate runat="server"></otechCustomControls:LastModifiedDate></time></span>
                                    </dd>
                                </dl>
                                <div class="clear"></div>
                                <!-- Date Modified ends / Fin de la date de modification -->
                                <!-- GC Web Usability theme ends / Fin du thème de la facilité d'emploi GC -->
                            </div><!-- cn-centre-col-inner -->
                        </div>
           </div>
                        <div id="wb-sec">
                            <div id="wb-sec-in">
                                <nav role="navigation">
                                    <h2 id="wb-nav"></h2>
                                    <div class="wb-sec-def">
                                        <otechCustomControls:LeftNavigation runat="server"></otechCustomControls:LeftNavigation>
                                    </div>
                                </nav>
                            </div>
                        </div>

</PublishingWebControls:EditModePanel>



     <PublishingWebControls:EditModePanel runat="server" PageDisplayMode="Edit">                              
                                <!-- s4-ca is the main body div -->
                                <!-- GC Web Usability theme begins / Début du thème de la facilité d'emploi GC -->
                                <div class="LACContentPage span-6">
	                                    <div class="LACThreeMid">
		                                    <PublishingWebControls:RichHtmlField ID="RichHtmlField1" FieldName="LACPageContent" HasInitialFocus="True" MinimumEditHeight="400px" runat="server"/>
	                                    </div>

	                                    <div class="LACThreeRight span-2">
		                                    <div class="LACThreeRightBox"><PublishingWebControls:RichHtmlField ID="RichHtmlField2" FieldName="PublishingPageContent" HasInitialFocus="True" MinimumEditHeight="400px" runat="server"/></div>
	                                    </div>

                                </div>
                                <!-- Date Modified begins / Début de la date de modification -->
                                <div style="float:right;"><%#SPUtility.GetLocalizedString("WET", "TopOfPage", 1033)%>&nbsp;&nbsp;</div>
                                
                                <!-- Print this page script. If JS is disable, then it won't show up. -->                                
                                <script type="text/javascript">
                                    var curUrl = document.URL;
                                    var lblPrint = "Print";
                                    if (curUrl.indexOf("/fra/") != -1)
                                        lblPrint = "Imprimer";
                                    var contentToShow = "<ul><li style='margin-left:10px;bottom:0px;left:0px;position:absolute;float:left; list-style-image:none; list-style-position:inside;list-style-type:none;margin-bottom:0px; margin-right:0px; margin-top:0px;line-height:1em; !important;'><a title='" + lblPrint + "' onclick='javascript:window.print();return false;' href='#' rel='alternate' style='padding-left:17px;text-decoration:none; color:#666 !important;line-height 1em !important;background-image: url(\"/_layouts/15/WET.Theme.Intranet/GC-SharePoint/Images/printbutton.gif\");background-position-x:left; background-position-y:1px;background-repeat:no-repeat;display:block;'>" + lblPrint + "</a></li></ul>";
                                    document.write(contentToShow);
                                </script>                                

                                <dl id="gcwu-date-mod" role="contentinfo">
                                    <dt><%#SPUtility.GetLocalizedString("WET", "DateModifiedText",1033)%></dt>
                                    <dd>
                                        <span><time><otechCustomControls:LastModifiedDate runat="server"></otechCustomControls:LastModifiedDate></time></span>
                                    </dd>
                                </dl>
                                <div class="clear"></div>
                                <!-- Date Modified ends / Fin de la date de modification -->
                                <!-- GC Web Usability theme ends / Fin du thème de la facilité d'emploi GC -->
                            </div><!-- cn-centre-col-inner -->
                        </div>
        </div>
                        <div id="wb-sec">
                            <div id="wb-sec-in">
                                <nav role="navigation">
                                    <h2 id="wb-nav"></h2>
                                    <div class="wb-sec-def">
                                        <otechCustomControls:LeftNavigation runat="server"></otechCustomControls:LeftNavigation>
                                    </div>
                                </nav>
                            </div>
                        </div>

</PublishingWebControls:EditModePanel>

<PublishingWebControls:EditModePanel runat="server" PageDisplayMode="Edit">
<div class="LACContentPage span-6">
	<div class="LACThreeMid">
		<PublishingWebControls:RichHtmlField ID="RichHtmlField3" FieldName="LACPageContent" HasInitialFocus="True" MinimumEditHeight="400px" runat="server"/>
	</div>

	<div class="LACThreeRight span-2">
		<div class="LACThreeRightBox"><PublishingWebControls:RichHtmlField ID="RichHtmlField4" FieldName="PublishingPageContent" HasInitialFocus="True" MinimumEditHeight="400px" runat="server"/></div>
	</div>

</div>
</PublishingWebControls:EditModePanel>

</asp:Content>




  

 




