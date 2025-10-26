<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TopicReport.aspx.vb" Inherits="Meeting_Crises.TopicReport" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>تقرير الموضوع</title>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/4.6.0/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body { direction: rtl; padding: 30px; font-family: 'Segoe UI'; background-color: #fff; }
        h4 { text-align: center; color: #003366; }
        .table th { background-color: #004080; color: #fff; text-align: center; }
        .table td { text-align: center; }
        @media print {
            .no-print { display: none; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="text-center mb-3">
            <h4 class="mb-1">
                تقرير نتائج خاصة نقاط : 
                <asp:Label ID="Label1" runat="server"  Font-Bold="True" Font-Size="Large"></asp:Label>
            </h4>
                <asp:Label ID="lblTopic" runat="server" CssClass="font-weight-bold d-block" Font-Size="Large" ForeColor="Green"></asp:Label>
            </div>

            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="d-block text-center mb-2"></asp:Label>

            <asp:GridView ID="gvTopicMeetings" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered mt-4" EmptyDataText="لا توجد بيانات">
                <Columns>
                    <asp:BoundField HeaderText="رقم الاجتماع" DataField="MeetingId" />
                    <asp:BoundField HeaderText="الموضوع" DataField="LocationName" />
                    <asp:BoundField HeaderText="القرار" DataField="Decision" />
                    <asp:BoundField HeaderText="جهة التنفيذ" DataField="Responsible" />
                    <asp:BoundField HeaderText="التاريخ" DataField="MeetingDate" DataFormatString="{0:yyyy-MM-dd}" />
                </Columns>
            </asp:GridView>

            <div class="text-center mt-3 no-print">
                <button onclick="window.print()" class="btn btn-primary">طباعة التقرير</button>
            </div>
        </div>
    </form>
</body>
</html>
