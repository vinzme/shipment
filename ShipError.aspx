<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ShipError.aspx.vb" Inherits="ShipError" %>
<%@ Import Namespace="System.Security.Cryptography" %>
<%@ Import Namespace="System.Threading" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<script runat="server">
    Sub Page_Load()
        Dim delay As Byte() = New Byte(0) {}
        Dim prng As RandomNumberGenerator = New RNGCryptoServiceProvider()
        
        prng.GetBytes(delay)
        Thread.Sleep(CType(delay(0), Integer))
        
        Dim disposable As IDisposable = TryCast(prng, IDisposable)
        If Not disposable Is Nothing Then
            disposable.Dispose()
        End If
    End Sub
</script>    
<head runat="server">
    <title>Error Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Sorry - an error occured
    </div>
    </form>
</body>
</html>
