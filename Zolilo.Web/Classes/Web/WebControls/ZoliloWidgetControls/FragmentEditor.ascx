<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FragmentEditor.ascx.cs" Inherits="Zolilo.Web.FragmentEditor" %>
<div runat="server" id="divFrag" class="zWidget">    
    <asp:TextBox ID="textBoxLRA" runat="server" Height="92px" 
        style="margin-bottom: 0px" TextMode="MultiLine" Width="413px"></asp:TextBox>
        <p>
        <zolilo:ZoliloButton ID="ButtonSubmit" runat="server" Text="Submit" 
                onclick="ButtonSubmit_Click" style="height: 26px" />
        </p>
</div>


