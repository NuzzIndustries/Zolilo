using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Data;

namespace Zolilo.Web
{
    public class FragmentReplyControl : ZoliloWebControl
    {
        DR_Fragments parentFragment;
        RadioButtonList feedback;

        TextBox text;
        ZoliloButton submit;
       
        protected override void OnInit(EventArgs e)
        {
            text = new TextBox();
            Controls.Add(new LiteralControl("Add Reply: <br />"));
            feedback = new RadioButtonList();
            feedback.ID = this.ID + "Feedback";
            feedback.Items.Add(new ListItem("Neutral (Not intended to agree or disagree with the above statement)"));
            feedback.Items.Add(new ListItem("Positive (I agree with the above statement)"));
            feedback.Items.Add(new ListItem("Negative (I disagree with the above statement)"));
            Controls.Add(feedback);
            Controls.Add(new LiteralControl("<br>"));

            text.TextMode = TextBoxMode.MultiLine;
            Controls.Add(text);
            Controls.Add(new LiteralControl("<br>"));

            submit = new ZoliloButton();
            submit.Text = "Submit";
            submit.Click += new EventHandler(submit_Click);
            Controls.Add(submit);

            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            text.ID = this.ID + "Text";
            submit.ID = this.ID + "Submit";

            if (parentFragment == null)
                throw new ZoliloWebException("Parent fragment must not be null on FragmentReplyControl");
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {

            base.OnPreRender(e);
        }

        void submit_Click(object sender, EventArgs e)
        {
            DR_Fragments newFrag = new DR_Fragments();
            newFrag.Text = text.Text;
            string feedback1 = feedback.SelectedValue; ; //Update this
            if (feedback1 == "Positive")
                newFrag._BaseWeight = FragmentFeedback.Positive;
            else if (feedback1 == "Negative")
                newFrag._BaseWeight = FragmentFeedback.Negative;
            else
                newFrag._BaseWeight = FragmentFeedback.None;
            newFrag.SaveChanges();
            
            parentFragment.AttachChildNodeWithNewEdge(newFrag, typeof(Fragment2Fragment_Reply));

            WebDirector.Instance.Redirect("/fragments/view?id=" + newFrag.ID);
        }

    

        public DR_Fragments ParentFragment
        {
            get { return parentFragment; }
            set { parentFragment = value; }
        }
    }
}