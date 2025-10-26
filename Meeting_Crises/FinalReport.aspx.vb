Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Data

Public Class FinalReport
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

        If Not IsPostBack Then
            Dim meetingIdStr As String = Request.QueryString("MeetingId")
            Dim meetingId As Integer
            If Integer.TryParse(meetingIdStr, meetingId) Then
                LoadMeetingResult(meetingId)
            Else
                lblMessage.Text = "رقم الاجتماع غير صالح."
                lblMessage.Visible = True
            End If
        End If
    End Sub

    Private Sub LoadMeetingResult(meetingId As Integer)
        Using conn As New SqlConnection(connStr)
            Dim query As String = "SELECT MR.MeetingId, M.MeetingDate, ROW_NUMBER() OVER (ORDER BY T.TopicTitle) AS RowNumber, L.LocationName, T.TopicTitle, MR.Decision, MR.Responsible FROM MeetingResult MR INNER JOIN Topics T ON MR.TopicId = T.TopicId INNER JOIN Locations L ON T.LocationId = L.LocationId INNER JOIN Meetings M ON MR.MeetingId = M.MeetingId WHERE MR.MeetingId = @MeetingId ORDER BY T.TopicTitle"

            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@MeetingId", meetingId)

            Dim dt As New DataTable()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                gvReport.DataSource = dt
                gvReport.DataBind()

                Dim meetingDate As DateTime = Convert.ToDateTime(dt.Rows(0)("MeetingDate"))
                lblMeetingDate.Text = "تاريخ الاجتماع: " & meetingDate.ToString("yyyy/MM/dd")
            Else
                lblMessage.Text = "لا توجد نتائج لهذا الاجتماع."
                lblMessage.Visible = True
            End If
        End Using
    End Sub
End Class
