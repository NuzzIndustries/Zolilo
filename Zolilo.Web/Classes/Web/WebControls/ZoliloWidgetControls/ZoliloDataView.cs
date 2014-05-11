using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Zolilo.Data;

namespace Zolilo.Web
{
    public class ZoliloDataView : ZoliloJavascriptWidget
    {
        public HtmlGenericControl divIntro;
        public HtmlGenericControl divDataGrid;
        protected GridView grid;

        public ZoliloDataView()
        {
            className = "dataView";

            divIntro = new HtmlGenericControl("div");
            divDataGrid = new HtmlGenericControl("div");

            grid = new GridView();
            
            grid.AllowSorting = true;
            grid.AutoGenerateColumns = false;
            grid.ShowHeader = true;
            grid.ShowHeaderWhenEmpty = true;
            grid.AutoGenerateEditButton = false;
        }

        protected virtual void grid_RowDataBound(object sender, GridViewRowEventArgs e) { }

        protected override void OnInit(EventArgs e)
        {
            divIntro.ID = this.ID + "_intro";
            divDataGrid.ID = this.ID + "_grid";
            grid.ID = this.ID + "_gridview";
            divMain.Controls.Add(divIntro);
            divMain.Controls.Add(divDataGrid);
            divDataGrid.Controls.Add(grid);
            base.OnInit(e);
        }

        protected void AddColumn<C, DR>(string headerName, 
            GridViewDataBindHandler<C, DR> dataBindCallback) 
            where C : Control, new() 
            where DR : DataRecord
        {
            GridViewTemplate<C, DR> gvtHeader, gvtItem;
            TemplateField tf;

            gvtHeader = new GridViewTemplate<C, DR>(ListItemType.Header, headerName, null);
            gvtItem = new GridViewTemplate<C, DR>(ListItemType.Item, null, new C());
            gvtItem.dataBindHandler = dataBindCallback;

            tf = new TemplateField();
            tf.HeaderTemplate = gvtHeader;
            tf.ItemTemplate = gvtItem;
            grid.Columns.Add(tf);
        }
    }
}