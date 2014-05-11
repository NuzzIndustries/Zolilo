using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Data;

namespace Zolilo.Web
{
    public partial class FragmentViewVerbose : System.Web.UI.UserControl
    {
        DR_Fragments fragment;

        protected override void OnInit(EventArgs e)
        {
            buttonDeleteFrag.Click += new EventHandler(buttonDeleteFrag_Click);
            base.OnInit(e);
        }

        
        protected override void OnLoad(EventArgs e)
        {
            if (fragment.IsGoalDefinition)
                buttonDeleteFrag.Visible = false;
            GraphNode singularParent = fragment.SingularParent;
            viewParent.NodeID = singularParent.ID;
            viewParent.NodeType = singularParent.NodeType;

            viewEdgeParent.Edge = fragment.AllParentEdges[0];

            fragmentControl.Fragment = fragment;
            fragmentControl.DisableHyperlink = true;

            mainTimeCreated.UtcTime = fragment.TimeCreatedUTC;

            if (fragment.TimeCreatedUTC != fragment.TimeModifiedUTC)
                mainTimeModified.UtcTime = fragment.TimeModifiedUTC;
            else
            {
                SpanTimeModified.Visible = false;
                mainTimeModified.Enabled = false;
            }

            mainCreatedBy.Agent = fragment.Agent;

            mainFeedbackRating.Controls.Add(new LiteralControl(fragment._BaseWeight.ToString()));

            reply1.ParentFragment = fragment;
            reply2.ParentFragment = fragment;
            viewReplies.ParentFragment = fragment;

            int replyCount = fragment.GetChildEdgeList<Fragment2Fragment_Reply>().Count;
            if (replyCount == 0)
                reply2.Visible = false;
            repliesCount.Controls.Add(new LiteralControl(replyCount.ToString() + " total<br>"));
            base.OnLoad(e);
        }

        void buttonDeleteFrag_Click(object sender, EventArgs e)
        {
            WebDirector.Instance.Redirect("/fragments/delete?id=" + fragment.ID.ToString());
        }

        public DR_Fragments Fragment
        {
            get { return fragment; }
            set { fragment = value; }
        }
    }
}