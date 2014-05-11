using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zolilo.Data
{
    internal interface IDataRecordContainsNoTimeModified {}

    public abstract class TimestampRecord : DataRecord
    {
        protected override int InsertNew()
        {
            TimeModifiedUTC = DateTime.UtcNow;
            TimeCreatedUTC = DateTime.UtcNow;
            return base.InsertNew();
        }

        protected override int UpdateToDB()
        {
            TimeModifiedUTC = DateTime.UtcNow;
            return base.UpdateToDB();
        }

        public DateTime TimeModifiedUTC
        {
            get
            {
                if (this is IDataRecordContainsNoTimeModified)
                    return TimeCreatedUTC;
                return ((DateTime)Cells["TIMEMODIFIED"]);
            }
            set 
            {
                if (!(this is IDataRecordContainsNoTimeModified))
                    Cells["TIMEMODIFIED"].Data = value; 
            }
        }

        public DateTime TimeCreatedUTC
        {
            get 
            {
                return (DateTime)Cells["TIMECREATED"]; 
            }
            set 
            {
                Cells["TIMECREATED"].Data = value; 
            }
        }

    }
}