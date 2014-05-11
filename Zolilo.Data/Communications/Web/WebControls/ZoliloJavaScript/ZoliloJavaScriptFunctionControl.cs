using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zolilo.Web
{
    public class ZoliloJavaScriptFunctionControl : ZoliloWebControl
    {
        string prototype;

        /// <summary>
        /// Gets or sets the prototype [e.g. name(param1, param2)] of the function
        /// </summary>
        public string Prototype
        {
            get { return prototype; }
            set { prototype = value; }
        }

        string codeBody;

        /// <summary>
        /// Gets or sets the body of code to be used within the function
        /// </summary>
        public string CodeBody
        {
            get { return codeBody; }
            set { codeBody = value; }
        }

        public ZoliloJavaScriptFunctionControl(string prototype)
        {
            this.Prototype = prototype;
            this.codeBody = "";
        }

        public ZoliloJavaScriptFunctionControl(string prototype, string codeBody)
            : this(prototype)
        {
            this.codeBody = codeBody;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.WriteLine("function " + prototype);
            writer.WriteLine("{");
            writer.WriteLine(codeBody);
            writer.WriteLine("}");
        }
    }
}