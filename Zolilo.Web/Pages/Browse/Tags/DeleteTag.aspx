<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="DeleteTag.aspx.cs" Inherits="Zolilo.Pages.DeleteTag" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
Are you sure you want to delete the following tag?<br />
<b>Warning: All connections to/from this tag will be lost.</b><br />
<zolilo:TagViewMini runat="server" ID="tagView" /><br />
<zolilo:ZoliloButton runat="server" ID="buttonDelete" Text="DELETE PERMANENTLY" /><br />
</asp:Content>
