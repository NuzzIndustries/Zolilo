<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountLoginControl_D.ascx.cs" Inherits="Zolilo.Web.AccountLoginControl_D" %>
<%@ Register TagPrefix="zolilo" Assembly="Zolilo.Data" Namespace="Zolilo.Web" %>

Login ID: <asp:TextBox ID="textName" runat="server"></asp:TextBox>
<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
        ControlToValidate="textName" Display="Dynamic" 
        ErrorMessage="Field is required." Height="20px"></asp:RequiredFieldValidator><br />

Login Password: <asp:TextBox ID="textPassword" runat="server" TextMode="Password"></asp:TextBox>
<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
        ControlToValidate="textPassword" Display="Dynamic" 
        ErrorMessage="Field is required." Height="20px"></asp:RequiredFieldValidator><br />
<zolilo:ZoliloButton runat="server" ID="buttonLogin" Text="Login" />
<span runat="server" ID="spanLogonFailed"><br /><b><font color="red">Logon Failed</font></b></span>