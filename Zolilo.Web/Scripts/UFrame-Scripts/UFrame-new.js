/*
UFrame 1.0.0
Modified version for Zolilo.com

License:
>Copyright (C) 2008 Omar AL Zabir - http://msmvps.com/blogs/omar
>	
>Permission is hereby granted, free of charge,
>to any person obtaining a copy of this software and associated
>documentation files (the "Software"),
>to deal in the Software without restriction,
>including without limitation the rights to use, copy, modify, merge,
>publish, distribute, sublicense, and/or sell copies of the Software,
>and to permit persons to whom the Software is furnished to do so,
>subject to the following conditions:
>
>The above copyright notice and this permission notice shall be included
>in all copies or substantial portions of the Software.
>
>THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
>INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
>FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
>IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
>DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
>ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
>OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 
Usage:

UFrame.init({
id: "iPanel1",  // id of the DIV
		
loadFrom: "somePage.aspx",  // Initial page to load from
initialLoad : "GET",    // Initial load mode
showProgress : true,    // Whether to show the progressTemplate
		
beforeLoad: function(url,data) { callback() },  // callback fired before content is loaded from loadFrom
afterLoad: function(data, response) { callback() }, // callback fired after response is loaded from loadFrom
beforePost: function(url,data) { callback() },  // callback fired before content is posted
afterPost: function(data, response) { callback() }, // callback fired after content is posted and response is available
		
params : { "Gaga" : "gugu" },   // parameters to post/get to 
		
progressTemplate : "<p>Loading...<p>",  // template shown when content is being loaded from or posted to
		
beforeBodyTemplate : "<p>This is rendered before the body</p>", // added before any response body
afterBodyTemplate : "<p>This is rendered after the body</p>",   // added after any response body
		
});
*/



(function ()
{

    UFrame = function (config)
    {
        this.url = "";
        this.config = config;
    };

    UFrame.prototype = {
        load: function ()
        {
            var c = this.config;
            if (c.loadFrom)
            {
                c.loadFrom = this.addQueryStringData(c.loadFrom);
                currentUFrame = c.id;
                UFrameManager.loadHtml(c.loadFrom, c.params, c);
                window.addEventListener("popstate", popStateHandler, true);
            }
        },

        //Adds necessary data to the query string for ajax request
        addQueryStringData: function (href)
        {
            var c = this.config;

            href = c.supervisor.addQueryStringKey(href, "uframe", c.id);
            href = c.supervisor.addQueryStringKey(href, "instance", c.supervisor.sessionkey);
            href = c.supervisor.addQueryStringKey(href, "redirect", "n");
            href = c.supervisor.addQueryStringKey(href, "supervisor", c.supervisor.objectidsuffix);

            return href;
        },

        submit: function (form)
        {
            UFrameManager.submitForm(form, null, config.id);
        },
        navigate: function (href, pushstate)
        {
            
            var c = this.config;
            currentUFrame = c.id;
            if (pushstate === undefined || pushstate)
            {
                window.history.pushState(null, "Zolilo.com", c.supervisor.createLinkableURL(href));
            }
            href = this.addQueryStringData(href);
            UFrameManager.loadHtml(href, null, this.config);
        },
    }

    UFrameManager =
{
    _uFrames: {},

    empty: function () { },

    init: function (config)
    {
        var o = new UFrame(config);
        UFrameManager._uFrames[config.id] = o;
        o.load();
    },
    getHtml: function (id, url, queryString, callback, method)
    {
        try
        {
            $.ajax({
                url: url,
                type: method || "GET",
                data: queryString,
                dataType: "html",
                success: callback,
                error: function (xml, status, e)
                {
                    //throw e;
                    //alert("Error occured while loading: " + url +'\n' + xml.status + ":" + xml.statusText + '\n' + xml.responseText); 
                    if (xml && xml.responseText)
                        callback(xml.responseText);
                    else
                        alert("Error occured while loading: " + url + '\n' + e.message);
                },
                cache: true
            });
        } catch (e)
        {
            alert(e);
            //throw e;
        }
    },

    getUFrame: function (id)
    {
        return UFrameManager._uFrames[id];
    },

    submitForm: function (form, submitData, uframeId)
    {
        var params = {};
        var uframe = UFrameManager.getUFrame(uframeId);
        var config = uframe.config;
        var container = $('#' + config.id);

        if (submitData)
            params[submitData.name] = submitData.value;
        var url = UFrameManager.resolveUrlZoliloPOST(this.url);

        if ((config.beforeLoad || UFrameManager.empty)(url, submitData) !== false)
        {
            if (config.progressTemplate) container.html(config.progressTemplate);

            UFrameManager.getHtml(config.id, url, submitData, function (data)
            {
                config.loadFrom = url;
                config.loadFrom = config.supervisor.addQueryStringKey(config.loadFrom, "uframe", config.id);
                (config.afterLoad || UFrameManager.empty)(url, data);
                UFrameManager.processHtml(data, container, config);
            }, "POST");
        }
    },

    loadHtml: function (url, params, config)
    {
        console.log("UFrameManager.loadHtml: " + url);
        config.loadFrom = url;
        var container = $('#' + config.id);
        var queryString = $.param(params || {});
        if ((config.beforeLoad || UFrameManager.empty)(url, params) !== false)
        {
            if (url.indexOf("&uframe=") < 0 && url.indexOf("?uframe=") < 0)
            {
                if (url.indexOf("?") > 0)
                    config.loadFrom = url + "&uframe=" + config.id;
                else
                    config.loadFrom = url + "?uframe=" + config.id;
            }
            this.url = url;
            var uframe = this.getUFrame(config.id);
            uframe.url = url;
           
            UFrameManager.getHtml(config.id, url, queryString, function (content)
            {
                (config.afterLoad || UFrameManager.empty)(url, content);
                UFrameManager.processHtml(content, container, config);
                Loading(false);
            });
        }
        else
        {
            Loading(false);
        }

    },

    processHtml: function (content, container, config)
    {
        window['test5'] = content;
        var result = UFrameManager.parseHtml(content, config);

        var head = document.getElementsByTagName('head')[0];
        window['test6'] = result;
        $(result.styles).each(function (index, text)
        {
            var styleNode = document.createElement("style");
            styleNode.setAttribute("type", "text/css");
            if (styleNode.styleSheet) // IE
            {
                styleNode.styleSheet.cssText = text;
            }
            else // w3c
            {
                var cssText = document.createTextNode(text);
                styleNode.appendChild(cssText);
            }

            head.appendChild(styleNode);
        });

        $(result.links).each(function (index, attrs)
        {
            window.setTimeout(function ()
            {
                var link = document.createElement('link');
                var href = "";
                for (var i = 0; i < attrs.length; i++)
                {
                    var attr = attrs[i];
                    if (attr.name == "href")
                    {
                        href = UFrameManager.resolveUrlNormalize(attr.value);
                    }

                    link.setAttribute("" + attr.name, "" + attr.value);
                }
                
                if (href.length > 0)
                {
                    link.href = UFrameManager.resolveUrlZoliloPOST(href); //////
                    if (!UFrameManager.isTagLoaded('link', 'href', link.href))
                        head.appendChild(link);
                }
            }, 0);
        });

        var scriptsToLoad = result.externalScripts.length;

        $(result.externalScripts).each(function (index, scriptSrc)
        {
            scriptSrc = UFrameManager.resolveUrlNormalize(scriptSrc);
            scriptSrc = UFrameManager.resolveUrlZoliloPOST(scriptSrc);

            if (UFrameManager.isTagLoaded('script', 'src', scriptSrc))
            {
                scriptsToLoad--;
            }
            else
            {
                $.ajax({
                    url: scriptSrc,
                    type: "GET",
                    data: null,
                    dataType: "script",
                    success: function () { scriptsToLoad--; },
                    error: function () { scriptsToLoad--; },
                    cache: true
                });
            }
        });

        // wait until all the external scripts are downloaded
        UFrameManager.until({
            test: function () { return scriptsToLoad === 0; },
            delay: 100,
            callback: function ()
            {
                // render the body
                var html = (config.beforeBodyTemplate || "") + result.body + (config.afterBodyTemplate || "");
                container.html(html);

                //window.setTimeout(alert('timeout'), 100);
               
                window['test16'] = window.setTimeout(function ()
                {
                    //alert("timeout");
                    // execute all inline scripts 
                    $(result.inlineScripts).each(function (index, script)
                    {
                        $.globalEval(script);
                    });

                    UFrameManager.hook(container, config);

                    if (typeof callback == "function") callback();
                }, 100);
                
            }
        });
    },

    isTagLoaded: function (tagName, attName, value)
    {
        // Create a temporary tag to see what value browser eventually 
        // gives to the attribute after doing necessary encoding
        var tag = document.createElement(tagName);
        tag[attName] = value;
        var tagFound = false;
        $(tagName, document).each(function (index, t)
        {
            if (tag[attName] === t[attName]) { tagFound = true; return false }
        });
        return tagFound;
    },

    hook: function (container, config)
    {
        // Add an onclick event on all <a> 
        
        $("select", container)
            .unbind("change")
            .change(function ()
            {
                return UFrameManager.submitInput(this);
            })
            .each(function ()
            {
                this.onchange = null;
            });
    },


    executeASPNETPostback: function (target, argument, href, old__doPostBack)
    {
        alert("executeASPNETPostback (" + target + "," + argument + "," + href + ")");
        
        var uframeid = GetContainingUFrame(target);
        var form = elem.parent;
        var uframe = UFrameManager.getUFrame(uframeid);
        href = uframe.config.loadFrom;

        var form = $("#" + uframeid).parents("form").get(0);
        form.__EVENTTARGET.value = unescape(target);
        form.__EVENTARGUMENT.value = unescape(argument);
        var formdata = GetAllHiddenFields(form);
        UFrameManager.submitForm(form, formdata, uframeid);
        return true;
    },

 
    submitInput: function (input)
    {
        var form = input.form;

        if (form.onsubmit && form.onsubmit() == false)
        {
            alert("submitInput:break");
            return false;
        }

        input = $(input);
        UFrameManager.submitForm(form, { name: input.attr("name"), value: input.attr("value") }, GetContainingUFrame(input.attr("name")));

        return false;
    },

    until: function (o /* o = { test: function(){...}, delay:100, callback: function(){...} } */)
    {
        if (o.test() === true) o.callback();
        else window.setTimeout(function () { UFrameManager.until(o); }, o.delay || 100);
    },

    delay: function (func, delay)
    {
        window.setTimeout(func, delay || 100);
    },

    resolveUrl: function (baseUrl, relativeUrl)
    {
        //alert("resolveURL: baseUrl=" + baseurl + "; relativeUrl=" + relativeUrl + ")");
        // Hack for firefox, as Firefox make any relative URL absolute by just adding the current document's absolute path
        // to the URL
        var retval = "nothing";
        var currentPageUrl = document.location.protocol + "//" + document.location.host + document.location.pathname;

        if (relativeUrl.indexOf(currentPageUrl) == 0) relativeUrl = relativeUrl.substring(currentPageUrl.length);

        // if it's an absolute URL, return as it is
        if (relativeUrl.indexOf("http://") >= 0)
        {
            retval = relativeUrl;
        }
        // If URL starts with root, then return it as it is
        else if (relativeUrl.indexOf("/") == 0)
        {
            retval = relativeUrl;
        }
        if (retval != "")
        {
            var lastSeparator = baseUrl.lastIndexOf("/");
            if (lastSeparator < 0)
            {
                retval = relativeUrl;
            }
            else
            {
                retval = baseUrl.substring(0, lastSeparator) + "/" + relativeUrl;
            }
        }
        if (retval.indexOf("//") == 0)
            retval = retval.substring(1, retval.length);
        return retval;



    },

    resolveUrlNormalize: function (href)
    {
        while (href.indexOf("../") >= 0)
        {
            href = href.replace("../", "");
        }
        if (href.length > 0)
        {
            if ((href.indexOf("http://") < 0 && href.indexOf("https://") < 0) && href.substring(0, 1) != "/")
            {
                href = "/" + href;
            }
        }
        return href;
    },

    resolveUrlZoliloPOST: function (pagePath)
    {
        // Hack for Zolilo.com, as http POST with submit forms will not work with default UFrame behavior
        // pagePath = /path/to/document.htm

        //if it's an absolute URL, return as it is
        if (pagePath.indexOf("http://") >= 0) return pagePath;
        var currentPageUrl = document.location.protocol + "//" + document.location.host + pagePath;
        return currentPageUrl;
    },

    parseHtml: function (content)
    {
        var result = { body: "", externalScripts: [], inlineScripts: [], links: [], styles: [] };

        var bodyContent = [];
        var bodyStarted = false;

        var inlineScriptStarted = false;
        var inlineScriptContent = [];

        var inlineStyleStarted = false;
        var inlineStyleContent = [];

        HTMLParser(content, {
            start: function (tag, attrs, unary)
            {
                if (tag == "body")
                {
                    bodyStarted = true;
                }
                else if (tag == "script")
                {
                    var srcFound = false;
                    $(attrs).each(function (index, attr)
                    {
                        if (attr.name == "src")
                        {
                            result.externalScripts.push(attr.value);
                            srcFound = true;
                        }
                    });
                    if (!srcFound)
                    {
                        // inline script
                        inlineScriptStarted = true;
                        inlineScriptContent = [];
                    }
                }
                else if (tag == "link")
                {
                    result.links.push(attrs);
                }
                else if (tag == "style")
                {
                    // inline style node
                    inlineStyleStarted = true;
                    inlineStyleContent = [];
                }
                else
                {
                    if (bodyStarted)
                    {
                        var attributes = [];
                        for (var i = 0; i < attrs.length; i++)
                            attributes.push(attrs[i].name + '="' + attrs[i].value + '"');

                        bodyContent.push("<" + tag + " " + attributes.join(" ") + (unary ? "/" : "") + ">");
                    }
                }
            },
            end: function (tag)
            {
                if (tag == "script")
                {
                    if (inlineScriptStarted)
                    {
                    /*
                        if (!window['testattrs'])
                            window['testattrs'] = {};
                        window['testattrs'][tag] = tag;
                        window['testattrs'][tag]["content"] = inlineScriptContent;
                        alert("UFrame-new.parseHtml.HTMLParser.start(" + tag + ", " + inlineScriptContent + ");");
                        */
                        inlineScriptStarted = false;
                        result.inlineScripts.push(inlineScriptContent.join("\n"));
                    }
                }
                else if (tag == "style")
                {
                    inlineStyleStarted = false;
                    result.styles.push(inlineStyleContent.join("\n"));
                }
                else
                {
                    if (bodyStarted)
                        bodyContent.push("</" + tag + ">");
                }
            },
            chars: function (text)
            {
                if (inlineScriptStarted)
                    inlineScriptContent.push(text);
                else if (inlineStyleStarted)
                    inlineStyleContent.push(text);
                else if (bodyStarted)
                    bodyContent.push(text);
            },
            comment: function (text)
            {
            }
        });

        result.body = bodyContent.join("");
        return result;
    },
    initContainers: function ()
    {
        $('div[src]', document).each(function ()
        {
            var container = $(this);
            var id = container.attr("id");
            if (null == UFrameManager._uFrames[id])
            {
                UFrameManager.init({
                    id: id,
                    supervisor: _zSupervisor.getSupervisor(),// eval(document.getElementsByName("zolilohf_supervisorid")[0].value),
                    loadFrom: container.attr("src"),
                    initialLoad: "GET",

                    progressTemplate: container.attr("progressTemplate") || null,

                    showProgress: container.attr("showProgress") || false,

                    beforeLoad: function (url, data) { return eval(container.attr("beforeLoad") || "true") },
                    afterLoad: function (data, response) { return eval(container.attr("afterLoad") || "true") },
                    beforePost: function (url, data) { return eval(container.attr("beforePost") || "true") },
                    afterPost: function (data, response) { return eval(container.attr("afterPost") || "true") },

                    params: null,

                    beforeBodyTemplate: container.attr("beforeBodyTemplate") || null,
                    afterBodyTemplate: container.attr("afterBodyTemplate") || null
                });
                // alert("InitContainers: " + container.attr("src"));
            }
        });
    }
}

    $(function ()
    {
        UFrameManager.initContainers();
    });

})();