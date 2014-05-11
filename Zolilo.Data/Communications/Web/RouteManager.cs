using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Zolilo.Data;
using System.IO;

namespace Zolilo.Web
{
    public class RouteManager
    {
        public static Dictionary<string, string> routePaths = new Dictionary<string, string>();
        public static string Route_Initializing = RegisterRoute("initializing", "~/Pages/Initializing.aspx").Url;

        public RouteManager()
        {
            
        }


        public static bool IsValidPath(string localPath)
        {
            if (routePaths.ContainsKey(localPath))
                return true;
            return File.Exists(HttpContext.Current.Server.MapPath(localPath));
        }

        internal void Initialize()
        {
            LogManager.Logger.Trace("Initializing RouteManager");

RegisterRoute("home",                       "~/Pages/Home.aspx?redirect=n");

RegisterRoute("about",                      "~/Pages/About.aspx");

RegisterRoute("account/idlink",             "~/Pages/Account/IDLink.aspx");
RegisterRoute("account/register",           "~/Pages/Account/Register.aspx");
RegisterRoute("account/login",              "~/Pages/Account/Login.aspx?deprecated=y");
RegisterRoute("account/logout",             "~/Pages/Account/Logout.aspx");
RegisterRoute("account/settings",           "~/Pages/Account/AccountSettings.aspx");

RegisterRoute("admin",                      "~/Pages/Admin/Admin.aspx");

RegisterRoute("agent/new",                  "~/Pages/Agent/NewAgent.aspx");

RegisterRoute("browse",                     "~/Pages/Browse/Browse.aspx");

RegisterRoute("errors/404",                 "~/Pages/Errors/404.aspx");

RegisterRoute("fragments/delete",           "~/Pages/Browse/Fragments/DeleteFragment.aspx");
RegisterRoute("fragments/view",             "~/Pages/Browse/Fragments/ViewFragment.aspx");

RegisterRoute("goals",                      "~/Pages/Browse/Goals/Goals.aspx");
RegisterRoute("goals/addtag",               "~/Pages/Browse/Goals/AddTag.aspx");
RegisterRoute("goals/addvertex",            "~/Pages/Browse/Goals/AddGoalVertex.aspx");
RegisterRoute("goals/delete",               "~/Pages/Browse/Goals/DeleteGoal.aspx");
RegisterRoute("goals/new",                  "~/Pages/Browse/Goals/NewGoal.aspx");
RegisterRoute("goals/view",                 "~/Pages/Browse/Goals/ViewGoal.aspx");

RegisterRoute("login",                      "~/Pages/Account/Login.aspx");

RegisterRoute("metrics",                     "~/Pages/Browse/Metrics/BrowseMetrics.aspx");
RegisterRoute("metrics/new",           "~/Pages/Browse/Metrics/DefineNewMetric.aspx");

RegisterRoute("search",                     "~/Pages/Search.aspx");

RegisterRoute("siteinfo",                   "~/Pages/SiteInfo.aspx");

RegisterRoute("tags",                       "~/Pages/Browse/Tags/BrowseTags.aspx");
RegisterRoute("tags/delete",                "~/Pages/Browse/Tags/DeleteTag.aspx");
RegisterRoute("tags/new",                   "~/Pages/Browse/Tags/NewTag.aspx");
RegisterRoute("tags/view",                  "~/Pages/Browse/Tags/ViewTag.aspx");

RegisterRoute("vertex/view",                "~/Pages/Browse/Vertex/ViewVertex.aspx");
RegisterRoute("vertex/delete",              "~/Pages/Browse/Vertex/DeleteVertex.aspx");

        }

        private static Route RegisterRoute(string routenameURL, string physicalfile)
        {
            Route route = RouteTable.Routes.MapPageRoute(routenameURL, routenameURL, physicalfile);
            routePaths.Add("/" + routenameURL, physicalfile);
            return route;
        }

        internal static RouteManager Instance
        {
            get { return zContext.System.WebDirector.RouteManager; }
        }
    }
}