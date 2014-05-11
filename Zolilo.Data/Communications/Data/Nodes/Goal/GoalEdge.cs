using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zolilo.Data;

namespace Zolilo.Data
{
    /// <summary>
    /// Edge child = Goal
    /// </summary>
    public abstract class GoalEdge : DR_GraphEdges
    {
        protected static long RegisterEdgeType(Type edgeType, NodeType parentNodeType, ushort edgeSubType)
        {
            return DR_GraphEdges.RegisterEdgeType(edgeType, parentNodeType, NodeType.Goal, edgeSubType);
        }

        internal GoalEdge()
            : base()
        {
        }


        internal new DR_Goals ChildObject
        {
            get { return (DR_Goals)base.ChildObject; }
            //set { base.ChildObject = value; }
        }
    }
}
