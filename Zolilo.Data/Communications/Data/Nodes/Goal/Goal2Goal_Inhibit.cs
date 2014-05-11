using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Zolilo.Data
{
    public class Goal2Goal_Inhibit : Goal2GoalEdge
    {
        public static long edgeType = edgeType == 0 ? Goal2GoalEdge.RegisterEdgeType(typeof(Goal2GoalEdge), Goal2GoalEdgeType.Inhibits) : edgeType;


        public override long EdgeType
        {
            get { return edgeType; }
        }

        public Goal2Goal_Inhibit() : base() { }

        public override Control GetContextControl()
        {
            throw new NotImplementedException();
        }
    }
}
