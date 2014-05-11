using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zolilo.Web
{
    /// <summary>
    /// Place this on a page to indicate that the page is a supervisor
    /// </summary>
    public class ZoliloSupervisorControl : ZoliloWebControl
    {
        int key = -1;
        ZoliloPageSupervisorContext context = new ZoliloPageSupervisorContext();

        public ZoliloSupervisorControl()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            this.ID = "zSupervisor" + key.ToString();
            base.OnLoad(e);
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            writer.WriteLine(Page.Snippets.HiddenField("zolilohf_sessionkey", int.Parse(Page.InstanceID).ToString()));
            writer.WriteLine(Page.Snippets.HiddenField("zolilohf_supervisorurl", Page.Request.Url.ToString()));
            writer.WriteLine(Page.Snippets.HiddenField("zolilohf_supervisorid", this.ID));
            writer.WriteLine(Page.Snippets.Javascript.BeginJavascript);
           // writer.WriteLine(this.ID + " = new ZoliloSupervisor('" + this.ID + "');");
            writer.WriteLine(Page.Snippets.Javascript.EndJavascript);
            base.Render(writer);
        }

        public int Key 
        {
            get 
            {
                if (key < 0)
                    throw new InvalidOperationException("Supervisor key is not set");
                return key; 
            }
            set { this.key = value; }
        }

        public ZoliloPageSupervisorContext Context
        {
            get { return context; }
        }
    }
}