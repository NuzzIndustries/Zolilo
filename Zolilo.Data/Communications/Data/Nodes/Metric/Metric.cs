using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zolilo.Data
{
    [Serializable]
    public class Metric
    {
        long dr_metric_id;
        bool changesMade = false;

        public Metric()
        {
        }

        public bool ChangesMade
        {
            get { return changesMade; }
            internal set { changesMade = value; }
        }

        internal long DR_metric_id
        {
            get { return dr_metric_id; }
            set 
            {
                if (dr_metric_id > 0)
                    throw new ZoliloSystemException("Cannot change metric ID after it is already set");
                dr_metric_id = value; 
            }
        }

        public DR_Metrics DataRecord
        {
            get { return DR_Metrics.Get(DR_metric_id); }
        }



    }
}
