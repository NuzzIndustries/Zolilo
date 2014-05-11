//zolilo.supervisor.js
//Designates a "supervisor" model of the web page, which consists of one "master" html page,
//and any number of "widgets" that are loaded dynamically via ajax html and managed by the supervisor
//One of these will only load once per full refresh, and the page will attempt to load it in the scope of Home.aspx

_zSupervisor = new zSupervisorStatic();
_zSupervisor.getSupervisor();

function zSupervisorStatic()
{
    this.getSupervisor = function ()
    {
        var id = document.getElementById("zolilohf_supervisorid").value + "_obj";
        if (!window[id])
            window[id] = new ZoliloSupervisor(id);
        //alert("initSupervisor: " + window[id].objectid);
        return window[id];
    };

}

function ZoliloSupervisor(objectid)
{
    this.objectid = objectid;
    this.objectidsuffix = objectid.substring("zSupervisor".length, objectid.length - "_obj".length);
    this.sessionkey = document.getElementById("zolilohf_sessionkey").value;
    this.supervisorURL = document.getElementById("zolilohf_supervisorurl").value;
    this.pageToLoad = null;
    this.frameToLoad = null;

    //Runs repeatedly on a specified interval, this is needed in order to facilitate the loading div
    this.supervisorLoop = function ()
    {
        var s = this;
        if (s.pageToLoad)
        {
            s.loadPage(s.pageToLoad, s.frameToLoad);
            s.pageToLoad = null;
            s.frameToLoad = null;
        }
        if (s.formSubmit)
        {
            s.formSubmit = false;
            var UFrameID = GetContainingUFrame(s.formTarget);
            var UFrame = UFrameManager.getUFrame(UFrameID);
            var url = UFrame.url;
            var form = GetFormByName("MainForm");
            executeASPNETPostback(s.formTarget, s.formAction, url, s.internalpostback);
            Loading(false);
        }
    };
    window.setInterval("_zSupervisor.getSupervisor().supervisorLoop()", 50);


    //Fixes a query string, to make it into a valid string for the HTTP request
    fixQueryStringURL = function (href)
    {
        //If first query string key is not referenced with ?, make it a ?
        if (href.indexOf("?") < 0)
        {
            var index = href.indexOf("&")
            if (index > 0)
                href = href.substring(0, index) + "?" + href.substring(index + 1)
        }
        return href;
    };

    //Adds a query string key to the url. Overwrites the existing key if it exists
    this.addQueryStringKey = function (href, key, value)
    {
        var indexOfBase = href.indexOf("?");

        if (href.substring(href.length - 1) == "/")
            href = href.substring(0, href.length - 1);

        var index = href.indexOf("&" + key + "=");
        var index2 = href.indexOf("?" + key + "=");

        //alert("addQueryStringKey1: " + href + ",key:" + key + ",value:" + value + ",index:" + index + ",index2:" + index2 + ",indexOfBase:" + indexOfBase);
        if (index < 0 && index2 < 0)
        {
            if (href.indexOf("?") < 0)
                href += "?" + key + "=" + value;
            else
                href += "&" + key + "=" + value;
        }
        else //a key already exists
        {

            var indexOfKey;
            if (index >= 0)
                indexOfKey = index;
            if (index2 >= 0)
                indexOfKey = index2;
            //alert("indexofkey: " + indexOfKey);
            var indexOfNextKey = href.indexOf("&", indexOfKey + 1);
            //alert("indexofnextkey: " + indexOfNextKey);
            if (indexOfNextKey < 0) //Replaced key was last key in the list
                href = href.substring(0, indexOfKey - 1);
            else
                href = href.substring(0, indexOfKey) + href.substring(indexOfNextKey); //remove key from string

            href = fixQueryStringURL(href);

            return _zSupervisor.getSupervisor().addQueryStringKey(href, key, value);
            //alert("addQueryStringKey2: " + href);
            //return eval(objectid + unescape(".addQueryStringKey(href, key, value)"));
            //return href;
        }
        return href;
    };

    this.removeQueryString = function (href, key)
    {
        var index = href.indexOf("&" + key + "=");
        var index2 = href.indexOf("?" + key + "=");

        if (index < 0 && index2 < 0)
        {
            //No key found to remove
            return href;
        }

        //Get start index
        index = Math.max(index, index2);

        //Get end index
        index2 = href.indexOf("&", index + 1);

        if (index2 < 0)
        {
            //Index being removed was last index
            href = href.substring(0, index);
            //alert("zolilo.supervisor.removeQueryString(1): " + href);
            return href;
        }
        else
        {
            //Index being removed was not last index
            href = href.substring(0, index) + href.substring(index2);
            href = fixQueryStringURL(href);
            //alert("zolilo.supervisor.removeQueryString(2): " + href);
            return href;
        }
    };

    //Creates a user-friendly url by removing unnecessary query strings
    this.createLinkableURL = function (href)
    {
        var _this = _zSupervisor.getSupervisor();

        href = _this.removeQueryString(href, 'instance');
        href = _this.removeQueryString(href, 'uframe');
        href = _this.removeQueryString(href, 'supervisor');
        href = _this.removeQueryString(href, 'redirect');

        return href;
    };



    this.hookLinks = function (id)
    {
        //alert("hooklinks(" + id + ")");
        var allA = $('a');
        var anchors = $('a[href$=\'#\']');
        var allNonAnchor = allA.not(anchors);

        allNonAnchor.unbind("click");
        allNonAnchor.bind('click', function ()
        {
            var href = $(this).attr("href");
            return _zSupervisor.getSupervisor().doLinkMain(href, id);
        });
    };

    this.doLinkMain = function (href, id)
    {
        if (href == "#")
            return true;
        if (href.indexOf('javascript:') > 0)
            return true;
        Loading(true);
        var s = _zSupervisor.getSupervisor();
        s.pageToLoad = href;
        s.frameToLoad = id;
        return false;
    };

    this.loadPage = function(href, id)
    {
        if (href)
        {
            // if href is an absolute URL to outside domain, leave it
            if ((href.indexOf("http://") >= 0 || href.indexOf("https://") >= 0) && href.indexOf(document.location.host) < 0)
            {
                return true;
            }
            else
            {
                if (href.indexOf('javascript:') !== 0)
                {
                    var supervisor = _zSupervisor.getSupervisor();
                    var uframe = UFrameManager.getUFrame(id);
                    href = uframe.addQueryStringData(href);
                    doLink(href, id);
                    return false;
                }
                if (UFrameManager.executeASPNETPostback(this, href))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        else
        {
            return true;
        }
    };
}



