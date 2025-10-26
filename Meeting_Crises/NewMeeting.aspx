<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NewMeeting.aspx.vb" Inherits="Meeting_Crises.New_Meeting" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>تسجيل اجتماع جديد</title>
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

        .table-section {
            margin: 40px auto;
            width: 90%;
            max-width: 950px;
        }

        label {
            font-size: 14px;
            font-weight: 600;
            color: #333;
        }

        .form-control {
            border-radius: 5px;
            font-size: 14px;
        }

        .form-control.notes {
            font-size: 16px !important;
        }

        .btn {
            min-width: 150px;
            font-weight: bold;
            padding: 10px 20px;
            border-radius: 6px;
        }

        .btn-primary {
            background-color: #004080;
            border-color: #004080;
        }

        .btn-primary:hover {
            background-color: #003366;
            border-color: #003366;
        }

        .btn-success {
            background-color: #2d6a4f;
            border-color: #2d6a4f;
        }

        .btn-success:hover {
            background-color: #22543d;
            border-color: #22543d;
        }

        .badge-info {
            background-color: #007bff;
            font-size: 14px;
            padding: 7px 12px;
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
            <h4 style="flex:1; margin:0;">تسجيل اجتماع جديد</h4>
            <asp:Button ID="btnGoMain" runat="server" Text="الرئيسية" CssClass="small-blue-btn" OnClick="btnGoMain_Click" CausesValidation="False" Style="width:100px;background-color:gold;border-radius: 20px;
             width: auto;
             border-radius: 20px;" />
        </div>


            <asp:Label ID="lblMeetingId" runat="server" CssClass="badge badge-info mb-3 d-block text-center" />
            <asp:Label ID="lblMessage" runat="server" CssClass="text-danger font-weight-bold d-block mb-3 text-center" />

            <div class="form-row">
                <div class="form-group col-md-6">
                    <label for="txtDate">تاريخ الاجتماع</label>
                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDate" ErrorMessage="التاريخ مطلوب" CssClass="text-danger" Display="Dynamic" />
                    <asp:CustomValidator ID="cvDate" runat="server" ControlToValidate="txtDate" ErrorMessage="التاريخ يجب أن يكون اليوم أو بعده" CssClass="text-danger" Display="Dynamic" OnServerValidate="cvDate_ServerValidate" />
                </div>

                <div class="form-group col-md-6">
                    <label for="ddlLocation">الموضوع</label>
                    <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                        <asp:ListItem Text="-- اختر الموضوع --" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ControlToValidate="ddlLocation" InitialValue="" ErrorMessage="اختر الموضوع" CssClass="text-danger" Display="Dynamic" />
                </div>
            </div>

            <div class="form-row">
                <div class="form-group col-md-6">
                    <label for="ddlTopic">النقاط</label>
                    <asp:DropDownList ID="ddlTopic" runat="server" CssClass="form-control">
                        <asp:ListItem Text="-- اختر النقاط --" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvTopic" runat="server" ControlToValidate="ddlTopic" InitialValue="" ErrorMessage="اختر النقاط" CssClass="text-danger" Display="Dynamic" />
                </div>

                <div class="form-group col-md-6">
                    <label for="txtDuration">مدة النقاش (بالدقائق)</label>
                    <asp:TextBox ID="txtDuration" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDuration" runat="server" ControlToValidate="txtDuration" ErrorMessage="مدة النقاش مطلوبة" CssClass="text-danger" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="revDuration" runat="server" ControlToValidate="txtDuration" ValidationExpression="^\d+$" ErrorMessage="أدخل رقمًا صحيحًا" CssClass="text-danger" Display="Dynamic" />
                </div>
            </div>

            <div class="form-group">
                <label for="txtNotes">البيان</label>
                <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control notes" TextMode="MultiLine" Rows="4"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNotes" runat="server" ControlToValidate="txtNotes" ErrorMessage="البيان مطلوب" CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="text-center">
                <asp:Button ID="btnAddToGrid" runat="server" Text="إضافة للجدول" CssClass="btn btn-primary" OnClick="btnAddToGrid_Click" />
            </div>
        </div>

        <div class="table-section">
            <asp:GridView ID="gvMeetings" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" EmptyDataText="لا توجد بيانات حتى الآن">
                <Columns>
                    <asp:BoundField HeaderText="رقم الاجتماع" DataField="MeetingId" />
                    <asp:BoundField HeaderText="الموضوع" DataField="LocationName" />
                    <asp:BoundField HeaderText="النقاط" DataField="TopicTitle" />
                    <asp:BoundField HeaderText="البيان" DataField="Notes" />
                    <asp:BoundField HeaderText="مدة النقاش (د)" DataField="DiscussionTime" />
                </Columns>
            </asp:GridView>

            <div class="text-center">
                <asp:Button ID="btnSave" runat="server" Text="حفظ الاجتماع" CssClass="btn btn-success mt-3" OnClick="btnSave_Click" />
            </div>
            <div style="text-align:center;">
                  <!-- زر تسجيل الخروج -->
            <asp:Button ID="btnLogout" runat="server" Text="تسجيل الخروج" CssClass="logout-btn" OnClick="btnLogout_Click" />
            </div>
        </div>
    </form>
</body>
</html>
