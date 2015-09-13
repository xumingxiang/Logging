<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MongoTools.aspx.cs" Inherits="Logging.Server.Site.MongoTools" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Button ID="btnCreateIndex" runat="server" Text="创建索引" OnClick="btnCreateIndex_Click" />
        <asp:Button ID="btnViewIndex" runat="server" Text="查看索引" OnClick="btnViewIndex_Click" />
        <asp:Button ID="btnDropDB" runat="server" OnClick="btnDropDB_Click" Text="删除数据库" />
    <div>
    
    </div>
    </form>
</body>
</html>
