using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Npgsql;

using Zolilo.Web;

namespace Zolilo.Data
{
    /// <summary>
    /// Handles atomic database transactions consisting of more than one SQL command
    /// </summary>
    internal class ZoliloTransaction : IDisposable
    {
        NpgsqlTransaction sqlTransaction;
        bool needsCommit = false;
        bool transactionTerminated = false; //true if an error occurred, prevents commit from taking place
        List<ZoliloDataOperation> transactionOperations;

        internal List<ZoliloDataOperation> TransactionOperations
        {
            get 
            {
                if (transactionOperations == null)
                    transactionOperations = new List<ZoliloDataOperation>();
                return transactionOperations; 
            }
        }

        /// <summary>
        /// Gets the DB2Transaction object and sets the needsCommit flag to true
        /// </summary>
        public NpgsqlTransaction SQLTransaction
        {
            get 
            {
                if (transactionTerminated)
                    return null;
                if (sqlTransaction == null)
                {
                    sqlTransaction = DataConnection.Current.SQLConnection.BeginTransaction();
                    needsCommit = true;
                }
                return sqlTransaction; 
            }
        }

        internal ZoliloTransaction()
        {
        }

        internal static ZoliloTransaction Current
        {
            get
            {
                if (zContext.Session != null)
                {
                    ZoliloTransaction t = ZoliloRequestContext.Current.Transaction;
                    if (t.transactionTerminated)
                        return null;
                    return t;
                }
                return null;
            }
        }
        
        /// <summary>
        /// If transaction was referenced (meaning if an insert or update was made):
        /// Commits any commands made with this transaction to the database.
        /// </summary>
        internal void Commit()
        {
            if (sqlTransaction != null && needsCommit)
            {
                if (transactionTerminated)
                    throw new InvalidOperationException("A database error occurred and the transaction was cancelled.");
                needsCommit = false;
                sqlTransaction.Commit();
                sqlTransaction.Dispose();
                sqlTransaction = null;
                transactionOperations = null;
            }
        }

        public void Dispose()
        {
            if (sqlTransaction != null)
            {
                sqlTransaction.Dispose();
                sqlTransaction = null;
            }
        }

        public bool TransactionIsTerminated 
        {
            get { return transactionTerminated; }
            set { transactionTerminated = value; }
        }

        internal void Rollback()
        {
            if (!TransactionIsTerminated)
                TransactionIsTerminated = true;
            if (sqlTransaction != null && needsCommit)
            {
                sqlTransaction.Rollback();
                sqlTransaction.Dispose();
                needsCommit = false;
            }
            sqlTransaction = null;
            for (int i = TransactionOperations.Count - 1; i >= 0; i--)
                transactionOperations[i].Rollback();
        }

        /// <summary>
        /// Returns true if the transaction is active and has pending commands
        /// </summary>
        public bool NeedsCommit
        {
            get { return needsCommit; }
        }
    }
}
