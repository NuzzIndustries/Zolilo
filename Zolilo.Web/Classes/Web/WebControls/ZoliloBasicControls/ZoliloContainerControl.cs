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
    public abstract class ZoliloContainerControl : ZoliloWebControl, INamingContainer
    {

        string contents;
        [Browsable(true)]
        public string Contents
        {
            get { return contents; }
            set { contents = value; }
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();
            this.contentDataObject = new ZoliloTemplateData();
            if (contentTemplate != null)
                contentTemplate.InstantiateIn(contentDataObject);
            Controls.Add(contentDataObject);
        }

        public override void DataBind()
        {
            CreateChildControls();
            this.ChildControlsCreated = true;
            base.DataBind();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (contents != null && contents.Length > 0)
                writer.Write(contents);
            else if (contentTemplate != null)
            {
                if (!ChildControlsCreated)
                    DataBind();
                contentDataObject.RenderControl(writer);
            }
            //base.Render(writer);
        }

        ITemplate contentTemplate;

        [
        Browsable(false),
        DefaultValue(null),
        Description("Content of control goes inside here."),
        TemplateContainer(typeof(ZoliloTabPanelData)),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual ITemplate ContentTemplate
        {
            get { return contentTemplate; }
            set { contentTemplate = value; }
        }

        ZoliloTemplateData contentDataObject;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ZoliloTemplateData ContentDataObject
        {
            get
            {
                this.EnsureChildControls();
                return contentDataObject;
            }
            set { contentDataObject = value; }
        }

        protected override void AddParsedSubObject(object obj)
        {
            if (obj is UserControl)
                Controls.Add((UserControl)obj);
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

    /// <summary>
    /// Placeholder data class for the template container
    /// http://msdn.microsoft.com/en-us/library/aa478964.aspx
    /// </summary>
    [ToolboxItem(false)]
    public class ZoliloTemplateData : ZoliloWebControl, INamingContainer
    {
        public override void LoadState(object savedState)
        {
            throw new NotImplementedException();
        }

        public override object SaveState()
        {
            throw new NotImplementedException();
        }
    }
}