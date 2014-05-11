using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using Zolilo.Communications;

namespace Zolilo
{
    public class HelloWorldHandler : IHttpModule
    {
        public HelloWorldHandler()
        {
        }

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            //HttpApplication app = (HttpApplication)sender;
            HttpRequest Request = context.Context.Request;
            HttpResponse Response = context.Context.Response;
            // This handler is called whenever a file ending 
            // in .sample is requested. A file with that extension
            // does not need to exist.
            Response.Write("<html>");
            Response.Write("<body>");
            Response.Write("<h1>Hello from a synchronous custom HTTP handler.</h1>");
            Response.Write("</body>");
            Response.Write("</html>");
        }

        void context_BeginRequest(object sender, EventArgs e)
        {

        }


        public void ProcessRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            // This handler is called whenever a file ending 
            // in .sample is requested. A file with that extension
            // does not need to exist.
            Response.Write("<html>");
            Response.Write("<body>");
            Response.Write("<h1>Hello from a synchronous custom HTTP handler.</h1>");
            Response.Write("</body>");
            Response.Write("</html>");
        }
        public bool IsReusable
        {
            // To enable pooling, return true here.
            // This keeps the handler in memory.
            get { return false; }
        }
    }
}