<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="AddGoalVertex.aspx.cs" Inherits="Zolilo.Pages.AddGoalVertex" %>
<%@ Register TagPrefix="zolilo" TagName="GoalSelector" Src="~/Classes/Web/WebControls/ZoliloWidgetControls/ViewGoal/GoalSelector.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<br />
<asp:Label runat="server" ID="labelHeader"></asp:Label>
<br />
Not on this list? <asp:HyperLink runat="server" ID="LinkNewGoal" Text="Create a new goal."></asp:HyperLink><br />
<zolilo:GoalSelector runat="server" ID="goalSelector" SelectionIsMandatory="true" DirectLinkEnabled="false" />

<asp:PlaceHolder runat="server" ID="LabelPlaceholder"></asp:PlaceHolder>

<div runat="server" ID="ContentDiv">
<br />
<br />
<zolilo:ZoliloButton runat="server" ID="buttonSubmit" Text="Submit" />
<zolilo:ZoliloButton runat="server" ID="buttonCancel" Text="Cancel" />
</div>

</asp:Content>
