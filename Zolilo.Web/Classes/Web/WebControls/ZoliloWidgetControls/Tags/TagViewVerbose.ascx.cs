using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Data;

namespace Zolilo.Web
{
    public partial class TagViewVerbose : System.Web.UI.UserControl
    {
        DR_Tags tag;

        protected override void OnInit(EventArgs e)
        {
            buttonDelete.Click += new EventHandler(buttonDelete_Click);
            
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            tagDescription.Fragment = tag.NodeDefinition;
            tagViewMini.Tag = Tag;
            createdBy.Agent = Tag.Agent;
            base.OnLoad(e);
        }

        void buttonDelete_Click(object sender, EventArgs e)
        {
            WebDirector.Instance.Redirect("/tags/delete?id=" + tag.ID.ToString());
        }

        public DR_Tags Tag
        {
            get { return tag; }
            set { tag = value; }
        }
    }
}