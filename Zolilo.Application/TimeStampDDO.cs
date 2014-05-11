using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zolilo.Data;

namespace Zolilo.Application
{
    public class TimeStampDDO : DataDrivenObject
    {
        internal TimeStampDDO(DataRecord dr) : base(dr) { }
        internal TimeStampDDO() : base() { }

        internal new TimestampRecord DataRecord
        {
            get { return (TimestampRecord)base.DataRecord; }
            set { base.DataRecord = value; }
        }

        public DateTime TimeCreated
        {
            get { return DataRecord.TimeModifiedUTC; }
            set 
            {
                AssertNotQuery();
                DataRecord.TimeCreatedUTC = value;
            }
        }

        internal DateTime TimeModified
        {
            get { return DataRecord.TimeModifiedUTC; }
            set
            {
                AssertNotQuery();
                DataRecord.TimeModifiedUTC = value;
            }
        }
    }
}
