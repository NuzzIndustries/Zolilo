<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="GlobalError.aspx.cs" Inherits="Zolilo.Pages.GlobalError" ValidateRequest="false" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h1>Oops!  An error has occurred.</h1>
<h4>
    <asp:Label runat="server" ID="LabelError" Font-Bold=true Font-Names="Courier New"></asp:Label>
</h4>
</asp:Content>
    
