using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using Zolilo.Data;
using Zolilo.Web;

namespace Zolilo
{
    //http://www.dotnetcurry.com/ShowArticle.aspx?ID=126
    public class Global : System.Web.HttpApplication
    {
        ZoliloSystem b;

        void Application_Start(object sender, EventArgs e)
        {
            b = zContext.System;
        }

        void Application_End(object sender, EventArgs e)
        {
            DataConnection.Current.CloseConnection();
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception err = Server.GetLastError();
            if (err is HttpUnhandledException && err.InnerException != null)
                err = err.InnerException;

            if (zContext.Session != null)
            {
                if (ZoliloTransaction.Current != null)
                    ZoliloTransaction.Current.Rollback();
                ZoliloRequestContext.Current.Dispose();
                ZoliloSession.LastError = err;
                Response.Redirect("/Pages/Errors/GlobalError.aspx?redirect=n", true);
            }
            else
                Response.Redirect("/Pages/Errors/GlobalError.aspx?redirect=n&error=" + HttpUtility.UrlEncode(err.Message), true);
            
        }

        void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 60;
        }

        void Session_End(object sender, EventArgs e)
        {
            if (zContext.Session != null)
                zContext.Session.Dispose();
        }

        void Application_BeginRequest(Object Sender, EventArgs e)
        {
            string localPath = Request.Url.LocalPath;

            if (zContext.Session != null)
            {
                //Clear last error if this is not an error request
                if (!localPath.Contains("/Pages/Errors/"))
                    ZoliloSession.LastError = null;

                zContext.Session.title = Context.Request.Path;
            }

            //Check initialization status
            if (!ZoliloSystem.systemInitialized && localPath != "/initializing")
            {
                if (!ZoliloSystem.systemInitializing && ZoliloSystem.LastInitializationException == null)
                    ZoliloSystem.BeginInit();
                Response.Redirect("/" + RouteManager.Route_Initializing + "?path=" + HttpUtility.UrlEncode(Request.Url.PathAndQuery), true);
            }

            //Check 404 status
            if (!RouteManager.IsValidPath(Request.Url.LocalPath))
            {
                Response.Redirect("/errors/404?redirect=n&path=" + localPath, true);
            }
            
        }

        void Application_EndRequest(Object sender, EventArgs e)
        {

        }

    }
}
