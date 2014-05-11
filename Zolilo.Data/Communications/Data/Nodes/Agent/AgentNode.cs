using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zolilo.Data;

namespace Zolilo.Data
{

    public class AgentNode : GraphNode
    {

        internal new DR_Agents DataRecord
        {
            get { return (DR_Agents)base.DataRecord; }
            set { base.DataRecord = value; }
        }

        public string Name
        {
            get { return DataRecord._AgentName; }
            set { DataRecord._AgentName = value; }
        }
    }
}