<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MeetingResults.aspx.vb" Inherits="Meeting_Crises.MeetingResults" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>تسجيل نتائج الاجتماع</title>
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-section">
            <div style="display:flex; justify-content: space-between; align-items: center; margin-bottom: 20px; gap: 200px;">
            <h4 style="flex:1; margin:0;">تسجيل نتائج الاجتماع </h4>
            <asp:Button ID="btnGoMain" runat="server" Text="الرئيسية" CssClass="small-blue-btn" OnClick="btnGoMain_Click" CausesValidation="False" Style="width:100px;background-color:gold;border-radius: 20px;" />
        </div>

            <div class="form-group">
                <label for="ddlMeetings">اختر اجتماعاً بالتاريخ</label>
                <asp:DropDownList ID="ddlMeetings" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlMeetings_SelectedIndexChanged">
                    <asp:ListItem Text="-- اختر اجتماعاً --" Value="0" />
                </asp:DropDownList>
            </div>
        </div>

        <div class="table-section">
            <asp:GridView ID="gvMeetingResults" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" EmptyDataText="لا توجد بيانات لهذا الاجتماع" DataKeyNames="TopicId">
                <Columns>
                    <asp:BoundField HeaderText="م" DataField="RowNumber" />
                    <asp:BoundField HeaderText="الموضوع" DataField="LocationName" />
                    <asp:BoundField HeaderText="النقاط" DataField="TopicTitle" />
                    <asp:BoundField DataField="TopicId" Visible="False" />
                   <asp:TemplateField HeaderText="القرارات">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDecision" runat="server" CssClass="form-control" Height="200px" TextMode="MultiLine" 
                                         Style="vertical-align:top; padding-top:8px; resize:none;"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="جهة التنفيذ">
                        <ItemTemplate>
                            <asp:TextBox ID="txtResponsible" runat="server" CssClass="form-control" Height="200px" TextMode="MultiLine" 
                                         Style="vertical-align:top; padding-top:8px; resize:none;"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <div class="text-center">
                <asp:Button ID="btnSave" runat="server" Text="حفظ" CssClass="btn btn-success mt-3" OnClick="btnSave_Click" />
                <asp:Button ID="btnPrint" runat="server" Text="طباعة التقرير" CssClass="btn btn-primary mt-3 ml-2" OnClientClick="window.open('MeetingReport.aspx', '_blank'); return false;" Visible="False" />

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

    <script type="text/javascript">
        $(document).ready(function () {
            // لمنع Enter من إرسال الفورم داخل أي textarea
            $("textarea").on('keydown', function (e) {
                if (e.key === "Enter") {
                    e.stopPropagation();  // يمنع وصول الحدث للفورم
                    // لو حابب هنا تضيف سطر جديد مع Enter، يمكن تحط:
                    // e.preventDefault();
                    // $(this).val($(this).val() + "\n");
                }
            });
        });
</script>

</body>
</html>
