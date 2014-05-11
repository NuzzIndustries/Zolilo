using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zolilo.Data
{
    //Linguistic Rational Argument (LRA) - Human-created response which is an argument 
    //which explains the rationale behind the argument.  This can take the form of 
    //multiple fragments, but only one rated fragment should be assigned to another 
    //fragment per user.  Rational arguments can be split into fragments and attached 
    //to the fragment above it
    public class DR_FragmentLRA : DataRecord
    {
        #region UniversalAbstractMethods
        
        internal static DR_FragmentLRA Get(long id)
        {
            return ZoliloCache.Instance.FragmentLRA[id];
        }
        #endregion

        public DR_FragmentLRA()
            : base()
        {
        }

        public string _Text
        {
            get { return (string)Cells["TEXT"].Data; }
            set { Cells["TEXT"].Data = value; }
        }

        public override void SaveChanges()
        {
            if (_Text == null)
                _Text = "";
            base.SaveChanges();
        }
    }
}