using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zolilo.Web
{
    public class AccountLoginControl : ZoliloWebControl, IZoliloWebControlHasDesigner
    {
        AccountLoginControl_D designer;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        public UserControl Designer
        {
            get
            {
                return designer;
            }
            set
            {
                this.designer = (AccountLoginControl_D)value;
            }
        }

        public string ControlPath
        {
            get
            {
                return "~/Classes/Web/WebControls/ZoliloWidgetControls/Account/AccountLoginControl.cs";
            }
        }
    }
}