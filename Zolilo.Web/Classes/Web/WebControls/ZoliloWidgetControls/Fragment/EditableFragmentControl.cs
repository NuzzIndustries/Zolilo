using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zolilo.Web
{
    public class EditableFragmentControl : FragmentControl
    {
        EditableFragmentControl_D designer;

        public TextBox editor = new TextBox();
        public ZoliloButton buttonSave = new ZoliloButton();
        public string title;

        public EditableFragmentControl()
            : base()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            this.EnableViewState = false;
            designer = (EditableFragmentControl_D)Page.LoadControl(EditableFragmentControl_D.PATH);

            editor.ReadOnly = false;
            editor.TextMode = TextBoxMode.MultiLine;
            editor.Text = "";
            editor.ID = "editor";
            editor.CssClass = "zElement";
            
            buttonSave.Text = "Save";
            buttonSave.ID = "buttonSave";
            buttonSave.Click += new EventHandler(buttonSave_Click);

            designer.PHFragmentText.Controls.Add(fragmentText);
            designer.PHEditor.Controls.Add(editor);
            designer.PHButtonSave.Controls.Add(buttonSave);

            Controls.Add(designer);
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            designer.PHFragmentLink.Controls.Add(link);
            editor.Height = 350;
            editor.Width = 600;
            if (title != null)
            {
                Designer.PHTitle1.Controls.Add(new LiteralControl("<b>" + title + "</b> "));
                Designer.PHTitle2.Controls.Add(new LiteralControl("<b>" + title + "</b> "));
            }
            base.OnLoad(e);
        }

        void buttonSave_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                Commit();
            }
        }
        
        /// <summary>
        /// Gets or sets the text to be displayed on the webpage in the fragment header, to describe what the fragment is
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public override string Text
        {
            get
            {
                return editor.Text;
            }
            set
            {
                editor.Text = value;
            }
        }

        public EditableFragmentControl_D Designer
        {
            get { return designer; }
        }
    }
}