using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zolilo.Web
{
    public class ZoliloJavaScriptImportControl : ZoliloWebControl
    {
        string src;

        /// <summary>
        /// The path to the source of the script
        /// </summary>
        public string Src
        {
            get { return src; }
            set { src = value; }
        } 

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            writer.WriteLine("<script type=\"text/javascript\" src=\"" + src + "\"></script>");
        }
    }

}