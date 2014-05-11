using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zolilo.Data
{
    public abstract class Goal2GoalEdge : GoalEdge, IParentGoalEdge
    {
        protected static long RegisterEdgeType(Type edgeType, Goal2GoalEdgeType edgeSubType)
        {
            return GoalEdge.RegisterEdgeType(edgeType, NodeType.Goal, (ushort)edgeSubType); 
        }

        public new DR_Goals ParentObject
        {
            get { return (DR_Goals)base.ParentObject; }
        }
    }

    public enum Goal2GoalEdgeType : ushort
    {
        None = 0,
        Achieves = 1,
        Inhibits = 2,
    }
}
