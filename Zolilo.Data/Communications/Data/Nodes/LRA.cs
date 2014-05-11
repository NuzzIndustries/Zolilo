using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zolilo.Data;

namespace Zolilo.Data
{

    internal class LRA : DR_FragmentLRA
    {
        internal LRA(DR_FragmentLRA dr)
            : base(dr)
        {
            if (dr == null)
                DataRecord = new DR_FragmentLRA();
        }

        internal new DR_FragmentLRA DataRecord
        {
            get { return (DR_FragmentLRA)base.DataRecord; }
            set { base.DataRecord = value; }
        }

        public string Text
        {
            get { return DataRecord._Text; }
            set { DataRecord._Text = value; }
        }
    }
}