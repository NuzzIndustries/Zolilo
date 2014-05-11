<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true"
    CodeBehind="Home.aspx.cs" Inherits="Zolilo._Default" EnableViewStateMac="false" %> 
<%@ Register assembly="DotNetOpenAuth, Version=3.4.6.10357, Culture=neutral, PublicKeyToken=2780ccd10d57b246" namespace="DotNetOpenAuth.OpenId.RelyingParty" tagprefix="rp" %>

<head runat="server" id="htmlhead">
    <title>Zolilo.com</title>
</head>
    <link type="text/css" href="/Styles/zolilo.css" rel="stylesheet" />	
    <script src="/Scripts/jquery-1.4.1.js"></script>
    <script src="/Scripts/zolilo/zolilo.supervisor.js"></script>
    <script src="/Scripts/zolilo/zolilo.js"></script>
    <script src="/Scripts/UFrame-Scripts/htmlparser.js"></script>
    <script src="/Scripts/UFrame-Scripts/UFrame-new.js"></script>
    <script src="/Scripts/other/History.js"></script>
    <script type="text/javascript">
        function doLink(arg, id)
        {
            //alert("doLink(" + arg + "," + id + ");");
            var uframe = UFrameManager.getUFrame(id);
            uframe.navigate(arg);
        }
    </script>   
        <script type="text/javascript" >            function MainFrameExists() { return true; }</script>

        <div id="GlobalDiv" style="width: 90%; height: 100%; position: relative; top: 0px; left: 0px;">
             <input type="hidden" id="input_TestHomeOnly" value="TestHomeOnly" />
             <input type="hidden" id="input_TestAll" value="TestHome" />
            <asp:Label ID="LabelDoLink"
                runat="server" Text="Label" Visible="False"></asp:Label>
            <div id="GlobalDivFixed" style="width: 100%; min-width: 800px; position: absolute">
                <div id="Header" style="margin: auto; top: 0px; bottom: 0px; position: relative; width: 100%; height: 35px; background-color: #EEEEEE;">
                    <div id="HeaderLogo" style="width: 81px; position: absolute; height: 100%; top: 0px; left: -1px;">
                        <div id="HeaderLogoContent" style="position: absolute; bottom: 3px; left: 0px; ">
                            <a href="/home">Home</a>
                        </div>
                    </div> 
                    <div id="HeaderMenu" style="position: absolute; left: 80px; right:250px; top: 0px; height: 100%; background-color: #F2F2F2;">
                        <div id="HeaderMenuContent" 
                            style="position: absolute; left: 100px; bottom: 3px; height: 19px;">
                            <a href="/browse">Browse</a>
                            <a href="/search">Search</a>
                            <a href="/siteinfo">Debug</a>
                            <a href="/admin">Admin</a>
                        </div>
                    </div>
                    <div id="HeaderLoginInfo" style="position: absolute; right: 0px; width: 250px; top:0px; bottom:0px; text-align: right; height: 100%; background-color: #F2F2F2;">
                        <div id="HeaderLoginInfoContent" style="position: absolute; right:3px; bottom: 0px; top:0px; font-size:small; width: 250px;">
                            <asp:Label ID="LoginLabel" runat="server" onprerender="LoginLabel_PreRender">
                            </asp:Label>
                        </div>
                    </div>
                </div>
                <asp:Literal ID="LiteralBody" runat="server"></asp:Literal>
                <!--<div id="Body" runat="server" src="/browse" style="position: relative; top: 0px;">-->
            </div>
        </div>
        
        <div id="loadingdiv" class="loading" style="display: hidden">
            Loading...
        </div>