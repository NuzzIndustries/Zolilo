using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
namespace Zolilo.Data
{
    internal class ZoliloEdgeCache : Dictionary<long, DR_GraphEdges>, IZoliloTableCache
    {
        string tableName = null;


        string tableSchemaName = null;
        ZoliloDataIndexCollection<DR_GraphEdges> indexes;

        /// <summary>
        /// Updates the cache from record
        /// </summary>
        /// <param name="item"></param>
        internal void InsertToCache(DataRecord newRecord)
        {
            AddID(newRecord);
        }


        public void LoadAllData()
        {
            ZoliloQueryCache.Instance["select * from " + tableSchemaName + ".\"" + tableName + "\";"].Execute();
        }

        public void SetTableName(string tableName)
        {
            this.tableName = tableName;
        }

        public void SetTableSchemaName(string tableSchemaName)
        {
            this.tableSchemaName = tableSchemaName;
        }

        public DataRecord GetRecord(long id)
        {
            return this[id];
        }

        public bool ContainsID(long id)
        {
            return this.ContainsKey(id);
        }

        public void AddID(DataRecord newRecord)
        {
            if (newRecord != null) // Do not update if the record is null (not found)
            {
                this[newRecord.ID] = (DR_GraphEdges)newRecord;
                //Update indexes

                foreach (IZoliloDataIndex index in Indexes.Values)
                {
                    index.IndexInsert(newRecord);
                }
            }
            else
            {
                throw new InvalidOperationException("SYSTEM ERROR: Attempting insert operation when record exists.");
            }
        }

        public void ReplaceRecord(DataRecord record)
        {
            //restore index first because two non-identical records are needed for part 2
            foreach (IZoliloDataIndex index in Indexes.Values)
                index.UpdateIndex(this[record.ID], record);
            this[record.ID].RestoreData(record);
        }

        public void RemoveRecord(DataRecord record)
        {
            Remove(record.ID);
            foreach (IZoliloDataIndex index in Indexes.Values)
            {
                index.IndexRemove(record);
            }
        }

        public void AddIndex(string colNames)
        {
            string[] aColNames = colNames.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            colNames = "";
            for (int i = 0; i < aColNames.Length; i++)
            {
                aColNames[i] = DatabaseDefinitionManager.Instance.DatabaseDef.Database.Tables[TableName].Columns.GetByColNumber(int.Parse(aColNames[i])).Colname;
                colNames += "\"" + aColNames[i] + "\"";
                if (i < aColNames.Length - 1)
                    colNames += " ";
            }
            string sColNames = colNames.Replace(' ', ',');
            int indexID = aColNames.Length;

            //Get records
            string schema = DatabaseDefinitionManager.Instance.DatabaseDef.Database.Tables.schemaName;

            string cmdtext = "select " + sColNames + ",\"ID\" from " + schema + ".\"" + tableName + "\" " +
                "order by " + sColNames + ",\"ID\";";
            NpgsqlCommand cmd = new NpgsqlCommand(cmdtext);
            cmd.Connection = DataConnection.Current.SQLConnection;
            NpgsqlDataReader set = cmd.ExecuteReader();

            //Get indexes
            ZoliloDataIndexCollection<DR_GraphEdges> indexes = (ZoliloDataIndexCollection<DR_GraphEdges>)ZoliloCache.Instance[tableName].Indexes;

            //Add index
            IZoliloDataIndex index = indexes.Add(sColNames.Replace("\"", ""));
            IZoliloDataIndex currentIndex = index;

            index.SetCache(this);

            //Create nested index levels
            IZoliloDataIndex[] aIndexes = new IZoliloDataIndex[aColNames.Length];

            //Fill values
            while (set.Read())
            {
                //Get handle to current index value
                for (int i = 1; i < aIndexes.Length; i++)
                    currentIndex = currentIndex.GetSubIndex(set[i - 1], aColNames[i]);

                //Add last depth index value
                currentIndex.AddIndexValue(set[indexID - 1], (long)set[indexID]);

                currentIndex = index;
            }
        }

        public Dictionary<string, IZoliloDataIndex> Indexes
        {
            get
            {
                if (indexes == null)
                    indexes = new ZoliloDataIndexCollection<DR_GraphEdges>(this); ;
                return indexes;
            }
        }


        public DataRecord Get(long id)
        {
            if (id == 0L)
                return null;
            DR_GraphEdges item;
            if (TryGetValue(id, out item))
                return item;
            else
                return null;
        }

        public new DR_GraphEdges this[long id]
        {
            get { return (DR_GraphEdges)Get(id); }
            private set //Private to prevent accidental corruption of cache by possible programming error. Use UpdateCache()
            {
                base[id] = value;
                value.LastUpdateToMemory = DateTime.UtcNow;
            }
        }


        public string TableName
        {
            get { return tableName; }
        }

        internal DR_GraphEdges CreateDataRecord(NpgsqlDataReader set)
        {
            Type t = DR_GraphEdges.edgeTypes1[(long)set["EDGETYPE"]];
            DR_GraphEdges record = (DR_GraphEdges)Activator.CreateInstance(t);
            record.Load(set);
            record.LastUpdateToMemory = DateTime.UtcNow;
            return record;
        }
    }
}
