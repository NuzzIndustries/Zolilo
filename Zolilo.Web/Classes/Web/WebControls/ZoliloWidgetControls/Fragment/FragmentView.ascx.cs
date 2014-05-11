using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Data;

namespace Zolilo.Web
{
    public partial class FragmentView : System.Web.UI.UserControl
    {
        long id;

        public FragmentView(long id)
        {
            this.id = id;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            

            DR_Fragments fragment = DR_Fragments.Get(id);
            DR_Agents agent = DR_Agents.Get(fragment._IDAgentCreated);
            DR_FragmentLRA LRA = DR_FragmentLRA.Get(fragment.ID);

            LabelAgent.Text = agent._AgentName;
            LabelDate.Text = fragment.TimeCreatedUTC.ToString();
            LabelLRA.Text = LRA._Text;
        }
    }
}