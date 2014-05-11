using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Zolilo.Data
{
    public class Goal2Fragment_GoalDefinition : Goal2FragmentEdge
    {
        public static long edgeType = edgeType == 0 ? RegisterEdgeType(typeof(Goal2Fragment_GoalDefinition), Goal2FragmentEdgeType.GoalDescription) : edgeType;

        public override long EdgeType
        {
            get { return edgeType; }
        }
    }
}
