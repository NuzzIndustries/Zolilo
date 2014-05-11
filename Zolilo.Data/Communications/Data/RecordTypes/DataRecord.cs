using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace Zolilo.Data
{
    //MUST CONTAIN A 64-BIT INTEGER NAMED "ID" AS PRIMARY KEY
    public abstract class DataRecord
    {
        /// <summary>
        /// Gets the linked static data table for the inherited object
        /// </summary>
        /// <returns></returns>
        internal ZoliloDataTable GetDataTable()
        {
            if (this is DR_GraphEdges)
                return DatabaseDefinitionManager.ByType[typeof(DR_GraphEdges)];
            return DatabaseDefinitionManager.ByType[GetType()];
        }

        #region fields
        DateTime lastUpdateToMemory = DateTime.UtcNow; //Must be expressed in Coordinated Universal Time
        ZoliloDataCellCollection cells;
        int sqlCode;
        bool isSearch;

        #endregion

        #region constructor
        internal DataRecord()
        {
            cells = new ZoliloDataCellCollection();
            ZoliloDataTable table = GetDataTable();
            foreach (ZoliloDataColumn column in table.Columns.Values)
            {
                ZoliloDataCell cell = new ZoliloDataCell(column);
                cell.ItemChanged += new EventHandler(cell_ItemChanged);
                cells.Add(column.Colname, cell);
            }
        }

        internal R CreateNew<R>() where R : DataRecord, new()
        {
            return new R();
        }
        #endregion

        #region properties

        public bool IsSearch
        {
            get { return isSearch; }
            internal set { isSearch = value; }
        }

        public long ID
        {
            get { return (long)Cells["ID"]; }
            internal set { Cells["ID"].Data = value; }
        }

        internal int SQLCode
        {
            get { return sqlCode; }
        }
         
        internal ZoliloDataCellCollection Cells
        {
            get { return cells; }
        }

        internal DataConnection Connection
        {
            get { return DataConnection.Current; }
        }

        internal DateTime LastUpdateToMemory
        {
            get { return lastUpdateToMemory; }
            set { lastUpdateToMemory = value; }
        }

        internal bool IsNull
        {
            get { return cells["ID"].Data == null; }
        }

        public virtual string SQLStatus
        {
            get
            {
                switch (sqlCode)
                {
                    case 0:
                        return "The operation was successful.";
                    case 23505:
                        return "A violation of the constraint imposed by a unique index or a unique constraint occurred.";
                    default:
                        return "Unknown SQLCode (" + sqlCode.ToString() + ")";
                }
            }
        }

        internal IZoliloTableCache Cache
        {
            get { return (IZoliloTableCache)ZoliloCache.Instance[GetDataTable().Tabname]; }
        }

        #endregion

        #region events
        private void cell_ItemChanged(object sender, EventArgs e)
        {
            LastUpdateToMemory = DateTime.UtcNow;
        }
        #endregion

        #region methods

        /// <summary>
        /// Throws an exception if the object is not a query
        /// </summary>
        protected void AssertNotQuery()
        {
            if (!IsSearch)
                throw new InvalidOperationException("Cannot set this property unless it is a query.");
        }

        /// <summary>
        /// Saves the record to the persistent data store
        /// </summary>
        public virtual void SaveChanges()
        {
            if (IsSearch)
                throw new InvalidOperationException("SYSTEM ERROR: Attempt to save a DDOQuery.");
            if (IsNull)
                InsertNew();
            else
                UpdateToDB();
        }

        /// <summary>
        /// Loads a raw data record into this object based on current reader state
        /// </summary>
        /// <param name="reader"></param>
        internal void Load(NpgsqlDataReader reader)
        {   
            foreach (ZoliloDataCell cell in Cells.Values)
                cell.InsertDataFromDB(reader[cell.ColumnDefinition.Colname]);
        }

        /// <summary>
        /// INSERTS A NEW ROW AND RETURNS THE NUMBER OF ROWS AFFECTED
        /// </summary>
        /// <returns></returns>
        protected virtual int InsertNew()
        {
            string insertCommand = BuildInsertCommand();

            NpgsqlTransaction tr = ZoliloTransaction.Current.SQLTransaction;
            NpgsqlCommand command = Connection.SQLConnection.CreateCommand();
            
            command.Transaction = tr; 

            try
            {
                command.CommandText = insertCommand;
                long id = (long)command.ExecuteScalar();

                ID = id;
                if (!Cache.ContainsID(id))
                {
                    Cache.AddID(this);
                }
                ZoliloTransaction.Current.TransactionOperations.Add(
                    new ZoliloDataOperation(ZoliloDataOperationType.INSERT, this));
            }
            catch (NpgsqlException ex)
            {
                ProcessSQLError(ex);
                throw ex;
            }
            return 1;
        }

        /// <summary>
        /// Deletes the record and all connecting edges.  Cannot be undone unless an exception is thrown during this page request.
        /// </summary>
        public virtual void DeletePermanently()
        {
            if (IsNull)
                throw new ZoliloSystemException("Cannot delete record with null ID");

            ZoliloDataTable table = GetDataTable();
            string cmd = "delete from \"" + table.Tabname + "\" where \"ID\"=" + Cells["ID"].LiteralToDB + ";";
            NpgsqlTransaction tr = ZoliloTransaction.Current.SQLTransaction;
            NpgsqlCommand command = Connection.SQLConnection.CreateCommand();
            command.Transaction = tr;
            command.CommandText = cmd;
            int rowsaffected = 0;
            try
            {
                //DataRecord snapshot = GetDRSnapshot();
                rowsaffected = command.ExecuteNonQuery();
                ZoliloTransaction.Current.TransactionOperations.Add(
                    new ZoliloDataOperation(ZoliloDataOperationType.DELETE, this));
                if (Cache.ContainsID(ID))
                    Cache.RemoveRecord(this);
            }
            catch (NpgsqlException ex)
            {
                ProcessSQLError(ex);
                throw ex;
            }
        }

        private string BuildInsertCommand()
        {
            StringBuilder sb = new StringBuilder();

            ZoliloDataTable table = GetDataTable();

            sb.Append("insert into " + DatabaseDefinitionManager.Instance.DatabaseDef.Database.Tables.schemaName + ".\"" + 
                table.Tabname + "\" (");
            //BUILD COLUMN NAMES
            foreach (ZoliloDataCell cell in Cells.Values)
            {
                if (cell.ColumnDefinition.Colname == "ID")
                    continue;
                sb.Append("\"" + cell.ColumnDefinition.Colname + "\","); //ADD COLUMN NAME IF CELL EXISTS
            }
            sb.Remove(sb.Length - 1, 1); //REMOVE LAST COMMA
            sb.Append(") values (");
            //BUILD LITERAL VALUES
            foreach (ZoliloDataCell cell in Cells.Values)
            {
                if (cell.ColumnDefinition.Colname == "ID")
                    continue;
                sb.Append(cell.LiteralToDB + ",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(") RETURNING \"" + table.Tabname + "\".\"ID\";");
            return sb.ToString();
        }

        private void ProcessSQLError(NpgsqlException ex)
        {
            sqlCode = int.Parse(ex.Code);
        }

        protected virtual int UpdateToDB()
        {
            DataRecord snapshot = GetDRSnapshot();
            ZoliloDataTable table = GetDataTable();
            StringBuilder sb = new StringBuilder();

            sb.Append("update " + DatabaseDefinitionManager.Instance.DatabaseDef.Database.Tables.schemaName + ".\"" + table.Tabname + 
                "\" set ");

            //BUILD COLUMN NAMES
                foreach (ZoliloDataCell cell in Cells.Values)
                {
                    if (cell.ColumnDefinition.Colname == "ID")
                        continue;
                    sb.Append("\"" + cell.ColumnDefinition.Colname + "\" = " + cell.LiteralToDB + ",");
                }
                sb.Remove(sb.Length - 1, 1); //REMOVE LAST COMMA
                sb.Append(" where \"ID\" = " + ID.ToString() + ";");

            NpgsqlCommand command = Connection.SQLConnection.CreateCommand();
            command.CommandText = sb.ToString();
            command.Transaction = ZoliloTransaction.Current.SQLTransaction;
            int rowsaffected = 0;
            try
            {
                rowsaffected = command.ExecuteNonQuery();
                DataRecord oldRecord = GetDRSnapshot();
                foreach (IZoliloDataIndex index in Cache.Indexes.Values)
                {
                    index.UpdateIndex(oldRecord, this);
                }
                ZoliloTransaction.Current.TransactionOperations.Add(
                    new ZoliloDataOperation(ZoliloDataOperationType.UPDATE, oldRecord));
            }

            catch (NpgsqlException ex)
            {
                //ZoliloTransaction.Current.Rollback();
                ProcessSQLError(ex);
                throw ex;
            }
            return rowsaffected;
        }

        internal T GetNewRecord<T>() where T : DataRecord, new()
        {
            T dr = new T();
            return dr;
        }

        internal T GetNewQueryable<T>() where T : DataRecord, new()
        {
            T dr = new T();
            dr.isSearch = true;
            return dr;
        }

        /// <summary>
        /// Returns a DataRecord snapshot from the database
        /// </summary>
        /// <returns></returns>
        private DataRecord GetDRSnapshot()
        {
            DataRecord d = (DataRecord)this.MemberwiseClone();
            d.cells = new ZoliloDataCellCollection();

            foreach (ZoliloDataColumn c in GetDataTable().Columns.Values)
            {
                d.cells.Add(c.Colname, new ZoliloDataCell(c));
                d.cells[c.Colname].Data = this.cells[c.Colname].Data;
            }
            return d;
        }

        internal void RestoreData(DataRecord backup)
        {
            if (this.GetType() != backup.GetType())
                throw new ZoliloSystemException("Backup type mismatch in RestoreData");
            foreach (ZoliloDataColumn c in GetDataTable().Columns.Values)
            {
                Cells[c.Colname].Data = backup.Cells[c.Colname].Data;
            }
        }

        internal string BuildSelectQuery()
        {
            ZoliloDataTable table = GetDataTable();
            StringBuilder sb = new StringBuilder();
            int counter = 0;            //Count of selectable elements

            //BUILD SELECT ELEMENTS
            foreach (ZoliloDataCell cell in Cells.Values)
            {
                if (cell.Data != null)
                {
                    counter++;
                    if (counter > 1)
                    {
                        sb.Append(" and ");
                    }
                    sb.Append(cell.ColumnDefinition.Colname + "=" + cell.LiteralToDB);
                }
            }
            if (counter < 1)
                throw new InvalidOperationException("record must contain filter data to select");
            return BuildSelectQuery(sb.ToString());
        }

        internal string BuildSelectQuery(string whereClause)
        {
            ZoliloDataTable table = GetDataTable();
            if (whereClause == null)
                return "select * from " + table.Tabname;
            StringBuilder sb = new StringBuilder();

            //BUILD SELECT QUERY
            sb.Append("select * from " + table.Tabname + " where " + whereClause);
            return sb.ToString();
        }


        /// <summary>
        /// Given incomplete information about a record, attempts to find the record which matches the information given.
        /// If more than one record exists, throws an exception (use QuerySet for queries which may return more than one record)
        /// </summary>
        internal DataRecord QueryRow()
        {
            //We want to be able to access any cache by name, and any object by id
            //Query from memory
            return ZoliloCache.Instance.DynamicLinqQueryRow(this); 
            //if (record != null)
              //  return record;
               /*
            else 
            {
                if (QueryRowNonCached() == null)
                    return null;
                dynamic cache = ZoliloCache.Instance[this.GetDataTable().Tabname];
                cache.InsertToCache(this);
                return this;
            }
                * */
        }

        /*
        /// <summary>
        /// Queries a record from database.
        /// </summary>
        /// <returns></returns>
        internal DataRecord QueryRowNonCached()
        {
            //Query from disk
            NpgsqlCommand cmd;

            cmd = new NpgsqlCommand(BuildSelectQuery(), Connection.SQLConnection);
            if (ZoliloTransaction.Current.NeedsCommit)
                cmd.Transaction = ZoliloTransaction.Current.SQLTransaction;
            NpgsqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
            if (!reader.HasRows)
                return null;
            reader.Read();
            Load(reader);
            return this;
        }
        */
        //Requires test
        internal List<DataRecord> QuerySet()
        {
            List<DataRecord> list = new List<DataRecord>();

            NpgsqlCommand cmd;
            cmd = new NpgsqlCommand(BuildSelectQuery(), Connection.SQLConnection);

            if (ZoliloTransaction.Current.NeedsCommit)
                cmd.Transaction = ZoliloTransaction.Current.SQLTransaction;
            using (NpgsqlDataReader set = cmd.ExecuteReader())
            {
                ZoliloDataTable t = GetDataTable();
                IZoliloTableCache c = (IZoliloTableCache)ZoliloCache.Instance[t.Tabname];
                while (set.Read())
                    list.Add(c.GetRecord((long)set["ID"]));
            }
            return list;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, ZoliloDataCell> cell in cells)
            {
                sb.AppendLine(cell.Key + ": " + cell.Value.Literal);
            }
            return sb.ToString();
        }
        #endregion

        #region operators
        public static implicit operator Int64(DataRecord record)
        {
            return record.ID;
        }
        #endregion

        #region inner classes
        public class DataRecordWriteException : InvalidOperationException
        {
            public DataRecordWriteException(string message)
                : base(message)
            {
            }
        }
        #endregion
    }

    internal class ZoliloDataCellCollection : Dictionary<string, ZoliloDataCell>
    {
    }
}