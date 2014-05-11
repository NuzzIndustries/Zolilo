using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using Zolilo.Data;

namespace Zolilo.Web
{
    public class GridViewTemplate<C, DR> : ITemplate
        where C : Control, new()
        where DR : DataRecord
    {
        ListItemType templateType;
        string columnName;
        Control itemControl;
        internal GridViewDataBindHandler<C, DR> dataBindHandler;

        public GridViewTemplate(ListItemType type, string colname, Control itemControl)
        {
            this.templateType = type;
            this.columnName = colname;
            this.itemControl = itemControl;
        }

        public void InstantiateIn(Control container)
        {
            switch (templateType)
            {
                case ListItemType.Header:
                    Label lbl = new Label();
                    lbl.Text = columnName;
                    container.Controls.Add(lbl);
                    break;
                case ListItemType.Item:
                    Control control = (Control)Activator.CreateInstance(itemControl.GetType());
                    control.DataBinding += new EventHandler(itemControl_DataBinding);
                    container.Controls.Add(control);
                    break;
                case ListItemType.EditItem:
                    break;
                case ListItemType.Footer:
                    CheckBox chkColumn = new CheckBox();
                    chkColumn.ID = "Chk" + columnName;
                    container.Controls.Add(chkColumn);
                    break;
 
            }
        }

        void itemControl_DataBinding(object sender, EventArgs e)
        {
            C control = (C)sender;
            GridViewRow container = (GridViewRow)control.NamingContainer;
            dataBindHandler(control, (DR)container.DataItem);
        }
    }

    public delegate void GridViewDataBindHandler<C, DR>(C control, DR data)
        where C : Control 
        where DR : DataRecord;

}