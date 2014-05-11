using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Linq.Expressions;
using Zolilo.Web;
using Npgsql;

namespace Zolilo.Data
{
    //http://blog.bodurov.com/Performance-SortedList-SortedDictionary-Dictionary-Hashtable/
    //http://stackoverflow.com/questions/1270084/best-way-to-cache-store-a-dictionary-in-c
    internal class ZoliloCache
    {
        static ZoliloCache instance;
        internal static bool cacheInitialized = false;

        ZoliloTableCache<DR_Accounts> accounts;
        ZoliloTableCache<DR_Agents> agents;
        ZoliloTableCache<DR_FragmentLRA> fragmentLRA;
        ZoliloTableCache<DR_Fragments> fragments;
        ZoliloTableCache<DR_Goals> goals;
        ZoliloTableCache<DR_Metrics> metrics;
        ZoliloTableCache<DR_OpenIDMap> openIDMap;
        ZoliloTableCache<DR_Tags> tags;
        ZoliloEdgeCache graphEdges;

        ZoliloCacheDict cacheDict;

        internal ZoliloCache()
        {
            cacheDict = new ZoliloCacheDict();

            accounts = new ZoliloTableCache<DR_Accounts>();
            agents = new ZoliloTableCache<DR_Agents>();
            fragmentLRA = new ZoliloTableCache<DR_FragmentLRA>();
            fragments = new ZoliloTableCache<DR_Fragments>();
            goals = new ZoliloTableCache<DR_Goals>();
            metrics = new ZoliloTableCache<DR_Metrics>();
            openIDMap = new ZoliloTableCache<DR_OpenIDMap>();
            tags = new ZoliloTableCache<DR_Tags>();
            graphEdges = new ZoliloEdgeCache();


            cacheDict.Add("ACCOUNTS", accounts);
            cacheDict.Add("AGENTS", agents);
            cacheDict.Add("FRAGMENTLRA", fragmentLRA);
            cacheDict.Add("FRAGMENTS", fragments);
            cacheDict.Add("GOALS", goals);
            cacheDict.Add("METRICS", metrics);
            cacheDict.Add("OPENIDMAP", openIDMap);
            cacheDict.Add("TAGS", tags);
            cacheDict.Add("GRAPHEDGES", graphEdges);
        }

        internal void Initialize()
        {


        }

        internal static ZoliloCache Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                instance = zContext.System.CommunicationsDirector.DataManager.Cache;
                return instance;
            }
            set { instance = value; }
        }

        internal DataRecord DynamicLinqQueryRow(DataRecord dataRecord)
        {
            //Get the cache corresponding with the table
            dynamic cache = this[dataRecord.GetDataTable().Tabname];

            //Attempt to query record
            DataRecord returnedRecord = (DataRecord)cache.DynamicLinqQueryRow(dataRecord);
            if (returnedRecord == null)
                return null;
            return cache[returnedRecord.ID]; //Return the item in memory direct from cache as it has been found and the ID identified
        }

        internal IZoliloTableCache this[string tableName]
        {
            get { return cacheDict[tableName]; }
        }

        #region Accessors
        internal ZoliloTableCache<DR_Accounts> Accounts
        {
            get { return accounts; }
        }

        internal ZoliloTableCache<DR_Agents> Agents
        {
            get { return agents; }
        }

        internal ZoliloTableCache<DR_FragmentLRA> FragmentLRA
        {
            get { return fragmentLRA; }
        }

        internal ZoliloTableCache<DR_Fragments> Fragments
        {
            get { return fragments; }
        }

        internal ZoliloTableCache<DR_Goals> Goals
        {
            get { return goals; }
        }

        internal ZoliloTableCache<DR_Metrics> Metrics
        {
            get { return metrics; }
        }

        internal ZoliloTableCache<DR_Tags> Tags
        {
            get { return tags; }
        }
        
        internal ZoliloEdgeCache GraphEdges
        {
            get { return graphEdges; }
        }
        

        internal ZoliloTableCache<DR_OpenIDMap> OpenIDMap
        {
            get { return openIDMap; }
        }

        #endregion

        internal void LoadAllData()
        {
            DataConnection.Current.OpenConnection();

            //Load data
            foreach (IZoliloTableCache cache in cacheDict.Values)
                cache.LoadAllData();

            //Add indexes

            NpgsqlCommand cmd = new NpgsqlCommand( 
                "select n.nspname as \"Schema\", " +
                    "c.relname as \"Name\", " + 
	                "CASE c.relkind when 'r' THEN 'table' WHEN 'v' THEN 'view' " +
                    "WHEN 'i' THEN 'index' WHEN 'S' THEN 'sequence' WHEN 's' THEN 'special' " +
	                "END as \"Type\", " +
	                "u.usename as \"Owner\", " +
	                "c2.relname as \"Table\", " +
	                "i.* " +
                "FROM pg_catalog.pg_class c " +
	                "JOIN pg_catalog.pg_index i ON i.indexrelid = c.oid " +
	                "JOIN pg_catalog.pg_class c2 ON i.indrelid = c2.oid " +
	                "LEFT JOIN pg_catalog.pg_user u ON u.usesysid = c.relowner " +
	                "LEFT JOIN pg_catalog.pg_namespace n ON n.oid = c.relnamespace " +
                "WHERE c.relkind IN ('i', '') " +
	                "AND i.indisprimary = 'f' " +
	                "AND n.nspname NOT IN ('pg_catalog', 'pg_toast') " +
	                "AND pg_catalog.pg_table_is_visible(c.oid) " +
                "ORDER BY 1,2;");

                cmd.Connection = DataConnection.Current.SQLConnection;
                NpgsqlDataReader set = cmd.ExecuteReader();
                while (set.Read())
                {
                    object o = set["indkey"];
                    Type t = o.GetType();

                    cacheDict[(string)set["Table"]].AddIndex((string)o);
                }
            cacheInitialized = true;
            DataConnection.Current.CloseConnection();
        }
    }

    internal class ZoliloCacheDict : Dictionary<string, IZoliloTableCache>
    {
        internal new void Add(string key, IZoliloTableCache value)
        {
            value.SetTableName(key);
            value.SetTableSchemaName("public");
            base.Add(key, value);
        }
    }
}