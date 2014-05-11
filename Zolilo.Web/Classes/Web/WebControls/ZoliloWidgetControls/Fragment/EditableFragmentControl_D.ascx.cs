using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Data;

namespace Zolilo.Web
{
    public partial class EditableFragmentControl_D : System.Web.UI.UserControl, IJavaScriptObject
    {
        public const string PATH = "~/Classes/Web/WebControls/ZoliloWidgetControls/Fragment/EditableFragmentControl_D.ascx";

        public string GetConstructorExpression()
        {
            string c = "new EditableFragmentControl('" + divEditableFragmentControl.ID + "','" + this.GetClientID() + "');";
            return c;
        }

        public string GetClientID()
        {
            return divEditableFragmentControl.ClientID;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public PlaceHolder PHButtonSave
        {
            get { return PHbuttonSave; }
        }

        public PlaceHolder PHEditor
        {
            get { return PHeditor; }
        }

        public PlaceHolder PHFragmentText
        {
            get { return PHfragmentText; }
        }

        public PlaceHolder PHTitle1
        {
            get { return PHtitle1; }
        }

        public PlaceHolder PHTitle2
        {
            get { return PHtitle2; }
        }

        public PlaceHolder PHFragmentLink
        {
            get { return fragmentLink1; }
        }

    }
}