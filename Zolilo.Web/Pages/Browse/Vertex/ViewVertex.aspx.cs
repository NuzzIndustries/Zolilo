using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Web;

namespace Zolilo.Pages
{
    public partial class ViewVertex : ZoliloPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            
            base.OnPreInit(e);
        }

        protected override void OnInit(EventArgs e)
        {
            viewVertex.VertexID = QueryStringID;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}