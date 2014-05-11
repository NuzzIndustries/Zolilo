using System;
using Zolilo.Data;

namespace Zolilo.Web
{
    public class NodeViewMini : ZoliloWebControl
    {
        long nodeID;
        NodeType nodeType;

        GraphNode node;
        ZoliloWebControl viewMorph;

        public NodeViewMini() : base()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            node = Node;
            if (node is DR_Goals)
            {
                viewMorph = new GoalViewMini((DR_Goals)node);
            }
            else if (node is DR_Fragments)
            {
                viewMorph = new FragmentViewMini((DR_Fragments)node);
            }
            else if (node is DR_Tags)
            {
                viewMorph = new TagViewMini((DR_Tags)node);
            }
            else if (node is DR_GraphEdges)
            {
                viewMorph = new EdgeViewMini((DR_GraphEdges)node);
            }
            else
                throw new NotImplementedException("Not implemented: NodeViewMini(" + node.GetType().ToString() + ")");
            
            Controls.Add(viewMorph);
            base.OnLoad(e);
        }

        public long NodeID
        {
            set { nodeID = value; }
        }

        public NodeType NodeType
        {
            set { nodeType = value; }
        }


        private GraphNode Node
        {
            get { return GraphNode.ResolveNode(nodeID, nodeType); }
        }
    }
}