using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Zolilo.Data;

namespace Zolilo.Web
{
    public class FragmentViewRepliesControl : ZoliloWebControl
    {
        DR_Fragments parentFragment;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (parentFragment == null)
                throw new ZoliloSystemException("parentFragment must not be null in FragmentViewRepliesControl");

            int count = 0;
            foreach (Fragment2Fragment_Reply reply in parentFragment.GetChildEdgeList<Fragment2Fragment_Reply>())
            {
                count++;
                ReadOnlyFragmentControl replyFrag = new ReadOnlyFragmentControl();
                replyFrag.Fragment = reply.ChildObject;
                replyFrag.ID = this.ID + "Reply" + count.ToString();
                Controls.Add(replyFrag);
                Controls.Add(new LiteralControl("<br>-------------<br>"));
            }
            base.OnLoad(e);
        }

        public DR_Fragments ParentFragment
        {
            get { return parentFragment; }
            set { parentFragment = value; }
        }
    }
}