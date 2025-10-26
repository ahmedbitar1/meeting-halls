<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CurrentMeetingReport.aspx.vb" Inherits="Meeting_Crises.CurrentMeetingReport" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>تقرير الاجتماع الحالي</title>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/4.6.0/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>

    <style>
        body {
            background-color: #eef3f7;
            direction: rtl;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        .form-section {
            background-color: #ffffff;
            padding: 30px;
            margin: 40px auto;
            border-radius: 10px;
            box-shadow: 0 0 15px rgba(0, 0, 0, 0.1);
            width: 90%;
            max-width: 850px;
        }

        .form-section h4 {
            color: #003366;
            font-weight: bold;
            margin-bottom: 25px;
        }

        .form-select, .form-control {
            border-radius: 5px;
            font-size: 14px;
            margin-bottom: 15px;
        }

        .btn {
            min-width: 150px;
            font-weight: bold;
            padding: 10px 20px;
            border-radius: 6px;
        }

        .btn-success {
            background-color: #2d6a4f;
            border-color: #2d6a4f;
        }

        .btn-success:hover {
            background-color: #22543d;
            border-color: #22543d;
        }

        .table-section {
            margin: 40px auto;
            width: 90%;
            max-width: 950px;
        }

        .table th {
            background-color: #004080;
            color: #fff;
            text-align: center;
        }

        .table td {
            text-align: center;
        }

        .text-danger {
            font-size: 13px;
        }

        .logout-btn {
            background-color: #dc3545;
            color: white;
            padding: 8px 16px;
            font-size: 14px;
            width: auto;
            display: inline-block;
            margin-top: 20px;
            transition: background-color 0.3s ease;
            border: none;
            border-radius: 8px;
            cursor: pointer;
        }

        .logout-btn:hover {
            background-color: #c82333 !important;
            color: white;
        }
      @media print {
    /* إخفاء الأزرار بس */
    #btnPrint,
    #btnLogout,
    #btnGoMain {
        display: none !important;
    }
    /* تأكيد إن الجدول يظهر بعرض الصفحة */
    table {
        width: 100% !important;
        border-collapse: collapse;
    }
}

    </style>
</head>
<body>
    <form id="form1" runat="server">
       <div class="form-section">
    <div style="display:flex; justify-content:space-between; align-items:center; margin-bottom: 20px; flex-direction:row-reverse;">
        <!-- زر الرئيسية -->
        <asp:Button ID="btnGoMain" runat="server" Text="الرئيسية"
            CssClass="small-blue-btn"
            OnClick="btnGoMain_Click"
            CausesValidation="False"
            Style="width:100px;background-color:gold;border-radius: 20px;" />
        
        <!-- العنوان والتاريخ في النص -->
        <div style="flex:1; text-align:center;">
            <h4 style="margin:0;">تقرير الاجتماع الحالي</h4>
            <p style="margin:5px 0 0 0; font-weight:bold;">
                تاريخ الاجتماع: <asp:Label ID="lblMeetingDate" runat="server" />
            </p>
        </div>

        <!-- مكان فاضي للتوازن -->
        <div style="width:100px;"></div>
    </div>
</div>
        <div class="table-section">
           <asp:GridView ID="gvMeetingResults" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" EmptyDataText="لا توجد بيانات لهذا الاجتماع" DataKeyNames="TopicId">
    <Columns>
        <asp:BoundField HeaderText="م" DataField="RowNumber" />
        <asp:BoundField HeaderText="الموضوع" DataField="LocationName" />
        <asp:BoundField HeaderText="النقاط" DataField="TopicTitle" />
        <asp:BoundField DataField="TopicId" Visible="False" />
      <asp:TemplateField HeaderText="بيان">
    <ItemTemplate>
        <asp:Literal ID="litNotes" runat="server"></asp:Literal>
    </ItemTemplate>
</asp:TemplateField>


        <asp:BoundField HeaderText="مدة النقاش" DataField="مدة النقاش" SortExpression="مدة النقاش" />
    </Columns>
</asp:GridView>

           <div class="text-center">
    <asp:Button ID="btnPrint" runat="server" Text="طباعة التقرير" CssClass="btn btn-primary mt-3 ml-2" OnClientClick="printCurrentTable(); return false;" Visible="False" />
</div>

            <div class="text-center mt-2">
                <asp:Label ID="lblMessage" runat="server" CssClass="text-danger" />
                <div style="text-align:center;">
                    <!-- زر تسجيل الخروج -->
                    <asp:Button ID="btnLogout" runat="server" Text="تسجيل الخروج" CssClass="logout-btn" OnClick="btnLogout_Click" />
                </div>
            </div>
        </div>
    </form>
    <script>
        function printCurrentTable() {
            // إخفاء الأزرار قبل الطباعة
            document.getElementById("btnPrint").style.display = "none";
            document.getElementById("btnLogout").style.display = "none";
            document.getElementById("btnGoMain").style.display = "none";

            // تنفيذ الطباعة
            window.print();

            // إرجاع الأزرار بعد الطباعة
            document.getElementById("btnPrint").style.display = "";
            document.getElementById("btnLogout").style.display = "";
            document.getElementById("btnGoMain").style.display = "";
        }

</script>
</body>
</html>
