using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Zolilo.Web;
using System.Text;

namespace Zolilo.Web
{
    /// <summary>
    /// Use this to dynamically alter viewstate values
    /// </summary>
    public class ZoliloViewStateControl : ZoliloWebControl, IZoliloViewStateControl
    {
        public HiddenField hf;

        Dictionary<string, string> hashTable;

        ZoliloJavascriptCallbackControl callbackParent;

        public ZoliloViewStateControl(ZoliloJavascriptCallbackControl callbackParent)
        {
            this.callbackParent = callbackParent;
        }

        protected override void OnInit(EventArgs e)
        {
            hf = new HiddenField();
            hashTable = new Dictionary<string, string>();
            Controls.Add(hf);
           // hf.Value = "hf_";
            hf.ID = "ZoliloViewState_hf";

            base.OnInit(e);
            
        }

        protected override void LoadControlState(object savedState)
        {
            base.LoadControlState(savedState);

        }

        protected override void OnLoad(EventArgs e)
        {

            base.OnLoad(e);
        }

        public string this[string key]
        {
            get 
            {
                if (!hashTable.ContainsKey(key))
                    return null;
                else
                    return hashTable[key]; 
            }
            set 
            {
                if (!hashTable.ContainsKey(key))
                    hashTable.Add(key, value);
                else
                    hashTable[key] = value; 
            }
        }

        internal void ParseViewState()
        {
            if (hf.Value != null && hf.Value.Length > 0)
            {
                string compiledViewState = hf.Value;
                hashTable.Clear();

                string[] separator = new string[1];
                separator[0] = "☻";
                string[] pairs = compiledViewState.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                if (pairs.Length > 0 && pairs[0].Length > 0)
                {
                    foreach (string pair in pairs)
                    {
                        string[] keyValue = pair.Split('☺');
                        hashTable[keyValue[0]] = keyValue[1];
                    }
                }
            }
        }

        public void LoadState(object savedState)
        {
        }

        public object SaveState()
        {
            int counter = 0;
            foreach (KeyValuePair<string, string> pair in hashTable)
            {
                //Ensure that this code executes first
                callbackParent.JavascriptExecutionBuffer.Insert(counter, Page.Snippets.UpdateZoliloViewState(pair.Key, pair.Value));
                counter++;
            }
            return null;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }
    }
}