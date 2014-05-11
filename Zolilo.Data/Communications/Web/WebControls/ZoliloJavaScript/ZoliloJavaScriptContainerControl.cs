using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zolilo.Web
{
    public class ZoliloJavaScriptContainerControl : ZoliloWebControl
    {
        public ZoliloJavaScriptContainerControl()
        {
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.WriteLine("<script type=\"text/javascript\">");
            foreach (Control c in Controls)
            {
                c.RenderControl(writer);
            }
            writer.WriteLine("</script>");
        }
    }
}