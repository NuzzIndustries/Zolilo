using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;

namespace Zolilo.Data
{
    /// <summary>
    /// Manages logging and debug status information.
    /// </summary>
    public class LogManager
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static LogManager Instance;

        public LogManager()
        {
           
        }

        public void Initialize()
        {
            LogManager.Logger.Trace("Initializing LogManager");
        }

        #region Properties

        public static Logger Logger
        {
            get { return logger; }
        }

        #endregion
    }
}