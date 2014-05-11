using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

namespace Zolilo.Web
{
    public delegate void ZoliloWebControlPostbackHandler(string args);
    public interface IZoliloViewStateControl
    {
        void LoadState(object savedState);
        object SaveState();
    }

    /// <summary>
    /// A marker interface to be used if this ZoliloWebControl object has a designer (ascx file)
    /// </summary>
    public interface IZoliloWebControlHasDesigner
    {
        UserControl Designer { get; set; }
        string ControlPath { get; }
    }
    [Serializable]
    public abstract class ZoliloWebControl : WebControl
    {
        /// <summary>
        /// Fired when the page posts back with this control. 
        /// </summary>
        protected event ZoliloWebControlPostbackHandler PostBackTrigger;

        public ZoliloWebControl()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            if (this is IZoliloWebControlHasDesigner)
            {
                IZoliloWebControlHasDesigner thisControl = (IZoliloWebControlHasDesigner)this;
                thisControl.Designer = (UserControl)Page.LoadControl(thisControl.ControlPath);
                Controls.Add((Control)thisControl);
            }
            base.OnInit(e);
        }

        internal void TriggerPostBack(string args)
        {
            PostBackTrigger(args);
        }
            
        /// <summary>
        /// Runs arbitrary javascript code when page finishes loading.
        /// </summary>
        /// <param name="code">the javascript code to run</param>
        internal void RunJavascript(string name, string code)
        {
            System.Text.StringBuilder cstext1 = new System.Text.StringBuilder();
            cstext1.Append("<script type=\"text/javascript\">" + code + "</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), name, cstext1.ToString());
        }

        /// <summary>
        /// Loads the serialized data to persist the data between postbacks
        /// </summary>
        /// <param name="savedState"></param>
        //public abstract void LoadState(object savedState);

        /// <summary>
        /// Saves the serialized data to be transferred between the server and client between postbacks
        /// </summary>
        /// <returns></returns>
        //public abstract object SaveState();

        protected object SaveControlStateZ()
        {
            ObjectStateFormatter o = new ObjectStateFormatter();
            return o.Serialize(this);
        }

        protected override object SaveControlState()
        {
            if (this is IZoliloViewStateControl)
                return ((IZoliloViewStateControl)this).SaveState();
            return null;
        }

        protected override void LoadControlState(object savedState)
        {
            if (this is IZoliloViewStateControl)
                ((IZoliloViewStateControl)this).LoadState(savedState);
            base.LoadControlState(savedState);
        }

        /// <summary>
        /// Converts dictionary to array for serialization
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static object[] ConvertDictToArray<A, B>(Dictionary<A, B> dict)
        {
            object[] array = new object[dict.Count * 2];
            int i = 0;
            foreach (KeyValuePair<A, B> pair in dict)
            {
                array[i] = pair.Key;
                array[i + 1] = pair.Value;
                i += 2;
            }
            return array;
        }

        /// <summary>
        /// Converts an object array to dictionary.  Object array MUST be correct type
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static Dictionary<A, B> ConvertArrayToDict<A, B>(object[] array)
        {
            Dictionary<A, B> dict = new Dictionary<A, B>();
            for (int i = 0; i < array.Length; i += 2)
            {
                dict.Add((A)array[i], (B)array[i + 1]);
            }
            return dict;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }

        public new ZoliloPage Page
        {
            get { return (ZoliloPage)(base.Page); }
        }

        public static object CloneControls(object o)
        {
            Type type = o.GetType();
            PropertyInfo[] properties = type.GetProperties();
            object retObject = type.InvokeMember("", BindingFlags.CreateInstance, null, o, null);
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(retObject, propertyInfo.GetValue(o, null), null);
                }
            }
            return retObject;
        }
    }

    /// <summary>
    /// If a widget has object-oriented client side functionality, then this 
    /// interface should go on the Server Control which generates the widget
    /// </summary>
    public interface IJavaScriptObject
    {
        /// <summary>
        /// Returns the client side expression which is used to instantiate the widget script
        /// Typically consists of the short ID, then the client ID
        /// </summary>
        /// <returns></returns>
        string GetConstructorExpression();

        /// <summary>
        /// Returns the ID of the div tag associated with this object
        /// </summary>
        /// <returns></returns>
        string GetClientID();
    }
}