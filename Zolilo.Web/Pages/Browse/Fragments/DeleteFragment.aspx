<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="DeleteFragment.aspx.cs" Inherits="Zolilo.Pages.DeleteFragment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:Label runat="server" ID="label"></asp:Label>
<zolilo:ZoliloButton runat="server" ID="buttonconfirm" Text="DELETE"/>
</asp:Content>
