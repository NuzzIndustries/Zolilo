<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FragmentViewVerbose.ascx.cs" Inherits="Zolilo.Web.FragmentViewVerbose" %>

View Fragment<br />
<zolilo:ZoliloButton runat="server" ID="buttonDeleteFrag" Text="Delete Fragment Tree" /><br />
<hr />
<b>Parent object</b><br />
<zolilo:NodeViewMini runat="server" ID="viewParent" /><br />
<zolilo:EdgeViewMini runat="server" ID="viewEdgeParent" /> <br />
<hr />
<b>Fragment Details</b><br />
Created By: <zolilo:AgentLabel runat="server" ID="mainCreatedBy" /><br />
Feedback rating (from the parent item): <asp:PlaceHolder runat="server" ID="mainFeedbackRating"></asp:PlaceHolder><br />
Time Created: <zolilo:TimeLabel runat="server" ID="mainTimeCreated" /><br />
<span runat="server" id="SpanTimeModified">Time Modified: <zolilo:TimeLabel runat="server" ID="mainTimeModified" /><br /></span>
<hr />
<b>Fragment Contents</b><br />
<zolilo:ReadOnlyFragmentControl runat="server" ID="fragmentControl" /><br />
<hr />
<zolilo:FragmentReplyControl runat="server" ID="reply1" /><br />
<hr />
Replies: <asp:PlaceHolder runat="server" ID="repliesCount"></asp:PlaceHolder><br />
<hr />
<zolilo:FragmentViewRepliesControl runat="server" ID="viewReplies" />
<zolilo:FragmentReplyControl runat="server" ID="reply2" /><br />