using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Web;

namespace Zolilo.Pages
{
    public partial class AccountSettings : ZoliloPage, IRestrictedPage
    {
        protected override void OnInit(EventArgs e)
        {
            buttonSave.Click += new EventHandler(buttonSave_Click);
            timeZoneValidator.ControlToValidate = TimeZoneOffset.ID;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                TimeZoneOffset.Text = zContext.Account._TimeZoneOffset.ToString();
        }

        void buttonSave_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                zContext.Account._TimeZoneOffset = short.Parse(TimeZoneOffset.Text);
                zContext.Account.SaveChanges();
            }
        }
    }
}