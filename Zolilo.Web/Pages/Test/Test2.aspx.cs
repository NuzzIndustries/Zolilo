using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Web;

namespace Zolilo.Pages
{
    public partial class Test2 : ZoliloPage, IPageIsReturnPersist
    {
        protected override void OnInit(EventArgs e)
        {
            button.Click += new EventHandler(button_Click);
            base.OnInit(e);
        }

        void button_Click(object sender, EventArgs e)
        {
            WebDirector.Instance.Redirect("/Pages/Test/Test3.aspx");
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}