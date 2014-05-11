using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
namespace Zolilo.Data
{
    public class Goal2Goal_Achieve : Goal2GoalEdge
    {
        public static long edgeType = edgeType == 0 ? Goal2GoalEdge.RegisterEdgeType(typeof(Goal2Goal_Achieve), Goal2GoalEdgeType.Achieves) : edgeType;

        public override long EdgeType
        {
            get { return edgeType; }
        }

        public Goal2Goal_Achieve() : base() { }
    }
}
