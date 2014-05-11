using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zolilo.Data
{
    /// <summary>
    /// Represents a single operation that takes place (such as an update or insert)
    /// This is used to implement rollback functionality on the in-memory database
    /// </summary>
    internal class ZoliloDataOperation
    {
        ZoliloDataOperationType opType;
        DataRecord record;

        internal ZoliloDataOperation(ZoliloDataOperationType opType, DataRecord record)
        {
            this.opType = opType;
            this.record = record;
        }

        /// <summary>
        /// Rolls back the operation.  Unsafe as this may undo transactions that have already been completed successfully
        /// Not a problem in single-user development mode
        /// </summary>
        internal void Rollback()
        {
            switch (OpType)
            {
                case ZoliloDataOperationType.INSERT:
                    RollbackInsert();
                    break;
                case ZoliloDataOperationType.None:
                    throw new InvalidOperationException("SYSTEM ERROR: Invalid operation type during rollback");
                case ZoliloDataOperationType.UPDATE:
                    RollbackUpdate();
                    break;
                case ZoliloDataOperationType.DELETE:
                    RollbackDelete();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void RollbackDelete()
        {
            record.Cache.AddID(record);
        }

        /// <summary>
        /// Replaces the current in-memory object with the one in this operation
        /// Not thread safe
        /// </summary>
        private void RollbackUpdate()
        {
            record.Cache.ReplaceRecord(record);
        }

        private void RollbackInsert()
        {
            record.Cache.RemoveRecord(record);
        }

        internal DataRecord DataRecord
        {
            get { return record; }
            set { record = value; }
        }

        internal ZoliloDataOperationType OpType
        {
            get { return opType; }
            set { opType = value; }
        }
    }

    internal enum ZoliloDataOperationType
    {
        None,
        UPDATE,
        INSERT,
        DELETE
    }
}
