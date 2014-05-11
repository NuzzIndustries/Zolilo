using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zolilo.Data;

namespace Zolilo.Web
{
    public abstract class FragmentControl : ZoliloWebControl
    {
        public DR_Fragments fragment;
        public GraphNode parentNode;
        public Type fragmentEdgeType;
        public HyperLink link;
        public Label fragmentText = new Label();
        bool disablehyperlink = false;

        bool renderChildFragments = true;

        public FragmentControl()
            : base()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            this.EnableViewState = false;
            fragmentText.ID = "reader";
            fragmentText.CssClass = "zElement";
            fragmentText.Width = 600;

            link = new HyperLink();
            link.Text = "Fragment Page";

            
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (fragment == null)
            {
                if (parentNode == null)
                    throw new ZoliloWebException("When creating a new fragment, ParentNode of FragmentControl must be set during OnInit");
                if (fragmentEdgeType == null)
                    throw new ZoliloWebException("When creating a new fragment, FragmentEdgeType must be set during OnInit");
                fragment = new DR_Fragments();
                link.Visible = false;
            }
            else
            {
                fragmentText.Text = fragment.Text.Replace("\n", "<br>");    
                link.Visible = true;
                link.NavigateUrl = "/fragments/view?id=" + fragment.ID.ToString();
            }
            if (disablehyperlink)
                link.Visible = false;
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        public abstract string Text {get; set; }

        public DR_Fragments Fragment
        {
            get { return fragment; }
            set { fragment = value; fragmentText.Text = fragment.Text; }
        }

        public GraphNode ParentNode
        {
            get { return fragment == null ? parentNode : fragment.SingularParent; }
            set { parentNode = value; }
        }

        public Type FragmentEdgeType
        {
            get { return fragmentEdgeType; }
            set
            {
                if (!typeof(FragmentEdge).IsAssignableFrom(value))
                    throw new ZoliloWebException("Invalid EdgeType");
                this.fragmentEdgeType = value;
            }
        }

        public bool RenderChildFragments
        {
            get { return renderChildFragments; }
            set { renderChildFragments = value; }
        }

        public void Commit()
        {
            fragment.Text = Text == null ? null : Text.Trim();
            
            if (fragment.IsNull) //new fragment
            {
                fragment.SaveChanges();
                parentNode.AttachChildNodeWithNewEdge(fragment, fragmentEdgeType);
            }
            else
                fragment.SaveChanges();

            OnLoad(null);
        }

        public bool DisableHyperlink
        {
            get { return disablehyperlink; }
            set { disablehyperlink = value; }
        }
    }
}