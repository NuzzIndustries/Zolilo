using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zolilo.Web
{
    public class ReadOnlyFragmentControl : FragmentControl
    {
        

        public ReadOnlyFragmentControl()
            : base()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            Controls.Add(fragmentText);
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (fragment != null)
                fragmentText.Text = fragment.Text;
            base.OnLoad(e);
        }

        public override string Text
        {
            get
            {
                return fragmentText.Text;
            }
            set
            {
                fragmentText.Text = value;
            }
        }
    }
}