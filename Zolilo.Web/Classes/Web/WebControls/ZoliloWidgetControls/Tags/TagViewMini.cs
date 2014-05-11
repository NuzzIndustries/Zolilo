using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zolilo.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zolilo.Web
{
    public class TagViewMini : ZoliloWebControl
    {
        DR_GraphEdges contextualEdge;

        DR_Tags tag;

        public TagViewMini()
        {
        }

        public TagViewMini(DR_Tags tag)
        {
            this.tag = tag;
        }

        protected override void OnLoad(EventArgs e)
        {
            Controls.Add(new LiteralControl(
                "<font face=\"Courier New\">" + 
                "<a href=\"/tags/view?id=" + tag.ID.ToString() + "\">" + tag._Name + "</a></font>"));
            if (contextualEdge != null)
            {
                Controls.Add(new LiteralControl("<br>"));
                Controls.Add(contextualEdge.GetContextControl());
            }
            base.OnLoad(e);
        }

        public DR_Tags Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public DR_GraphEdges ContextualEdge
        {
            get { return contextualEdge; }
            set { contextualEdge = value; }
        }
    }
}