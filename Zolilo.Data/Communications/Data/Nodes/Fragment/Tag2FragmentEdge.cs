using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zolilo.Data
{
    public abstract class Tag2FragmentEdge : FragmentEdge
    {
        protected static long RegisterEdgeType(Type type, Tag2FragmentEdgeType subType)
        {
            return FragmentEdge.RegisterEdgeType(type, NodeType.Tag, (ushort)subType);
        }
    }

    public enum Tag2FragmentEdgeType : ushort
    {
        None = 0,
        Definition = 1,
    }
}
