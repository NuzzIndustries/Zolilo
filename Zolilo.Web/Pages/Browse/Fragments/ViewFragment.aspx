<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="ViewFragment.aspx.cs" Inherits="Zolilo.Pages.ViewFragment" %>
<%@ Register TagPrefix="zolilo" TagName="FragmentViewVerbose" Src="~/Classes/Web/WebControls/ZoliloWidgetControls/Fragment/FragmentViewVerbose.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<zolilo:FragmentViewVerbose runat="server" ID="view" ></zolilo:FragmentViewVerbose>
</asp:Content>
