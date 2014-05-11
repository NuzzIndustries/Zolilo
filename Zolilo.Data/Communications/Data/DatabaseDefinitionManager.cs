using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;

namespace Zolilo.Data
{
    public class DatabaseDefinitionManager
    {
        static DatabaseDefinitionManager instance;
        DataConnection connection;
        ZoliloDatabaseDefinitions databaseDef;
        static Dictionary<Type, ZoliloDataTable> byType;
        internal static Dictionary<Type, ZoliloDataTable> ByType { get { return byType; } }

        internal ZoliloDatabaseDefinitions DatabaseDef
        {
            get { return databaseDef; }
        }

        internal DatabaseDefinitionManager()
        {
        }

        internal void Initialize()
        {
            try
            {
                connection = DataManager.Instance.SystemDataConnection;
                LoadCurrentDatabaseDefinitions();
                SetDatabaseSchemaObjects();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// LOADS DATABASE DEFINITONS INTO DatabaseDefinitionManager
        /// </summary>
        void LoadCurrentDatabaseDefinitions()
        {
            databaseDef = ZoliloDatabaseDefinitions.ReturnNewDatabaseDefinitions();
        }

        /// <summary>
        /// MAKES FINAL CONNECTIONS FROM DATA LAYER TO CODE LAYER
        /// </summary>
        private void SetDatabaseSchemaObjects()
        {
            byType = new Dictionary<Type, ZoliloDataTable>();
            ByType.Add(typeof(DR_Accounts), databaseDef.Database["ACCOUNTS"]);
            ByType.Add(typeof(DR_Agents), databaseDef.Database["AGENTS"]);
            ByType.Add(typeof(DR_FragmentLRA), databaseDef.Database["FRAGMENTLRA"]);
            ByType.Add(typeof(DR_Fragments), databaseDef.Database["FRAGMENTS"]);
            ByType.Add(typeof(DR_Goals), databaseDef.Database["GOALS"]);
            ByType.Add(typeof(DR_GraphEdges), databaseDef.Database["GRAPHEDGES"]);
            ByType.Add(typeof(DR_Tags), databaseDef.Database["TAGS"]);
            ByType.Add(typeof(DR_OpenIDMap), databaseDef.Database["OPENIDMAP"]);
        }

        internal DataConnection DDO
        {
            get { return connection; }
        }

        internal static DatabaseDefinitionManager Instance
        {
            get { return instance; }
            set { instance = value; }
        }
    }

    internal class ZoliloDatabaseDefinitions
    {
        ZoliloDatabase database;

        internal ZoliloDatabase Database
        {
            get { return database; }
            set { database = value; }
        }

        internal static ZoliloDatabaseDefinitions ReturnNewDatabaseDefinitions()
        {
            ZoliloDatabaseDefinitions definitions = new ZoliloDatabaseDefinitions();

            string dbname = "zolilo";

            definitions.database = new ZoliloDatabase(dbname);
            definitions.database.LoadDatabaseDefinition();

            return definitions;
        }
    }

    /// <summary>
    /// ABSTRACT DEFINITON OF DATABASE
    /// </summary>
    internal class ZoliloDatabase
    {
        string dbname;
        ZoliloDataTableCollection tables;

        internal ZoliloDataTableCollection Tables
        {
            get { return tables; }
        }
        
        internal ZoliloDatabase(string databasename)
        {
            dbname = databasename;
        }

        internal void LoadDatabaseDefinition()
        {
            tables = new ZoliloDataTableCollection();
            tables.schemaName = "public";

            NpgsqlConnection connection = DatabaseDefinitionManager.Instance.DDO.SQLConnection;
            connection.Open();
            if (connection.Database != dbname)
                throw new Exception("Database name mismatch in ZoliloDatabaseDefinitions");
            connection.Close();

            NpgsqlCommand cmd = new NpgsqlCommand(@"select * from pg_tables where schemaname = '" + tables.schemaName + "' order by tablename;", connection);
            connection.Open();
            NpgsqlDataReader set = cmd.ExecuteReader();
            while (set.Read())
            {
                ZoliloDataTable table = new ZoliloDataTable(this, set);
                tables.Add(table.Tabname, table);
            }
            connection.Close();

            foreach (ZoliloDataTable table in tables.Values)
            {
                table.LoadTableDefinition();
            }

            //Load comments
            cmd = new NpgsqlCommand(
                @"select pc.relname as tablename, pa.attname as column, pd.description " +
                "from pg_description pd, pg_class pc, pg_attribute pa " +
                "where pa.attrelid = pc.oid " +
                "and pd.objoid = pc.oid " +
                "and pd.objsubid = pa.attnum ", connection);
            connection.Open();
            set = cmd.ExecuteReader();
            while (set.Read())
            {
                this[(string)set["tablename"]][(string)set["column"]].Comment = (string)set["description"];
            }

        }

        internal ZoliloDataTable this[string name]
        {
            get { return tables[name]; }
        }

        internal string DBName
        {
            get { return dbname; }
        }
    }

    /// <summary>
    /// COLLECTION OF ABSTRACT TABLE DEFINITONS
    /// </summary>
    internal class ZoliloDataTableCollection : Dictionary<string, ZoliloDataTable>
    {
        public string schemaName;

    }

    /// <summary>
    /// ABSTRACT DEFINITION OF A DATA TABLE
    /// </summary>
    internal class ZoliloDataTable
    {
        ZoliloDatabase database;

        ZoliloDataColumnCollection columns;

        internal ZoliloDataColumnCollection Columns
        {
            get { return columns; }
        }

        string tabschema;

        string tabname;

        /*
        short colcount;

        public short Colcount
        {
            get { return colcount; }
        }
        */
        protected ZoliloDataTable()
        {
        }

        /// <summary>
        /// CREATES A ZOLILODATATABLE OBJECT WITH CURRENT SCHEMA TABLE RESULT SET
        /// </summary>
        /// <param name="set"></param>
        internal ZoliloDataTable(ZoliloDatabase database, NpgsqlDataReader set)
        {
            this.database = database;

            tabschema = (string)set["schemaname"];
            tabname = (string)set["tablename"];
            //colcount = (short)set["COLCOUNT"];
        }

        internal void LoadTableDefinition()
        {
            columns = new ZoliloDataColumnCollection();
            NpgsqlConnection connection = DatabaseDefinitionManager.Instance.DDO.SQLConnection;
            connection.Open();
            if (connection.Database != database.DBName)
                throw new Exception("Database name mismatch in ZoliloDatabaseDefinitions");
            connection.Close();
            NpgsqlCommand cmd = new NpgsqlCommand(@"select * from information_schema.columns where table_catalog = '" + 
                database.DBName + "' and table_schema = '" +  tabschema + "' and table_name = '" + tabname + 
                "' order by ordinal_position;", connection);

            connection.Open();
            NpgsqlDataReader set = cmd.ExecuteReader();
            while (set.Read())
            {
                ZoliloDataColumn column = new ZoliloDataColumn(this, set);
                column.LoadColumnDefinition(set);
                columns.Add(column.Colname, column);
            }
            connection.Close();
        }

        internal ZoliloDataColumn this[string name]
        {
            get { return columns[name]; }

        }
        internal ZoliloDatabase Database
        {
            get { return database; }
        }

        public string Tabschema
        {
            get { return tabschema; }
        }

        public string Tabname
        {
            get { return tabname; }
        }
    }

    /// <summary>
    /// ABSTRACT DATA COLUMN, REPRESENTING IBM DB2 DATABASE SETTINGS
    /// </summary>
    internal class ZoliloDataColumn
    {
        ZoliloDataTable table;

        string table_catalog;
        string table_schema;
        string table_name;
        string column_name;
        int ordinal_position;
        string column_default;
        string is_nullable;
        string data_type;
        int character_maximum_length;
        int character_octet_length;
        int numeric_precision;
        int numeric_precision_radix;
        int numeric_scale;
        int datetime_precision;
        string interval_type;
        string interval_precision;
        string character_set_catalog;
        string character_set_schema;
        string character_set_name;
        string collation_catalog;
        string collation_schema;
        string collation_name;
        string domain_catalog;
        string domain_schema;
        string domain_name;
        string udt_catalog;
        string udt_schema;
        string udt_name;
        string scope_catalog;
        string scope_schema;
        string scope_name;
        int maximum_cardinality;
        string dtd_identifier;
        string is_self_referencing;
        string is_identity;
        string identity_generation;
        string identity_start;
        string identity_increment;
        string identity_maximum;
        string identity_minimum;
        string identity_cycle;
        string is_generated;
        string generation_expression;
        string is_updatable;

        string comment;
        bool flag_CaseSensitive;

        protected ZoliloDataColumn()
        {
        }

        /// <summary>
        /// CREATES A ZOLILODATACOLUMN OBJECT WITH CURRENT COLUMN RESULT SET
        /// </summary>
        /// <param name="set"></param>
        internal ZoliloDataColumn(ZoliloDataTable table, NpgsqlDataReader set)
        {
            this.table = table;
            column_name = (string)set["column_name"];
        }

        internal void LoadColumnDefinition(NpgsqlDataReader set)
        {
            /*
            NpgsqlConnection connection = DatabaseDefinitionManager.Instance.DDO.SQLConnection;
            connection.Open();
            if (connection.Database != table.Database.DBName)
                throw new Exception("Database name mismatch in ZoliloDatabaseDefinitions");
            connection.Close();

            NpgsqlCommand cmd = new NpgsqlCommand(@"select * from syscat.columns where tabschema = '" + table.Tabschema +
                "' and tabname = '" + table.Tabname + "' and colname = '" + colname + "'", connection);

            connection.Open();
            NpgsqlDataReader record = cmd.ExecuteReader();
                throw new InvalidOperationException("Column not found.");
            record.Read();
            connection.Close();
            */
            
            table_catalog = GetString(set["table_catalog"]);
            table_schema = GetString(set["table_schema"]);
            table_name = GetString(set["table_name"]);
            column_name = GetString(set["column_name"]);
            ordinal_position = GetInt(set["ordinal_position"]);
            column_default = GetString(set["column_default"]);
            is_nullable = GetString(set["is_nullable"]);
            data_type = GetString(set["data_type"]);
            character_maximum_length = GetInt(set["character_maximum_length"]);
            character_octet_length = GetInt(set["character_octet_length"]);
            numeric_precision = GetInt(set["numeric_precision"]);
            numeric_precision_radix = GetInt(set["numeric_precision_radix"]);
            numeric_scale = GetInt(set["numeric_scale"]);
            datetime_precision = GetInt(set["datetime_precision"]);
            interval_type = GetString(set["interval_type"]);
            interval_precision = GetString(set["interval_precision"]);
            character_set_catalog = GetString(set["character_set_catalog"]);
            character_set_schema = GetString(set["character_set_schema"]);
            character_set_name = GetString(set["character_set_name"]);
            collation_catalog = GetString(set["collation_catalog"]);
            collation_schema = GetString(set["collation_schema"]);
            collation_name = GetString(set["collation_name"]);
            domain_catalog = GetString(set["domain_catalog"]);
            domain_schema = GetString(set["domain_schema"]);
            domain_name = GetString(set["domain_name"]);
            udt_catalog = GetString(set["udt_catalog"]);
            udt_schema = GetString(set["udt_schema"]);
            udt_name = GetString(set["udt_name"]);
            scope_catalog = GetString(set["scope_catalog"]);
            scope_schema = GetString(set["scope_schema"]);
            scope_name = GetString(set["scope_name"]);
            maximum_cardinality = GetInt(set["maximum_cardinality"]);
            dtd_identifier = GetString(set["dtd_identifier"]);
            is_self_referencing = GetString(set["is_self_referencing"]);
            is_identity = GetString(set["is_identity"]);
            identity_generation = GetString(set["identity_generation"]);
            identity_start = GetString(set["identity_start"]);
            identity_increment = GetString(set["identity_increment"]);
            identity_maximum = GetString(set["identity_maximum"]);
            identity_minimum = GetString(set["identity_minimum"]);
            identity_cycle = GetString(set["identity_cycle"]);
            is_generated = GetString(set["is_generated"]);
            generation_expression = GetString(set["generation_expression"]);
            is_updatable = GetString(set["is_updatable"]);
        }

        private int GetInt(object cell)
        {
 	        if (cell is DBNull)
                return int.MinValue;
            return (int)cell;
        }

        private string GetString(object cell)
        {
            if (cell is DBNull)
                return null;
            return (string)cell;
        }

        internal string Colname
        {
            get { return column_name; }
        }

        internal bool NullableID
        {
            get { return is_nullable == "YES"; }
        }

        internal string Typename
        {
            get { return data_type; }
        }

        internal string TypenameUDT
        {
            get { return udt_name; }
        }

        public string Comment 
        {
            get { return comment; }
            internal set 
            { 
                comment = value;
                if (comment.Contains("{CASESENSITIVE}"))
                    this.flag_CaseSensitive = true;
            }
        }

        public bool IsIgnoreCaseString
        {
            get
            {
                return (TypenameUDT == "varchar" || TypenameUDT == "text") && !Flag_CaseSensitive;
            }
        }

        internal bool Flag_CaseSensitive 
        { 
            get { return flag_CaseSensitive; }
        }

        internal static StringComparison IgnoreCaseComparisonType
        {
            get { return StringComparison.OrdinalIgnoreCase; }
        }
    }

    /// <summary>
    /// COLLECTION OF DATA COLUMN DEFINITIONS, TO REPRESENT THE MAKEUP OF A TABLE
    /// </summary>
    internal class ZoliloDataColumnCollection : Dictionary<string, ZoliloDataColumn>
    {
        List<ZoliloDataColumn> byColNumber = new List<ZoliloDataColumn>();

        internal new void Add(string key, ZoliloDataColumn value)
        {
            base.Add(key, value);
            byColNumber.Add(value);
        }

        internal ZoliloDataColumn GetByColNumber(int number)
        {
            return byColNumber[number - 1];
        }
    }


    internal class ZoliloDataCell
    {
        internal event EventHandler ItemChanged;

        ZoliloDataColumn columnDefinition;

        internal ZoliloDataColumn ColumnDefinition
        {
            get { return columnDefinition; }
        }

        /// <summary>
        /// Underlying Data
        /// </summary>
        object data;

        public object Data
        {
            get 
            { 
                return data; 
            }
            set 
            { 
                Set(value); 
                if (ItemChanged != null)
                    ItemChanged(this, null); 
            }
        }

        /// <summary>
        /// INITIALIZES A NEW DATA CELL, TO BE USED IN A DDO CONSTRUCTOR ONLY
        /// </summary>
        /// <param name="columnDefinition"></param>
        internal ZoliloDataCell(ZoliloDataColumn columnDefinition)
        {
            this.columnDefinition = columnDefinition;
        }

        //DATA TYPES
        //NULL = Do nothing
        //DBNULL = Set to null in database
        //STRING = Set to varchar or char type depending on DB column settings

        public static explicit operator Int32(ZoliloDataCell cell)
        {
            if (cell.data == null)
                return 0;
            if (cell.data.GetType() == typeof(int))
                return (int)cell.data;
            else
                throw new InvalidCastException();
        }

        public static explicit operator String(ZoliloDataCell cell)
        {
            if (cell.data == null)
                return null;
            if (cell.data.GetType() == typeof(String))
                return (String)cell.data;
            else
                throw new InvalidCastException();
        }

        public static explicit operator Int16(ZoliloDataCell cell)
        {
            if (cell.data == null)
                return 0;
            if (cell.data.GetType() == typeof(Int16))
                return (Int16)cell.data;
            else
                throw new InvalidCastException();
        }

        public static explicit operator Int64(ZoliloDataCell cell)
        {
            if (cell.data == null)
                return 0;
            if (cell.data.GetType() == typeof(Int64))
                return (Int64)cell.data;
            else
                throw new InvalidCastException();
        }

        public static explicit operator DateTime(ZoliloDataCell cell)
        {
            if (cell.data == null)
                return DateTime.MinValue;
            if (cell.data.GetType() == typeof(DateTime))
                return (DateTime)cell.data;
            else
                throw new InvalidCastException();
        }

        private void Set(object data)
        {
            if (data == null || data.GetType() == typeof(DBNull))
            {
                if (ColumnDefinition.NullableID)
                    this.data = null;
                else
                    throw new InvalidOperationException("Specified column may not be null");
                return;
            }
            this.data = data;
        }

        internal string LiteralToDB
        {
            get
            {
                if (data == null)
                    return "null";
                if (data.GetType() != typeof(string))
                    return Literal;
                return "'" + ParseLiteralToDB((string)data) + "'";
            }
        }

        /// <summary>
        /// To reduce SQL injection vulnerabilities and allow special characters, this parses string characters
        /// This will have to be fixed for efficiency
        /// </summary>
        /// <param name="literal"></param>
        /// <returns></returns>
        static string ParseLiteralToDB(string literal)
        {
            literal = literal.Replace("<", "<<");
            literal = literal.Replace(">", "<>");
            literal = literal.Replace(";", "<sc>");
            literal = literal.Replace("'", "<sq>");
            literal = literal.Replace("\"", "<dq>");
            return literal;
        }

        internal static string ParseDBToLiteral(string dbText)
        {
            dbText = dbText.Replace("<<", "<");
            dbText = dbText.Replace("<>", ">");
            dbText = dbText.Replace("<sc>", ";");
            dbText = dbText.Replace("<sq>", "'");
            dbText = dbText.Replace("<dq>", "\"");
            return dbText;
        }

        //Must return a literal that can be used with DB SQL queries
        internal string Literal
        {
            get 
            {
                if (data == null)
                    return "null";
                Type t = data.GetType();
                switch (t.FullName)
                {
                    case "System.String":
                        return "'" + (string)data + "'";
                    case "System.Int16":
                        return ((short)data).ToString();
                    case "System.Int32":
                        return ((int)data).ToString();
                    case "System.Int64":
                        return ((long)data).ToString();
                    case "System.DBNull":
                        return "null";
                    case "System.DateTime":
                        {
                            DateTime date = (DateTime)data;
                            return "'" + date.Year.ToString() + "-" + date.Month.ToString().PadLeft(2, '0') + "-" + date.Day.ToString().PadLeft(2, '0') + " " +
                                date.Hour.ToString().PadLeft(2, '0') + ":" + date.Minute.ToString().PadLeft(2, '0') + ":" + date.Second.ToString().PadLeft(2, '0') + "." +
                                date.Millisecond.ToString().PadLeft(3, '0').PadRight(6, '0') + "'";
                        }
                    default:
                        break;
                }
                throw new NotImplementedException("Literal type " + t.FullName + " not implemented.");
            }
        }

        public override string ToString()
        {
            return columnDefinition.Colname + ": " + Literal;
        }

        internal void InsertDataFromDB(object data)
        {
            if (data.GetType() == typeof(string))
                Data = ParseDBToLiteral((string)data);
            else
                Data = data;
        }
    }
}