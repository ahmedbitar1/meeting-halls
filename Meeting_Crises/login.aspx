<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="login.aspx.vb" Inherits="Meeting_Crises.Login" %>

<!DOCTYPE html>
<html lang="ar" dir="rtl">
<head>
    <meta charset="UTF-8">
    <title>تسجيل الدخول</title>
    <link rel="icon" href="favicon.ico" type="images/mail.png" />
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
           background: url('images/meeting.jpg') no-repeat center center fixed;
    background-size: cover; 
    height: 100vh;
    display: flex;
    align-items: center;
    justify-content: center;
    direction: rtl;
        }
        .login-box {
            background-color: white;
            padding: 30px;
            border-radius: 8px;
            width: 100%;
            max-width: 400px;
            box-shadow: 0 0 15px rgba(0,0,0,0.2);
        }
        .login-box h2 {
            margin-bottom: 25px;
            text-align: center;
            color: #333;
        }
    </style> 
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-box">
            <h2>تسجيل الدخول</h2>
            <div class="mb-3">
                <asp:Label ID="lblUsername" runat="server" Text="اسم المستخدم:"></asp:Label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
            </div>
            <div class="mb-3">
                <asp:Label ID="lblPassword" runat="server" Text="كلمة المرور:"></asp:Label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
            </div>
            <div class="d-grid">
                <asp:Button ID="btnLogin" runat="server" Text="دخول" CssClass="btn btn-primary" OnClick="btnLogin_Click" />
            </div>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="d-block text-center mt-3" />
        </div>
    </form>
</body>
</html>
