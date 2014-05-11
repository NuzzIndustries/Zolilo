using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zolilo.Data
{
    /// <summary>
    /// DESIGNATED RESPONSE OBJECT FOR A FUNCTION. REPORTS RESULTS TO END USER
    /// </summary>
    public class FunctionResponse
    {
        string responseText = "Default response";

        public string ResponseText
        {
            get { return responseText; }
            set { responseText = value; }
        }

        bool functionSucceeded = false;

        public bool FunctionSucceeded
        {
            get { return functionSucceeded; }
            set { functionSucceeded = value; }
        }

        int dbResponse;

        public int DBResponse
        {
            get { return dbResponse; }
            set { dbResponse = value; }
        }


        public FunctionResponse()
        {
        }
    }
}