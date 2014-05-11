using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Data;

namespace Zolilo.Web
{
    public class EdgeViewMini : ZoliloWebControl
    {
        DR_GraphEdges edge;

        public EdgeViewMini() { }

        public EdgeViewMini(DR_GraphEdges edge)
        {
            this.edge = edge;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            LiteralControl text = new LiteralControl("Connection type: " + edge.EdgeTypeString);
            Controls.Add(text);
            base.OnLoad(e);
        }

        public DR_GraphEdges Edge
        {
            get { return edge; }
            set { edge = value; }
        }
    }
}