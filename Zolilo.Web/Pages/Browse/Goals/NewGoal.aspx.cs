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
    public partial class NewGoal : ZoliloPage//Restricted
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
        }

        protected override void OnInit(EventArgs e)
        {
            ButtonSave.Click += new EventHandler(ButtonSave_Click);
            base.OnInit(e);
        }

        protected override void FrameworkInitialize()
        {
            base.FrameworkInitialize();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (zContext.Session.Agent != null && Page.IsValid)
            {
                DR_Goals g = DR_Goals.CreateNewFromForm(
                    TextBoxGoalName.Text,
                    zContext.Session.Agent.ID,
                    GoalSelectorParent.SelectedGoal);
                WebDirector.Instance.Redirect("/goals/view?id=" + g.ID.ToString());
            }
        }
    }
}