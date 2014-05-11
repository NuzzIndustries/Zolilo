<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="AddTag.aspx.cs" Inherits="Zolilo.Pages.AddTag" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
Adding tag to:<br />

<zolilo:GoalViewMini runat="server" ID="goalViewMini" /><br />
<zolilo:TagViewList runat="server" ID="tagViewList" /><br /> 
</asp:Content>
