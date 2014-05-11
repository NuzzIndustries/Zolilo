using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zolilo.Web
{
    /// <summary>
    /// Contains a piece of JavaScript code
    /// </summary>
    public class ZoliloJavaScriptControl : ZoliloWebControl
    {
        string code;
        public const string JAVASCRIPT_BEGIN_TAG = "<script type=\"text/javascript\">\r\n";
        public const string JAVASCRIPT_END_TAG = "</script>\r\n";

        /// <summary>
        /// Gets or sets the javascript code to be rendered
        /// </summary>
        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public ZoliloJavaScriptControl()
        {
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.WriteLine(code);
        }

        /// <summary>
        /// Surrounds the javascript 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string CreateEnclosingScript(string code)
        {
            return JAVASCRIPT_BEGIN_TAG + code + Environment.NewLine + JAVASCRIPT_END_TAG;
        }
    }

    public class Snippets
    {
        ZoliloPage page;
        JavaScriptSnippets javascript;

        public JavaScriptSnippets Javascript
        {
            get { return javascript; }
        }

        public ZoliloPage Page
        {
            get { return page; }
        }

        public Snippets(ZoliloPage page)
        {
            this.page = page;
            this.javascript = new JavaScriptSnippets(this);
        }

        public string ForcePostback
        {
            get
            {
                return Page.CallbackObjectName + ".CompileZoliloViewState();\r\n" +
                "document.forms[0].submit();";
            }
        }

        internal string UpdateZoliloViewState(string key, string value)
        {
            return Page.CallbackObjectName + ".UpdateZoliloViewState('" + key + "','" + value + "');";
        }



        //public string EndJavascript

        public class JavaScriptSnippets
        {
            ZoliloPage page;
            Snippets snippets;

            public JavaScriptSnippets(Snippets snippets)
            {
                this.snippets = snippets;
                this.page = snippets.page;
            }

            public string BeginJavascript
            {
                get { return "<script type=\"text/javascript\">"; }
            }

            public string EndJavascript
            {
                get { return "</script>"; }
            }
        }
        
        /// <summary>
        /// Generates a hidden field tag
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        internal string HiddenField(string id, string value)
        {
            return "<input type=\"hidden\" id=\"" + id + "\" value=\"" + value + "\">";
        }
    }

   
}