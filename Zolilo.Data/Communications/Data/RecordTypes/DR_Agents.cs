using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zolilo.Data
{
    //An agent can be seen as a sort of "avatar" in the system.  This is a psychological tool for disconnecting the philosophical definition of "self", and re-defining it in the context of the GOLEM system
    //Agents will be responsible for creating and controlling the Searches, Questions, Feedback, Scores, Credit Allocation, Fragments, and Goals (at least).  
    //Agents can compete or work together for various purposes.  
    public class DR_Agents : GraphNode
    {
        #region UniversalAbstractMethods
        public static DR_Agents Get(long id)
        {
            return ZoliloCache.Instance.Agents[id];
        }
        #endregion

        DR_Accounts account;

        public DR_Agents() : base()
        {
        }

        public long _IDAccount
        {
            get { return (long)Cells["IDACCOUNT"].Data; }
            set { Cells["IDACCOUNT"].Data = value; }
        }

        public string _AgentName
        {
            get { return (string)Cells["AGENTNAME"].Data; }
            set { Cells["AGENTNAME"].Data = value; }
        }

        public long _PrimaryGoal
        {
            get { return (long)Cells["PRIMARYGOAL"].Data; }
            set { Cells["PRIMARYGOAL"].Data = value; }
        }

        public override long _IDAgentCreated
        {
            get
            {
                return ID;
            }
        }

        public override NodeType NodeType
        {
            get { return NodeType.Agent; }
        }
    }
}