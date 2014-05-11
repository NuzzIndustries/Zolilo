<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Zolilo.Pages.Page_Register"%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <input id="testreg" type="hidden" runat="server" value="testreg" />
    Account Registration<br />
    <br />
    <asp:Label ID="LabelUserName" runat="server" Text="User Name" Height="20px" 
        style="text-align: right" Width="130px"></asp:Label>
    <asp:TextBox ID="TextBoxUserName" runat="server" Width="242px" 
        style="margin-left: 30px"></asp:TextBox>
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
        style="margin-left: 30px" CausesValidation="True" MaxLength="35" 
        TextMode="Password"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
        ControlToValidate="TextBoxPassword" Display="Dynamic" 
        ErrorMessage="Field is required." Height="20px" 
        ondatabinding="ButtonSubmit_Click"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
        ControlToValidate="TextBoxPassword" Display="Dynamic" 
        ErrorMessage="Must be 8-30 characters in length." Height="20px" 
        ValidationExpression="^.{8,30}$" ondatabinding="ButtonSubmit_Click"></asp:RegularExpressionValidator>
        <br />
    
    <asp:Label ID="LabelConfirmPassword" runat="server" Text="Confirm Password" Height="20px" 
        style="text-align: right" Width="130px"></asp:Label>
    <asp:TextBox ID="TextBoxConfirmPassword" runat="server" Width="242px" 
        style="margin-left: 30px" TextMode="Password"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
        ControlToValidate="TextBoxConfirmPassword" Display="Dynamic" 
        ErrorMessage="Field is required." Height="20px" 
        ondatabinding="ButtonSubmit_Click"></asp:RequiredFieldValidator>
    <asp:CompareValidator ID="CompareValidator1" runat="server" 
        ControlToCompare="TextBoxPassword" ControlToValidate="TextBoxConfirmPassword" 
        Display="Dynamic" ErrorMessage="Password and confirm password does not match." 
        Height="20px" ondatabinding="ButtonSubmit_Click"></asp:CompareValidator>
        <br />
    <asp:Label ID="LabelEmail" runat="server" Text="E-Mail Address" Height="20px" 
        style="text-align: right" Width="130px"></asp:Label>
    <asp:TextBox ID="TextBoxEmail" runat="server" Width="242px" 
        style="margin-left: 30px"></asp:TextBox>
        <asp:RequiredFieldValidator 
            ID="RequiredFieldValidator4" 
            runat="server" 
            ControlToValidate="TextBoxEmail" 
            Display="Dynamic" 
            ErrorMessage="Field is required." Height="20px" 
        ondatabinding="ButtonSubmit_Click"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator 
            ID="RegularExpressionValidator3" 
            runat="server" 
            ErrorMessage="E-mail address format not valid." 
            ValidationExpression="^(?(&quot;&quot;)(&quot;&quot;.+?&quot;&quot;@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&amp;'\*\+/=\?\^`\{\}\|~\w])*)(?&lt;=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$" 
            ControlToValidate="TextBoxEmail" Display="Dynamic" Height="20px" 
        ondatabinding="ButtonSubmit_Click" Width="186px"></asp:RegularExpressionValidator>
        <br />
        <p>&nbsp;<zolilo:ZoliloButton ID="ButtonSubmit" runat="server" Text="Submit" />
        <p>
            Result<br />
    <asp:TextBox ID="TextBoxResult" runat="server" style="margin-left: 0px" Height="21px" 
                Width="373px"></asp:TextBox>
    </p>
</asp:Content>
