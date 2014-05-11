using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zolilo.Data;

namespace Zolilo.Web
{
    /// <summary>
    /// Contains the list of values which persist in the context of the current loaded page + 
    /// </summary>
    public class ZoliloPageSupervisorContext : ZoliloStateTable
    {
        public ZoliloStateTable Frames
        {
            get 
            {
                if (this["Frames"] == null)
                    this["Frames"] = new ZoliloStateTable();
                return (ZoliloStateTable)this["Frames"]; 
            }
        }
    }
}
