using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Data;

namespace Zolilo.Web
{
    public partial class GoalDefinitionControl : System.Web.UI.UserControl
    {
        EditableFragmentControl f = new EditableFragmentControl();
        DR_Goals goal;

        public GoalDefinitionControl() : base() { }

        public GoalDefinitionControl(DR_Goals goal)
            : base()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            f.Title = "Goal Definition";
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            fragmentplaceholder.Controls.Add(f);
            f.Fragment = goal.NodeDefinition;
            base.OnLoad(e);
        }

        public DR_Goals Goal
        {
            get { return goal; }
            set { goal = value; }
        }
    }
}