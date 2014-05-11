using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zolilo.Pages
{
    public partial class Initializing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string path = Request.QueryString["path"] == null ? "/home?redirect=n" : HttpUtility.UrlDecode(Request.QueryString["path"]);

            if (ZoliloSystem.systemInitialized)
                Response.Redirect(path, true);
            if (ZoliloSystem.LastInitializationException != null)
                placeholder.Controls.Add(new LiteralControl(
                    "Error during initialization.<br>" + ZoliloSystem.LastInitializationException.ToString()));
            else
                placeholder.Controls.Add(new LiteralControl(
                    "System currently initializing."));
        }
    }
}