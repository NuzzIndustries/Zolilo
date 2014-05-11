using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zolilo.Data
{
    public abstract class Tag2GoalEdge : GoalEdge
    {
        protected static long RegisterEdgeType(Type edgeType, Tag2GoalEdgeType edgeSubType)
        {
            return GoalEdge.RegisterEdgeType(edgeType, NodeType.Tag, (ushort)edgeSubType);
        }

        public new DR_Tags ParentObject
        {
            get { return (DR_Tags)base.ParentObject; }
        }
    }

    public enum Tag2GoalEdgeType : ushort
    {
        None = 0,
        GoalTag = 1,
    }
}
