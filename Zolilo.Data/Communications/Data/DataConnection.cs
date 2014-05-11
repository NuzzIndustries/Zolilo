using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Npgsql;

namespace Zolilo.Data
{
    internal class DataConnection : IDisposable
    {
        NpgsqlConnection sqlConnection;
        bool connectionLocked; //If true, connection cannot be closed or opened
        bool requestContext;

        /// <summary>
        /// Creates a new DataConnection object
        /// </summary>
        /// <param name="requestContext">True if the connection is made by a HTTP Request</param>
        internal DataConnection(bool requestContext)
        {
            this.requestContext = requestContext;
            sqlConnection = new NpgsqlConnection(DataManager.Instance.ConnectionString);
        }

        internal NpgsqlConnection SQLConnection
        {
            get { return sqlConnection; }
        }

        public override string ToString()
        {
            return base.ToString();
        }

        internal void OpenConnection()
        {
            if (!connectionLocked && !(SQLConnection.State == System.Data.ConnectionState.Open))
                SQLConnection.Open();
        }

        internal void CloseConnection()
        {
            if (!connectionLocked && SQLConnection.State == System.Data.ConnectionState.Closed)
                SQLConnection.Close();
        }

        /// <summary>
        /// Returns the request-context DataConnection, or the System DataConnection if not available
        /// If Data Connection should not be opened, use DataManger.Instance.DataConnection instead
        /// 
        /// </summary>
        internal static DataConnection Current
        {
            get 
            {
                if (ZoliloRequestContext.Current != null)
                    return ZoliloRequestContext.Current.Connection;
                return DataManager.Instance.SystemDataConnection;
            }
        }

        internal void LockConnection()
        {
            connectionLocked = true;
        }

        internal void UnlockConnection()
        {
            connectionLocked = false;
        }

        public void Dispose()
        {
            CloseConnection();
            sqlConnection.Dispose();
        }
    }
}