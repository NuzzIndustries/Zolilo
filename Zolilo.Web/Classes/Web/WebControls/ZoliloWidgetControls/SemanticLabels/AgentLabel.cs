using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Zolilo.Data;

namespace Zolilo.Web
{
    public class AgentLabel : ZoliloWebControl
    {
        DR_Agents agent;

        protected override void OnLoad(EventArgs e)
        {
            if (Agent == null)
                throw new ZoliloWebException("Agent is null on AgentLabel");
            Controls.Add(new LiteralControl("<font face=\"courier\">" + agent._AgentName + "</font>"));
            base.OnLoad(e);
        }

        public DR_Agents Agent
        {
            get { return agent; }
            set { agent = value; }
        }
    }
}