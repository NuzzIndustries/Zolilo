using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Zolilo.Web
{
    public partial class ZoliloButton : ZoliloWebControl, IZoliloViewStateControl
    {
        public event EventHandler Click;

        public ZoliloButton()
        {
            Click += new EventHandler(ZoliloButton_Clicked);
        }

        void ZoliloButton_Clicked(object sender, EventArgs e)
        {
            this.Enabled = false;
        }

        protected override void OnInit(EventArgs e)
        {
            Page.RegisterRequiresControlState(this);
            base.PostBackTrigger += new ZoliloWebControlPostbackHandler(ZoliloButton_PostBackTrigger);
            base.OnInit(e);
        }

        void ZoliloButton_PostBackTrigger(string args)
        {
            if (Click != null)
                Click(this, null);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write("<input name=\"" + this.UniqueID + "\" type=\"button\" id=\"" + this.ID + "\" value=\"" + this.text + "\" " +
                "onclick=\"this.disabled = true;__doPostBack('" + this.UniqueID + "','" + "" +"');\" />");
            base.Render(writer);
        }

        
        public void LoadState(object savedState)
        {
            object[] state = (object[])savedState;
            text = (string)state[0];
        }

        public object SaveState()
        {
            object[] o = new object[1];
            o[0] = this.text;
            return o;
        }

        string text = "";

        [Browsable(true)]
        public string Text
        {
            get { return text; }
            set { this.text = value; }
        }
    }
}