using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Configuration;

namespace Zolilo.Data
{
    /// <summary>
    /// Database abstraction layer.  Manages connections and transactions 
    /// from the application layer to the data layer    
    /// </summary>
    internal class DataManager
    {
        string connectionString;
        DataConnection systemDataConnection;

        ZoliloCache cache;
        ZoliloQueryCache queryCache;

        internal string ConnectionString
        {
            get { return connectionString; }
        }

        internal DataManager()
        {
            cache = new ZoliloCache();
            ZoliloCache.Instance = cache;
            queryCache = new ZoliloQueryCache();
        }

        internal void Initialize()
        {
            LogManager.Logger.Trace("Initializing DataManager");
            LoadConnectionString();
            systemDataConnection = new DataConnection(false);
            cache.Initialize();
            queryCache.Initialize();
            NodeDict.Init();
        }

        private void LoadConnectionString()
        {
            try
            {
                Configuration rootWebConfig =
                WebConfigurationManager.OpenWebConfiguration("~/web.config");
                ConnectionStringSettings connString;
                if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
                {
                    connString =
                        rootWebConfig.ConnectionStrings.ConnectionStrings["PostgresConnection"];
                    if (connString != null)
                        Console.WriteLine("Loaded database connection string from config",
                            connString.ConnectionString);
                    else
                        Console.WriteLine("Warning: connection string not found in config");
                    this.connectionString = connString.ConnectionString;
                }
                else
                    Console.WriteLine("Warning: no connection strings not found in config");
            }
            catch (Exception e)
            {
                throw new Exception("Unable to load connection string from config");
            }
        }

        public void EncryptConnectionString()
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            ConfigurationSection section = config.GetSection("connectionStrings");
            if (!section.SectionInformation.IsProtected)
            {
                section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                config.Save();
            }
        }

        private void DecryptConnectionString()
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            ConfigurationSection section = config.GetSection("connectionStrings");
            if (section.SectionInformation.IsProtected)
            {
                section.SectionInformation.UnprotectSection();
                config.Save();
            }
        }
        public static DataManager Instance;

        internal ZoliloCache Cache
        {
            get { return cache; }
        }

        internal DataConnection SystemDataConnection
        {
            get { return systemDataConnection; }
        }

        internal ZoliloQueryCache QueryCache
        {
            get { return queryCache; }
        }
    }
}