using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zolilo.Web
{
    public class ZoliloPageFrameContext
    {
        string frameID;
        string lastURL;
        string nextURL;
        string savedURL;

        public ZoliloPageFrameContext(string frameID)
            : base()
        {
            this.frameID = frameID;
        }

        public string FrameID
        {
            get { return this.frameID; }
        }

        /// <summary>
        /// Gets the previous page that was navigated with this frame
        /// </summary>
        public string LastURL
        {
            get { return this.lastURL; }
            internal set { this.lastURL = value; }
        }

        /// <summary>
        /// Gets or sets an arbitrary URL to save for later redirection.
        /// This should be set to = LastURL property when the page is the first URL 
        /// in a series of pages that will end up using the SavedURL property
        /// </summary>
        public string SavedURL
        {
            get { return this.savedURL; }
            internal set { this.savedURL = value; }
        }

        /// <summary>
        ///  Sets the next URL to be placed into the "LastURL" variable at the beginning of the next request
        /// </summary>
        internal string NextURL
        {
            get { return this.nextURL; }
            set { this.nextURL = value; }
        }
    }
}
