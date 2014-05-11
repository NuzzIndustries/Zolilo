using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zolilo.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Text;

namespace Zolilo.Web
{
    internal class WebDirector
    {
        RouteManager routeManager;

        internal WebDirector()
        {
            routeManager = new RouteManager();
        }

        internal void Initialize()
        {
            routeManager.Initialize();
        }

        public void RedirectBack(string defaultURL)
        {
            if (zContext.Frame.SavedURL != null)
                Redirect(zContext.Frame.SavedURL);
            else
                Redirect(defaultURL);
        }

        internal void Redirect(string url)
        {
            Redirect(url, true);
        }

        private void Redirect(string url, bool allowFrameTransfer)
        {
            ValidateURL(url);

            if (IsURLInternal(url) && zContext.Page.IsUFrame)
            {
                RedirectPageNormal(url, zContext.Page, allowFrameTransfer);
            }
            else
                HttpContext.Current.Response.Redirect(url, true); //Do a redirect *instead of* transfer
        }

        internal void RedirectToNode(GraphNode node)
        {
            if (node is DR_Goals)
                Redirect("/goals/view?id=" + node.ID.ToString());
            else if (node is DR_Fragments)
                Redirect("/fragments/view?id=" + node.ID.ToString());
            else throw new NotImplementedException();
          
        }

        //Performs crude, not perfect validation on a URL to help force standardization of URL syntax for processing
        private void ValidateURL(string url)
        {
            if (url.Substring(0, 4) == "http") //Fine if it contains http
                return;
            if (url[0] == '~')
                throw new InvalidOperationException("Virtual paths not allowed in this context. URL: " + url);
            if (url[0] == '/') //Internal url
                return;
            throw new InvalidOperationException("URL must be one of the following: " + Environment.NewLine +
                "External URL starting with http" + Environment.NewLine +
                "Absolute internal path (/path/to/page)" + Environment.NewLine +
                "Offending URL: " + url);
        }

        private bool IsURLInternal(string url)
        {
            return (url[0] == '/' || url.Contains("://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"]));
        }

        /// <summary>
        /// Performs an internal page transfer, without spawning a full refresh
        /// </summary>
        /// <param name="url"></param>
        private void RedirectPageNormal(string url, ZoliloPage page, bool allowFrameTransfer)
        {
            if (!page.IsUFrame)
            {
                if (page.UFrameID != null && page.UFrameID.Length > 0)
                {
                    url = AddQueryString(url, "uframe", page.UFrameID);
                }
                if (page.InstanceID != null && page.InstanceID.Length > 0)
                {
                    url = AddQueryString(url, "instance", page.InstanceID);
                }
                if (page.SupervisorID > 0)
                {
                    url = AddQueryString(url, "supervisor", page.SupervisorID.ToString());
                }
                if (page.Request.QueryString["redirect"] == "n")
                {
                    url = AddQueryString(url, "redirect", "n");
                }
                HttpContext.Current.Response.Redirect(url, true);
            }
            else if (!allowFrameTransfer)
            {
                string requesturl = HttpContext.Current.Request.Url.AbsoluteUri;
                url = requesturl.Substring(0, requesturl.IndexOf(url)) + url;
                
                HtmlForm form = (HtmlForm)zContext.Page.Master.FindControl("MainForm");
                form.Attributes.Add("style", "visibility: hidden; display:none");
                
                page.ClientScript.RegisterStartupScript(page.GetType(), "redirect",
                    "<script type=\"text/javascript\">location.href = '" + url + "';</script>");
            }
            else
            {
                HtmlForm form = (HtmlForm)zContext.Page.Master.FindControl("MainForm");
                form.Attributes.Add("style", "visibility: hidden; display:none");
                page.ClientScript.RegisterStartupScript(page.GetType(), "redirect",
                    "<script type=\"text/javascript\">_zSupervisor.getSupervisor().doLinkMain('" + url + "','" + page.UFrameID + "');</script>");
            }
            page.CancelLoading = true;
        }

        internal string AddQueryString(string url, string key, string value)
        {
            if (!url.Contains("?" + key + "=") && !url.Contains("&" + key + "="))
            {
                if (url.Contains("?"))
                    url = url + "&" + key + "=" + value;
                else
                    url = url + "?" + key + "=" + value;
            }
            return url;
        }

        internal string RemoveQueryString(string url, string key)
        {
            int index = url.IndexOf("&" + key + "=");
            int index2 = url.IndexOf("?" + key + "=");

            if (index < 0 && index2 < 0)
                return url;

            //Get start index
            index = Math.Max(index, index2);

            //Get end index
            index2 = url.IndexOf("&", index + 1);

            if (index2 < 0)
            {
                //Index being removed was last index
                url = url.Substring(0, index);
                return url;
            }
            else
            {
                //Index being removed was not last index
                url = url.Substring(0, index) + url.Substring(index2);
                url = FixQueryStringURL(url);
                return url;
            }
        }

        /// <summary>
        /// Fixes a query string, to make it into a valid string for the HTTP request
        /// </summary>
        string FixQueryStringURL(string url)
        {
            if (url.IndexOf("?") < 0)
            {
                int index = url.IndexOf("&");
                if (index > 0)
                    url = url.Substring(0, index) + "?" + url.Substring(index + 1);
            }
            return url;
        }
        
        /// <summary>
        /// Performs an internal page transfer, while doing a full refresh via Base64 state parameter to spawn the UI
        /// </summary>
        /// <param name="url"></param>
        internal void TransferPageFullRefresh(string url)
        {
            if (url == "/home" || url.Contains("Pages/Home.aspx")) //will do a full reload if it is intended to go directly home
            {
                url = "/browse";
                HttpContext.Current.Response.Redirect(url);
                //_Zolilo.Session.PageHandle.Response.Redirect("/home");
                return;
            }
            HttpContext.Current.Server.Transfer(GetBatchURL(url));
            //SERVER TRANSFER TRANSFERS WITHIN THE CONTEXT OF THE INTERNAL FRAME AND NOT THE ENTIRE PAGE
            //_Zolilo.Session.PageHandle.Request.RequestContext.HttpContext.RewritePath(url2);
        }

        /// <summary>
        /// Gets a URL which accesses the requested page within the main form context
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        internal string GetBatchURL(string url)
        {
            return "/Pages/Home.aspx?x=" + GetBase64("loadpage " + url) + "&redirect=n";
        }


        internal string GetBase64(string command)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(command));
        }

        internal static WebDirector Instance
        {
            get { return zContext.System.WebDirector; }
        }

        internal RouteManager RouteManager
        {
            get { return RouteManager; }
        }


        /// <summary>
        /// Redirects the page, ignoring any supervisor/frame relations
        /// </summary>
        /// <param name="p"></param>
        internal void RedirectEscapeFrame(string url)
        {
            url = RemoveQueryString(url, "uframe");
            url = RemoveQueryString(url, "supervisor");
            url = RemoveQueryString(url, "instance");
            url = RemoveQueryString(url, "redirect");
            Redirect(url, false);
        }
    }
}