using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Zolilo.Data;
using Zolilo.Web;


namespace Zolilo.Pages
{
    public partial class SiteInfo : ZoliloPage, IRestrictedPage
    {
        protected override void OnInit(EventArgs e)
        {
            buttonDeleteAll.Click += new EventHandler(buttonDeleteAll_Click);
            base.OnInit(e);
        }

        void buttonDeleteAll_Click(object sender, EventArgs e)
        {
            List<DataRecord> list = new List<DataRecord>();
            foreach (DR_GraphEdges edge in ZoliloCache.Instance.GraphEdges.Values)
            {
                list.Add(edge);
            }
            /*
            foreach (DR_GraphNodes node in ZoliloCache.Instance.g)
            {
                list.Add(node);
            }
             * */
            foreach (DR_Goals goal in ZoliloCache.Instance.Goals)
            {
                list.Add(goal);
            }
            foreach (DataRecord record in list)
            {
                record.DeletePermanently();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            TextBoxInfo.Text = GenerateInfo();
            ScriptManager.GetCurrent(Page);
        }

        private static string GenerateInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SessionID: " + HttpContext.Current.Session.SessionID);
            sb.AppendLine();
            sb.AppendLine(zContext.Session.ToString());
            return sb.ToString();
        }
    }
}