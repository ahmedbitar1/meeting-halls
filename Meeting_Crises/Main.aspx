<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Main.aspx.vb" Inherits="Meeting_Crises.Main" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>الصفحة الرئيسية </title>
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

        .main-container p {
            margin-bottom: 30px;
            font-size: 16px;
            color: #333;
        }

        .main-container .btn {
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

        .main-container .btn:hover {
            background-color: #005f73;
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
          .small-blue-btn {
    background-color: #005f73;
    color: white;
    padding: 6px 12px;
    font-size: 14px;
    border: none;
    border-radius: 8px;
    cursor: pointer;
    transition: background-color 0.3s ease;
    margin-top: 10px;
}

.small-blue-btn:hover {
    background-color: #004653;
}
.message-label {
    display: block;
    margin-top: 8px;
    color: red;
    font-size: 14px;
}
        #btnShowPreviousMeetings {
             background-color: #626262f0;
             width: auto;
        }
        #btnShowCurrentMeetings{
             background-color: #626262f0;
             width: auto;
        }
        #btnShowOldMeetings{
             background-color: #626262f0;
             width: auto;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main-container">
            <h1>نظام إدارة الاجتماعات</h1>
        
                                    <!-- زرارين تسجيل موقع و موضوع جديد -->
                        <asp:Button ID="btnAddLocation" runat="server" Text="تسجيل موضوع جديد" CssClass="btn" OnClick="btnAddLocation_Click" />
                                                 <!-- إدخال موقع جديد -->
                        <asp:Panel ID="pnlLocation" runat="server" Visible="False">
                            <asp:TextBox ID="txtLocationName" runat="server" CssClass="form-control mt-3" placeholder="اكتب اسم الموضوع"></asp:TextBox>
    
                            <asp:Button ID="btnSaveLocation" runat="server" Text="حفظ الموضوع" CssClass="small-blue-btn" OnClick="btnSaveLocation_Click" />
    
                            <!-- الرسالة تظهر تحت زر الحفظ -->
                            <asp:Label ID="lblMessage" runat="server" CssClass="message-label"></asp:Label>
                        </asp:Panel>

                        
            <asp:Button ID="btnAddTopic" runat="server" Text="تسجيل نقاط جديد" CssClass="btn" OnClick="btnAddTopic_Click" />   
                                             <!-- إدخال موضوع جديد -->
                        <asp:Panel ID="pnlTopic" runat="server" Visible="False">
                            <asp:TextBox ID="txtTopicTitle" runat="server" CssClass="form-control mt-3" placeholder="اكتب عنوان النقاط"></asp:TextBox>
                            <asp:TextBox ID="txtTopicNotes" runat="server" CssClass="form-control mt-2" placeholder="ملاحظات (اختياري)"></asp:TextBox>
                            <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control mt-2" AppendDataBoundItems="true">
                                <asp:ListItem Text="اختر الموضوع" Value="" />
                            </asp:DropDownList>
    
                            <asp:Button ID="btnSaveTopic" runat="server" Text="حفظ النقاط" CssClass="small-blue-btn" OnClick="btnSaveTopic_Click" />
    
                            <!-- الرسالة تظهر تحت زر الحفظ -->
                            <asp:Label ID="Label1" runat="server" CssClass="message-label"></asp:Label>
                        </asp:Panel>
                             
                         <asp:Button ID="btnShowCurrentMeetings" runat="server" Text="عرض اجتماعات حالية" CssClass="btn" OnClick="btnShowCurrentMeetings_Click" />
                            <asp:Panel ID="pnlCurrentMeetings" runat="server" Visible="False" style="margin-top:10px;">
                            <asp:DropDownList ID="ddlCurrentMeetings" runat="server" Width="300px" CssClass="form-control"></asp:DropDownList>
                            <asp:Button ID="btnViewCurrentMeeting" runat="server" Text="عرض الاجتماع" OnClick="btnViewCurrentMeeting_Click" OnClientClick="return openCurrentMeeting();" CssClass="small-blue-btn" />
                        </asp:Panel>

                        <asp:Button ID="btnShowPreviousMeetings" runat="server" Text="عرض نتائج نقاط سابقة" CssClass="btn btn-burgundy" OnClick="btnShowPreviousMeetings_Click" />
                        <asp:Panel ID="pnlPreviousMeetings" runat="server" Visible="False" style="margin-top:10px;">
                            <asp:DropDownList ID="ddlTopics" runat="server" Width="300px" CssClass="form-control"></asp:DropDownList>
                           <asp:Button ID="btnViewTopicReport" runat="server" Text="عرض التقرير" OnClick="btnViewTopicReport_Click" OnClientClick="return openReport();" CssClass="small-blue-btn"  />
                        </asp:Panel>
                         
                                    <asp:Button ID="btnShowOldMeetings" runat="server" Text="عرض نتائج اجتماعات سابقة" CssClass="btn btn-burgundy" OnClick="btnShowOldMeetings_Click" />
                        <asp:Panel ID="pnlOldMeetings" runat="server" Visible="False" style="margin-top:10px;">
                            <asp:DropDownList ID="ddlOldMeetings" runat="server" Width="300px" CssClass="form-control" AppendDataBoundItems="true">
                                <asp:ListItem Text="اختر اجتماع" Value="" />
                            </asp:DropDownList>
                          <asp:Button ID="btnViewOldMeetingReport" runat="server" Text="عرض التقرير"
                                           CssClass="small-blue-btn" OnClientClick="return openOldMeetingReport();" />
                        </asp:Panel>

            <asp:Button ID="btnNewMeeting" runat="server" Text="تسجيل اجتماع جديد" CssClass="btn" PostBackUrl="~/NewMeeting.aspx" />
            <asp:Button ID="btnMeetingResults" runat="server" Text="تسجيل نتائج اجتماع" CssClass="btn" PostBackUrl="~/MeetingResults.aspx" />
           
             <!-- زر تسجيل الخروج -->
            <asp:Button ID="btnLogout" runat="server" Text="تسجيل الخروج" CssClass="logout-btn" OnClick="btnLogout_Click" />
        </div>
    </form>

    <script type="text/javascript">
        function openReport() {
            var ddl = document.getElementById('<%= ddlTopics.ClientID %>');
        var topicId = ddl.options[ddl.selectedIndex].value;
        if (topicId === "") {
            alert('يجب اختيار نقطه محدده.');
            return false; // يمنع عمل البوست باك للسيرفر
        }
        window.open('TopicReport.aspx?TopicId=' + topicId, '_blank');
        return false; // يمنع عمل البوست باك للسيرفر لأننا فتحنا الصفحة في تاب جديد
        }

        function openCurrentMeeting() {
            var ddl = document.getElementById('<%= ddlCurrentMeetings.ClientID %>');
           var meetingId = ddl.options[ddl.selectedIndex].value;
           if (meetingId === "") {
               alert('يجب اختيار اجتماع.');
               return false;
           }
           // فتح نافذة جديدة مع تمرير MeetingId في الـ URL
           window.open('CurrentMeetingReport.aspx?MeetingId=' + meetingId, '_blank');
           return false; // لمنع الـ postback
       }

        function openOldMeetingReport() {
            var ddl = document.getElementById('<%= ddlOldMeetings.ClientID %>');
            var meetingId = ddl.options[ddl.selectedIndex].value;

            if (meetingId === "") {
                alert('يرجى اختيار اجتماع أولاً.');
                return false;
            }

            window.open('FinalReport.aspx?MeetingId=' + meetingId, '_blank');
            return false;
        }
</script>

</body>
</html>
