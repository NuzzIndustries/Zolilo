using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Reflection;
using Npgsql;
using NpgsqlTypes;

namespace Zolilo.Data
{

    internal class ZoliloTableCache<T> : Dictionary<long, T>, IZoliloTableCache //Derive from other datasource instead that implements ienumerable and 'emulates' dictionary by using some sort of cache object
        //inherit / overwrite dictionary and hack it toward the cache perhaps
        where T : DataRecord, new()
    {
        string tableName = null; //table name of the table being cached
        string tableSchemaName = null;

        ZoliloDataIndexCollection<T> indexes;

        internal ZoliloTableCache()
            : base()
        {
        }

        public string TableName
        {
            get { return tableName; }
        }

        public DataRecord Get(long id)
        {
            if (id == 0L)
                return null;
            T item;
            if (TryGetValue(id, out item))
                return item;
            else
                return null;
        }

        public virtual new T this[long id]
        {
            get
            {
                return (T)Get(id);
            }
            private set //Private to prevent accidental corruption of cache by possible programming error. Use UpdateCache()
            {
                base[id] = value;
                value.LastUpdateToMemory = DateTime.UtcNow;
                //Update Time
            }
        }

        public Dictionary<string, IZoliloDataIndex> Indexes
        {
            get
            {
                if (indexes == null)
                    indexes = new ZoliloDataIndexCollection<T>(this);
                return (Dictionary<string, IZoliloDataIndex>)indexes;
            }
        }

        internal T DynamicLinqQueryRow(DataRecord record)
        {
            IEnumerable<long> result = from item in this.Values
                                       where (long)record.Cells["ID"].Data == item.ID
                                       select item.ID;

            PropertyInfo[] prop = typeof(ZoliloDataCellCollection).GetProperties();

            // implementing the Cast method over the items of List
            IQueryable<T> query = this.Values.AsQueryable().Cast<T>();

            //IQueryable<T> query = this.AsQueryable<T>();

            List<Expression> expressions = new List<Expression>();

            ParameterExpression expressionParam = Expression.Parameter(typeof(T));

            //record.Cells
            Expression expressionCells = Expression.Property(Expression.Constant(record), "Cells");

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                ZoliloDataCell cell;
                string propertyName = (property.Name.Substring(0, 1).Replace("_", "") + property.Name.Substring(1)).ToUpper();
                if (record.Cells.TryGetValue(propertyName, out cell) && cell.Data != null)
                {
                    
                    ParameterExpression key = Expression.Parameter(typeof(string), "key");

                    Expression element = Expression.Property(
                            expressionCells,
                            record.Cells.GetType().GetProperty("Item"),
                            Expression.Constant(cell.ColumnDefinition.Colname));

                    //record.Cells["COLNAME"].Data
                    Expression expressionCellData = Expression.Property(element, "Data");
                    expressionCellData = Expression.Convert(expressionCellData, cell.Data.GetType());

                    //Item.X
                    Expression expressionItem = Expression.Property(expressionParam, property);

                    //record.Cells["COLNAME"].Data == Item.X
                    Expression expressionEquality = Expression.Equal(expressionCellData, expressionItem);

                    //StringComparison.OrdinalIgnoreCase
                    Expression expressionOrdinalIgnoreCase = Expression.Property(null, typeof(ZoliloDataColumn).GetProperty("IgnoreCaseComparisonType", BindingFlags.NonPublic | BindingFlags.Static));

                    //String.Equals(record.Cells["COLNAME"].Data, Item.X, StringComparison.OrdinalIgnoreCase)
                    Expression expressionEqualsMethodCall =
                        Expression.Call(null, typeof(String).GetMethod("Equals", new Type[]{typeof(string), typeof(string), typeof(StringComparison)}), expressionCellData, expressionItem, expressionOrdinalIgnoreCase);
                    //Add expression to the list of fields to search
                    if (cell.ColumnDefinition.IsIgnoreCaseString)
                        expressions.Add(expressionEqualsMethodCall);
                    else
                        expressions.Add(expressionEquality);
                }
            }

            Expression expressionFinal = null;
            if (expressions.Count == 0)
                return null;
            if (expressions.Count == 1)
                expressionFinal = expressions[0];
            if (expressions.Count > 1)
            {
                expressionFinal = expressions[0];
                expressions.RemoveAt(0);
                foreach (Expression ex in expressions)
                    expressionFinal = Expression.And(expressionFinal, ex);
            }
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(expressionFinal, expressionParam);

            IQueryable<T> queryResult = query.Where(lambda);

            int num;
            num = queryResult.Count<object>();
            if (num > 1)
                throw new ZoliloSystemException("Query returned multiple rows when single row is expected.  This may be due to a bug.");
            if (num == 0)
                return null;
            return queryResult.ElementAt(0);
        }

        #region IEnumerable<T> Members

        public new IEnumerator<T> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Loads all of the records into this cache object.  This will occur at application initialization.
        /// </summary>
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

        #region Dynamic Methods


        internal T CreateDataRecord(NpgsqlDataReader set)
        {
            T record = new T();
            record.Load(set);
            record.LastUpdateToMemory = DateTime.UtcNow;
            return record;
        }

        /// <summary>
        /// Updates the cache from record
        /// </summary>
        /// <param name="item"></param>
        internal void InsertToCache(DataRecord newRecord)
        {
            if (newRecord != null) // Do not update if the record is null (not found)
            {
                this[newRecord.ID] = (T)newRecord;
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
        #endregion


        public bool ContainsID(long id)
        {
            return ContainsKey(id);
        }

        public void AddID(DataRecord dataRecord)
        {
            InsertToCache(dataRecord);
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
            ZoliloDataIndexCollection<T> indexes = (ZoliloDataIndexCollection<T>)ZoliloCache.Instance[tableName].Indexes;

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
                    currentIndex = currentIndex.GetSubIndex(set[i-1], aColNames[i]);
                
                //Add last depth index value
                currentIndex.AddIndexValue(set[indexID - 1], (long)set[indexID]);

                currentIndex = index;
            }
        }

    }
}
