using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zolilo.Data
{
    public class DR_Tags : GraphNode, IGraphNodeHasDefinition<Tag2Fragment_Definition>
    {
        #region UniversalAbstractMethods
        public static DR_Tags Get(long id)
        {
            return ZoliloCache.Instance.Tags[id];
        }
        #endregion

        DR_Fragments definition;

        public override NodeType NodeType
        {
            get { return NodeType.Tag; }
        }

        public string _Name
        {
            get { return (string)Cells["NAME"]; }
            set { Cells["NAME"].Data = value; }
        }

        public DR_Fragments NodeDefinition
        {
            get { return base.GetDefinition<Tag2Fragment_Definition>(); }
        }
    }
}
