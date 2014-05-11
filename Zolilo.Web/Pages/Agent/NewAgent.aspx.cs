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
    public partial class NewAgent : ZoliloPage
    {
        long currentAgentID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!zContext.Session.LoggedIn)
            {
                Response.RedirectToRoute("account/login");
                return;
            }
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                DR_Agents agent = GetAgent();
                if (agent == null)
                    return;

                DR_Agents agentSearch = new DR_Agents();
                agentSearch._AgentName = TextBoxAgentName.Text;

                agentSearch = (DR_Agents)agentSearch.QueryRow();
                if (agentSearch != null)
                {
                    LabelResult.Text = "The name '" + TextBoxAgentName.Text + "' has already been taken.  Choose a different name.";
                    return;
                }

                agent._AgentName = TextBoxAgentName.Text;
                agent.SaveChanges();

                DR_Accounts account = DR_Accounts.Get(agent._IDAccount);
                account._IDAgentCurrent = agent.ID;
                account.SaveChanges();

                LabelResult.Text = "Successfully registered agent.";
            }
        }

        DR_Agents GetAgent()
        {
            DR_Agents agent = new DR_Agents();
            agent._IDAccount = zContext.Session.CurrentAccount.ID;
            agent = (DR_Agents)agent.QueryRow();
            if (agent == null)
            {
                agent = new DR_Agents();
                agent._IDAccount = zContext.Session.CurrentAccount.ID;
                agent.SaveChanges();

                agent = new DR_Agents();
                agent._IDAccount = zContext.Session.CurrentAccount.ID;
                agent = (DR_Agents)agent.QueryRow();
                if (agent == null)
                {
                    LabelResult.Text = "Error creating agent. (2)";
                    return null;
                }
            }
            return agent;
        }
    }
}