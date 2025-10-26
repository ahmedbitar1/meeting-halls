Imports System.Data.SqlClient
Imports System.Configuration

Public Class CurrentMeetingReport
    Inherits System.Web.UI.Page

    ' اتصال بقاعدة البيانات
    Dim connStr As String = ConfigurationManager.ConnectionStrings("MainConn").ConnectionString

    ' استرجاع البيانات لملء جدول الاجتماع
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' منع التخزين المؤقت
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
        Response.Cache.SetNoStore()

        If Session("Username") Is Nothing Then
            Response.Redirect("Login.aspx")
        End If
        If Not IsPostBack Then
            LoadMeetingData()
        End If
    End Sub

    ' استرجاع بيانات الاجتماع وملء GridView
    Private Sub LoadMeetingData()
        Dim meetingDateStr As String = Request.QueryString("MeetingId")
        If String.IsNullOrEmpty(meetingDateStr) Then
            lblMessage.Text = "لم يتم تحديد تاريخ الاجتماع."
            btnPrint.Visible = False
            Exit Sub
        End If

        Dim meetingDate As DateTime
        Try
            meetingDate = DateTime.ParseExact(meetingDateStr, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        Catch ex As Exception
            lblMessage.Text = "صيغة التاريخ غير صحيحة."
            btnPrint.Visible = False
            Exit Sub
        End Try

        Dim query As String = "SELECT ROW_NUMBER() OVER (ORDER BY mp.ID) AS RowNumber, " &
                              "l.LocationName, t.TopicTitle, t.TopicId, mp.Notes AS 'بيان', mp.DiscussionTime AS 'مدة النقاش', m.MeetingDate " &
                              "FROM MeetingPlan mp " &
                              "INNER JOIN Meetings m ON mp.MeetingId = m.MeetingId " &
                              "INNER JOIN Topics t ON mp.TopicId = t.TopicId " &
                              "INNER JOIN Locations l ON t.LocationId = l.LocationId " &
                              "WHERE CAST(m.MeetingDate AS DATE) = @MeetingDate"

        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@MeetingDate", meetingDate.Date)
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                Dim dt As New DataTable()
                dt.Load(reader)

                If dt.Rows.Count > 0 Then
                    lblMeetingDate.Text = CDate(dt.Rows(0)("MeetingDate")).ToString("yyyy/MM/dd")
                    gvMeetingResults.DataSource = dt
                    gvMeetingResults.DataBind()
                    btnPrint.Visible = True
                Else
                    lblMessage.Text = "لا توجد بيانات لهذا الاجتماع."
                    btnPrint.Visible = False
                End If
            End Using
        End Using
    End Sub

    Protected Sub gvMeetingResults_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvMeetingResults.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim notes As String = If(DataBinder.Eval(e.Row.DataItem, "بيان"), "").ToString()
            Dim items = notes.Split("·"c, "-"c, "•"c) ' تقسيم على أساس النقاط

            Dim htmlList As String = "<ul style='text-align:right; list-style-type: disc; padding-right:20px;'>"
            For Each item In items
                If Not String.IsNullOrWhiteSpace(item) Then
                    htmlList &= "<li>" & item.Trim() & "</li>"
                End If
            Next
            htmlList &= "</ul>"

            Dim lit As Literal = CType(e.Row.FindControl("litNotes"), Literal)
            lit.Text = htmlList
        End If
    End Sub

    ' الرجوع إلى الصفحة الرئيسية
    Protected Sub btnGoMain_Click(sender As Object, e As EventArgs)
        Response.Redirect("Main.aspx")
    End Sub

    ' تسجيل الخروج
    Protected Sub btnLogout_Click(sender As Object, e As EventArgs)
        ' تسجيل الخروج
        Session.Clear()
        Response.Redirect("Login.aspx")
    End Sub
End Class
