using System;
using System.Web.UI.WebControls;
using Zolilo.Data;

namespace Zolilo.Web
{
    public class TagViewList : ZoliloDataView
    {
        public event GridViewDataBindHandler<HyperLink, DR_Tags> CellDataBinding;

        public TagViewList()
        {
            AddColumn<HyperLink, DR_Tags>("Tag", new GridViewDataBindHandler<HyperLink, DR_Tags>(TagName_CellDataBinding));
            CellDataBinding += new GridViewDataBindHandler<HyperLink, DR_Tags>(TagViewList_CellDataBinding);
        }

        protected override void OnLoad(EventArgs e)
        {
            grid.DataSource = ZoliloCache.Instance.Tags.Values;
            grid.DataBind();
            base.OnLoad(e);
        }

        void TagName_CellDataBinding(HyperLink control, DR_Tags dr)
        {
            control.Text = dr._Name;
            control.NavigateUrl = "/tags/view?id=" + dr.ID.ToString();
            CellDataBinding(control, dr);
        }

        void TagViewList_CellDataBinding(HyperLink control, DR_Tags data)
        {
        }
    }
}