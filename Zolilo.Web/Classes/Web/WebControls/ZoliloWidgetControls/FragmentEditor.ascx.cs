using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Data;

namespace Zolilo.Web
{
    public partial class FragmentEditor : System.Web.UI.UserControl
    {
        long parentFragment;

        protected void Page_Load(object sender, EventArgs e)
        {
            parentFragment = 0; //update
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            DR_Fragments frag = new DR_Fragments();
            frag.SaveChanges();

            DR_FragmentLRA LRA = new DR_FragmentLRA();
            LRA.ID = frag.ID;
            LRA._Text = textBoxLRA.Text;
        }
    }
}