using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Zolilo.Data;
using Zolilo.Web;
using System.Threading.Tasks;

namespace Zolilo.Pages
{

    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Page.IsPostBack) { }//Set to true if it is detected to be a postback
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        public new ZoliloPage Page
        {
            get { return (ZoliloPage)(base.Page); }
        }
    }
}