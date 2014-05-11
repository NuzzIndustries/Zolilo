using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zolilo.Data
{
    public abstract class Metric2FragmentEdge : FragmentEdge
    {
        protected static long RegisterEdgeType(Type type, Metric2FragmentEdgeType subType)
        {
            return FragmentEdge.RegisterEdgeType(type, NodeType.Metric, (ushort)subType);
        }
    }


    public enum Metric2FragmentEdgeType : ushort
    {
        None = 0,
        Definition = 1,
    }
}
