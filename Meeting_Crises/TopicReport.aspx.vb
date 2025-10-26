Imports System.Data.SqlClient
Imports System.Data

Partial Class TopicReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' منع التخزين المؤقت
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
        Response.Cache.SetNoStore()

        If Session("Username") Is Nothing Then
            Response.Redirect("Login.aspx")
        End If

        If Not IsPostBack Then
            If Request.QueryString("TopicId") IsNot Nothing Then
                Dim topicId As Integer
                If Integer.TryParse(Request.QueryString("TopicId"), topicId) Then
                    LoadTopicReport(topicId)
                    LoadTopicTitle(topicId)
                Else
                    lblMessage.Text = "معرف النقاط غير صالح"
                End If
            Else
                lblMessage.Text = "هلا يوجد نقاط محدد"
            End If
        End If
    End Sub
    Private Sub LoadTopicTitle(topicId As Integer)
        Dim connString As String = ConfigurationManager.ConnectionStrings("MainConn").ConnectionString
        Dim query As String = "SELECT TopicTitle FROM Topics WHERE TopicId = @TopicId"

        Using conn As New SqlConnection(connString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@TopicId", topicId)
                conn.Open()
                Dim result As Object = cmd.ExecuteScalar()

                If result IsNot Nothing Then
                    lblTopic.Text = result.ToString()
                Else
                    lblTopic.Text = "نقطه غير معروفه"
                End If
            End Using
        End Using
    End Sub

    Private Sub LoadTopicReport(topicId As Integer)
        Dim connString As String = ConfigurationManager.ConnectionStrings("MainConn").ConnectionString
        Dim query As String = "SELECT R.MeetingId, L.LocationName, R.Decision, R.Responsible, M.MeetingDate FROM MeetingResult R JOIN Meetings M ON R.MeetingId = M.MeetingId JOIN Topics T ON R.TopicId = T.TopicId JOIN Locations L ON T.LocationId = L.LocationId WHERE R.TopicId = @TopicId AND T.CommitteeId = @CommitteeId"

        Using conn As New SqlConnection(connString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@TopicId", topicId)
                cmd.Parameters.AddWithValue("@CommitteeId", Session("CommitteeId"))

                Dim dt As New DataTable()
                Using adapter As New SqlDataAdapter(cmd)
                    adapter.Fill(dt)
                End Using

                gvTopicMeetings.DataSource = dt
                gvTopicMeetings.DataBind()
            End Using
        End Using
    End Sub
End Class
