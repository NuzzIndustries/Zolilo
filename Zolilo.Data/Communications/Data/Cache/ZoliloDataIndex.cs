using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Zolilo.Data
{
    internal class ZoliloDataIndex<T, R> : 
        Dictionary<T, List<R>>, 
        IZoliloDataIndex 
        where R : DataRecord
    {
        IZoliloTableCache cache;
        Dictionary<T, IZoliloDataIndex> indexes;
        string colName, nestedColName;
        Type nextIndexType;

        internal Dictionary<T, IZoliloDataIndex> Indexes
        {
            get { return indexes; }
            set { indexes = value; }
        }
        
        public void SetColName(string name)
        {
            colName = name;
        }

        public new List<R> this[T key]
        {
            get
            {
                if (!ContainsKey(key))
                    Add(key, new List<R>());
                return base[key];
            }
        }

        public void AddIndexValue(object columnValue, long id)
        {
            this[(T)columnValue].Add((R)cache.Get(id));
        }

        public void SetCache(IZoliloTableCache cache)
        {
            if (cache == null)
                throw new InvalidOperationException("Cache is null");
            this.cache = cache;
        }

        public void IndexInsert(DataRecord newRecord)
        {
            if (nestedColName != null)
            {
                GetNextIndex((R)newRecord).IndexInsert(newRecord);
                return;
            }
            GetThisIndex((R)newRecord).Add((R)newRecord);
        }

        private IZoliloDataIndex GetNextIndex(R record)
        {
            return GetSubIndex((T)record.Cells[colName].Data, nestedColName);      
        }

        private List<R> GetThisIndex(R record)
        {
            return this[(T)record.Cells[colName].Data];
        }

        public void IndexRemove(DataRecord recordToRemove)
        {
            T key = (T)recordToRemove.Cells[colName].Data;
            if (ContainsKey(key) && nestedColName == null)
            {
                this[key].Remove((R)recordToRemove);
            }
            else if (nestedColName != null)
            {
                GetNextIndex((R)recordToRemove).IndexRemove(recordToRemove);
                
            }
            else
                throw new InvalidOperationException("SYSTEM ERROR: Attempting to remove already removed index");
        }

        internal static IZoliloDataIndex CreateNew(string key)
        {
            IZoliloDataIndex index;

            switch (DatabaseDefinitionManager.ByType[typeof(R)].Columns[key].TypenameUDT)
            {
                case "int2":
                    index = new ZoliloDataIndex<Int16, R>();
                    break;
                case "int4":
                    index = new ZoliloDataIndex<Int32, R>();
                    break;
                case "int8":
                    index = new ZoliloDataIndex<Int64, R>();
                    break;
                case "text":
                    index = new ZoliloDataIndex<string, R>();
                    break;
                case "varchar":
                    index = new ZoliloDataIndex<string, R>();
                    break;
                case "bpchar":
                    index = new ZoliloDataIndex<char, R>();
                    break;
                case "float4":
                    index = new ZoliloDataIndex<float, R>();
                    break;
                case "float8":
                    index = new ZoliloDataIndex<double, R>();
                    break;
                case "timestamp":
                    index = new ZoliloDataIndex<DateTime, R>();
                    break;
                case "oid":
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException("Index has unknown DB datatype");
            }
            index.SetColName(key);
            return index;
        }

        private IZoliloDataIndex NewSubIndexKey(string colName)
        {
            IZoliloDataIndex index = ZoliloDataIndex<T, R>.CreateNew(colName);
            index.SetCache(cache);
            return index;
        }

        public IZoliloDataIndex GetSubIndex(object key, string nestedColName)
        {
            if (indexes == null)
                indexes = new Dictionary<T, IZoliloDataIndex>();
            if (!indexes.ContainsKey((T)key))
            {
                if (this.nestedColName == null)
                    this.nestedColName = nestedColName;
                indexes.Add((T)key, NewSubIndexKey(nestedColName));
            }
            return indexes[(T)key];
        }

        public IZoliloDataIndex GetSubIndex(object key)
        {
            if (nestedColName == null)
                throw new InvalidOperationException("SYSTEM ERROR: Unable to determine nested column name when retrieving secondary index");
            return GetSubIndex(key, nestedColName);
        }

        public object GetIndexValues(object key)
        {
            try 
            {
                object o = this[(T)key];
            }
            catch (Exception e)
            {
            }
            return this[(T)key];
        }

        public void UpdateIndex(DataRecord oldRecord, DataRecord newRecord)
        {
            IndexRemove(oldRecord);
            IndexInsert(newRecord);
        }

        public void SetNestedColName(string nestedColName)
        {
            this.nestedColName = nestedColName;
        }
    }

    internal class ZoliloDataIndexCollection<T> : Dictionary<string, IZoliloDataIndex>
        where T : DataRecord
    {
        IZoliloTableCache cache;

        public ZoliloDataIndexCollection(IZoliloTableCache cache)
        {
            this.cache = cache;
        }

        //Adding indexes this way enforces the constraint that indexes will be set by the db management system
        internal IZoliloDataIndex Add(string key)
        {
            string[] cols = key.Split(',');
            IZoliloDataIndex index = ZoliloDataIndex<object, T>.CreateNew(cols[0]);
            if (cols.Length > 1)
                index.SetNestedColName(cols[1]);
            Add(key, index);
            return this[key];
        }
    }
}
