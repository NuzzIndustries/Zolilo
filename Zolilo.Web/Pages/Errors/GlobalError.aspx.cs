using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Web;
using System.Web.Security;
using Zolilo.Data;

namespace Zolilo.Pages
{
    public partial class GlobalError : ZoliloPage
    {
        Exception ex;
        string exText;

        protected override void OnPreInit(EventArgs e)
        {
            ex = ZoliloSession.LastError;
            if (ex != null)
            {
                if (ex.GetType() == typeof(HttpException))
                {
                    HttpException httpEx = (HttpException)ex;
                    Response.StatusCode = httpEx.GetHttpCode();
                }
                exText = ex.ToString();
            }
            else
            {
                if (Request.QueryString["error"] == null)
                    exText = "Unspecified error";
                else
                    exText = HttpUtility.UrlDecode(Request.QueryString["error"]);
            }
            base.OnPreInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LabelError.Text += exText;
        }
    }
}