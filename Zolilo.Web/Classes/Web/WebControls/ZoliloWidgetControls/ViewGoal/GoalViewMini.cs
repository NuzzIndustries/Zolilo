using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Data;

namespace Zolilo.Web
{
    public class GoalViewMini : ZoliloWebControl
    {
        DR_Goals goal;
        DR_GraphEdges contextualEdge;



        public GoalViewMini() { }

        public GoalViewMini(DR_Goals goal)
            : base()
        {
            this.goal = goal;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            Literal labelName = new Literal();
            labelName.Text = "Name: " + goal._Name + "<br>";
            Controls.Add(labelName);

            
            HyperLink linkGoal = new HyperLink();
            linkGoal.NavigateUrl = "/goals/view?id=" + goal.ID.ToString();
            linkGoal.Text = "View Goal Details";
            Controls.Add(linkGoal);

            Controls.Add(new LiteralControl("<br>"));
            if (ContextualEdge != null)
                Controls.Add(ContextualEdge.GetContextControl());
            base.OnLoad(e);
        }

        public long GoalID
        {
            set { this.goal = DR_Goals.Get(value); }
        }

        public DR_GraphEdges ContextualEdge
        {
            get { return contextualEdge; }
            set { contextualEdge = value; }
        }
    }
}