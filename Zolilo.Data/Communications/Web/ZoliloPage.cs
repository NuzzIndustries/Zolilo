using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zolilo.Data;
using Zolilo.Web;
using System.Text;
using System.Web.UI;
using System.Web.SessionState;
using System.Web.UI.Adapters;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Configuration;

namespace Zolilo.Web
{
    public interface IZoliloPageNoFrame { }

    public class ZoliloPage : Page, INamingContainer
    {
        const bool DARKNET_ENABLED = true;

        protected bool isLoaded = false;
        protected bool MainFrameExists;
        protected ZoliloPageFullURL currentURL;
        Snippets snippets;
        protected bool isSupervisor = false;
        bool pageLoadComplete;
        int tempsupervisorID = -1; //Temporary ID to be assigned when the supervisor ID is being created for the first time
        string instanceID;
        bool cancelLoading = false; //True if page content shouldn't be loaded



        protected override void OnPreInit(EventArgs e)
        {
            ZoliloRequestContext.Current.Page = this;

            if (DARKNET_ENABLED)
                RestrictedPage.ValidateLoginDarknet(this);

            if (this is IRestrictedPage)
            {
                RestrictedPage.ValidateLogin(this);
            }
            
            

            DataConnection.Current.OpenConnection();

            //Process query string
            if (!IsUFrame)
            {
                //Possible supervisor
                if (Request.QueryString["redirect"] == "n" || this is IZoliloPageNoFrame)
                {
                    //no redirect to main supervisor page, must set up supervisor on this page
                    this.isSupervisor = true;
                }
                else
                {
                    string url = Page.Request.ServerVariables["URL"];
                    if (Page.Request.ServerVariables["QUERY_STRING"] != null && Page.Request.ServerVariables["QUERY_STRING"].Length > 0)
                        url += "?" + Page.Request.ServerVariables["QUERY_STRING"];
                    WebDirector.Instance.TransferPageFullRefresh(url);
                }
            }
            else if (IsUFrame)
            {
                if (this is IZoliloPageNoFrame)
                    WebDirector.Instance.RedirectEscapeFrame(Request.Url.PathAndQuery);
            }


            BuildURLFromRequest();

            snippets = new Snippets(this);
            base.OnPreInit(e);
        }

        protected override void OnInit(EventArgs e)
        {
            if (CancelLoading)
                return;
            InitializeSupervisor();
            if (zContext.Frame != null)
            {
                if (!(this is IPageIsReturnPersist))
                {
                    zContext.Frame.SavedURL = null;
                }
                if (this is IPageIsReturnBegin)
                {
                    zContext.Frame.SavedURL = zContext.Frame.LastURL;
                }
            }
            base.OnInit(e);
            
        }

        private void PostProcessControls(Control cBase)
        {
            if (cBase is IJavaScriptObject)
            {
                IJavaScriptObject js = (IJavaScriptObject)cBase;
                LiteralControl lit = new LiteralControl();
                lit.Text = "<script type='text/javascript'>" + 
                    "getPageObjectByFrame('" + ((ZoliloPage)Page).UFrameID + "')['div_" + js.GetClientID() + "'] = " +
                    js.GetConstructorExpression() + ";</script>";
                cBase.Controls.Add(lit);
            }
            if (cBase is HyperLink)
            {
                //Set default hyperlink URL to anchor hash
                if (((HyperLink)cBase).NavigateUrl == null || ((HyperLink)cBase).NavigateUrl == "")
                    ((HyperLink)cBase).NavigateUrl = ((ZoliloPage)Page).ClientURL + "#";
            }
            foreach(Control cSub in cBase.Controls)
                PostProcessControls(cSub);
        }

        private void InitializeSupervisor()
        {
            if (isSupervisor)
            {
                ZoliloSupervisorControl supervisor = new ZoliloSupervisorControl();
                if (SupervisorID > 0)
                    supervisor.Key = SupervisorID;
                else
                {
                    supervisor.Key = zContext.UserInstance.GenerateKey("zSupervisor");
                    tempsupervisorID = supervisor.Key;
                }
                this.SupervisorControl = supervisor;
                Controls.AddAt(1, supervisor);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (CancelLoading)
                return;
            if (!IsPostBack)
            {
                string pageName = this.GetType().Name;
                if (pageName != "pages_errors_globalerror_aspx")
                {
                    zContext.Session.PageHandle = this;
                    Title = zContext.Session.title;
                }

                string url = Page.ResolveClientUrl("~/browse");
                string url2 = VirtualPathUtility.ToAbsolute("~/browse");
            }
            base.OnLoad(e);
            
            isLoaded = true;
            PageLoadComplete = true;
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            if (CancelLoading)
                return;
            base.OnLoadComplete(e);
            PostProcessControls(this);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (CancelLoading)
                return;
            if (IsPostBack)
            {
                ProcessPostBackControl();
            }
            base.OnPreRender(e);
        }

        /// <summary>
        /// Creates a ZoliloPageFullURL struct from request information
        /// </summary>
        /// <returns></returns>
        private void BuildURLFromRequest()
        {
            string protocol = Request.ServerVariables["HTTPS"] == "ON" ? "https://" : "http://";
            string servername = Request.ServerVariables["HTTP_HOST"];
            string url = Request.ServerVariables["URL"];
            string querystring = "?" + Request.ServerVariables["QUERY_STRING"];
            if (querystring.Length <= 1)
                querystring = "";

            currentURL = new ZoliloPageFullURL(this.Title, protocol, servername, url, querystring);
        }
        
        public bool PageLoadComplete 
        { 
            get { return pageLoadComplete; }
            private set { pageLoadComplete = value; }
        }

        /// <summary>
        /// True if the page was loaded dynamically by UFrame
        /// </summary>
        public bool IsUFrame
        {
            get { return UFrameID != null && UFrameID != ""; } 
        }

        public string UFrameID
        {
            get { return (Page.Request.QueryString["uframe"]); }
        }

        /// <summary>
        /// Gets the query string id (id tag) - -1 if invalid or not found
        /// </summary>
        public long QueryStringID
        {
            get
            {
                long id = -1;
                try
                {
                    id = long.Parse(Request.QueryString["id"]);
                }
                catch (Exception) { }
                return id;
            }
        }
        
        public bool RedirectBackEnabled 
        { 
            get { return Request.QueryString["back"] == "y"; }
        }

        /// <summary>
        /// Gets the name of the Javascript callback object
        /// </summary>
        public string CallbackObjectName
        {
            get
            {
                string JSTag = "";
                if (UFrameID != null)
                {
                    JSTag = "_" + UFrameID;
                }
                return "zCallback" + JSTag;
            }
        }

        /// <summary>
        /// Gets the class containing javascript code snippets that can be used on this page
        /// </summary>
        public Snippets Snippets
        {
            get { return snippets; }
        }

        protected override PageStatePersister PageStatePersister
        {
            get
            {
                HiddenFieldPageStatePersister p = new HiddenFieldPageStatePersister(this);
                return base.PageStatePersister;
            }
        }

        
        /// <summary>
        /// Gets the session ID that contains the page-scoped data.  This must be passed in every URL and post back, unless there is a full refresh
        /// </summary>
        public string InstanceID 
        {
            get
            {
                if (instanceID == null)
                {
                    if (Request.QueryString["instance"] == null || !zContext.Session.PageInstances.ContainsKey(Request.QueryString["instance"]))
                    {
                        this.instanceID = ZoliloInstanceContext.GetNewInstanceID();
                    }
                    else
                    {
                        this.instanceID = Request.QueryString["instance"];
                    }
                }
                return this.instanceID;
            }
        }

        public ZoliloInstanceContext UserInstance
        {
            get 
            {
                return zContext.Session.PageInstances[InstanceID]; 
            }
        }

        public string URL 
        {
            get { return currentURL.ToString(); }
        }

        string clientURL;
        public string ClientURL 
        {
            get
            {
                if (clientURL == null)
                {
                    clientURL = URL;
                    if (UFrameID != null)
                        clientURL = WebDirector.Instance.RemoveQueryString(clientURL, "redirect"); //If UFrame is null, then redirect=n was placed explicitly in browser and should not be removed
                    clientURL = WebDirector.Instance.RemoveQueryString(clientURL, "uframe");
                    clientURL = WebDirector.Instance.RemoveQueryString(clientURL, "supervisor");
                    clientURL = WebDirector.Instance.RemoveQueryString(clientURL, "instance");
                }
                return clientURL;
            }
        }


        
        /// <summary>
        /// Gets the supervisor ID passed in the query URL
        /// </summary>
        internal int SupervisorID
        {
            get
            {
                if (tempsupervisorID > 0)
                    return tempsupervisorID;
                if (Request.QueryString["supervisor"] != null)
                    return int.Parse(Request.QueryString["supervisor"]);
                return -1;
            }
        }

        /// <summary>
        /// Gets the supervisor that this page loaded from
        /// </summary>
        public ZoliloSupervisorControl SupervisorControl
        {
            get
            {
                if (SupervisorID > 0)
                    return (ZoliloSupervisorControl)zContext.UserInstance["zSupervisor" + SupervisorID.ToString()];
                return null;
            }
            set
            {
                if (SupervisorID > 0)
                    zContext.UserInstance["zSupervisor" + SupervisorID.ToString()] = value;
                else
                    throw new InvalidOperationException("Cannot set supervisor: ID is null");
            }
        }

        public void ProcessPostBackControl()
        {
            Control control = null;

            string ctrlname = Request.Params.Get("__EVENTTARGET");
            if (ctrlname != null && ctrlname != string.Empty)
            {
                control = FindControl(ctrlname);
                if (typeof(ZoliloWebControl).IsAssignableFrom(control.GetType()))
                    ((ZoliloWebControl)control).TriggerPostBack(Request.Params.Get("__EVENTARGUMENT"));
            }
        }
        public bool CancelLoading
        {
            get { return cancelLoading; }
            set { cancelLoading = value; }
        }
    }

    public class ZoliloPageAdapter : PageAdapter
    {
        public ZoliloPageAdapter()
            : base()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void LoadAdapterViewState(object state)
        {
            base.LoadAdapterViewState(state);
        }

        protected override void LoadAdapterControlState(object state)
        {
            base.LoadAdapterControlState(state);
        }

        protected override object SaveAdapterControlState()
        {
            return base.SaveAdapterControlState();
        }

        protected override object SaveAdapterViewState()
        {
            return base.SaveAdapterViewState();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            if (zContext.Frame != null)
            {
                zContext.Frame.LastURL = zContext.Frame.NextURL;
                zContext.Frame.NextURL = zContext.Page.ClientURL;
            }
            if (ZoliloTransaction.Current != null)
                ZoliloTransaction.Current.Commit();
            ZoliloRequestContext.Current.Dispose();
        }
    }

    public class ZoliloHtmlForm : HtmlForm
    {
        public ZoliloHtmlForm()
            : base()
        {
        }

    }

    public struct ZoliloPageFullURL
    {
        internal string pageTitle;

        internal string Protocol;
        internal string ServerName;
        internal string PageURL;
        
        string querystring;

        internal ZoliloPageFullURL(string pageTitle, string protocol, string servername, string pageurl, string querystring)
        {
            this.pageTitle = pageTitle;
            this.Protocol = protocol;
            this.ServerName = servername;
            this.PageURL = pageurl;
            this.querystring = querystring; //Must assign before property can be used
            QueryString = querystring;
        }

        internal string QueryString
        {
            get { return querystring; }
            set
            {
                querystring = value;
                //ASP.net does not pass querystrings with the ? character.  Add character if needed
                if (value.Length > 0 && querystring[0] != '?')
                    querystring = "?" + value;
            }
        }

        public override string ToString()
        {
            return Protocol + ServerName + PageURL + QueryString;
        }

        public string GetAbsolutePath()
        {
            return PageURL + QueryString;
        }

    }

}