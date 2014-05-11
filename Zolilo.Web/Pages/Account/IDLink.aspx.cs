using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Data;
using Zolilo.Security;
using Zolilo.Web;

namespace Zolilo.Pages
{
    public partial class IDLink : ZoliloPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (zContext.Session.LoggedIn || (zContext.Session.OpenIDAuthenticationInformation == null || zContext.Session.OpenIDAuthenticationInformation.OpenIdentifier == null))
            {
                Response.RedirectToRoute("home");
            }
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                DR_Accounts record = new DR_Accounts();
                record._Username = TextBoxUserName.Text;
                record.QueryRow();
                
                if (record.Cells["ID"].Data != null)
                {
                    if (SecurityEncryption.VerifyCode(TextBoxPassword.Text, record._PCode))
                    {
                        //Successful login
                        DR_OpenIDMap openidrec = new DR_OpenIDMap();
                        openidrec._AccountID = record.ID;
                        openidrec._OpenIdentifier = zContext.Session.OpenIDAuthenticationInformation.OpenIdentifier;
                        openidrec.SaveChanges();
                        int records = 1; //refactor
                        if (records > 0)
                        {
                            TextBoxResult.Text = "Successfully linked accounts!";
                            zContext.Session.Flags.OpenIDNotLinked = false;
                            zContext.Session.ProcessLogin();
                            Response.Redirect("~/home");
                        }
                    }
                    else
                    {
                        TextBoxResult.Text = "Invalid Password.";
                    }
                }
                else
                {
                    TextBoxResult.Text = "Username not found.";
                }
            }
        }
    }
}