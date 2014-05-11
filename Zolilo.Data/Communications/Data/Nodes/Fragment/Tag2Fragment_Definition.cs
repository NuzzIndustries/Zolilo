using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Zolilo.Data
{
    public class Tag2Fragment_Definition : Tag2FragmentEdge
    {
        public static long edgeType = edgeType == 0 ? 
            RegisterEdgeType(
            typeof(Tag2Fragment_Definition), 
            Tag2FragmentEdgeType.Definition) 
            : edgeType;

        public override long EdgeType
        {
            get { return edgeType; }
        }

        public override Control GetContextControl()
        {
            return null;
        }
    }
}
