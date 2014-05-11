using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zolilo.Data
{
    public class Edge2Fragment_Definition : Edge2FragmentEdge
    {
        public static long edgeType = edgeType == 0 ? RegisterEdgeType(typeof(Edge2Fragment_Definition), Edge2FragmentEdgeType.Definition) : edgeType;

        public override long EdgeType
        {
            get { return edgeType; }
        }
    }
}
