using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zolilo.Data
{
    public class Fragment2Fragment_Reply : Fragment2FragmentEdge
    {
        public static long edgeType = edgeType == 0 ? RegisterEdgeType(typeof(Fragment2Fragment_Reply), 1) : edgeType;

        public override long EdgeType
        {
            get { return edgeType; }
        }

        public Fragment2Fragment_Reply() : base() { }

        public override System.Web.UI.Control GetContextControl()
        {
            throw new NotImplementedException();
        }
    }
}
