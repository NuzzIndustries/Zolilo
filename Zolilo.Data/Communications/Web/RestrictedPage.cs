using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zolilo.Data;

namespace Zolilo.Web
{
    public static class RestrictedPage
    {
        public static void ValidateLogin(ZoliloPage page)
        {
            if (!zContext.Session.LoggedIn)
            {
                WebDirector.Instance.Redirect("/account/login");
                return;
            }
            if (zContext.Session.CurrentAccount.ID <= 0)
            {
                WebDirector.Instance.Redirect("/account/idlink");
                return;
            }
            if (zContext.Session.Agent == null)
            {
                WebDirector.Instance.Redirect("/agent/new");
                return;
            }
        }

        internal static void ValidateLoginDarknet(ZoliloPage page)
        {
            if (!zContext.Session.LoggedIn)
            {
                if (HttpContext.Current.Request.Url.LocalPath != "/login")
                {
                    if (page.IsUFrame)
                        WebDirector.Instance.Redirect("/login");
                    else
                        HttpContext.Current.Response.Redirect("/login");
                }
                else
                {
                }
                //if (HttpContext.Current.Request.Url.Query != null && HttpContext.Current.Request.Url.Query.Length == 0)
                  //  HttpContext.Current.Response.Redirect("/login");
                    
            }
        }
    }
}