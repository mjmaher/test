<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TimerExtendTest.aspx.cs" Inherits="TestTimeoutTool.TimerExtendTest" %>

<%@ Register Assembly="SessionTimeoutTool" Namespace="SessionTimeoutTool" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    This page loaded at <%= DateTime.Now.ToLongTimeString() %>.

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            This Panel refreshed at <%= DateTime.Now.ToLongTimeString() %>.
            <br />
            <asp:Button ID="Button1" runat="server" Text="Button" />
            <cc1:TimeoutWatcherControl ID="TimeoutWatcherControl1" runat="server" TimeoutMode="PageRedirect" RedirectPage="http://www.google.com"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
    
    </div>
    </form>
</body>
</html>
