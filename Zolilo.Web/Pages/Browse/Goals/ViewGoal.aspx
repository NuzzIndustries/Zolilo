<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="ViewGoal.aspx.cs" Inherits="Zolilo.Pages.ViewGoal" %>
<%@ Register TagPrefix="zolilo" TagName="GoalView" Src="~/Classes/Web/WebControls/ZoliloWidgetControls/ViewGoal/GoalView.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <zolilo:GoalView runat="server" ID="GoalViewControl" />
</asp:Content>
