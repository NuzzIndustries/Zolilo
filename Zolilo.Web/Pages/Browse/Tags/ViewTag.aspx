<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="ViewTag.aspx.cs" Inherits="Zolilo.Pages.ViewTag" %>
<%@ Register TagPrefix="zolilo" TagName="TagViewVerbose" Src="~/Classes/Web/WebControls/ZoliloWidgetControls/Tags/TagViewVerbose.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<zolilo:TagViewVerbose runat="server" id="tagView" />
</asp:Content>
