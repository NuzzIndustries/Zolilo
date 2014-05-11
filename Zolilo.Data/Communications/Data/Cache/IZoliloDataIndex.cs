using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zolilo.Data
{
    internal interface IZoliloDataIndex
    {
        void AddIndexValue(object columnValue, long id);
        void SetCache(IZoliloTableCache cache);
        void IndexInsert(DataRecord newRecord);
        void IndexRemove(DataRecord recordToRemove);

        IZoliloDataIndex GetSubIndex(object key, string colName);

        /// <summary>
        /// Gets the next nested index, narrowed by the specified key of the current index
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IZoliloDataIndex GetSubIndex(object key);

        /// <summary>
        /// Gets the list of records associated with this key
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns></returns>
        object GetIndexValues(object key);

        void SetColName(string key);
        void SetNestedColName(string nestedColName);

        /// <summary>
        /// Given a previous record and a new record,updates the index accordingly
        /// </summary>
        /// <param name="oldRecord"></param>
        /// <param name="newRecord"></param>
        void UpdateIndex(DataRecord oldRecord, DataRecord newRecord);

        
    }
}
