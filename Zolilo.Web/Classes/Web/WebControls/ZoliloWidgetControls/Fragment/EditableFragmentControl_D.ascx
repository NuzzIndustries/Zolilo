<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditableFragmentControl_D.ascx.cs" Inherits="Zolilo.Web.EditableFragmentControl_D" %>
<div id="divEditableFragmentControl" runat="server" class="zWidget">
    <div runat="server" id="view">
        <asp:PlaceHolder runat="server" ID="PHtitle1"></asp:PlaceHolder>
        <asp:HyperLink ID="HyperLinkEdit" runat="server" NavigateUrl="" onclick="getZWidget(this).edit();return false;">Edit</asp:HyperLink> | <asp:PlaceHolder runat="server" ID="fragmentLink1" /><br />
        <asp:PlaceHolder runat="server" ID="PHfragmentText"></asp:PlaceHolder><br />
    </div>
    <div runat="server" id="edit" style="visibility:hidden; display:none">
        <asp:PlaceHolder runat="server" ID="PHtitle2"></asp:PlaceHolder>
        <asp:HyperLink ID="HyperLinkCancelEdit" runat="server" NavigateUrl="" onclick="getZWidget(this).cancelEdit();return false;">Cancel Edit</asp:HyperLink><br />
        <asp:PlaceHolder runat="server" ID="PHeditor"></asp:PlaceHolder><br />
        <asp:PlaceHolder runat="server" ID="PHbuttonSave"></asp:PlaceHolder>
    </div>
</div>