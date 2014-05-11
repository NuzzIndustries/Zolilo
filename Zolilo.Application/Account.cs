using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zolilo.Data;

namespace Zolilo.Application
{
    public sealed class Account : TimeStampDDO, IDDOQueryable<Account>
    {
        /// <summary>
        /// Public constructor required for templated DDOQuery - Do not use
        /// </summary>
        public Account() : base(){}

        internal Account(DR_Accounts dr)
            : base(dr)
        {
        }

        private new DR_Accounts DataRecord
        {
            get { return (DR_Accounts)base.DataRecord; }
            set { base.DataRecord = value; }
        }

        public DataDrivenObject GetNewQueryable()
        {
            Account account = new Account();
            account.DataRecord = new DR_Accounts();
            account.IsSearch = true;
            return account;
        }

        public List<Account> PerformQuery()
        {
            List<Account> list = new List<Account>();
            List<DataRecord> set = DataRecord.QuerySet();
            foreach (DR_Accounts record in set)
                list.Add(new Account(record));
            set = null;
            return list;
        }

        public string _Username
        {
            get { return DataRecord._Username; }
            set { DataRecord._Username = value; }
        }

        /// <summary>
        /// This field must only contain encrypted hash values.
        /// </summary>
        public string _PasswordHash
        {
            get { return DataRecord._PasswordHash; }
            set { DataRecord._PasswordHash = value; }
        }

        public string _Email
        {
            get { return DataRecord._Email; }
            set { DataRecord._Email = value; }
        }

        internal long _IDAgentCurrent
        {
            get { return DataRecord._IDAgentCurrent; }
            set { DataRecord._IDAgentCurrent = value; }
        }
    }
}
