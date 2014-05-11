using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zolilo.Data
{
    public class Metric2Fragment_Definition : Metric2FragmentEdge
    {
        public static long edgeType = edgeType == 0 ? RegisterEdgeType(typeof(Metric2Fragment_Definition), Metric2FragmentEdgeType.Definition) : edgeType;

        public override long EdgeType
        {
            get { return edgeType; }
        }
    }
}
