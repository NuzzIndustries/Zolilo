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
    public partial class NewTag : ZoliloPage
    {
        protected override void OnInit(EventArgs e)
        {
            buttonSubmit.Click += new EventHandler(buttonSubmit_Click);
            base.OnInit(e);
        }

        void buttonSubmit_Click(object sender, EventArgs e)
        {
            DDOQuery<DR_Tags> query = new DDOQuery<DR_Tags>();
            query.Object._Name = txtTagName.Text.Trim();
            DR_Tags tag = query.PerformQuery();
            if (tag != null)
            {
                phTagExists.Controls.Add(new LiteralControl("Tag name exists (<a href=\"/tags/view?id=" + tag.ID.ToString() +
                    "\">View Tag Details</a>)"));
            }
            else
            {
                tag = new DR_Tags();
                tag._Name = txtTagName.Text;
                tag.SaveChanges();
                WebDirector.Instance.Redirect("/tags/view?id=" + tag.ID.ToString());
            }
        }
    }
}