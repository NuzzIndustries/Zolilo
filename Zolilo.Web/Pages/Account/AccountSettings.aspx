<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="AccountSettings.aspx.cs" Inherits="Zolilo.Pages.AccountSettings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    Time zone offset (-11 to +14): <asp:TextBox runat="server" ID="TimeZoneOffset"></asp:TextBox>
    <asp:RangeValidator runat="server" ID="timeZoneValidator" MinimumValue="-11" MaximumValue="14" EnableViewState="False" Type="Integer" Text="Must be -11 to +14, whole numbers only"></asp:RangeValidator><br />
    <zolilo:ZoliloButton runat="server" ID="buttonSave" Text="Save" />
    
</asp:Content>