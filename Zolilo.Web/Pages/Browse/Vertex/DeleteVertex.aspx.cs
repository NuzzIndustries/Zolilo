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
    public partial class DeleteVertex : ZoliloPage
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
                label.Text = "Are you sure you want to delete connection " + QueryStringID.ToString() + "?";
                buttonconfirm.Visible = true;
            }
            base.OnLoad(e);
        }

        void buttonconfirm_Click(object sender, EventArgs e)
        {
            DR_GraphEdges edge = DR_GraphEdges.Get(QueryStringID);
            edge.DeletePermanently();
            if (edge.ParentObject is DR_Goals)
            {
                WebDirector.Instance.Redirect("/goals/view?id=" + edge.ParentObject.ID.ToString());
            }
            else
            {
                WebDirector.Instance.Redirect("/browse");
            }
            
        }
    }
}