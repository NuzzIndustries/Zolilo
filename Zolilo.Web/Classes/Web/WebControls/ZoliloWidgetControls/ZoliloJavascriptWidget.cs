using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Zolilo.Web
{
    public abstract class ZoliloJavascriptWidget : ZoliloWebControl, IJavaScriptObject
    {
        protected string className;

        public string GetConstructorExpression()
        {
            string c = "new " + className + "('" + "div" + "','" + GetClientID() + "');";
            return c;
        }

        public string GetClientID()
        {
            return divMain.ClientID;
        }

        protected HtmlGenericControl divMain;

        public ZoliloJavascriptWidget()
        {
            divMain = new HtmlGenericControl("div");
            divMain.Attributes.Add("class", "zWidget");
        }

        protected override void OnInit(EventArgs e)
        {
            divMain.ID = this.ID + "_div";
            Controls.Add(divMain);
            base.OnInit(e);
        }

    }
}