<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="NewTag.aspx.cs" Inherits="Zolilo.Pages.NewTag" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
New Tag<br />
<br />
Enter Tag Name: <asp:TextBox runat="server" ID="txtTagName" />
<asp:RequiredFieldValidator runat="server" ID="validatorTagName" ControlToValidate="txtTagName" Text="Field is required."></asp:RequiredFieldValidator><br />
<zolilo:ZoliloButton runat="server" ID="buttonSubmit" Text="Submit" /><br />
<asp:PlaceHolder runat="server" ID="phTagExists"></asp:PlaceHolder><br />
</asp:Content>
