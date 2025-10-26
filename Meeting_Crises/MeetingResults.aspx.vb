Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Data

Public Class MeetingResults
    Inherits System.Web.UI.Page
    Protected WithEvents lblMessage As Label
    Dim connStr As String = ConfigurationManager.ConnectionStrings("MainConn").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
        Response.Cache.SetNoStore()
        If Session("Username") Is Nothing Then
            Response.Redirect("Login.aspx")
        End If
        If Not IsPostBack Then
            LoadMeetings()
            lblMessage.Text = ""
        End If
    End Sub

    'Private Sub LoadMeetings()
    '    Dim committeeId As Integer = 0
    '    If Session("CommitteeId") IsNot Nothing Then
    '        committeeId = Convert.ToInt32(Session("CommitteeId"))
    '    Else
    '        ddlMeetings.Items.Clear()
    '        ddlMeetings.Items.Insert(0, New ListItem("-- اختر اجتماعاً --", "0"))
    '        Return
    '    End If

    '    Using conn As New SqlConnection(connStr)
    '        Dim query As String = "SELECT DISTINCT M.MeetingId, CONVERT(varchar, M.MeetingDate, 23) AS MeetingDate, M.MeetingDate AS SortDate FROM Meetings M INNER JOIN MeetingPlan MP ON M.MeetingId = MP.MeetingId INNER JOIN Topics T ON MP.TopicId = T.TopicId WHERE T.CommitteeId = @CommitteeId AND NOT EXISTS (SELECT 1 FROM MeetingResult MR WHERE MR.MeetingId = M.MeetingId) ORDER BY SortDate DESC"
    '        Dim cmd As New SqlCommand(query, conn)
    '        cmd.Parameters.AddWithValue("@CommitteeId", committeeId)
    '        conn.Open()
    '        Dim rdr As SqlDataReader = cmd.ExecuteReader()
    '        ddlMeetings.DataSource = rdr
    '        ddlMeetings.DataTextField = "MeetingDate"
    '        ddlMeetings.DataValueField = "MeetingId"
    '        ddlMeetings.DataBind()
    '        ddlMeetings.Items.Insert(0, New ListItem("-- اختر اجتماعاً --", "0"))
    '    End Using
    'End Sub
    Private Sub LoadMeetings()
        Dim committeeId As Integer = 0
        If Session("CommitteeId") IsNot Nothing Then
            committeeId = Convert.ToInt32(Session("CommitteeId"))
        Else
            ddlMeetings.Items.Clear()
            ddlMeetings.Items.Insert(0, New ListItem("-- اختر اجتماعاً --", "0"))
            Return
        End If

        Using conn As New SqlConnection(connStr)
            ' هنجيب أقل MeetingId لكل تاريخ علشان ما نكررشي التاريخ
            Dim query As String = "SELECT MIN(M.MeetingId) AS MeetingId, CONVERT(date, M.MeetingDate) AS MeetingDate FROM Meetings M INNER JOIN MeetingPlan MP ON M.MeetingId = MP.MeetingId INNER JOIN Topics T ON MP.TopicId = T.TopicId WHERE T.CommitteeId = @CommitteeId AND CONVERT(date, M.MeetingDate) NOT IN (SELECT CONVERT(date, M2.MeetingDate) FROM Meetings M2 INNER JOIN MeetingResult MR ON MR.MeetingId = M2.MeetingId) GROUP BY CONVERT(date, M.MeetingDate) ORDER BY MeetingDate DESC"

        Dim cmd As New SqlCommand(query, conn)
        cmd.Parameters.AddWithValue("@CommitteeId", committeeId)
        conn.Open()
        Dim rdr As SqlDataReader = cmd.ExecuteReader()
        ddlMeetings.DataSource = rdr
        ddlMeetings.DataTextField = "MeetingDate"
        ddlMeetings.DataValueField = "MeetingId"
            ddlMeetings.DataBind()
            For Each item As ListItem In ddlMeetings.Items
                If Not String.IsNullOrEmpty(item.Text) AndAlso item.Text <> "-- اختر اجتماعاً --" Then
                    item.Text = DateTime.Parse(item.Text).ToString("yyyy-MM-dd") ' أو التنسيق اللي تحبه
                End If
            Next
        ddlMeetings.Items.Insert(0, New ListItem("-- اختر اجتماعاً --", "0"))
    End Using
    End Sub



    Protected Sub ddlMeetings_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMeetings.SelectedIndexChanged
        If ddlMeetings.SelectedValue <> "0" Then
            ' هنا هنستخدم التاريخ بدل الـ MeetingId
            LoadMeetingTopicsByDate(ddlMeetings.SelectedItem.Text)
        Else
            gvMeetingResults.DataSource = Nothing
            gvMeetingResults.DataBind()
        End If
    End Sub

    Private Sub LoadMeetingTopicsByDate(selectedDate As String)
        Using conn As New SqlConnection(connStr)
            Dim query As String = "SELECT ROW_NUMBER() OVER (ORDER BY M.MeetingDate, T.TopicTitle) AS RowNumber, L.LocationName, T.TopicTitle, T.TopicId, M.MeetingId FROM Meetings M INNER JOIN MeetingPlan MP ON M.MeetingId = MP.MeetingId INNER JOIN Topics T ON MP.TopicId = T.TopicId INNER JOIN Locations L ON T.LocationId = L.LocationId WHERE CONVERT(date, M.MeetingDate) = @MeetingDate ORDER BY M.MeetingDate, T.TopicTitle"
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.Add("@MeetingDate", SqlDbType.Date).Value = DateTime.Parse(selectedDate, System.Globalization.CultureInfo.InvariantCulture)
            Dim dt As New DataTable()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dt)
            dt.Columns.Add("Decision", GetType(String))
            dt.Columns.Add("Responsible", GetType(String))
            gvMeetingResults.DataSource = dt
            gvMeetingResults.DataBind()
        End Using
    End Sub

    'Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
    '    lblMessage.Text = ""
    '    If ddlMeetings.SelectedValue = "0" Then
    '        lblMessage.Text = "يجب اختيار اجتماع أولاً."
    '        Return
    '    End If

    '    Dim valid As Boolean = True
    '    For Each row As GridViewRow In gvMeetingResults.Rows
    '        Dim txtDecision As TextBox = CType(row.FindControl("txtDecision"), TextBox)
    '        Dim txtResponsible As TextBox = CType(row.FindControl("txtResponsible"), TextBox)
    '        If String.IsNullOrWhiteSpace(txtDecision.Text) OrElse String.IsNullOrWhiteSpace(txtResponsible.Text) Then
    '            valid = False
    '            Exit For
    '        End If
    '    Next

    '    If Not valid Then
    '        lblMessage.Text = "يجب كتابة القرارات وجهة التنفيذ لكل صف."
    '        Return
    '    End If

    '    Using conn As New SqlConnection(connStr)
    '        conn.Open()
    '        For Each row As GridViewRow In gvMeetingResults.Rows
    '            Dim topicId As Integer = Convert.ToInt32(gvMeetingResults.DataKeys(row.RowIndex).Value)
    '            Dim txtDecision As TextBox = CType(row.FindControl("txtDecision"), TextBox)
    '            Dim txtResponsible As TextBox = CType(row.FindControl("txtResponsible"), TextBox)
    '            Dim query As String = "INSERT INTO MeetingResult (MeetingId, TopicId, Decision, Responsible) VALUES (@MeetingId, @TopicId, @Decision, @Responsible)"
    '            Using cmd As New SqlCommand(query, conn)
    '                cmd.Parameters.AddWithValue("@MeetingId", ddlMeetings.SelectedValue)
    '                cmd.Parameters.AddWithValue("@TopicId", topicId)
    '                cmd.Parameters.AddWithValue("@Decision", txtDecision.Text.Trim())
    '                cmd.Parameters.AddWithValue("@Responsible", txtResponsible.Text.Trim())
    '                cmd.ExecuteNonQuery()
    '            End Using
    '        Next
    '    End Using

    '    lblMessage.Text = "تم حفظ البيانات بنجاح."
    '    ddlMeetings.SelectedIndex = 0
    '    gvMeetingResults.DataSource = Nothing
    '    gvMeetingResults.DataBind()
    '    LoadMeetings()
    '    Session("MeetingReport") = gvMeetingResults.DataSource
    '    btnPrint.Visible = True
    'End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        lblMessage.Text = ""
        If ddlMeetings.SelectedValue = "0" Then
            lblMessage.Text = "يجب اختيار اجتماع أولاً."
            Return
        End If

        Dim valid As Boolean = True
        For Each row As GridViewRow In gvMeetingResults.Rows
            Dim txtDecision As TextBox = CType(row.FindControl("txtDecision"), TextBox)
            Dim txtResponsible As TextBox = CType(row.FindControl("txtResponsible"), TextBox)
            If String.IsNullOrWhiteSpace(txtDecision.Text) OrElse String.IsNullOrWhiteSpace(txtResponsible.Text) Then
                valid = False
                Exit For
            End If
        Next

        If Not valid Then
            lblMessage.Text = "يجب كتابة القرارات وجهة التنفيذ لكل صف."
            Return
        End If

        Using conn As New SqlConnection(connStr)
            conn.Open()
            For Each row As GridViewRow In gvMeetingResults.Rows
                Dim topicId As Integer = Convert.ToInt32(gvMeetingResults.DataKeys(row.RowIndex).Value)
                Dim txtDecision As TextBox = CType(row.FindControl("txtDecision"), TextBox)
                Dim txtResponsible As TextBox = CType(row.FindControl("txtResponsible"), TextBox)
                Dim query As String = "INSERT INTO MeetingResult (MeetingId, TopicId, Decision, Responsible) VALUES (@MeetingId, @TopicId, @Decision, @Responsible)"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@MeetingId", ddlMeetings.SelectedValue)
                    cmd.Parameters.AddWithValue("@TopicId", topicId)
                    cmd.Parameters.AddWithValue("@Decision", txtDecision.Text.Trim())
                    cmd.Parameters.AddWithValue("@Responsible", txtResponsible.Text.Trim())
                    cmd.ExecuteNonQuery()
                End Using
            Next
        End Using

        lblMessage.Text = "تم حفظ البيانات بنجاح."
        ' لا تعيد تعيين الـ SelectedIndex
        gvMeetingResults.DataSource = Nothing
        gvMeetingResults.DataBind()
        LoadMeetings() ' تحميل الاجتماعات بعد الحفظ ولكن بدون إعادة تعيين الـ SelectedIndex

        ' حفظ البيانات للطباعة
        Session("MeetingReport") = gvMeetingResults.DataSource

        ' تأكد من إظهار زر الطباعة بعد الحفظ
        btnPrint.Visible = True
    End Sub

    Protected Sub btnLogout_Click(sender As Object, e As EventArgs)
        Session.Clear()
        Session.Abandon()
        Response.Redirect("Login.aspx")
    End Sub
    Protected Sub btnGoMain_Click(sender As Object, e As EventArgs)
        Response.Redirect("Main.aspx")
    End Sub
End Class
