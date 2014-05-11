using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zolilo.Data;
using Zolilo.Security;
using System.Text.RegularExpressions;

using Zolilo.Accounts;
using Zolilo.Web;

namespace Zolilo.Accounts
{
    /// <summary>
    /// CONTAINS ALL THE METHODS USED TO ACCESS ACCOUNT INFORMATION FROM THE DATABASE
    /// </summary>
    internal class AccountDirector
    {
        internal AccountDirector()
        {
        }

        internal void Initialize()
        {
            LogManager.Logger.Trace("Initializing AccountDirector");
        }
        




        internal static AccountDirector Instance
        {
            get { return zContext.System.AccountDirector; }
        }
    }
}