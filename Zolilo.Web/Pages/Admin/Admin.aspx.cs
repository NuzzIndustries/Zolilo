using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Web;
using Zolilo.Data;


namespace Zolilo.Pages
{
    public partial class Admin : ZoliloPage
    {
        protected override void OnInit(EventArgs e)
        {
            buttonEncrypt.Click += new EventHandler(buttonEncrypt_Click);
            base.OnInit(e);
        }

        void buttonEncrypt_Click(object sender, EventArgs e)
        {
            //DataManager.Instance.EncryptConnectionString();
        }
    }
}