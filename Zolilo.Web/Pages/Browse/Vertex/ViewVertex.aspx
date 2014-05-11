<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="ViewVertex.aspx.cs" Inherits="Zolilo.Pages.ViewVertex" %>
<%@ Register TagPrefix="zolilo" TagName="VertexView" Src="~/Classes/Web/WebControls/ZoliloWidgetControls/ViewVertex/VertexView.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<title>Vertex</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<zolilo:VertexView runat="server" ID="viewVertex"></zolilo:VertexView>
</asp:Content>
