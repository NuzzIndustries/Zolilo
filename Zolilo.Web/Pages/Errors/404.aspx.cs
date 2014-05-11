using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Web;

namespace Zolilo.Pages
{
    public partial class _404 : ZoliloPage
    {
        protected override void OnInit(EventArgs e)
        {
            if (Request.QueryString["path"] == null)
                WebDirector.Instance.Redirect("/home?redirect=n");
            LabelError.Text += "404: Page not found: " + Request.QueryString["path"];
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}