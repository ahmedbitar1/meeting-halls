<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Committees.aspx.vb" Inherits="Meeting_Crises.Committees" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>اللجان</title>
    <link href="https://fonts.googleapis.com/css2?family=Cairo:wght@400;700&display=swap" rel="stylesheet" />
    <style>
        body {
            font-family: 'Cairo', sans-serif;
            background: linear-gradient(to right, #007991, #78ffd6);
            height: 100vh;
            margin: 0;
            display: flex;
            align-items: center;
            justify-content: center;
            direction: rtl;
        }

        .main-container {
            background-color: white;
            padding: 40px;
            border-radius: 20px;
            box-shadow: 0 0 15px rgba(0,0,0,0.2);
            text-align: center;
            width: 400px;
        }

        .main-container h1 {
            margin-bottom: 20px;
            color: #007991;
        }

        .btn {
            background-color: #007991;
            color: white;
            border: none;
            padding: 12px 20px;
            margin: 10px;
            border-radius: 10px;
            font-size: 16px;
            cursor: pointer;
            width: 100%;
            transition: background-color 0.3s ease;
        }

        .btn:hover {
            background-color: #005f73;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main-container">
            <h1>اللجان</h1>
            <asp:Button ID="btnCrisisCommittee" runat="server" Text="لجنة الأزمات" CssClass="btn" OnClick="btnCrisisCommittee_Click" />
        </div>
    </form>
</body>
</html>
