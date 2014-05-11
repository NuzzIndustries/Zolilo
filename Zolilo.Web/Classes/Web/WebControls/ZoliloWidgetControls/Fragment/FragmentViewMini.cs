using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zolilo.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zolilo.Web
{
    public class FragmentViewMini : ZoliloWebControl
    {
        DR_Fragments fragment;
        ReadOnlyFragmentControl readOnlyFragmentControl;

        public FragmentViewMini(DR_Fragments fragment)
        {
            this.fragment = fragment;
        }

        protected override void OnInit(EventArgs e)
        {
            readOnlyFragmentControl = new ReadOnlyFragmentControl();

            readOnlyFragmentControl = new ReadOnlyFragmentControl();
            readOnlyFragmentControl.ID = this.ID + "Parent";
            readOnlyFragmentControl.RenderChildFragments = false;

            Controls.Add(readOnlyFragmentControl);

            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            readOnlyFragmentControl.Fragment = fragment;
            base.OnLoad(e);
        }
    }
}