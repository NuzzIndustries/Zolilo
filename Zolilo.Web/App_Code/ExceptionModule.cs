using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Zolilo.Web
{
    public class ExceptionModule : IHttpModule
    {
        Dictionary<object, object> requestTags = new Dictionary<object, object>();
 
        public void Dispose()
        {

        }
        public ExceptionModule()
        {
        }

        /// <summary>
        /// Init method to register the event handlers for 
        /// the HttpApplication
        /// </summary>
        /// <param name="application">Http Application object</param>
        public void Init(HttpApplication application)
        {
            
            application.Error += this.Application_Error_Event;
            application.BeginRequest += new EventHandler(Application_BeginRequest);
            application.PostAcquireRequestState += new EventHandler(Application_PostAcquireRequestState);
            application.PostMapRequestHandler += new EventHandler(Application_PostMapRequestHandler);
        }

        void Application_BeginRequest(object source, EventArgs e)
        {
            HttpApplication app = (HttpApplication)source;
            requestTags.Add(app.Context.Request, null);
        }

        void Application_PostMapRequestHandler(object source, EventArgs e)
        {
            HttpApplication app = (HttpApplication)source;
            HttpRequest request = app.Context.Request;

            if (app.Context.Handler is IReadOnlySessionState || app.Context.Handler is IRequiresSessionState)
            {
                // no need to replace the current handler
                return;
            }

            // swap the current handler
            app.Context.Handler = new SwapHttpHandler(app.Context.Handler);
        }

        void Application_PostAcquireRequestState(object source, EventArgs e)
        {
            HttpApplication app = (HttpApplication)source;
            HttpRequest request = app.Context.Request;

            SwapHttpHandler resourceHttpHandler = HttpContext.Current.Handler as SwapHttpHandler;

            if (resourceHttpHandler != null)
            {
                // set the original handler back
                HttpContext.Current.Handler = resourceHttpHandler.OriginalHandler;
            }

            // -> at this point session state should be available
            if (requestTags[request] != null) //error exists
            {
                if (app.Context.Session != null)
                {
                   // Application_Error_Event(source, e);
                }
                else
                {   
                    app.Context.Response.Write("ERROR: Session not valid.  Cookies must be enabled to use this site.");
                }
            }
            

         //   Debug.Assert(app.Session != null, "it did not work :(");
        }



        private void Application_Error_Event(object sender, EventArgs e)
        {
            return;//TODO: Implement custom errors - Current implementation of this method causes hang

            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;
            HttpRequest request = application.Context.Request;
                //context.ClearError();
                //return;
           // throw new InvalidOperationException("Error triggered");
            // Create HttpApplication and HttpContext objects to access
            // request and response properties.
            

            
            Exception ex;

            
            //return; //remove this later

            if (context.Session != null)
            {
                if (requestTags[request] != null)
                    ex = (Exception)requestTags[request];
                else
                    ex = context.Server.GetLastError();

                
                requestTags[request] = null;
                context.Session["LastError"] = ex;
                
                if (application.Context.Handler is SwapHttpHandler)
                {
                    context.Response.Write("SwapHttpHandler cannot process requests.");
                }
                else
                {
                    context.ClearError();
                    context.Server.Transfer("/Pages/Errors/GlobalError.aspx");
                }
                return;
                //context.Response.Write("session exists " + ex.ToString());
            }
            else
            {
                context.ClearError();
                return;
                //Error will be handled later
                context.ClearError();
                requestTags[request] = ex;
                
                context.Response.Write("no session " + ex.ToString());
            }

            
            
           // context.Server.Transfer("Pages/Errors/GlobalError.aspx");

         }
    }

    // a temp handler used to force the SessionStateModule to load session state
    //http://stackoverflow.com/questions/276355/can-i-access-session-state-from-an-httpmodule
    public class SwapHttpHandler : IHttpHandler, IRequiresSessionState
    {
        internal readonly IHttpHandler OriginalHandler;

        public SwapHttpHandler(IHttpHandler originalHandler)
        {
            OriginalHandler = originalHandler;
        }

        public void ProcessRequest(HttpContext context)
        {
            // do not worry, ProcessRequest() will not be called, but let's be safe
            throw new InvalidOperationException("SwapHttpHandler cannot process requests.");
        }

        public bool IsReusable
        {
            // IsReusable must be set to false since class has a member!
            get { return false; }
        }
    }
}