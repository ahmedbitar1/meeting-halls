Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Data

Public Class MeetingReport
    Inherits System.Web.UI.Page

    Dim connStr As String = ConfigurationManager.ConnectionStrings("MainConn").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
        Response.Cache.SetNoStore()

        ' التأكد من أن المستخدم مسجل الدخول
        If Session("Username") Is Nothing Then
            Response.Redirect("Login.aspx")
        End If

        ' عند أول تحميل للصفحة
        If Not IsPostBack Then
            ' استرجاع آخر اجتماع تم حفظه
            LoadLastMeetingResult()
        End If
    End Sub

    ' استرجاع أحدث نتيجة اجتماع تم حفظها
    Private Sub LoadLastMeetingResult()
        Using conn As New SqlConnection(connStr)
            ' استعلام للحصول على آخر نتيجة اجتماع مع تاريخ الاجتماع من جدول Meetings
            Dim query As String = "SELECT MR.MeetingId, M.MeetingDate, ROW_NUMBER() OVER (ORDER BY T.TopicTitle) AS RowNumber, L.LocationName, T.TopicTitle, MR.Decision, MR.Responsible FROM MeetingResult MR INNER JOIN Topics T ON MR.TopicId = T.TopicId INNER JOIN Locations L ON T.LocationId = L.LocationId INNER JOIN Meetings M ON MR.MeetingId = M.MeetingId WHERE MR.MeetingId = (SELECT MAX(MeetingId) FROM MeetingResult) ORDER BY T.TopicTitle"

            Dim cmd As New SqlCommand(query, conn)
            conn.Open()
            Dim dt As New DataTable()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dt)

            ' ربط البيانات بالـ GridView وعرض التاريخ
            If dt.Rows.Count > 0 Then
                gvReport.DataSource = dt
                gvReport.DataBind()

                ' عرض التاريخ في Label
                Dim meetingDate As DateTime = Convert.ToDateTime(dt.Rows(0)("MeetingDate"))
                lblMeetingDate.Text = "تاريخ الاجتماع: " & meetingDate.ToString("yyyy/MM/dd")
            Else
                lblMessage.Text = "لا توجد بيانات للاجتماع."
                lblMessage.Visible = True
            End If
        End Using
    End Sub

End Class
