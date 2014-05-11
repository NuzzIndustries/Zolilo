using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Web;
using Zolilo.Data;

namespace Zolilo.Pages
{
    public partial class ViewTag : ZoliloPage
    {
        protected override void OnInit(EventArgs e)
        {
            tagView.Tag = DR_Tags.Get(QueryStringID);
            base.OnInit(e);
        }
    }
}