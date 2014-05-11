<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="IDLink.aspx.cs" Inherits="Zolilo.Pages.IDLink" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        Thank you for logging in.&nbsp; As this is your first time logging in with this account, you must enter your Zolilo username and password to link the accounts.
    </p>
    <asp:Label ID="LabelUserName" runat="server" Text="User Name" Height="20px" 
        style="text-align: right" Width="130px"></asp:Label>
    <asp:TextBox ID="TextBoxUserName" runat="server" Width="242px" 
        style="margin-left: 30px" CausesValidation="True"></asp:TextBox>
           <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
        ControlToValidate="TextBoxUserName" Display="Dynamic" 
        ErrorMessage="Field is required." Height="20px" 
        ondatabinding="ButtonSubmit_Click"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
        ControlToValidate="TextBoxUserName" Display="Dynamic" 
        ErrorMessage="Username must be between 3-30 alphanumeric characters" 
        Height="20px" ValidationExpression="^[0-9a-zA-Z]{3,30}$" 
        ondatabinding="ButtonSubmit_Click"></asp:RegularExpressionValidator>
    <br />
    <asp:Label ID="LabelPassword" runat="server" Text="Password" Height="20px" 
        style="text-align: right" Width="130px"></asp:Label>
    <asp:TextBox ID="TextBoxPassword" runat="server" Width="242px" 
        style="margin-left: 30px" MaxLength="35" 
        TextMode="Password" CausesValidation="True"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
        ControlToValidate="TextBoxPassword" Display="Dynamic" 
        ErrorMessage="Field is required." Height="20px" 
        ondatabinding="ButtonSubmit_Click"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
        ControlToValidate="TextBoxPassword" Display="Dynamic" 
        ErrorMessage="Must be 8-30 characters in length." Height="20px" 
        ValidationExpression="^.{8,30}$" ondatabinding="ButtonSubmit_Click"></asp:RegularExpressionValidator>
    <p>
        <asp:Button ID="ButtonSubmit" runat="server" Text="Submit" onclick="ButtonSubmit_Click" />
    </p>
    <p>
    Result:<br />
        <asp:TextBox ID="TextBoxResult" runat="server" style="margin-left: 0px" Height="21px" 
                Width="569px"></asp:TextBox>
    </p>
</asp:Content>
