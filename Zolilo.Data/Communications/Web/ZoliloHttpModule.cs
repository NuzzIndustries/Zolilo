using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace Zolilo
{
    public class ZoliloHttpModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication application)
        {
            
            application.BeginRequest += new EventHandler(application_BeginRequest);
            application.EndRequest += new EventHandler(application_EndRequest);
        }

        void application_EndRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;
            //context.Response.Write("<hr><h1><font color=red>HelloWorldModule: End of Request</font></h1>");
        }

        void application_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;
            HttpRequest Request = context.Request;
            string method = Request.ServerVariables["REQUEST_METHOD"];

            if (method == "POST")
            {
                string url = Request.ServerVariables["URL"];
            }

            //context.Response.Write("<h1><font color=red>HelloWorldModule: Beginning of Request</font></h1><hr>");
            
        }
    }
}