<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Zolilo.Pages.Login" %>
<%@ Register TagPrefix="zolilo" TagName="Login" Src="~/Classes/Web/WebControls/ZoliloWidgetControls/Account/AccountLoginControl_D.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <zolilo:Login runat="server" ID="loginControl" />
</asp:Content>
