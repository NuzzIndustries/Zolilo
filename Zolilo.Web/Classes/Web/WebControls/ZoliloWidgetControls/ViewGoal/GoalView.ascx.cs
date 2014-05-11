using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Data;

namespace Zolilo.Web
{
    public partial class GoalView : System.Web.UI.UserControl, IJavaScriptObject
    {
        DR_Goals goal;

        public string GetConstructorExpression()
        {
            return "new goalView('" + divGoalView.ID + "','" + GetClientID() + "');";
        }    

        public string GetClientID()
        {
            return divGoalView.ClientID;
        }

        protected override void OnInit(EventArgs e)
        {
            button_delete.Click += new EventHandler(button_delete_Click);
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            GoalDef.Goal = goal;

            phAddTag.Controls.Add(new LiteralControl("<a href=\"/goals/addtag?id=" + goal.ID.ToString() + "\">Add Tag</a>"));
            foreach(Tag2Goal_GoalTag tagConnection in goal.GetParentEdgeList<Tag2Goal_GoalTag>())
            {
                TagViewMini tagView = new TagViewMini(tagConnection.ParentObject);
                tagView.ContextualEdge = tagConnection;
                phTags.Controls.Add(tagView);
                phTags.Controls.Add(new LiteralControl("<br>"));
            }

            foreach (Goal2Goal_Achieve parentAchieve in goal.GetParentEdgeList<Goal2Goal_Achieve>())
            {
                GoalViewMini parentView = new GoalViewMini(parentAchieve.ParentObject);
                parentView.ContextualEdge = parentAchieve;
                phParents.Controls.Add(parentView);
                phParents.Controls.Add(new LiteralControl("<br>"));
            }

            foreach (Goal2Goal_Achieve childAchieve in goal.GetChildEdgeList<Goal2Goal_Achieve>())
            {
                GoalViewMini childView = new GoalViewMini(childAchieve.ParentObject);
                childView.ContextualEdge = childAchieve;
                phParents.Controls.Add(childView);
                phParents.Controls.Add(new LiteralControl("<br>"));
            }

            base.OnLoad(e);
        }
        void button_delete_Click(object sender, EventArgs e)
        {
            WebDirector.Instance.Redirect("/goals/delete?id=" + goal.ID);
        }

        internal long GoalID
        {
            set
            {
                GoalObject = DR_Goals.Get(value);
            }
        }

        private DR_Goals GoalObject
        {
            set
            {
                this.goal = value;
                InitData(value);
            }
        }

        private void InitData(DR_Goals goal)
        {
            if (goal != null)
            {
                DR_Agents agent = DR_Agents.Get(goal._IDAgentCreated);

                LabelGoalName.Controls.Add(new LiteralControl("<h3><b>" + goal._Name + "</b></h3>"));
                LabelAgentCreated.Agent = agent;
                LabelTimeCreated.UtcTime = goal.TimeCreatedUTC;

                //DataParentGoals.Goal = goal;
                //DataChildGoals.Goal = goal;

                HyperLinkAddParent.NavigateUrl = "/goals/addvertex?id=" + goal.ID.ToString() + "&type=p";
                HyperLinkAddChild.NavigateUrl = "/goals/addvertex?id=" + goal.ID.ToString() + "&type=c";
            }
            else
            {
                LabelGoalName.Controls.Add(new LiteralControl("Invalid Goal ID"));
            }
        }

        protected void Test_CheckedChanged(object sender, EventArgs e)
        {
            Response.Write("Success");
        }
    }
}