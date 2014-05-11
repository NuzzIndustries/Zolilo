using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Zolilo.Web;

namespace Zolilo.Data
{

    /// <summary>
    /// OBJECT REPRESENTING DATA TO BE WRITTEN TO/FROM DB
    /// </summary>
    public class DR_Accounts : GraphNode
    {
        #region UniversalAbstractMethods
        public static DR_Accounts Get(long id)
        {
            return ZoliloCache.Instance.Accounts[id];
        }
        #endregion

        public DR_Accounts()
            : base()
        {
        }

        internal DR_Accounts(string openIdentifier) 
            : this()
        {

        }

        DR_Accounts GetAccount(string openIdentifier)
        {
            DDOQuery<DR_OpenIDMap> openidquery = new DDOQuery<DR_OpenIDMap>();
            openidquery.Object._OpenIdentifier = openIdentifier;

            DR_OpenIDMap openidrecord = openidquery.PerformQuery();

            if (openidrecord.Cells["ACCOUNTID"].Data != null)
            {
                DDOQuery<DR_Accounts> accountQuery = new DDOQuery<DR_Accounts>();
                accountQuery.Object.ID = openidrecord._AccountID;
                return accountQuery.PerformQuery();
            }
            else //OpenID Link Not Found
            {
                zContext.Session.Flags.OpenIDNotLinked = true;
                return null;
            }
        }

        public string _Username
        {
            get { return (string)Cells["USERNAME"]; }
            set { Cells["USERNAME"].Data = value; }
        }

        /// <summary>
        /// This field must only contain encrypted hash values.
        /// </summary>
        public string _PCode
        {
            get { return (string)Cells["PCODE"]; }
            set 
            {
                Cells["PCODE"].Data = value; 
            }
        }

        public string _Email
        {
            get { return (string)Cells["EMAIL"]; }
            set { Cells["EMAIL"].Data = value; }
        }

        public long _IDAgentCurrent
        {
            get { return (long)Cells["IDAGENTCURRENT"]; }
            set { Cells["IDAGENTCURRENT"].Data = value; }
        }

        public short _TimeZoneOffset
        {
            get { return (short)Cells["TIMEZONEOFFSET"]; }
            set { Cells["TIMEZONEOFFSET"].Data = value; }
        }

        public override long _IDAgentCreated
        {
            get
            {
                return -1;
            }
        }

        public override string ToString()
        {
            return "Account Information" + Environment.NewLine + base.ToString();
        }


        public override NodeType NodeType
        {
            get { return Data.NodeType.Account; }
        }

        public static DR_Accounts ResolveFromOpenID(string openIdentifier)
        {
            DR_Accounts account = new DR_Accounts(openIdentifier);
            return DR_Accounts.Get(account);
        }
    }
}