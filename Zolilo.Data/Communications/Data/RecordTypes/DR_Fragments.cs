using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zolilo.Data
{
    public interface IGraphNodeHasDefinition<TEdge> where TEdge : FragmentEdge
    {
        DR_Fragments NodeDefinition { get; }
    }

    public class DR_Fragments : GraphNode
    {
        #region UniversalAbstractMethods
        public static DR_Fragments Get(long id)
        {
            return ZoliloCache.Instance.Fragments[id];
        }
        #endregion

        public DR_Fragments() : base()
        {
        }

        internal static DR_Fragments CreateNewNodeDefinition<TEdge>(IGraphNodeHasDefinition<TEdge> node) 
            where TEdge : FragmentEdge
        {
            DR_Fragments f = new DR_Fragments();
            f._IDAgentCreated = ((GraphNode)node)._IDAgentCreated;
            f._BaseWeight = FragmentFeedback.Positive;
            f.SaveChanges();

            ((GraphNode)node).AttachChildNodeWithNewEdge(f, typeof(TEdge));
            return f;
        }

        public bool IsGoalDefinition
        {
            get { return GetParentEdgeList<Goal2Fragment_GoalDefinition>().Count > 0; }
        }

        public override void DeletePermanently()
        {
            if (IsGoalDefinition && SingularParent != null)
                throw new ZoliloWebException("Cannot delete goal definition.");
            foreach (Fragment2Fragment_Reply edge in GetChildEdgeList<Fragment2Fragment_Reply>())
            {
                edge.ChildObject.DeletePermanently();
            }
            LRA.DeletePermanently();
            base.DeletePermanently();
        }

        public override void SaveChanges()
        {
            object l = _LRA; //ensure that LRA ID Exists
            base.SaveChanges();
            LRA.SaveChanges();
        }

        public DR_Agents AgentNode
        {
            get { return DR_Agents.Get(_IDAgentCreated); }
        }

        public DR_FragmentLRA LRA
        {
            get
            {
                if (_LRA <= 0)
                {
                    DR_FragmentLRA lra = new DR_FragmentLRA();
                    lra.SaveChanges();
                    _LRA = lra.ID;
                }
                return DR_FragmentLRA.Get(_LRA);
            }
        }

        public string Text
        {
            get
            {
                return LRA._Text;
            }
            set { LRA._Text = value; }
        }



        public FragmentFeedback _BaseWeight
        {
            get { return (FragmentFeedback)Cells["BASEWEIGHT"].Data; }
            set { Cells["BASEWEIGHT"].Data = (short)value; }
        }

        public long _LRA
        {
            get 
            {
                if (Cells["LRA"].Data == null)
                    return -1;
                return (long)Cells["LRA"].Data; 
            }
            set { Cells["LRA"].Data = value; }
        }

        public override NodeType NodeType
        {
            get { return NodeType.Fragment; }
        }
    }

    public enum FragmentFeedback : short
    {
        None = 0,
        Positive = 1,
        Negative = -1
    }
}