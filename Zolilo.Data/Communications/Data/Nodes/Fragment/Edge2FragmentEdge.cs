using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zolilo.Data
{
    public interface IParentEdgeEdge { }

    public abstract class Edge2FragmentEdge : FragmentEdge, IParentEdgeEdge
    {
        protected static long RegisterEdgeType(Type type, Edge2FragmentEdgeType subType)
        {
            return FragmentEdge.RegisterEdgeType(type, NodeType.GraphEdge, (ushort)subType);
        }
    }

    public enum Edge2FragmentEdgeType : ushort
    {
        None = 0,
        Definition = 1
    }
}
