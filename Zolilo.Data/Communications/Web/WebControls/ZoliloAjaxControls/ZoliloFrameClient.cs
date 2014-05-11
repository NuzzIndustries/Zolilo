using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Zolilo.Web
{
    /// <summary>
    /// Place this on a page in order to allow this page to be displayed in a ZoliloFrame
    /// </summary>
    public class ZoliloFrameClient : ZoliloWebControl
    {
        public ZoliloFrameClient()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (Page.UFrameID != null)
            {
                writer.WriteLine(Page.Snippets.Javascript.BeginJavascript);
                if (Page.SupervisorControl != null)
                    writer.WriteLine(Page.SupervisorControl.ID + "_obj.hookLinks(" + Page.UFrameID + ".id);");
                else
                    writer.WriteLine("location.reload(true);");
                writer.WriteLine("hookPostBack();");
                writer.WriteLine(Page.Snippets.Javascript.EndJavascript);
            }
            base.Render(writer);
        }
    }
}