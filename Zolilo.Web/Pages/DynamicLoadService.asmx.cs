using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using AjaxControlToolkit;
using Zolilo.Communications;

namespace Zolilo.Pages
{
    /// <summary>
    /// Summary description for DynamicLoadService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService()]
    public class DynamicLoadService : System.Web.Services.WebService
    {

        [System.Web.Services.WebMethod(enableSession:true)]
        [System.Web.Script.Services.ScriptMethod()]
        public string DoLink(string val)
        {
            //alert("As you can see, the link no longer took you to jquery.com");
            UpdatePanel context;
            context = ZoliloSession.Current.UpdatePanelContext;
            //context.PopulateTriggerControlID = this.TagName;
            return "Hello World";
        }


        
        [System.Web.Services.WebMethod()]
        [System.Web.Script.Services.ScriptMethod()]
        public string DynamicPopulateMethod(string contextKey)
        {
            
            return "0" + contextKey + "1";
        }
    }
}
