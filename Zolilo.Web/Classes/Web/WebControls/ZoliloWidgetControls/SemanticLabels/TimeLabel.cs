using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Zolilo.Data;

namespace Zolilo.Web
{
    public class TimeLabel : ZoliloWebControl
    {
        DateTime utcTime;
        DateTime timeToRender;

        protected override void OnLoad(EventArgs e)
        {
            if (this.Enabled)
            {
                if (utcTime == new DateTime())
                    throw new ZoliloWebException("Either UtcTime is not set on TimeLabel or timestamp was initialized with default value from DB.");
                if (zContext.Account != null)
                    timeToRender = utcTime.AddHours(zContext.Account._TimeZoneOffset);
                else
                    timeToRender = utcTime;
                Controls.Add(new LiteralControl("<font face=\"courier\">" + timeToRender.ToString() + "</font>"));
            }
            base.OnLoad(e);
        }


        public DateTime UtcTime
        {
            get { return utcTime; }
            set { utcTime = value; }
        }
    }
}