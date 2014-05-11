using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.CompilerServices;
using Zolilo.Web;

namespace Zolilo.Data
{
    
    /// <summary>
    /// Manages all communications between layers in TheBrain. 
    /// </summary>
    internal class CommunicationsDirector
    {
        DataManager dataManager;
        ExceptionManager exceptionManager;
        LogManager logManager;
        DatabaseDefinitionManager databaseDefinitionManager;
        
        internal CommunicationsDirector()
        {
            dataManager = new DataManager();
            DataManager.Instance = dataManager;

            databaseDefinitionManager = new DatabaseDefinitionManager();
            DatabaseDefinitionManager.Instance = databaseDefinitionManager;

            exceptionManager = new ExceptionManager();
            ExceptionManager.Instance = exceptionManager;

            logManager = new LogManager();
            LogManager.Instance = logManager;
        }

        internal void Initialize()
        {
            try
            {
                LogManager.Logger.Trace("Initializing CommunicationsDirector");
                dataManager.Initialize();
                databaseDefinitionManager.Initialize();
                exceptionManager.Initialize();
                logManager.Initialize();
                dataManager.Cache.LoadAllData();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal DataManager DataManager
        {
            get { return dataManager; }
        }

        internal DatabaseDefinitionManager DatabaseDefinitionManager
        {
            get { return databaseDefinitionManager; }
        }

        internal ExceptionManager ExceptionManager
        {
            get { return exceptionManager; }
        }

        internal LogManager LogManager
        {
            get { return logManager; }
        }

        internal static CommunicationsDirector Instance
        {
            get { return zContext.System.CommunicationsDirector; }
        }


    }
}