using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Text;
using Zolilo.Web;

namespace Zolilo.Data
{
    /// <summary>s
    /// Represents a session.
    /// </summary>
    public class ZoliloSession : IDisposable
    {
        #region items
        DR_Accounts currentAccount;
        Dictionary<string, ZoliloInstanceContext> pageInstances = new Dictionary<string, ZoliloInstanceContext>();
        AuthenticationInformation authenticationInformation;
        SessionStateFlags flags;
        ZoliloPage pageHandle;
        internal string title;
        RequestContextTable requests;
        
        long currentAccountIDOverrideAdmin = 0;

        internal DR_Accounts CurrentAccount
        {
            get 
            {
                if (currentAccountIDOverrideAdmin > 0)
                    return DR_Accounts.Get(currentAccountIDOverrideAdmin);
                return currentAccount; 
            }
        }

        internal AuthenticationInformation OpenIDAuthenticationInformation
        {
            get 
            {
                return authenticationInformation;
            }
            set 
            {
                authenticationInformation = value;
            }
        }

        internal SessionStateFlags Flags
        {
            get { return flags; }
        }

        /// <summary>
        /// Gets the hash table associated with the specified page-scoped data
        /// </summary>
        public Dictionary<string, ZoliloInstanceContext> PageInstances
        {
            get { return pageInstances; }
        }

        #endregion

        internal ZoliloSession()
        {
        }

        internal void BeginProcessLogin(DR_Accounts currentAccount)
        {
            if (currentAccount != null && ZoliloCache.Instance.Accounts.ContainsKey(currentAccount.ID))
            {
                this.currentAccount = currentAccount;
                ProcessLogin();
            }
            else
                throw new ZoliloWebException("Login error");
        }

        internal void ProcessLogin()
        {
            if (requests == null)
                requests = new RequestContextTable();
            if (flags == null)
                flags = new SessionStateFlags();
            if (OpenIDAuthenticationInformation != null)
                currentAccount = DR_Accounts.ResolveFromOpenID(OpenIDAuthenticationInformation.OpenIdentifier);
        }

        //Needs refactor
        internal void ResetData()
        {
            currentAccount = null;
            requests = new RequestContextTable();
            //RefreshSessionObjects();

            if (flags == null)
                flags = new SessionStateFlags();

            if (OpenIDAuthenticationInformation != null)
                LoadCurrentUserInfo(OpenIDAuthenticationInformation);
        }

        private void LoadCurrentUserInfo(AuthenticationInformation info)
        {
            currentAccount = DR_Accounts.ResolveFromOpenID(info.OpenIdentifier);
        }

        internal static ZoliloSession Current // Returning null
        {
            get 
            {
                if (HttpContext.Current == null || HttpContext.Current.Session == null)
                    return null;
                if (HttpContext.Current.Session.Count < 1 || HttpContext.Current.Session["ZoliloSession"] == null)
                {
                    ZoliloSession session = new ZoliloSession();
                    HttpContext.Current.Session.Add("ZoliloSession", session);
                    session.ResetData();
                }
                return (ZoliloSession)HttpContext.Current.Session["ZoliloSession"]; 
            }
        }

        internal bool LoggedIn
        {
            get { return (CurrentAccount != null && CurrentAccount.ID > 0); }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (LoggedIn)
            {
                sb.AppendLine("ZoliloSession");
                sb.AppendLine(CurrentAccount == null ? "CurrentAccount: null" : CurrentAccount.ToString());
                sb.AppendLine(OpenIDAuthenticationInformation == null ? "AuthenticationInformation: null" : OpenIDAuthenticationInformation.ToString());
                //sb.AppendLine(flags == null ? "SessionStateFlags: null" : flags.ToString());
                //sb.AppendLine(ddo == null ? "Connection: null" : ddo.ToString());
            }
            else
            {
                sb.AppendLine("Not logged in.");
            }
            return sb.ToString();

        }

        internal DR_Agents Agent
        {
            get 
            {
                if (LoggedIn)
                {
                    long currentAgentID = DR_Accounts.Get(CurrentAccount.ID)._IDAgentCurrent;
                    if (currentAgentID > 0)
                        return DR_Agents.Get(currentAgentID);
                    return null;
                }
                return null;
            }
        }

        internal static void VerifyFullLogin(ZoliloPage page)
        {
            if (zContext.Session.Agent == null)
                page.Response.RedirectToRoute("home");
        }

        public ZoliloPage PageHandle 
        {
            get
            {
                return pageHandle;
            }
            set
            {
                pageHandle = value;
            }
        }

        public static Exception LastError 
        { 
            get { return (Exception)HttpContext.Current.Session["LastError"]; }
            set { HttpContext.Current.Session["LastError"] = value; }
        }

        public RequestContextTable Requests
        {
            get { return requests; }
        }

        public void Dispose()
        {
        }
    }
}