using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zolilo.Data
{
    public abstract class Goal2FragmentEdge : FragmentEdge, IParentGoalEdge
    {
        protected static long RegisterEdgeType(Type type, Goal2FragmentEdgeType subType)
        {
            return FragmentEdge.RegisterEdgeType(type, NodeType.Goal, (ushort)subType);
        }
    }

    
    public enum Goal2FragmentEdgeType : ushort
    {
        None = 0,
        GoalDescription = 1,
    }
}
