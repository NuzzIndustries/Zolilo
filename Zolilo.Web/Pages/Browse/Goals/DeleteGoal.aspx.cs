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
    public partial class DeleteGoal : ZoliloPage, IRestrictedPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            buttonconfirm.Click += new EventHandler(buttonconfirm_Click);
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (QueryStringID <= 0)
            {
                label.Text = "Invalid Goal";
                buttonconfirm.Visible = false;
            }
            else
            {
                label.Text = "Are you sure you want to delete goal " + QueryStringID.ToString() + "?";
                buttonconfirm.Visible = true;
            }
            base.OnLoad(e);
        }

        void buttonconfirm_Click(object sender, EventArgs e)
        {
            DR_Goals goal = DR_Goals.Get(QueryStringID);
            goal.DeletePermanently();
            WebDirector.Instance.Redirect("/goals");
        }
    }
}