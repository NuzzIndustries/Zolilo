<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="Login_OpenID.aspx.cs" Inherits="Zolilo.Pages.Login_OpenID" %>
<%@ Register Assembly="DotNetOpenAuth" Namespace="DotNetOpenAuth.OpenId.RelyingParty" TagPrefix="rp" %>
<%@ Register assembly="DotNetOpenAuth, Version=3.4.6.10357, Culture=neutral, PublicKeyToken=2780ccd10d57b246" namespace="DotNetOpenAuth.OpenId.Provider" tagprefix="op" %>
<%@ Register assembly="DotNetOpenAuth, Version=3.4.6.10357, Culture=neutral, PublicKeyToken=2780ccd10d57b246" namespace="DotNetOpenAuth.InfoCard" tagprefix="ic" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    Sign in with one of the following. 
    <br />(Development note: The below text should be moved to a tooltip or separate help context hidden unless otherwise requested) 
    <br />Note: None of these sites have access to any private or personal information on Zolilo.com
    <br />Note: Be advised that no password will be required to log in to Zolilo.com as long as you are logged into any of these providers below and the account is linked.
    <p>
        <rp:OpenIdButton runat="server" 
            Identifier="https://www.google.com/account/o8/id"  
            ReturnToUrl="~/account/login" Text="Google" ID="OpenIdButtonGoogle" 
            onloggedin="OpenIdButtonGoogle_LoggedIn" 
            ImageUrl="~/content/images/Logo_GoogleAccounts.PNG" 
            RealmUrl="~/account/login" LogOnMode="None" PrecreateRequest="True">
        </rp:OpenIdButton>
    </p>
    <p>
        <rp:OpenIdButton ID="OpenIdButtonYahoo" runat="server" 
            Identifier="https://me.yahoo.com/" ImageUrl="~/content/images/Yahoo_ai.png" 
            Text="Yahoo!" onloggedin="OpenIdButtonYahoo_LoggedIn" 
            RealmUrl="~/Pages/Account/Login.aspx" ReturnToUrl="~/Pages/Account/Login.aspx" 
            LogOnMode="None" PrecreateRequest="True">
        </rp:OpenIdButton>
    </p>
    <p>
        <rp:OpenIdButton ID="OpenIdButtonOpenID" runat="server" 
            Identifier="https://www.myopenid.com/" 
            ImageUrl="~/content/images/Logo_OpenID.PNG" Text="OpenID" 
            onloggedin="OpenIdButtonOpenID_LoggedIn" RealmUrl="~/Pages/Account/Login.aspx" 
            ReturnToUrl="~/Pages/Account/Login.aspx" LogOnMode="None" 
            PrecreateRequest="True">
        </rp:OpenIdButton>
    </p>
    <p>
        <asp:TextBox ID="TextBox1" runat="server" Height="142px" TextMode="MultiLine" Width="467px"></asp:TextBox>
    </p>
</asp:Content>
