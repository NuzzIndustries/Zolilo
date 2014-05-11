using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zolilo.Data
{
    /// <summary>
    /// Handles program exceptions, errors, access violations, etc.
    /// </summary>
    public class ExceptionManager
    {
        public static ExceptionManager Instance;

        public ExceptionManager()
        {
        }

        public void Initialize()
        {
            LogManager.Logger.Trace("Initializing ExceptionManager");
        }

        public void ProcessException(Exception e)
        {
            LogManager.Logger.ErrorException(e.Message, e);
            throw e;
        }
    }

    public class ZoliloException : InvalidOperationException
    {
        public ZoliloException(string error) : base(error) { }

    }

    /// <summary>
    /// Use when an error in underlying system code (Zolilo.Data, etc) is suspected
    /// </summary>
    public class ZoliloSystemException : ZoliloException
    {
        public ZoliloSystemException(string error) : base(error) { }
    }

    /// <summary>
    /// Use when an error in Zolilo.Web assembly is suspected
    /// </summary>
    public class ZoliloWebException : ZoliloException
    {

        public ZoliloWebException(string error) : base(error) { }
    }
}