using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zolilo.Data
{
    public class DR_Goals : GraphNode, IGraphNodeHasDefinition<Goal2Fragment_GoalDefinition>
    {
        #region UniversalAbstractMethods
        public static DR_Goals Get(long id)
        {
            return ZoliloCache.Instance.Goals[id];
        }
        #endregion

        public string _Name
        {
            get { return (string)Cells["NAME"]; }
            set { Cells["NAME"].Data = value; }
        }


        public override string SQLStatus
        {
            get
            {
                switch (SQLCode)
                {
                    case 23505:
                        return "The goal named '" + _Name + "' already exists.";
                    default:
                        return base.SQLStatus;
                }
            }
        }

        public override NodeType NodeType
        {
            get { return Data.NodeType.Goal; }
        }

        #region fields
        

        #endregion

        internal void AddChildGoal(DR_Goals goal, Type edgeType)
        {
            AttachChildNodeWithNewEdge(goal, edgeType);
        }

        internal void AddChildFragment(DR_Fragments fragment, Type edgeType)
        {
            AttachChildNodeWithNewEdge(fragment, edgeType);
        }

        public DR_Fragments NodeDefinition
        {
            get { return base.GetDefinition<Goal2Fragment_GoalDefinition>(); }
        }

        public override void DeletePermanently()
        {
            DR_Fragments f = NodeDefinition;
            base.DeletePermanently();
            f.DeletePermanently();
        }

        internal static DR_Goals CreateNewFromForm(string name, long agentID, DR_Goals parentNode)
        {
            DR_Goals g = new DR_Goals();
            g._Name = name;
            g.SaveChanges();

            if (parentNode != null)
                parentNode.AddChildGoal(g, typeof(Goal2Goal_Achieve));

            return g;
        }

        public DR_Agents AgentCreated
        {
            get { return DR_Agents.Get(_IDAgentCreated); }
        }


    }
}