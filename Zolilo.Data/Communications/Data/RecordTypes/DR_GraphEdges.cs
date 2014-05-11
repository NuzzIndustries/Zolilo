using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

namespace Zolilo.Data
{
    public interface IParentFragmentEdge { }
    public interface IParentGoalEdge { }

    /// <summary>
    /// The type of the child object determines what datatype this is
    /// </summary>
    public abstract class DR_GraphEdges : GraphNode, IDataRecordContainsNoTimeModified, IGraphNodeHasDefinition<Edge2Fragment_Definition>
    {
        #region UniversalAbstractMethods
        public static DR_GraphEdges Get(long id)
        {
            return ZoliloCache.Instance.GraphEdges[id];
        }
        #endregion

        internal static Dictionary<long, Type> edgeTypes1 = new Dictionary<long, Type>();
        internal static Dictionary<Type, long> edgeTypes2 = new Dictionary<Type, long>();
        static bool edgeTypesInit = GetEdgeTypesDict();
        GraphNode parentObject;
        GraphNode childObject;

        #region indexes

        internal static List<DR_GraphEdges> Index_GetParentsOfNode(long nodeChildID)
        {
            return (List<DR_GraphEdges>)ZoliloCache.Instance.GraphEdges.Indexes["IDCHILD"].GetIndexValues(nodeChildID);
        }

        internal static List<DR_GraphEdges> Index_GetChildrenOfNode(long nodeParentID)
        {
            return (List<DR_GraphEdges>)ZoliloCache.Instance.GraphEdges.Indexes["IDPARENT"].GetIndexValues(nodeParentID);
        }

        internal static List<DR_GraphEdges> Index_GetChildrenOfNode(long nodeParentID, long edgeType)
        {
            return (List<DR_GraphEdges>)ZoliloCache.Instance.GraphEdges.Indexes["IDPARENT,EDGETYPE"].GetSubIndex(nodeParentID).GetIndexValues(edgeType);
        }

        internal static List<DR_GraphEdges> Index_GetParentsOfNode(long nodeChildID, long edgeType)
        {
            return (List<DR_GraphEdges>)ZoliloCache.Instance.GraphEdges.Indexes["IDCHILD,EDGETYPE"].GetSubIndex(nodeChildID).GetIndexValues(edgeType);
        }

        #endregion

        public DR_GraphEdges()
            : base()
        {
        }

        #region Columns

        public long _IDPARENT
        {
            get { return (long)Cells["IDPARENT"].Data; }
            set { Cells["IDPARENT"].Data = value; }
        }

        public long _IDCHILD
        {
            get { return (long)Cells["IDCHILD"].Data; }
            set { Cells["IDCHILD"].Data = value; }
        }

        /// <summary>
        /// This is convertable into a node-context-specific enum type
        /// </summary>
        public long _EDGETYPE
        {
            get { return (long)Cells["EDGETYPE"].Data; }
            set { Cells["EDGETYPE"].Data = value; }
        }

        public NodeType ParentType
        {
            get { return (NodeType)(_EDGETYPE >> 32); }
        }

        public NodeType ChildType
        {
            get { return (NodeType)(_EDGETYPE >> 16); }
        }
        #endregion

        public override NodeType NodeType
        {
            get { return Data.NodeType.GraphEdge; }
        }

        public override void SaveChanges()
        {
            if (ParentObject.Equals(ChildObject))
                throw new ZoliloSystemException("Connection parent and child must not be the same!");
            _EDGETYPE = (long)this.GetType().GetProperty("EdgeType").GetValue(this, null);
            base.SaveChanges();
        }

#region Static Functions
       
        private static bool GetEdgeTypesDict()
        {
            Dictionary<int, Type> dict = new Dictionary<int, Type>();
            Assembly assembly = typeof(DR_GraphEdges).Assembly;
            foreach (Type type in assembly.GetTypes())
            {
                if ((typeof(DR_GraphEdges).IsAssignableFrom(type)))
                {
                    if (!type.IsAbstract)
                        type.GetField("edgeType").GetValue(null);
                }
            }
            return true;
        }

        protected static long RegisterEdgeType(Type edgeType, NodeType parentNodeType, NodeType childNodeType, ushort edgeSubType)
        {
            long key = (((long)parentNodeType << 32) + ((long)childNodeType << 16) + (long)edgeSubType);
            edgeTypes1.Add(key, edgeType);
            edgeTypes2.Add(edgeType, key);
            return key;
        }

        internal static DR_GraphEdges ConstructBase(GraphNode parent, GraphNode child, Type edgeType)
        {
            DR_GraphEdges dr = (DR_GraphEdges)Activator.CreateInstance(edgeType);

            dr._EDGETYPE = dr.EdgeType;// (long)dr.GetType().GetProperty("EdgeType").GetValue(edge, null);
            dr._IDPARENT = parent.ID;
            dr._IDCHILD = child.ID;
            return dr;
        }

#endregion

#region Properties

        public string EdgeTypeString
        {
            get
            {
                return edgeTypes1[EdgeType].Name;
            }
        }

        public GraphNode ParentObject
        {
            get
            {
                if (parentObject == null)
                    parentObject = GraphNode.ResolveNode(_IDPARENT, ParentType);
                return parentObject;
            }
            set
            {
                if (parentObject != null)
                    throw new ZoliloSystemException("Cannot change edge after it has been created");
                parentObject = value;
                _IDPARENT = parentObject.ID;
            }
        }

        public GraphNode ChildObject
        {
            get
            {
                if (childObject == null)
                    childObject = GraphNode.ResolveNode(_IDCHILD, ChildType);
                return childObject;
            }
            set
            {
                if (childObject != null)
                    throw new ZoliloSystemException("Cannot change edge after it has been created");
                childObject = value;
                _IDCHILD = childObject.ID;
            }
        }

#endregion

        #region abstract
        
        public abstract long EdgeType { get; }

        public virtual Control GetContextControl()
        {
            PlaceHolder ph = new PlaceHolder();
            ph.Controls.Add(new LiteralControl("<br>"));
            ph.Controls.Add(new LiteralControl("<a href=\"/vertex/view?id=" + ID.ToString() + "\">View Connection</a><br>"));
            ph.Controls.Add(new LiteralControl("<a href=\"/vertex/delete?id=" + ID.ToString() + "\">Delete Connection</a><br>"));
            return ph;
        }


        #endregion

        public DR_Fragments NodeDefinition
        {
            get { return base.GetDefinition<Edge2Fragment_Definition>(); }
        }
    }
}