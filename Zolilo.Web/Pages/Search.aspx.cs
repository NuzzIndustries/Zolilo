using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Web;
using Zolilo.Data;
using AjaxControlToolkit;
using Zolilo.Application;

namespace Zolilo.Pages
{
    public partial class Search : ZoliloPage
    {   
        protected override void OnInit(EventArgs e)
        {
            button.Click += new EventHandler(button_Click);
            base.OnInit(e);
        }

        void button_Click(object sender, EventArgs e)
        {
            WebDirector.Instance.Redirect("/Pages/Test/Test1.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DDOQuery<DR_Accounts> query = new DDOQuery<DR_Accounts>();
            query.Object._Username = "Admin";
            DR_Accounts list = query.PerformQuery();
        }
    }
}