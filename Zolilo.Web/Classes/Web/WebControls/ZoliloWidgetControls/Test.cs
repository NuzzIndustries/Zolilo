using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zolilo.Web
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:Test runat=server></{0}:Test>")]
    [ParseChildren(false)]
    [PersistChildren(true)]
    public class Test : ZoliloWebControl
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            output.Write(Text);
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public DataControlFieldCollection Columns
        {
            get { return null; }
        }
    }
}
