<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="Goals.aspx.cs" Inherits="Zolilo.Pages.Goals" %>
<%@ Register TagPrefix="zolilo" TagName="GoalSelector" Src="~/Classes/Web/WebControls/ZoliloWidgetControls/ViewGoal/GoalSelector.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        Goals<br />
        <br />
        <a href="/goals/new">Create New Goal</a><br />
    <zolilo:GoalSelector ID="goalSelector" runat="server" DirectLinkEnabled="true" SelectGoalIntroText="Show All Goals" />
</asp:Content>