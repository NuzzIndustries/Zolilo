using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zolilo.Data
{
    public class DR_OpenIDMap : TimestampRecord
    {
        internal static DR_OpenIDMap Get(long id)
        {
            return ZoliloCache.Instance.OpenIDMap[id];
        }

        public DR_OpenIDMap()
            : base()
        {
        }

        public string _OpenIdentifier
        {
            get { return (string)Cells["OPENIDENTIFIER"]; }
            set { Cells["OPENIDENTIFIER"].Data = value; }
        }

        public long _AccountID
        {
            get { return (long)Cells["ACCOUNTID"]; }
            set { Cells["ACCOUNTID"].Data = value; }
        }

    }
}