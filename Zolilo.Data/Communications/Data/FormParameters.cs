using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zolilo.Data
{
    /// <summary>
    /// REPRESENTS A UNIT OF VALIDATED FORM DATA
    /// CONTAINS: PARAMETER DEFINITONS, AND SERVER VALIDATION LOGIC
    /// THIS CLASS TO BE OVERRIDEN FOR EACH SPECIFIC FORM
    /// </summary>
    internal abstract class FormParameters
    {
        internal FormParameters()
        {
        }

        /// <summary>
        /// GENERATES A NEW ABSTRACT DATA RECORD FROM FORM PARAMETERS
        /// </summary>
        internal abstract DataRecord GenerateNewDataRecord();
    }
}
