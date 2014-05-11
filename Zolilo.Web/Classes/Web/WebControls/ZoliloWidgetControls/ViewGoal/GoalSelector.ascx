<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GoalSelector.ascx.cs" Inherits="Zolilo.Web.GoalSelector" %>
<div class="zWidget" runat="server" id="divGoal">
<asp:RangeValidator ID="validator" runat="server" MinimumValue="1" MaximumValue="2147483647" Type="Integer"></asp:RangeValidator>
<asp:HiddenField runat="server" ID="inputFieldHidden" Value="-1" />
<asp:TextBox runat="server" ID="inputFieldText" Text="-1"></asp:TextBox>
<div runat="server" id="intro">
<asp:HyperLink ID="HyperLinkSelectGoalIntro" runat="server" NavigateUrl="" onclick="getZWidget(this).selectGoalRequest();return false;">
<asp:Literal ID="LiteralSelectGoalIntro" runat="server" Text="Select Goal">
</asp:Literal>
</asp:HyperLink> 
</div>

<div runat="server" id="final" style="visibility:hidden; display:none">
    <div runat="server" id="result">
    </div>
    <div runat="server" id="back">
        <asp:HyperLink ID="HyperLinkSelectGoalReset" runat="server" NavigateUrl="" onclick="getZWidget(this).resetView();return false;">Unselect</asp:HyperLink>
    </div>
</div>

<div runat="server" id="select" style="visibility:hidden; display:none">
    <asp:Label ID="LabelHeader" runat="server" Text="" />
    <asp:GridView ID="GridViewGoals" runat="server" AllowSorting="True" AutoGenerateColumns="False" onrowdatabound="GridViewGoals_RowDataBound" ShowHeader="True" ShowHeaderWhenEmpty="true">
    <Columns>
        <asp:TemplateField HeaderText="ID">
            <ItemTemplate>
                <asp:Label ID="LabelID" runat="server" Text="ID"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Goal Name">
            <ItemTemplate>
                <asp:HyperLink ID="HyperLinkGoalName" runat="server" NavigateUrl="">Goal Name</asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        This goal is an orphaned goal.  It is not connected to any other goals.
    </EmptyDataTemplate>
    </asp:GridView>
</div>
</div>
