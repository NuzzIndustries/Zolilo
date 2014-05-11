using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Npgsql;

namespace Zolilo.Data
{
    internal class ZoliloQueryCache : Dictionary<string, ZoliloQuery>
    {
        internal new ZoliloQuery this[string querystring]
        {
            get 
            {
                ZoliloQuery q;
                if (!TryGetValue(querystring, out q))
                {
                    q = new ZoliloQuery(querystring);
                    this.Add(querystring, q);
                }
                return q;
            }
            set 
            {
                throw new InvalidOperationException("ZoliloQueryCache cannot be modified");
            }
        }

        internal static ZoliloQueryCache Instance
        {
            get { return DataManager.Instance.QueryCache; }
        }


        internal void Initialize()
        {
            
        }
    }

    internal class ZoliloQuery
    {
        string query;
        string tablename;

        List<long> items;
        dynamic cache;

        internal ZoliloQuery(string query)
        {
            this.query = query;
            tablename = GetTableName(query);
            cache = ZoliloCache.Instance[tablename.ToUpper()];
        }

        private string GetTableName(string query)
        {
            int indexTable = query.IndexOf(".\"") + 2;
            int length = query.IndexOf('"', indexTable + 1) - indexTable;
            if (length < 0)
                length = query.Length - indexTable - 1;
            string tablename1 = query.Substring(indexTable, length);
            return tablename1;
        }

        //Doesn't use cache advantage yet
        internal List<long> Execute()
        {
            items = new List<long>();
            NpgsqlCommand command = new NpgsqlCommand(query);
            if (ZoliloTransaction.Current != null && ZoliloTransaction.Current.NeedsCommit)
                command.Transaction = ZoliloTransaction.Current.SQLTransaction;
            command.Connection = DataConnection.Current.SQLConnection;
            NpgsqlDataReader set = command.ExecuteReader();
            while(set.Read())
            {
                items.Add((Int64)set["ID"]);
                cache.InsertToCache(cache.CreateDataRecord(set));
            }
            return items;
        }
    }
}