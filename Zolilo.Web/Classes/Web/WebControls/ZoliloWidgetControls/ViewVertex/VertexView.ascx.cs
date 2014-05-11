using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Data;

namespace Zolilo.Web
{
    public partial class VertexView : System.Web.UI.UserControl
    {
        DR_GraphEdges vertexObject;

        protected override void OnInit(EventArgs e)
        {
            button_delete.Click += new EventHandler(button_delete_Click);
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            fragmentControl.Fragment = vertexObject.NodeDefinition;
            base.OnLoad(e);
        }

        void button_delete_Click(object sender, EventArgs e)
        {
            WebDirector.Instance.Redirect("/vertex/delete?id=" + vertexObject.ID.ToString());
        }

        public long VertexID 
        {
            set 
            { 
                vertexObject = DR_GraphEdges.Get(value);
                if (vertexObject == null)
                    divVertexView.Attributes.Add("style", "display: none");
                nodeParent.NodeID = vertexObject._IDPARENT;
                nodeParent.NodeType = vertexObject.ParentType;
                nodeChild.NodeID = vertexObject._IDCHILD;
                nodeChild.NodeType = vertexObject.ChildType;
            }
        }

    }
}