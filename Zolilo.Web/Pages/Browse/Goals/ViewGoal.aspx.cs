using System;   
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Data;
using Zolilo.Web;

namespace Zolilo.Pages
{
    public partial class ViewGoal : ZoliloPage
    {
        public ViewGoal()
            : base()
        {
        }

        protected override void OnPreInit(EventArgs e)
        {
            
            base.OnPreInit(e);
        }

        protected override void OnInit(EventArgs e)
        {
            GoalViewControl.GoalID = QueryStringID;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}