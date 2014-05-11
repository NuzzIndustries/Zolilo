using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zolilo.Data
{
    /// <summary>
    /// Represents a class that queries the cache for information
    /// Create an instance of this class, set the parameters of the DataRecord, and then PerformQuery()
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DDOQuery<T> where T : DataRecord, new()
    {
        T ddo;

        public DDOQuery()
        {
            ddo = new T();
            ddo = ddo.GetNewQueryable<T>();
            ddo.IsSearch = true;
        }

        public T Object
        {
            get { return ddo; }
        }

        public T PerformQuery()
        {
            DataRecord d = ddo.QueryRow();
            if (d == null)
                return null;
            return (T)d;
        }
    }
}
