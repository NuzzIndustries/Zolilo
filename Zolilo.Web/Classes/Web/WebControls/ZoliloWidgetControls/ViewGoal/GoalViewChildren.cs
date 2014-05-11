using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zolilo.Data;

namespace Zolilo.Web.GoalViewControls
{
    public class GoalViewChildren : GoalViewEdgesBase
    {
        public GoalViewChildren() : base(false) { }

        protected override void OnLoad(EventArgs e)
        {
            grid.DataSource = currentGoal.GetChildEdgeList<Goal2Goal_Achieve>();
            grid.DataBind();

            base.OnLoad(e);
        }
    }
}