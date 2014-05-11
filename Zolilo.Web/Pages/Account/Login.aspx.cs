using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Data;
using Zolilo.Web;

namespace Zolilo.Pages
{
    public partial class Login : ZoliloPage, IZoliloPageNoFrame
    {
        protected override void OnInit(EventArgs e)
        {
            loginControl.LoginAccepted += new Web.LogonAcceptedHandler(loginControl_LoginAccepted);
            loginControl.LoginFailed += new EventHandler(loginControl_LoginFailed);
            base.OnInit(e);
        }

        void loginControl_LoginFailed(object sender, EventArgs e)
        {
            
        }

        void loginControl_LoginAccepted(DR_Accounts account)
        {
            ZoliloSession.Current.BeginProcessLogin(account);
            WebDirector.Instance.Redirect("/browse");
        }
    }
}