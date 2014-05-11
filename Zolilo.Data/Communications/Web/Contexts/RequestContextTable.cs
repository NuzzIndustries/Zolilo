using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Zolilo.Web;

namespace Zolilo.Data
{
    public class RequestContextTable : Dictionary<HttpRequest, ZoliloRequestContext>
    {
        public new ZoliloRequestContext this[HttpRequest key]
        {
            get
            {
                if (!base.ContainsKey(key))
                    base.Add(key, new ZoliloRequestContext());
                return base[key];
            }
        }
    }

    /// <summary>
    /// Gets the set of objects that persist throughout the current request
    /// </summary>
    public class ZoliloRequestContext : IDisposable
    {
        ZoliloTransaction transaction;
        DataConnection connection;
        ZoliloPage page;

        /// <summary>
        /// Gets the database transcation associated with this request
        /// </summary>
        internal ZoliloTransaction Transaction
        {
            get 
            {
                if (transaction == null)
                    transaction = new ZoliloTransaction();
                return transaction; 
            }
        }

        public ZoliloPage Page
        {
            get { return page; }
            internal set { page = value; }
        }

        public static ZoliloRequestContext Current
        {
            get 
            { 
                if (zContext.Session != null)
                    return zContext.Session.Requests[HttpContext.Current.Request];
                return null;
            }
        }

        public void Dispose()
        {
            if (transaction != null)
            {
                transaction.Dispose();
                transaction = null;
            }
            if (connection != null)
            {
                connection.Dispose();
                connection = null;
            }
            zContext.Session.Requests.Remove(HttpContext.Current.Request);
        }

        internal DataConnection Connection 
        {
            get
            {
                if (connection == null)
                    connection = new DataConnection(true);
                return connection;
            }
        }
    }
}
