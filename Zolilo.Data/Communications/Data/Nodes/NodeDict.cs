using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zolilo.Data
{
    /// <summary>
    /// Contains meta data about a node
    /// </summary>
    internal static class NodeDict
    {
        static Dictionary<NodeType, NodeMetaData> byNodeType;
        static Dictionary<Type, NodeMetaData> byType;

        internal static void Init()
        {
            byNodeType = new Dictionary<NodeType, NodeMetaData>();
            byType = new Dictionary<Type, NodeMetaData>();

            NodeMetaData agent = new NodeMetaData();
            agent.Cache = ZoliloCache.Instance.Agents;
            agent.DrType = typeof(DR_Agents);
            //agent.EdgeType = typeof(AgentEdge);
            //agent.ParentEdgeInterfaceType = typeof(IParentAgentEdge);
            agent.GraphObjectType = typeof(DR_Agents);
            agent.TypeEnum = NodeType.Agent;
            byNodeType.Add(agent.TypeEnum, agent);
            byType.Add(agent.GraphObjectType, agent); 

            NodeMetaData fragment = new NodeMetaData();
            fragment.Cache = ZoliloCache.Instance.Fragments;
            fragment.DrType = typeof(DR_Fragments);
            fragment.EdgeType = typeof(FragmentEdge);
            fragment.ParentEdgeInterfaceType = typeof(IParentFragmentEdge);
            fragment.GraphObjectType = typeof(DR_Fragments);
            fragment.TypeEnum = NodeType.Fragment;
            byNodeType.Add(fragment.TypeEnum, fragment);
            byType.Add(fragment.GraphObjectType, fragment);

            NodeMetaData goal = new NodeMetaData();
            goal.Cache = ZoliloCache.Instance.Goals;
            goal.DrType = typeof(DR_Goals);
            goal.EdgeType = typeof(GoalEdge);
            goal.ParentEdgeInterfaceType = typeof(IParentGoalEdge);
            goal.GraphObjectType = typeof(DR_Goals);
            goal.TypeEnum = NodeType.Goal;
            byNodeType.Add(goal.TypeEnum, goal);
            byType.Add(goal.GraphObjectType, goal);

            NodeMetaData edge = new NodeMetaData();
            edge.Cache = ZoliloCache.Instance.GraphEdges;
            edge.DrType = typeof(DR_GraphEdges);
            //edge.EdgeType = typeof(EdgeEdge);
            //edge.ParentEdgeInterfaceType = typeof(IParentEdgeEdge);
            edge.GraphObjectType = typeof(DR_GraphEdges);
            edge.TypeEnum = NodeType.GraphEdge;
            byNodeType.Add(edge.TypeEnum, edge);
            byType.Add(edge.GraphObjectType, edge);

            NodeMetaData tag = new NodeMetaData();
            tag.Cache = ZoliloCache.Instance.Tags;
            tag.DrType = typeof(DR_Tags);
            //edge.EdgeType = typeof(EdgeEdge);
            //edge.ParentEdgeInterfaceType = typeof(IParentEdgeEdge);
            tag.GraphObjectType = typeof(DR_Tags);
            tag.TypeEnum = NodeType.Tag;
            byNodeType.Add(tag.TypeEnum, tag);
            byType.Add(tag.GraphObjectType, tag);
        }

        internal static Dictionary<Type, NodeMetaData> ByType
        {
            get { return byType; }
        }

        internal static Dictionary<NodeType, NodeMetaData> ByNodeType
        {
            get { return byNodeType; }
        }
    }

    internal class NodeMetaData
    {
        NodeType typeEnum;
        Type type;
        Type parentEdgeInterfaceType;
        Type edgeType;
        Type drType;
        IZoliloTableCache cache;

        public NodeType TypeEnum
        {
            get { return typeEnum; }
            set { typeEnum = value; }
        }

        public Type GraphObjectType
        {
            get { return type; }
            set { type = value; }
        }

        public Type ParentEdgeInterfaceType
        {
            get { return parentEdgeInterfaceType; }
            set { parentEdgeInterfaceType = value; }
        }

        public Type EdgeType
        {
            get { return edgeType; }
            set { edgeType = value; }
        }

        public Type DrType
        {
            get { return drType; }
            set { drType = value; }
        }

        internal IZoliloTableCache Cache
        {
            get { return cache; }
            set { cache = value; }
        }
    }
}