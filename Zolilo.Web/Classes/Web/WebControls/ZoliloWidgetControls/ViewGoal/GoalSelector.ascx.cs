using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Zolilo.Data;

namespace Zolilo.Web
{
    //http://forums.asp.net/t/1149549.aspx
    public partial class GoalSelector : System.Web.UI.UserControl, IJavaScriptObject
    {
        bool selectionIsMandatory; //If true, uses a page validation mechanism
        
        public GoalSelector()
        {
            GridViewGoals = new GridView();
        }

        public string GetConstructorExpression()
        {
            string c = "new goalSelector('" + divGoal.ID + "','" + GetClientID() + "'";
                c += ",'" + inputFieldHidden.ClientID + "'";
            c += ");";
            return c;
               
        }

        public string GetClientID()
        {
            return divGoal.ClientID;
        }

        protected override void OnInit(EventArgs e)
        {
            inputFieldText.Attributes.Add("style", "display:none");
            inputFieldText.ID = "inputFieldText";
            validator.ControlToValidate = inputFieldText.ID;
            validator.ID = this.ID + "$" + "validator";
            validator.Text = "Must select a goal.";
            if (!selectionIsMandatory)
            {
                validator.Enabled = false;
            }
            base.OnInit(e);
        }

        //This will be slow and problematic with large number of goals and will need redesign.  For demo purposes only.
        protected void Page_Load(object sender, EventArgs e)
        {
            inputFieldText.Text = inputFieldHidden.Value;
            GridViewGoals.DataSource = ZoliloCache.Instance.Goals;
            GridViewGoals.DataBind();
        }

        protected void GridViewGoals_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label labelID = (Label)e.Row.FindControl("LabelID");
                HyperLink linkName = (HyperLink)e.Row.FindControl("HyperLinkGoalName");
                KeyValuePair<long, DR_Goals> item = (KeyValuePair<long, DR_Goals>)e.Row.DataItem;

                labelID.Text = item.Value.ID.ToString();
                if (directLinkEnabled)
                    linkName.NavigateUrl = "/goals/view?id=" + item.Value.ID.ToString();
                else
                {
                    linkName.NavigateUrl = ((ZoliloPage)Page).ClientURL + "#";
                    linkName.Attributes.Add("onclick", "getZWidget(this).selectGoal(" + item.Value.ID.ToString() + ");return false;");
                }
                linkName.Text = item.Value._Name;
            }
        }

        bool directLinkEnabled;

        public bool DirectLinkEnabled
        {
            get { return directLinkEnabled; }
            set { directLinkEnabled = value; }
        }

        public string SelectGoalIntroText
        {
            get { return LiteralSelectGoalIntro.Text; }
            set { LiteralSelectGoalIntro.Text = value; }
        }

        /// <summary>
        /// Set to true if client side validation will be made
        /// </summary>
        public bool SelectionIsMandatory
        {
            get { return selectionIsMandatory; }
            set { selectionIsMandatory = value; }
        }

        internal DR_Goals SelectedGoal 
        {
            get
            {
                long id = -1;
                if (Int64.TryParse(inputFieldText.Text, out id) && id > 0)
                    return DR_Goals.Get(id);
                return null;
            }
        }
    }


}