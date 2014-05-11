using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zolilo.Data
{
    internal interface IZoliloTableCache
    {
        /// <summary>
        /// Loads all of the data into the cache from the database
        /// </summary>
        void LoadAllData();

        /// <summary>
        /// Notifys the table cache of the name of the table being cached, so that query operations can be performed
        /// </summary>
        /// <param name="tableName"></param>
        void SetTableName(string tableName);

        void SetTableSchemaName(string tableSchemaName);

        DataRecord Get(long id);

        /// <summary>
        /// Given the record ID, returns the DataRecord in the cache
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataRecord GetRecord(long id);

        bool ContainsID(long id);

        void AddID(DataRecord dataRecord);

        void ReplaceRecord(DataRecord record);

        void RemoveRecord(DataRecord record);

        void AddIndex(string colNames);

        Dictionary<string, IZoliloDataIndex> Indexes { get; }
    }
}
