using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Web;

namespace Zolilo.Pages
{
    public partial class Logout : ZoliloPage, IZoliloPageNoFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DoLogout();
        }

        private void DoLogout()
        {
            HttpContext.Current.Session.Abandon();
            Response.RedirectToRoute("home");
        }
    }
}