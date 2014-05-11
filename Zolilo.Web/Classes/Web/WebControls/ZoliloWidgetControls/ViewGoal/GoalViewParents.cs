using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zolilo.Web;
using System.Web.UI.WebControls;
using Zolilo.Data;

namespace Zolilo.Web.GoalViewControls
{
    public class GoalViewParents : GoalViewEdgesBase
    {
        public GoalViewParents()
            : base(true)
        {
        }


        protected override void OnLoad(EventArgs e)
        {
            grid.DataSource = currentGoal.GetParentEdgeList<Goal2Goal_Achieve>();
            grid.DataBind();
            
            base.OnLoad(e);
        }
    }
}