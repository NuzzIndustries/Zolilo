using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Data;

namespace Zolilo.Web
{
    public delegate void LogonAcceptedHandler(DR_Accounts account);

    public partial class AccountLoginControl_D : System.Web.UI.UserControl
    {
        public event LogonAcceptedHandler LoginAccepted;
        public event EventHandler LoginFailed;

        protected override void OnInit(EventArgs e)
        {
            buttonLogin.Click += new EventHandler(buttonLogin_Click);
            LoginAccepted += new LogonAcceptedHandler(AccountLoginControl_D_LoginAccepted);
            LoginFailed += new EventHandler(AccountLoginControl_D_LoginFailed);
            spanLogonFailed.Visible = false;
            base.OnInit(e);
        }

        void AccountLoginControl_D_LoginAccepted(DR_Accounts account)
        {
        }

        void AccountLoginControl_D_LoginFailed(object sender, EventArgs e)
        {
            spanLogonFailed.Visible = true;
        }

        void buttonLogin_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                DDOQuery<DR_Accounts> query = new DDOQuery<DR_Accounts>();
                query.Object._Username = textName.Text.ToLower();
                DR_Accounts account = query.PerformQuery();

                if (account == null)
                    LoginFailed(this, null);
                else
                {
                    if (!Security.SecurityEncryption.VerifyCode(textPassword.Text, account._PCode))
                        LoginFailed(this, null);
                    else
                        LoginAccepted(account);
                }
            }
        }
    }
}