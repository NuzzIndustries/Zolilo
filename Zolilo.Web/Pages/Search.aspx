<%@ Page Title="" Language="C#" MasterPageFile="/Pages/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Zolilo.Pages.Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<zolilo:ZoliloDataView runat="server" ID="dataviewtest" /><br />
<a href="/TEST&2?1?1">ERROR TEST</a><br />
<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
<zolilo:ZoliloButton ID="button" runat="server" Text="Test Redirect" />
</asp:Content>
