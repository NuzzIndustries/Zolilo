using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace Zolilo.Data
{
    public interface IGraphNodeNoAgentCreatedColumn { }

    /// <summary>
    /// Represents a database record that has a 1 to 1 relationship with a GraphNode
    /// </summary>
    public abstract class GraphNode : TimestampRecord
    {
        //public abstract NodeType NodeType { get; }

        EdgeCollection<DR_GraphEdges> allParentEdges, allChildEdges;
        Dictionary<Type, IEdgeCollection> parentEdges, childEdges;

        internal GraphNode() { }

        #region Static Initializers
        static Dictionary<Type, NodeType> typeDict = BuildTypeDict();
        private static Dictionary<Type, NodeType> BuildTypeDict()
        {
            Dictionary<Type, NodeType> d = new Dictionary<Type, NodeType>();

            d.Add(typeof(DR_Accounts), NodeType.Account);
            d.Add(typeof(DR_Agents), NodeType.Agent);
            d.Add(typeof(DR_Fragments), NodeType.Fragment);
            d.Add(typeof(DR_Goals), NodeType.Goal);
            d.Add(typeof(DR_GraphEdges), NodeType.GraphEdge);
            d.Add(typeof(DR_Tags), NodeType.Tag);
            

            return d;
        }
        #endregion

        #region static constructors
        private static GraphNode FromCache(long id)
        {
            throw new InvalidOperationException("FromCache: Calling this method on base class is not allowed.");
        }

        public static GraphNode ResolveNode(long id, NodeType nodeType)
        {
            object o = NodeDict.ByNodeType[nodeType].GraphObjectType.InvokeMember("Get", BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.Public, null, null, new object[] { id });
            if (o == null)
                return null;
            return (GraphNode)o;
        }

        /// <summary>
        /// Attaches a node to this node and automatically commits the changes to the database
        /// </summary>
        /// <param name="childNode"></param>
        /// <returns></returns>
        internal DR_GraphEdges AttachChildNodeWithNewEdge(GraphNode childNode, Type edgeType)
        {
            NodeType type = GraphNode.ResolveNodeType(childNode);
            DR_GraphEdges edge = DR_GraphEdges.ConstructBase(this, childNode, edgeType);
            //IEdgeCollection collection = GetChildEdgeList(edgeType);
            edge.SaveChanges();
            //collection.AddEdge(edge);
            return edge;
        }

        public override void SaveChanges()
        {
            if (!(this is IGraphNodeNoAgentCreatedColumn) && Cells["IDAGENTCREATED"].Data == null)
                _IDAgentCreated = ZoliloSession.Current.Agent.ID;
            base.SaveChanges();
            //object o = NodeHandle; //Ensure that handle exists
        }

        public List<DR> GetParents<DR, TEdge>()
            where TEdge : DR_GraphEdges
            where DR : GraphNode
        {
            EdgeCollection<TEdge> collectionCopy = GetParentEdgeList<TEdge>().GetTemporaryCopy();
            List<DR> parents = new List<DR>(collectionCopy.Count + 10);
            foreach (TEdge edge in collectionCopy)
            {
                parents.Add((DR)edge.ParentObject);
            }
            return parents;
        }

        public List<DR> GetChildren<DR, TEdge>()
            where TEdge : DR_GraphEdges
            where DR : GraphNode
        {
            EdgeCollection<TEdge> collectionCopy = GetChildEdgeList<TEdge>().GetTemporaryCopy();
            List<DR> children = new List<DR>(collectionCopy.Count + 10);
            foreach (TEdge edge in collectionCopy)
            {
                children.Add((DR)edge.ChildObject);
            }
            return children;
        }

        internal EdgeCollection<T> GetParentEdgeList<T>() where T : DR_GraphEdges
        { 
            EdgeCollection<DR_GraphEdges> protolist = new EdgeCollection<DR_GraphEdges>(DR_GraphEdges.Index_GetParentsOfNode(this.ID, DR_GraphEdges.edgeTypes2[typeof(T)]));
            return protolist.Convert<T>();
        }

        internal EdgeCollection<T> GetChildEdgeList<T>() where T : DR_GraphEdges
        {
            EdgeCollection<DR_GraphEdges> protolist = new EdgeCollection<DR_GraphEdges>(DR_GraphEdges.Index_GetChildrenOfNode(this.ID, DR_GraphEdges.edgeTypes2[typeof(T)]));
            return protolist.Convert<T>();
        }

        #endregion

        #region Properties

        public DR_Agents Agent
        {
            get { return DR_Agents.Get(_IDAgentCreated); }
        }

        public virtual long _IDAgentCreated
        {
            get { return (long)Cells["IDAGENTCREATED"].Data; }
            protected set { Cells["IDAGENTCREATED"].Data = value; }
        }

        public abstract NodeType NodeType { get; }

        internal EdgeCollection<DR_GraphEdges> AllParentEdges
        {
            get
            {
                if (allParentEdges == null)
                    return new EdgeCollection<DR_GraphEdges>(DR_GraphEdges.Index_GetParentsOfNode(this.ID)); 
                return allParentEdges;
            }
        }

        internal EdgeCollection<DR_GraphEdges> AllChildEdges
        {
            get
            {
                if (allChildEdges == null)
                    return new EdgeCollection<DR_GraphEdges>(DR_GraphEdges.Index_GetChildrenOfNode(this.ID));
                return allChildEdges;
            }
        }


        /// <summary>
        /// Returns the single parent connection if one exists.  Assumes this property will be used
        /// in the context of an object where only a single parent exists
        /// </summary>
        public GraphNode SingularParent
        {
            get
            {
                if (AllParentEdges.Count == 1)
                    return AllParentEdges[0].ParentObject;
                return null;
            }
        }

        public GraphNode SingularChild
        {
            get
            {
                if (AllChildEdges.Count == 1)
                    return  AllChildEdges[0].ChildObject;
                return null;
            }
        }

        //internal List<DR_GraphNodes>

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            if (!(obj is GraphNode))
                return false;
            return this.GetType() == obj.GetType() && this.ID == ((GraphNode)obj).ID;
        }

        /// <summary>
        /// Provide a valid Type that inherits from GraphNode and this function returns a corresponding NodeType enum
        /// which can then be used as an input for the EdgeTable[NodeType] to get a list of edges of a given child type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static NodeType ResolveNodeType(GraphNode node)
        {
            Type t = node.GetType();
            if (node is DR_GraphEdges)
                t = typeof(DR_GraphEdges);
            return typeDict[t];
        }

        public override void DeletePermanently()
        {
            AllParentEdges.DeleteAllEdgesPermanently();
            AllChildEdges.DeleteAllEdgesPermanently();
            base.DeletePermanently();
        }
        #endregion

        protected DR_Fragments GetDefinition<TEdge>() where TEdge : FragmentEdge
        {
            DR_Fragments nodeDefinition;
            if (!(this is IGraphNodeHasDefinition<TEdge>))
                throw new ZoliloSystemException("Attempt to call GetDefinition<TEdge> from wrong class");

            EdgeCollection<TEdge> list = GetChildEdgeList<TEdge>();
            if (list.Count > 0)
                nodeDefinition = list[0].ChildObject;
            else
                nodeDefinition = DR_Fragments.CreateNewNodeDefinition<TEdge>((IGraphNodeHasDefinition<TEdge>)this);

            return nodeDefinition;
        }
    }


    public enum NodeType : ushort
    {
        None = 0,
        Goal = 1,
        Fragment = 2,
        Agent = 3,
        GraphEdge = 4,
        Account = 5,
        Metric = 6,
        Tag = 7,
    }
}
