using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Zolilo.Web;
using System.Threading.Tasks;
using System.Text;


namespace Zolilo.Web
{
    /// <summary>
    /// Delegate to handle the event of receiving asynchronous postback client data
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    /// <param name="result"></param>
    public delegate void ZoliloJSClientReceiver(ZoliloJavascriptCallbackControl sender, string eventArgs, out string result);
    public delegate void ZoliloCometHandler(int serverPageCommandID, string commandResult);
    public delegate string ZoliloJSCommand(string[] commandArgs);

    [Serializable]
    public class ZoliloJavascriptCallbackControl : ZoliloWebControl, ICallbackEventHandler, IZoliloViewStateControl
    {
        ZoliloJavaScriptContainerControl JS;
        /*
        ZoliloViewStateControl viewstateCallback;

        /// <summary>
        /// The tier 2 callback hashtable viewstate which is modifiable by javascript callbacks
        /// </summary>
        public ZoliloViewStateControl ViewstateCallback
        {
            get { return viewstateCallback; }
        }
        */
        bool stateLoaded;

        public string callbackreturnvalue;

        public event ZoliloCometHandler CometReturn;

        StringBuilder codeToExecSB;
        List<string> codeToExec;

        /// <summary>
        /// Use this to directly edit the code or the order of the code to be executed.  Otherwise, use ExecuteCode()
        /// </summary>
        public List<string> JavascriptExecutionBuffer
        {
            get { return codeToExec; }
        }


        /// <summary>
        /// A dictionary and queue to keep track of the async responses
        /// Javascript is single-threaded on the client, therefore only one script at a time
        /// Queue will control the order of which the scripts execute
        /// Dictionary will keep track of the script results
        /// Both objects contain the same key values
        /// </summary>
        Dictionary<int, string> commandQueue;

        Dictionary<int, string> commandResponses;
    

        /// <summary>
        /// List of delegates that correspond to executable Ajax actions
        /// </summary>
        Dictionary<string, ZoliloJSCommand> commands;


        public const string TIMEOUT = "[Timed Out]";
        private const string STILL_EXECUTING = "[still executing]";

        /// <summary>
        /// Gets or sets the dictionary of command delegates, key = command token, value = method to execute
        /// MUST VALIDATE INPUT TO DELEGATES, IT IS CLIENT-EDITABLE
        /// RETURN VALUES WILL ALSO BE SENT TO THE CLIENT
        /// </summary>
        public Dictionary<string, ZoliloJSCommand> Commands
        {
            get { return commands; }
        }

        public ZoliloJavascriptCallbackControl()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            /*
            viewstateCallback = new ZoliloViewStateControl(this);
            viewstateCallback.ID = "ZoliloViewState";
            Controls.AddAt(0, viewstateCallback);
            */
            //Initialize viewstate items
            if (!Page.IsPostBack)
            {
                commandQueue = new Dictionary<int, string>();
                commandResponses = new Dictionary<int, string>();
            }
            CometReturn += new ZoliloCometHandler(ZoliloJavascriptCallbackControl_CometReturn);
            

            //Initialize non-viewstate items
            
            MakeCallback();

            commands = new Dictionary<string, ZoliloJSCommand>();
            commands.Add("Comet", new ZoliloJSCommand(JSCommand_Comet));
            commands.Add("CometReturn", new ZoliloJSCommand(JSCommand_CometReturn));
            codeToExec = new List<string>();
            codeToExecSB = new StringBuilder();

            RunJavascript("BeginPolling", Page.CallbackObjectName + ".BeginShortPolling(300);");
            base.OnInit(e);
        }

        void ZoliloJavascriptCallbackControl_CometReturn(int serverPageCommandID, string commandResult)
        {
        }



        public void LoadState(object savedState)
        {
            object[] state = (object[])savedState;
            commandQueue = ConvertArrayToDict<int, string>((object[])state[0]);
            commandResponses = ConvertArrayToDict<int, string>((object[])state[1]);
            callbackreturnvalue = (string)state[2];
            //viewstateCallback.LoadState(null);
            stateLoaded = true;
        }

        public object SaveState()
        {
            object[] state = new object[3];
            state[0] = ConvertDictToArray(commandQueue);
            state[1] = ConvertDictToArray(commandResponses);
            state[2] = callbackreturnvalue;
           // viewstateCallback.SaveState();
            return state;
        }

        protected override void OnLoad(EventArgs e)
        {
            //Start Callserver
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", Page.CallbackObjectName + ".ReceiveServerData", "context", true);
            ZoliloJavaScriptFunctionControl JSCallServer = new ZoliloJavaScriptFunctionControl("CallServer" + CallbackID + "(arg, context)");
            //JSCallServer.CodeBody = "alert('CallServer" + CallbackID + "');" + cbReference;
            JSCallServer.CodeBody = cbReference;
            JS.Controls.Add(JSCallServer);

            base.OnLoad(e);
        }

        private void MakeCallback()
        {
            ZoliloJavaScriptImportControl JSCometMain = new ZoliloJavaScriptImportControl();
            JSCometMain.Src = "/Scripts/zolilo/zolilo.comet.main.js";
            Controls.Add(JSCometMain);

            JS = new ZoliloJavaScriptContainerControl();
            Controls.Add(JS);
            
            //Initialize object
            ZoliloJavaScriptControl comet = new ZoliloJavaScriptControl();
            comet.Code = "var " + Page.CallbackObjectName + " = new ZoliloCallback('" + CallbackID + "');";
            JS.Controls.Add(comet);
            

        }

        /// <summary>
        /// Gets the name of the Callback ID (same as UFrame div ID)
        /// </summary>
        public string CallbackID
        {
            get
            {
                if (Page.UFrameID != null)
                {
                    return Page.UFrameID;
                }
                return "";
            }
        }

        public int BeginExecuteClientStatement(string statement)
        {
            int key = ExecuteClientStatementInternal(statement);
            return key;
        }

        private int ExecuteClientStatementInternal(string statement)
        {
            DateTime timeBegin = DateTime.Now;
            
            string commandText;
            int key;
            do
            {
                key = new Random((int)(timeBegin.Ticks % (int.MaxValue - 1))).Next();
            }
            while (commandResponses.TryGetValue(key, out commandText)); //Repeat in the unlikely event that the key exists in the hash table

            commandQueue.Add(key, statement);

            return key;
        }

        #region ICallbackEventHandler Callbacks

        /// <summary>
        /// This function sends data to the client javascript to continue executing
        /// This function automatically executes (via ASP.NET) when RaiseCallbackEvent is completed
        /// The result is then transferred to the client and client javascript will resume.
        /// </summary>
        /// <returns></returns>
        public string GetCallbackResult()
        {
            return callbackreturnvalue;
        }

        /// <summary>
        /// This function is called from the client javascript
        /// The client javascript will HALT until this function returns, upon which 
        /// the return value of GetCallbackResult will be passed to the client
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaiseCallbackEvent(string eventArgument)
        {
            if (eventArgument != null)
            {
                string[] args = eventArgument.Split('◄');
                if (args.Length >= 3)//If format is valid (count◄command◄args
                {
                    string counter = args[0];
                    string commandType = args[1]; 
                    string[] commandArgs = new string[args.Length - 2];
                    Array.Copy(args, 2, commandArgs, 0, args.Length - 2); //Create new commandargs array

                    //counter + return val + optional args
                    callbackreturnvalue = Page.CallbackObjectName + "↨" + counter + "↨" + commands[commandType](commandArgs);
                }
            }
        }

        #endregion

        #region Delegate ZoliloJSCommand Commands

        /// <summary>
        /// This command will execute at a regular basis
        /// args = blank
        /// Reads the command queue and then returns a list of commands for the client to execute
        /// </summary>
        /// <param name="args">this parameter will be ignored</param>
        /// <returns>a literal array of commands for the client to execute [[key1,script1],[key2,script2],[key3,script3]], etc</returns>
        private string JSCommand_Comet(string[] args)
        {
            if (commandQueue.Count == 0)
                return null;
            List<int> commandsToPurge = new List<int>(); //Keep a separate list of commands (by key) to purge instead of removing directly to preserve for loop continuity

            //Construct the literal array to pass to client
            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            foreach (KeyValuePair<int, string> pair in new Dictionary<int, string>(commandQueue))
            {
                sb.Append("[" + pair.Key.ToString() + ",\\\"" + pair.Value + "\\\"],");
                //Remove item from queue. However, it is still pending response in responses dictionary
                commandQueue.Remove(pair.Key);
            }
            sb.Remove(sb.Length - 1, 1); //Remove last comma
            sb.Append("]");
            
            return sb.ToString();
        }

        public const string COMETRETURN_INVALID = "[INVALID]";
        public const string COMETRETURN_SUCCESS = "♠"; //alt 6
        private string JSCommand_CometReturn(string[] args)
        {
            int key = 0;
            if (args != null && args.Length == 2 && int.TryParse(args[0], out key))
            {
                CometReturn(key, args[1]);
                
                string returnValue = "\\\"" + COMETRETURN_SUCCESS + FlushJavascriptExecutionBuffer() + "\\\"";
                returnValue = returnValue.Replace("\r\n", "\\\r\n"); //Multiline terminator required for javascript eval statement
                JavascriptExecutionBuffer.Clear();
                return returnValue;
            }
            return COMETRETURN_INVALID;
        }

        private string FlushJavascriptExecutionBuffer()
        {
            codeToExecSB.Clear();
            foreach (string code in JavascriptExecutionBuffer)
                codeToExecSB.AppendLine(code);
            return codeToExecSB.ToString();
        }

        #endregion

        /// <summary>
        /// Queues a sequence of javascript code to execute when the callback is finished
        /// </summary>
        /// <param name="code"></param>
        public void ExecuteCode(string code)
        {
            JavascriptExecutionBuffer.Add(code);
        }
    }
}