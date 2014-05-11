using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace Zolilo.Web
{
    [ParseChildren(false)]
    public class ZoliloTabContainer : ZoliloContainerControl
    {

        #region Properties

        ZoliloTabPanel selectedTab;
        public ZoliloTabPanel SelectedTab
        {
            get { return selectedTab; }
            set { selectedTab = value; }
        }

        [Browsable(true)]
        int SelectedTabIndex
        {
            get { return Tabs.IndexOf(selectedTab); }
            set { selectedTab = Tabs[value]; }
        }

        List<ZoliloTabPanel> tabs;
        [EditorAttribute(typeof(CollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public List<ZoliloTabPanel> Tabs
        {
            get { return tabs; }
            set { tabs = value; }
        }

        string name;
        [Browsable(true)]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        #endregion

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            foreach (ZoliloTabPanel tab in tabs)
            {
                Controls.Add(tab);
            }
        }

        protected override void AddParsedSubObject(object obj)
        {
            if (tabs == null)
                tabs = new List<ZoliloTabPanel>();
            if (obj is ZoliloTabPanel)
                tabs.Add((ZoliloTabPanel)obj);
        }

        protected override void OnInit(EventArgs e)
        {
            if (tabs == null)
            {
                tabs = new List<ZoliloTabPanel>();
                EnsureChildControls();
            }
            if (this.SelectedTabIndex < 0 && tabs.Count > 0)
                this.SelectedTabIndex = 0;
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {

            base.OnLoad(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {

            writer.WriteLine("<script src=\"/Scripts/TabView/js/tab-view.js\"></script>");
            writer.WriteLine("<link rel=\"stylesheet\" href=\"" + CssClass + "\" type=\"text/css\" media=\"screen\">");

            writer.WriteLine("<div id=\"" + ID + "\">");
            foreach (ZoliloTabPanel tab in tabs)
            {
                writer.WriteLine("<div class=\"dhtmlgoodies_aTab\">");
                tab.RenderControl(writer);
                writer.WriteLine("</div>");
            }

            writer.WriteLine("<script type=\"text/javascript\">");
            writer.Write("initTabs('" + ID + "',Array(");
            for (int i = 0; i < tabs.Count; i++)
            {
                writer.Write("'" + tabs[i].ID + "'");
                if (i != tabs.Count - 1)
                    writer.Write(", ");
            }
            writer.Write("), " + SelectedTabIndex.ToString() + ", " + Width.Value.ToString() + ", " + Height.Value.ToString() + ");");
            writer.WriteLine();
            writer.WriteLine("</script>");

            /* //OLD
            writer.WriteLine("<script src=\"/Scripts/TabView/js/tab-view.js\"></script>");
            writer.WriteLine("<link rel=\"stylesheet\" href=\"" + CssClass + "\" type=\"text/css\" media=\"screen\">");
            
            writer.WriteLine("<div id=\"" + ID + "\">");
            foreach (ZoliloTab tab in items)
            {
                writer.WriteLine("<div class=\"dhtmlgoodies_aTab\">");
                writer.WriteLine(tab.Contents);
                writer.WriteLine("</div>");
            }
            writer.Write("</div>");

            writer.WriteLine("<script type=\"text/javascript\">");
            writer.Write("initTabs('" + ID + "',Array(");
            for (int i = 0; i < items.Count; i++)
            {
                writer.Write("'" + items[i].ID + "'");
                if (i != items.Count - 1)
                    writer.Write(", ");
            }
            writer.Write("), " + SelectedTabIndex.ToString() + ", " + Width.Value.ToString() + ", " + Height.Value.ToString() + ");");
            writer.WriteLine();
            writer.WriteLine("</script>");
             * */
           // base.Render(writer);
        }

        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }


        public override void LoadState(object savedState)
        {
            throw new NotImplementedException();
        }

        public override object SaveState()
        {
            throw new NotImplementedException();
        }
    }

    public class ZoliloTab : ZoliloWebControl
    {

        public ZoliloTab(string id)
        {
            this.ID = id;
        }

        string contents;
        [Browsable(true)]
        public string Contents
        {
            get { return contents; }
            set { contents = value; }
        }


        public override void LoadState(object savedState)
        {
            throw new NotImplementedException();
        }

        public override object SaveState()
        {
            throw new NotImplementedException();
        }


        internal void SetURL(string pagelocation)
        {
            Contents = "<div id=\"" + ID + "\" runat=\"server\" src=\"" + pagelocation + "\" style=\"position: relative; top: 0px;\"></div>";
        }
    }

}