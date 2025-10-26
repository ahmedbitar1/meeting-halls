<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MeetingReport.aspx.vb" Inherits="Meeting_Crises.MeetingReport" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>تقرير نتائج الاجتماع</title>
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
            <h4 class="mb-1">تقرير نتائج الاجتماع</h4>
            <asp:Label ID="lblMeetingDate" runat="server" CssClass="font-weight-bold d-block"></asp:Label>
        </div>

            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="False"></asp:Label>

            <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered mt-4">
                <Columns>
                    <%--<asp:BoundField HeaderText="م" DataField="RowNumber" />--%>
                    <asp:BoundField HeaderText="الموضوع" DataField="LocationName" />
                    <asp:BoundField HeaderText="النقاط" DataField="TopicTitle" />
                    <asp:BoundField HeaderText="القرار" DataField="Decision" />
                    <asp:BoundField HeaderText="جهة التنفيذ" DataField="Responsible" />
                </Columns>
            </asp:GridView>
            <div class="text-center mt-3 no-print">
                <button onclick="window.print()" class="btn btn-primary">طباعة</button>
            </div>
        </div>
    </form>
</body>
</html>
