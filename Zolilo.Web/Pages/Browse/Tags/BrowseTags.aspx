<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="BrowseTags.aspx.cs" Inherits="Zolilo.Pages.BrowseTags" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
Below are the current tags that exist.<br />
<a href="/tags/new">Create a new tag.</a><br />
<zolilo:TagViewList runat="server" ID="dataViewTags" />
</asp:Content>
