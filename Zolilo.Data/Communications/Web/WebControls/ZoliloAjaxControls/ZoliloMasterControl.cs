using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Threading.Tasks;
using Zolilo.Web;
using Zolilo.Data;

namespace Zolilo.Web
{


    /// <summary>
    /// The ZoliloMasterControl is loaded on every page via Site.Master.  It detects whether the control already
    /// exists on a loaded page and then if it does not exist, loads the ZoliloMasterControlInner
    /// 
    /// The control's purpose serves to hold the front end architecture together by managing javascript includes
    /// This control is required to ensure that a single object can be loaded among multiple dynamic web URL loads 
    /// into an aggregate page
    /// </summary>
    [ToolboxData("<{0}:ZoliloMasterControl runat=server></{0}:ZoliloMasterControl>")]
    [Serializable]
    public class ZoliloMasterControl : ZoliloWebControl, IZoliloViewStateControl
    {
        ZoliloMasterControlInner Inner;
        ZoliloJavascriptCallbackControl callback;
        

        bool stateLoaded = false;
        int handleInnerRequest;

        InnerControlState innerControlState = InnerControlState.Unknown;
         
        public ZoliloMasterControl()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            Page.RegisterRequiresControlState(this);
            callback = new ZoliloJavascriptCallbackControl();
            callback.ID = "callback";
            Controls.AddAt(0, callback);



            base.OnInit(e);
        }

        public void LoadState(object savedState)
        {
            object[] state = (object[])savedState;
            callback.LoadState(state[0]);
            handleInnerRequest = (int)state[1];
            innerControlState = (InnerControlState)state[2];
            
            stateLoaded = true;
        }

        public object SaveState()
        {
            object[] state = new object[3];
            state[0] = callback.SaveState();
            state[1] = handleInnerRequest;
            state[2] = innerControlState;
            
            return (object)state;
        }

        protected override void OnLoad(EventArgs e)
        {
           // callback.ViewstateCallback.ParseViewState();

            if (innerControlState == InnerControlState.NeedsLoad)
            {
                LoadInnerControl();
            }

            callback.CometReturn += new ZoliloCometHandler(EndSendClientStatement);
            if (!Page.IsPostBack)
            {
                if (innerControlState == InnerControlState.Unknown)
                    handleInnerRequest = AttemptLoadInner_Begin();
                base.OnLoad(e);
            }
        }

        void EndSendClientStatement(int serverPageCommandID, string commandResult)
        {
            if (serverPageCommandID == handleInnerRequest)
                AttemptLoadInner_End(commandResult);
        }
        
        int AttemptLoadInner_Begin()
        {
            return callback.BeginExecuteClientStatement("eval('typeof ZoliloInnerControlExists');");
        }

        void AttemptLoadInner_End(string result)
        {
            handleInnerRequest = 0;
            if (result != "function")
            {
                innerControlState = InnerControlState.NeedsLoad;

                zContext.UserInstance["ZoliloMasterControl:innerControlState"] = innerControlState;
                //callback.ViewstateCallback["ZoliloMasterControl:innerControlState"] = ((int)innerControlState).ToString();
                
                callback.ExecuteCode(Page.Snippets.ForcePostback);
            }
            else
            {
                innerControlState = InnerControlState.Unknown;
            }
            SaveControlState();
        }



        private void LoadInnerControl()
        {
            Inner = new ZoliloMasterControlInner(this);
            Controls.AddAt(1, Inner);

            innerControlState = InnerControlState.Unknown;
            return;
        }



        private void FindInnerControl()
        {
            //throw new NotImplementedException();
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            foreach (Control c in Controls)
                c.RenderControl(output);
        }

        /*
        public override void LoadViewStateTier2()
        {
            object oInnerControlState = Page.State["ZoliloMasterControl:innerControlState"];
            if (oInnerControlState != null)
                innerControlState = (InnerControlState)(oInnerControlState);
        }
        */

        /// <summary>
        /// True if redirect=n in URL querystring. Prevents page transfer
        /// </summary>
        public bool StayOnPageExplicit
        {
            get
            {
                return (Page.Request.QueryString["redirect"] != null) &&
                    (Page.Request.QueryString["redirect"].ToLower() == "n");
            }
        }
    }

    /// <summary>
    /// Contains the items to be loaded only once per full page load.
    /// </summary>
    public class ZoliloMasterControlInner : ZoliloWebControl
    {
        ZoliloMasterControl masterControlOuter;
        
        //ScriptManager ScriptManager1;
        List<string> scriptSrc = new List<string>();
        
        private ZoliloMasterControlInner() { }

        public ZoliloMasterControlInner(ZoliloMasterControl masterControl)
        {
            this.masterControlOuter = masterControl;
            
        }

        protected override void OnLoad(EventArgs e)
        {
         //   scriptSrc.Add("/Scripts/jquery.livequery-1.1.1/jquery.livequery.js");
            //scriptSrc.Add("/Scripts/UFrame-Scripts/jquery.form.js");
          //  scriptSrc.Add("/Scripts/UFrame-Scripts/innerxhtml.js");
            scriptSrc.Add("/Scripts/UFrame-Scripts/htmlparser.js");
            scriptSrc.Add("/Scripts/UFrame-Scripts/UFrame-new.js");
            scriptSrc.Add("/Scripts/zolilo/zolilo.js");
            //scriptSrc.Add("/Scripts/jqueryui/js/dev/jquery.ui.core.js");
          //  scriptSrc.Add("/Scripts/jqueryui/js/dev/jquery.ui.widget.js");
           // scriptSrc.Add("/Scripts/jqueryui/js/dev/jquery.ui.tabs-new.js");
            
            StringBuilder sb = new StringBuilder();
            foreach(string s in scriptSrc)
            {
                sb.Append("<script src=\"");
                sb.Append(s);
                sb.AppendLine("\" type=\"text/javascript\"></script>");
            }

            LiteralControl literal = new LiteralControl();
            literal.Text = sb.ToString();
            Controls.Add(literal);
                /*
            ScriptManager1 = new ScriptManager();
            ScriptManager1.ScriptMode = ScriptMode.Release;

            ScriptManager1.Scripts.Add(new ScriptReference("/Scripts/jqueryui/js/jquery-1.5.1.js"));
            ScriptManager1.Scripts.Add(new ScriptReference("/Scripts/jquery.livequery-1.1.1/jquery.livequery.js"));
            ScriptManager1.Scripts.Add(new ScriptReference("/Scripts/UFrame-Scripts/innerxhtml.js"));
            ScriptManager1.Scripts.Add(new ScriptReference("/Scripts/UFrame-Scripts/UFrame-new.js"));
            ScriptManager1.Scripts.Add(new ScriptReference("/Scripts/UFrame-Scripts/htmlparser.js"));
            ScriptManager1.Scripts.Add(new ScriptReference("/Scripts/zolilo/zolilo.js"));
            ScriptManager1.Scripts.Add(new ScriptReference("/Scripts/jqueryui/js/dev/jquery.ui.core.js"));
            ScriptManager1.Scripts.Add(new ScriptReference("/Scripts/jqueryui/js/dev/jquery.ui.widget.js"));
            ScriptManager1.Scripts.Add(new ScriptReference("/Scripts/jqueryui/js/dev/jquery.ui.tabs-new.js"));
            
            Controls.Add(ScriptManager1);
                */
            //AddTrigger();
            if (!masterControlOuter.StayOnPageExplicit)
            {
                HtmlButton trigger1 = new HtmlButton();
                trigger1.ID = "trigger1";
                trigger1.Attributes.Add("style", "visibility:hidden");
                trigger1.ServerClick += new EventHandler(trigger1_ServerClick);
                Controls.Add(trigger1);
            }
            //Literal Code
            /*
            LiteralControl LiteralTriggerInner;
            LiteralTriggerInner = new LiteralControl();
            string code = "function MainFrameExists(){return true;}";
            LiteralTriggerInner.Text += "<script type=\"text/javascript\">" + code + "</script>";
            Controls.Add(LiteralTriggerInner);
            */

            base.OnLoad(e);
        }

        #region TransferIfNecessary
        void trigger1_ServerClick(object sender, EventArgs e)
        {
            TransferIfNecessary();
        }

        private void TransferIfNecessary()
        {
            if (!masterControlOuter.StayOnPageExplicit)
                WebDirector.Instance.TransferPageFullRefresh(Page.Request.ServerVariables["URL"]);
        }
        
        /// <summary>
        /// Adds an htmlinput button that acts as a trigger to a server-side event if main frame is not detected
        /// </summary>
        private void AddTrigger()
        {
            Control main;
            LiteralControl triggerClick;
            main = Page.FindControl("__Page").FindControl("MasterPage").FindControl("MainForm");
            if (main != null)
            {
                triggerClick = new LiteralControl();
                triggerClick.ID = "triggerClick";
                //Controls.Add(triggerClick);
                //string code = "if(!(typeof MainFrameExists == 'function')){document.getElementById(\"trigger1\").click();}";
                //triggerClick.Text += "<script type=\"text/javascript\">" + code + "</script>";
            }
        }
        #endregion

        protected override void RenderContents(HtmlTextWriter output)
        {
            foreach (Control c in Controls)
                c.RenderControl(output);
            output.Write(ZoliloJavaScriptControl.CreateEnclosingScript("if(!(typeof MainFrameExists == 'function')){alert('mainframe does not exist');document.getElementById(\"trigger1\").click();}"));
            //output.Write("TEST1111111");
        }
    }

    public enum InnerControlState
    {
        Unknown,
        NeedsLoad,
        DoNotLoad
    }
}
