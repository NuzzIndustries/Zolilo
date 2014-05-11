using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace Zolilo.Web
{
    public class ZoliloTabPanel : ZoliloContainerControl
    {
        public ZoliloTabPanel()
        {
        }

        internal void SetURL(string pagelocation)
        {
            Contents = "<div id=\"" + ID + "\" runat=\"server\" src=\"" + pagelocation + "\" style=\"position: relative; top: 0px;\"></div>";
        }

        public override void LoadState(object savedState)
        {
            throw new NotImplementedException();
        }

        public override object SaveState()
        {
            throw new NotImplementedException();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }
    }


    /// <summary>
    /// Placeholder data class for the template container
    /// http://msdn.microsoft.com/en-us/library/aa478964.aspx
    /// </summary>
    [ToolboxItem(false)]
    public class ZoliloTabPanelData : ZoliloWebControl, INamingContainer
    {
        public override void LoadState(object savedState)
        {
            throw new NotImplementedException();
        }

        public override object SaveState()
        {
            throw new NotImplementedException();
        }
    }
}