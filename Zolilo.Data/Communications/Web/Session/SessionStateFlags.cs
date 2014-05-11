using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Zolilo.Data
{
    internal class SessionStateFlags
    {
        bool openIDNotLinked;

        public bool OpenIDNotLinked
        {
            get { return openIDNotLinked; }
            set { openIDNotLinked = value; }
        }

        internal SessionStateFlags()
        {
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SessionStateFlags");
            sb.AppendLine("OpenIDNotLinked: " + openIDNotLinked.ToString());
            return sb.ToString();
        }
    }
}