<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GoalView.ascx.cs" Inherits="Zolilo.Web.GoalView" %>
<%@ Register TagPrefix="zolilo" TagName="GoalDefinitionControl" Src="~/Classes/Web/WebControls/ZoliloWidgetControls/ViewGoal/GoalDefinitionControl.ascx" %>
<div runat="server" class="zWidget" id="divGoalView">
    <div runat="server" id="header">
        <hr />
        <asp:PlaceHolder ID="LabelGoalName" runat="server" />
        Created Time: <zolilo:TimeLabel runat="server" ID="LabelTimeCreated" /><br />
        Created By: <zolilo:AgentLabel runat="server" ID="LabelAgentCreated" /><br />
        <zolilo:ZoliloButton runat="server" ID="button_delete" Text="Delete Goal" />
    </div>
        <hr />
        <b>Tags</b> <asp:PlaceHolder runat="server" ID="phAddTag" /><br />
        <asp:PlaceHolder runat="server" ID="phTags" /><br />
        <hr />
        <zolilo:GoalDefinitionControl runat="server" ID="GoalDef" />
        <hr />
        Achieving this goal helps achieve the following goals:
        <br />
        <asp:HyperLink ID="HyperLinkAddParent" runat="server" Text="Add to this list" /><br />
        <asp:PlaceHolder runat="server" ID="phParents" /><br />
        <hr />
        Acheving these goals helps achieve this goal:
        <br />
        <asp:HyperLink ID="HyperLinkAddChild" runat="server" Text="Add to this list" />
        <asp:PlaceHolder runat="server" ID="phChildren" /><br />
        <hr /> 
 </div>
