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
    public partial class AddGoalVertex : ZoliloPage
    {
        DR_Goals goal;
        bool invalid = false;
        string type;

        protected override void OnInit(EventArgs e)
        {
            string idstring = Request.QueryString["id"];
            string typestring = Request.QueryString["type"];
            long id;
            
            try
            {
                id = Convert.ToInt64(idstring);
                goal = DR_Goals.Get(id);
                LinkNewGoal.NavigateUrl = "/goals/new";
            }
            catch (Exception ex)
            {
                invalid = true;
            }
            if (!invalid && typestring == "p")
            {
                labelHeader.Text = "Adding parent goal to goal " + goal._Name;
                goalSelector.SelectGoalIntroText = "Select New Parent Goal";
                type = "p";
            }
            else if (!invalid && typestring == "c")
            {
                labelHeader.Text = "Adding child goal to goal " + goal._Name;
                goalSelector.SelectGoalIntroText = "Select New Child Goal";
                type = "c";
            }
            else
            {
                invalid = true;
            }

            buttonSubmit.Click += new EventHandler(buttonSubmit_Click);
            buttonCancel.Click += new EventHandler(buttonCancel_Click);

            base.OnInit(e);
        }

        void buttonCancel_Click(object sender, EventArgs e)
        {
            WebDirector.Instance.Redirect("/goals/view?id=" + goal.ID.ToString());
        }

        void buttonSubmit_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                if (type == "c")
                {
                    DR_Goals node = goalSelector.SelectedGoal;
                    goal.AddChildGoal(node, typeof(Goal2Goal_Achieve));
                }
                else if (type == "p")
                {
                    DR_Goals parent = goalSelector.SelectedGoal;
                    parent.AddChildGoal(goal, typeof(Goal2Goal_Achieve));
                }
                else
                    throw new InvalidOperationException();
                WebDirector.Instance.Redirect("/goals/view?id=" + goal.ID.ToString());
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (invalid)
            {
                ContentDiv.Visible = false;
                goalSelector.Visible = false;
                Label label = new Label();
                label.Text = "Invalid parameters.  You may have reached this page in error.";
                LabelPlaceholder.Controls.Add(label);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }
    }
}