checkLeftMenuLinks();
function checkLeftMenuLinks() {
    var wb = document.getElementsByClassName("wb-sec-def")[0];

    var childs = wb.getElementsByTagName("a");
    for (var i = 0; i < childs.length; i++) {
        if (endsWith(window.location.toString().toLowerCase(), childs[i].href.toString().toLowerCase())) {
            childs[i].className = "nav-current";
        }
        else {
            if (getDepth(childs[i]) == 17)
                childs[i].className = "nav-normal-2nd";
            else if (getDepth(childs[i]) == 16)
                childs[i].className = "nav-normal-1st";
        }
    }
}

function endsWith(str, suffix) {
    return str.indexOf(suffix, str.length - suffix.length) !== -1;
}

function getDepth(elem) {
    var i = 0;
    while (elem.parentNode != null) {
        i++;
        elem = elem.parentNode;
    }
    return i;
}
