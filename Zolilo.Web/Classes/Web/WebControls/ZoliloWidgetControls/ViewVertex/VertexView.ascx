<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VertexView.ascx.cs" Inherits="Zolilo.Web.VertexView" %>
<div runat="server" id="divVertexView">
    <div runat="server" id="header">
        <hr />
        <b>Parent Object</b><br />
        <zolilo:NodeViewMini runat="server" ID="nodeParent" /><br />
        <hr />
        <b>Child Object</b><br />
        <zolilo:NodeViewMini runat="server" ID="nodeChild" /><br />
        <hr />
    </div>
    <div runat="server" id="body">
        <b>Definition</b>
        <zolilo:EditableFragmentControl runat="server" ID="fragmentControl" /><br />
        <hr />
        <b>Options</b><br />
        <zolilo:ZoliloButton runat="server" ID="button_delete" Text="Delete Connection" />
    </div>
</div>