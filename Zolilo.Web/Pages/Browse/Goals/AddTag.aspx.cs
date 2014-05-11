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
    public partial class AddTag : ZoliloPage
    {
        DR_Goals goal;

        protected override void OnInit(EventArgs e)
        {
            goal = DR_Goals.Get(QueryStringID);
            goalViewMini.GoalID = goal.ID;
            tagViewList.CellDataBinding += new GridViewDataBindHandler<HyperLink, DR_Tags>(tagViewList_CellDataBinding);
            base.OnInit(e);
        }

        void tagViewList_CellDataBinding(HyperLink control, DR_Tags data)
        {   
            control.NavigateUrl = "/goals/addtag?id=" + goal.ID.ToString() + "&tag=" + data.ID.ToString() + "&confirm=y";
        }

        protected override void OnLoad(EventArgs e)
        {
            if (Request.QueryString["confirm"] == "y")
            {
                DR_Tags tag = DR_Tags.Get(long.Parse(Request.QueryString["tag"]));
                tag.AttachChildNodeWithNewEdge(goal, typeof(Tag2Goal_GoalTag));
                WebDirector.Instance.Redirect("/goals/view?id=" + goal.ID.ToString());
            }
            base.OnLoad(e);
        }
    }
}