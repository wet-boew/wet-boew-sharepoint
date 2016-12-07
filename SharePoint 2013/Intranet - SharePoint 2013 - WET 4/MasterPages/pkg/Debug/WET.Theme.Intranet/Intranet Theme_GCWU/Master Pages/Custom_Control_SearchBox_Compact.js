/* Ce fichier est actuellement associÃƒÂ© ÃƒÂ  un fichier HTML du mÃƒÂªme nom, auprÃƒÂ¨s duquel il rÃƒÂ©cupÃƒÂ¨re du contenu. Tant que les fichiers ne sont pas dissociÃƒÂ©s, vous ne pouvez pas dÃƒÂ©placer, supprimer, renommer ou modifier ce fichier. */

function DisplayTemplate_e2e6c7bcac7347eab0c9564ce05651dd(ctx) {
    var ms_outHtml = [];
    var cachePreviousTemplateData = ctx['DisplayTemplateData'];
    ctx['DisplayTemplateData'] = new Object();
    DisplayTemplate_e2e6c7bcac7347eab0c9564ce05651dd.DisplayTemplateData = ctx['DisplayTemplateData'];

    ctx['DisplayTemplateData']['TemplateUrl'] = '~sitecollection\u002f_catalogs\u002fmasterpage\u002fDisplay Templates\u002fSearch\u002fCustom_Control_SearchBox_Compact.js';
    ctx['DisplayTemplateData']['TemplateType'] = 'Control';
    ctx['DisplayTemplateData']['TargetControlType'] = ['SearchBox'];
    this.DisplayTemplateData = ctx['DisplayTemplateData'];

    ms_outHtml.push('', ''
    );
    var showQuerySuggestions = ctx.ClientControl.get_showQuerySuggestions();
    var showNavigation = ctx.ClientControl.get_showNavigation();

    var prompt = ctx.ClientControl.get_initialPrompt();
    if ($isNull(prompt)) {
        prompt = Srch.Res.sb_Prompt;
    }

    var searchBoxDivId = ctx.ClientControl.get_id() + "_sboxdiv";
    var searchBoxId = ctx.ClientControl.get_id() + "_sbox";
    var navButtonId = ctx.ClientControl.get_id() + "_NavButton";
    var suggestionsListId = ctx.ClientControl.get_id() + "_AutoCompList";
    var navListId = ctx.ClientControl.get_id() + "_NavDropdownList";
    var searchBoxLinkId = ctx.ClientControl.get_id() + "_SearchLink";
    var searchBoxProgressClass = "ms-srch-sbprogress";
    var searchBoxPromptClass = "ms-srch-sb-prompt ms-helperText";

    ctx.OnPostRender = function (rCtx) {
        ctx.ClientControl.activate(
            prompt,
            searchBoxId,
            searchBoxDivId,
            navButtonId,
            suggestionsListId,
            navListId,
            searchBoxLinkId,
            searchBoxProgressClass,
            searchBoxPromptClass);
    }
    ms_outHtml.push(''
    , '        <div id="wet-srchbx SearchBox" name="Control">'
    , '            <div class="wet-srchbx-in ms-srch-sb ms-srch-sb-border" id="', $htmlEncode(searchBoxDivId), '">'
    , '                <input type="text" value="', $htmlEncode(ctx.ClientControl.get_currentTerm()), '" maxlength="2048" class="ms-textSmall wet-srch" accessKey="', $htmlEncode(Srch.Res.sb_AccessKey), '" title="', $htmlEncode(prompt), '" id="', $htmlEncode(searchBoxId), '" autocomplete="off" autocorrect="off" onkeypress="if (Srch.U.isEnterKey(String.fromCharCode(event.keyCode))) { $getClientControl(this).search(this.value);return Srch.U.cancelEvent(event); }" onkeydown="var ctl = $getClientControl(this);ctl.activateDefaultQuerySuggestionBehavior();" onfocus="var ctl = $getClientControl(this);ctl.hidePrompt();ctl.setBorder(true);" onblur="var ctl = $getClientControl(this);ctl.showPrompt();ctl.setBorder(false);" />'
    );
    var imagesUrl = GetThemedImageUrl('searchresultui.png');
    if (showNavigation) {
        ms_outHtml.push(''
        , '                    <a class="ms-srch-sb-navLink" title="', $htmlEncode(Srch.Res.sb_GoNav), '" id="', $htmlEncode(navButtonId), '" onclick="$getClientControl(this).activateDefaultNavigationBehavior();return Srch.U.cancelEvent(event);" href="javascript: {}">'
        , '                        <img src="', $urlHtmlEncode(imagesUrl), '" class="ms-srch-sb-navImg" id="navImg" alt="', $htmlEncode(Srch.Res.sb_GoNav), '" />'
        , '                    </a>'
        );
    }
    ms_outHtml.push(''
    , '				<a title="', $htmlEncode(Srch.Res.sb_GoSearch), '" class="" id="', $htmlEncode(searchBoxLinkId), '" onclick="$getClientControl(this).search($get(\'', $scriptEncode(searchBoxId), '\').value);" href="javascript: {}">'
    , '					<input id="wet-srch-submit" name="wet-srch-submit" type="button" value="Search" data-icon="search" class="button button-accent" /> '
    , '                </a>'
    );
    if (showQuerySuggestions) {
        ms_outHtml.push(''
        , '                <div class="ms-qSuggest-container ms-shadow" id="AutoCompContainer">'
        , '                    <div id="', $htmlEncode(suggestionsListId), '"></div>'
        , '                </div>'
        );
    }

    if (showNavigation) {
        ms_outHtml.push(''
        , '                <div class="ms-qSuggest-container ms-shadow" id="NavDropdownListContainer">'
        , '                    <div id="', $htmlEncode(navListId), '"></div>'
        , '                </div>'
        );
    }
    ms_outHtml.push(''
    , '            </div>'
    );
    if (ctx.ClientControl.get_showAdvancedLink()) {
        var advancedUrl = ctx.ClientControl.get_advancedSearchPageAddress();
        if (!$isEmptyString(advancedUrl)) {
            ms_outHtml.push(''
            , '                    <div class="ms-srch-sb-link"><a id="AdvancedLink" href="', $urlHtmlEncode(advancedUrl), '">', $htmlEncode(Srch.Res.sb_AdvancedLink), '</a></div>'
            );
        }
    }
    if (ctx.ClientControl.get_showPreferencesLink()) {
        var preferencesUrl = ctx.ScriptApplicationManager.get_preferencesUrl();
        if (!$isEmptyString(preferencesUrl)) {
            ms_outHtml.push(''
            , '                    <div class="ms-srch-sb-link"><a id="PreferencesLink" href="', $urlHtmlEncode(preferencesUrl), '">', $htmlEncode(Srch.Res.sb_PreferencesLink), '</a></div>'
            );
        }
    }
    ms_outHtml.push(''
    , '        </div>'
    , '    '
    );

    ctx['DisplayTemplateData'] = cachePreviousTemplateData;
    return ms_outHtml.join('');
}
function RegisterTemplate_e2e6c7bcac7347eab0c9564ce05651dd() {

    if ("undefined" != typeof (Srch) && "undefined" != typeof (Srch.U) && typeof (Srch.U.registerRenderTemplateByName) == "function") {
        Srch.U.registerRenderTemplateByName("Control_SearchBox_Compact", DisplayTemplate_e2e6c7bcac7347eab0c9564ce05651dd);
    }

    if ("undefined" != typeof (Srch) && "undefined" != typeof (Srch.U) && typeof (Srch.U.registerRenderTemplateByName) == "function") {
        Srch.U.registerRenderTemplateByName("~sitecollection\u002f_catalogs\u002fmasterpage\u002fDisplay Templates\u002fSearch\u002fCustom_Control_SearchBox_Compact.js", DisplayTemplate_e2e6c7bcac7347eab0c9564ce05651dd);
    }

}
RegisterTemplate_e2e6c7bcac7347eab0c9564ce05651dd();
if (typeof (RegisterModuleInit) == "function" && typeof (Srch.U.replaceUrlTokens) == "function") {
    RegisterModuleInit(Srch.U.replaceUrlTokens("~sitecollection\u002f_catalogs\u002fmasterpage\u002fDisplay Templates\u002fSearch\u002fCustom_Control_SearchBox_Compact.js"), RegisterTemplate_e2e6c7bcac7347eab0c9564ce05651dd);
}