<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="NewAgent.aspx.cs" Inherits="Zolilo.Pages.NewAgent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        Create your Agent profile that you will use on Zolilo.com</p>
    <p>
        <asp:Label ID="Label1" runat="server" Text="Agent Name" Width="130px" style="text-align: right"></asp:Label>
        <asp:TextBox ID="TextBoxAgentName" runat="server" style="margin-left: 30px" CausesValidation="True"></asp:TextBox>

        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
        ControlToValidate="TextBoxAgentName" Display="Dynamic" 
        ErrorMessage="Field is required." Height="20px" 
        ondatabinding="ButtonSubmit_Click"></asp:RequiredFieldValidator>

    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
        ControlToValidate="TextBoxAgentName" Display="Dynamic" 
        ErrorMessage="Name must be between 3-30 alphanumeric characters" 
        Height="20px" ValidationExpression="^[0-9a-zA-Z]{3,30}$" 
        ondatabinding="ButtonSubmit_Click"></asp:RegularExpressionValidator>
        <br />
        <asp:Label ID="Label2" runat="server" Width="130px" style="text-align: right" Text="(not your real name)"></asp:Label>
    </p>
    <p>
        <asp:Label ID="LabelResult" runat="server" Text=""></asp:Label>
    </p>
    <p>
    <asp:Button ID="ButtonSubmit" runat="server" Text="Submit" onclick="ButtonSubmit_Click" />
    </p>
    
</asp:Content>
