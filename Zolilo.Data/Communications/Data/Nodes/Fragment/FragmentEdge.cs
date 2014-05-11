using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zolilo.Data;

namespace Zolilo.Data
{
    
    public abstract class FragmentEdge : DR_GraphEdges
    {
        protected static long RegisterEdgeType(Type type, NodeType parentNodeType, ushort subType)
        {
            return DR_GraphEdges.RegisterEdgeType(type, parentNodeType, NodeType.Fragment, subType);
        }

        internal FragmentEdge()
            : base()
        {
        }


        public new DR_Fragments ChildObject
        {
            get { return (DR_Fragments)base.ChildObject; }
            set { base.ChildObject = value; }
        }
    }
}
