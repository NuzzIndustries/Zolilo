using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Zolilo
{
    /// <summary>
    /// Summary description for ZoliloHttpHandler
    /// </summary>
    public class ZoliloHttpHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}