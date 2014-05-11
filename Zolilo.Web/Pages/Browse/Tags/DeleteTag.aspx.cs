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
    public partial class DeleteTag : ZoliloPage
    {
        protected override void OnInit(EventArgs e)
        {
            tagView.Tag = DR_Tags.Get(QueryStringID);
            buttonDelete.Click += new EventHandler(buttonDelete_Click);
            base.OnInit(e);
        }

        void buttonDelete_Click(object sender, EventArgs e)
        {
            tagView.Tag.DeletePermanently();
            WebDirector.Instance.Redirect("/tags/browse");
        }
    }
}