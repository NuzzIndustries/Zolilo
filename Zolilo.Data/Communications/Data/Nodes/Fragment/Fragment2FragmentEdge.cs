using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zolilo.Data
{
    public abstract class Fragment2FragmentEdge : FragmentEdge, IParentFragmentEdge
    {
        protected static long RegisterEdgeType(Type type, ushort subType)
        {
            return FragmentEdge.RegisterEdgeType(type, NodeType.Fragment, subType);
        }

        public Fragment2FragmentEdge()
        {
        }

        public new DR_Fragments ParentObject
        {
            get { return (DR_Fragments)base.ParentObject; }
        }
    }
}
