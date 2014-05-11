<%@ Page Title="" Language="C#" MasterPageFile="/Pages/Site.Master" AutoEventWireup="true" CodeBehind="NewGoal.aspx.cs" Inherits="Zolilo.Pages.NewGoal" %>
<%@ Register TagPrefix="zolilo" TagName="GoalSelector" Src="/Classes/Web/WebControls/ZoliloWidgetControls/ViewGoal/GoalSelector.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<input type="hidden" id="input_newGoalParent" runat="server" value="-1" />
    <p>
        Create Goal
    </p>
    <p>
        <asp:Label ID="LabelGoalName" runat="server" Text="Goal Name:"></asp:Label>
        <asp:TextBox ID="TextBoxGoalName" runat="server" Height="22px" Width="300px" 
            MaxLength="50"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidatorGoalName" runat="server" 
            ControlToValidate="TextBoxGoalName" Display="Dynamic" 
            ErrorMessage="Field is required." Height="20px" 
            ondatabinding="ButtonSave_Click"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidatorGoalName" runat="server" 
            ControlToValidate="TextBoxGoalName" Display="Dynamic" 
            ErrorMessage="Name must be between 1-50 alphanumeric characters" 
            Height="20px" ValidationExpression="^[0-9a-zA-Z\s]{1,50}$" 
            ondatabinding="ButtonSave_Click"></asp:RegularExpressionValidator>
    </p>
   
        Parent Goal
        <zolilo:GoalSelector runat="server" ID="GoalSelectorParent" 
            SelectGoalIntroText="Select Parent Goal" />
        <br />
    
    <p>
    </p>
    <p>
        <zolilo:ZoliloButton ID="ButtonSave" runat="server" Text="Save"/>
    </p>
    <p>
        <zolilo:ZoliloButton ID="ButtonMakeActive" runat="server" Text="Make Active" 
            Visible="False" />
    </p>
    <p>
</asp:Content>
