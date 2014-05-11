<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="SiteInfo.aspx.cs" Inherits="Zolilo.Pages.SiteInfo" %>
<%@ Register assembly="DotNetOpenAuth, Version=3.4.6.10357, Culture=neutral, PublicKeyToken=2780ccd10d57b246" namespace="DotNetOpenAuth.OpenId.RelyingParty" tagprefix="rp" %>
<%@ Register assembly="DotNetOpenAuth, Version=3.4.6.10357, Culture=neutral, PublicKeyToken=2780ccd10d57b246" namespace="DotNetOpenAuth" tagprefix="dnoa" %>
<%@ Register assembly="DotNetOpenAuth, Version=3.4.6.10357, Culture=neutral, PublicKeyToken=2780ccd10d57b246" namespace="DotNetOpenAuth.OpenId.Provider" tagprefix="op" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <asp:Label ID="LabelHeader" runat="server" Text="Debugging information is as follows:"></asp:Label>
    </p>
    <asp:TextBox ID="TextBoxInfo" runat="server" Height="406px" TextMode="MultiLine" Width="646px"></asp:TextBox>
    <p>
    </p>
    <zolilo:ZoliloButton runat="server" ID="buttonDeleteAll" Text="DELETE ALL NODES" />
</asp:Content>
