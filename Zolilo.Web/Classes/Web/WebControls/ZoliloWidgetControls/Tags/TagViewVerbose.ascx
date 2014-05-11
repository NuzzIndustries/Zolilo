<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagViewVerbose.ascx.cs" Inherits="Zolilo.Web.TagViewVerbose" %>
<zolilo:TagViewMini runat="server" ID="tagViewMini" /><br />
<hr />
<b>Tag Details</b><br />
Created By: <zolilo:AgentLabel runat="server" ID="createdBy" /><br />
<hr />
<zolilo:EditableFragmentControl runat="server" ID="tagDescription" Title="Tag Description" /><br />
<hr />
<b>Options</b><br />
<zolilo:ZoliloButton runat="server" ID="buttonDelete" Text="Delete Tag" />