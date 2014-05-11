using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zolilo.Web;
using System.Web.UI.WebControls;
using Zolilo.Data;

namespace Zolilo.Web.GoalViewControls
{
    public class GoalViewEdgesBase : ZoliloDataView
    {
        internal DR_Goals currentGoal;
        bool getParents;

        internal DR_Goals Goal
        {
            set { currentGoal = value; }
        }

        public GoalViewEdgesBase(bool getParents)
            : base()
        {
            this.getParents = getParents;
            AddColumn<Label, GoalEdge>("Connection ID", new GridViewDataBindHandler<Label, GoalEdge>(trID_CellDataBinding));
            AddColumn<HyperLink, GoalEdge>("Goal Name", new GridViewDataBindHandler<HyperLink, GoalEdge>(trGoalName_CellDataBinding));
            AddColumn<HyperLink, GoalEdge>("Details", new GridViewDataBindHandler<HyperLink,GoalEdge>(trConnectionSummary_CellDataBinding));
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        void trID_CellDataBinding(System.Web.UI.Control control, object data)
        {
            Label label = (Label)control;
            if (getParents)
                label.Text = ((GoalEdge)data).ID.ToString();
            else
                label.Text = ((GoalEdge)data).ID.ToString();
        }

        void trGoalName_CellDataBinding(System.Web.UI.Control control, object data)
        {
            HyperLink label = (HyperLink)control;
            DR_Goals goal;
            if (getParents)
                goal = (DR_Goals)(((GoalEdge)data).ParentObject);
            else
                goal = (DR_Goals)(((GoalEdge)data).ChildObject);
            label.Text = goal._Name;
            label.NavigateUrl = "/goals/view?id=" + goal.ID.ToString();
        }

        void trConnectionSummary_CellDataBinding(System.Web.UI.Control control, object data)
        {
            HyperLink label = (HyperLink)control;
            GoalEdge edge = (GoalEdge)data;
            label.Text = "Details";
            label.NavigateUrl = "/vertex/view?id=" + edge.ID;
        }
    }
}