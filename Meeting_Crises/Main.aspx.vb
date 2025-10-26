Imports System.Data.SqlClient
Imports System.Configuration

Partial Class Main
    Inherits System.Web.UI.Page

    Dim connStr As String = ConfigurationManager.ConnectionStrings("MainConn").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' منع التخزين المؤقت
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
        Response.Cache.SetNoStore()

        If Session("Username") Is Nothing Then
            Response.Redirect("Login.aspx")
        End If
        If Session("CommitteeId") Is Nothing Then
            Response.Redirect("Committees.aspx") ' لو ما اختارش لجنة يرجع يختار
        End If
        If Not IsPostBack Then
            LoadLocations()
        ElseIf Request.Form("__EVENTTARGET") = btnShowPreviousMeetings.UniqueID Then
            'LoadTopics()
        End If
    End Sub

    ' عرض بانل تسجيل موقع جديد
    Protected Sub btnAddLocation_Click(sender As Object, e As EventArgs)
        pnlLocation.Visible = True
        pnlTopic.Visible = False
    End Sub

    ' حفظ الموقع الجديد
    Protected Sub btnSaveLocation_Click(sender As Object, e As EventArgs)
        Dim locationName As String = txtLocationName.Text.Trim()
        lblMessage.Text = "" ' تفريغ أي رسالة سابقة

        If locationName = "" Then
            lblMessage.Text = "يجب إدخال اسم الموضوع."
            Return
        End If

        Using conn As New SqlConnection(connStr)
            conn.Open()

            ' التحقق من تكرار الاسم
            Dim checkQuery As String = "SELECT COUNT(*) FROM Locations WHERE LocationName = @LocationName"
            Using checkCmd As New SqlCommand(checkQuery, conn)
                checkCmd.Parameters.AddWithValue("@LocationName", locationName)
                Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                If count > 0 Then
                    lblMessage.Text = "اسم الموضوع موجود بالفعل."
                    Return
                End If
            End Using

            ' إذا لم يكن مكررًا، يتم الحفظ
            Dim insertQuery As String = "INSERT INTO Locations (LocationName) VALUES (@LocationName)"
            Using insertCmd As New SqlCommand(insertQuery, conn)
                insertCmd.Parameters.AddWithValue("@LocationName", locationName)
                insertCmd.ExecuteNonQuery()
            End Using
        End Using

        txtLocationName.Text = ""
        pnlLocation.Visible = False
        LoadLocations()
        lblMessage.Text = "تم حفظ الموضوع بنجاح."
    End Sub


    ' عرض بانل تسجيل موضوع جديد
    Protected Sub btnAddTopic_Click(sender As Object, e As EventArgs)
        pnlTopic.Visible = True
        pnlLocation.Visible = False
    End Sub

    ' حفظ الموضوع الجديد
    Protected Sub btnSaveTopic_Click(sender As Object, e As EventArgs)
        Dim topicTitle As String = txtTopicTitle.Text.Trim()
        Dim topicNotes As String = txtTopicNotes.Text.Trim()
        Dim locationId As Integer
        Label1.Text = ""

        ' التحقق من صحة الإدخال
        If Not Integer.TryParse(ddlLocation.SelectedValue, locationId) OrElse locationId = 0 Then
            Label1.Text = "يجب اختيار موضوع."
            Return
        End If

        If topicTitle = "" Then
            Label1.Text = "يجب إدخال عنوان النقاط."
            Return
        End If

        Using conn As New SqlConnection(connStr)
            conn.Open()

            ' التحقق من وجود الموضوع بالفعل لنفس الموقع
            Dim checkQuery As String = "SELECT COUNT(*) FROM Topics WHERE TopicTitle = @TopicTitle AND LocationId = @LocationId AND CommitteeId = @CommitteeId"
            Using checkCmd As New SqlCommand(checkQuery, conn)
                checkCmd.Parameters.AddWithValue("@TopicTitle", topicTitle)
                checkCmd.Parameters.AddWithValue("@LocationId", locationId)
                checkCmd.Parameters.AddWithValue("@CommitteeId", Session("CommitteeId"))
                Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                If count > 0 Then
                    Label1.Text = "هذه النقطه مسجله بالفعل لهذا الموضوع في هذه اللجنة."
                    Return
                End If
            End Using


            ' الحفظ
            Dim insertQuery As String = "INSERT INTO Topics (TopicTitle, Notes, LocationId, CommitteeId) VALUES (@TopicTitle, @Notes, @LocationId, @CommitteeId)"
            Using insertCmd As New SqlCommand(insertQuery, conn)
                insertCmd.Parameters.AddWithValue("@TopicTitle", topicTitle)
                insertCmd.Parameters.AddWithValue("@Notes", topicNotes)
                insertCmd.Parameters.AddWithValue("@LocationId", locationId)
                insertCmd.Parameters.AddWithValue("@CommitteeId", Session("CommitteeId"))
                insertCmd.ExecuteNonQuery()
            End Using
        End Using

        txtTopicTitle.Text = ""
        txtTopicNotes.Text = ""
        ddlLocation.ClearSelection()
        pnlTopic.Visible = False
        Label1.Text = "تم حفظ النقاط بنجاح."
    End Sub

    ' تحميل المواقع في القائمة المنسدلة
    Private Sub LoadLocations()
        Using conn As New SqlConnection(connStr)
            Dim query As String = "SELECT DISTINCT LocationId, LocationName FROM Locations"
            Using cmd As New SqlCommand(query, conn)
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                ddlLocation.DataSource = reader
                ddlLocation.DataTextField = "LocationName"
                ddlLocation.DataValueField = "LocationId"
                ddlLocation.DataBind()
            End Using
        End Using

    End Sub

    ' الذهاب لتسجيل اجتماع جديد
    Protected Sub btnNewMeeting_Click(sender As Object, e As EventArgs)
        Response.Redirect("NewMeeting.aspx")
    End Sub

    Private Sub LoadTopics()
        If ddlTopics.Items.Count = 0 Then
            Dim query As String = "SELECT TopicId, TopicTitle FROM Topics WHERE CommitteeId = @CommitteeId"
            Dim parameters As New List(Of SqlParameter) From {
                New SqlParameter("@CommitteeId", Session("CommitteeId"))
            }

            Dim dt As DataTable = DatabaseHelper.ExecuteQuery(query, parameters)

            ddlTopics.DataSource = dt
            ddlTopics.DataTextField = "TopicTitle"
            ddlTopics.DataValueField = "TopicId"
            ddlTopics.DataBind()

            ddlTopics.Items.Insert(0, New ListItem("-- اختر نقاط --", ""))
        End If
    End Sub

    Protected Sub btnViewTopicReport_Click(sender As Object, e As EventArgs)
        Dim selectedTopicId As String = ddlTopics.SelectedValue
        If Not String.IsNullOrEmpty(selectedTopicId) Then
            Response.Redirect("TopicReport.aspx?TopicId=" & selectedTopicId)
        Else
            lblMessage.Text = "يجب اختيار نقطه."
        End If
    End Sub
    ' الذهاب لتسجيل نتائج اجتماع
    Protected Sub btnMeetingResults_Click(sender As Object, e As EventArgs)
        Response.Redirect("MeetingResults.aspx")
    End Sub

    Protected Sub btnShowPreviousMeetings_Click(sender As Object, e As EventArgs)
        pnlPreviousMeetings.Visible = True
        LoadTopics()
    End Sub

    Protected Sub btnShowCurrentMeetings_Click(sender As Object, e As EventArgs)
        pnlCurrentMeetings.Visible = True
        LoadCurrentMeetings()
    End Sub
    'Private Sub LoadCurrentMeetings()
    '    Using conn As New SqlConnection(connStr)
    '        Dim query As String = "SELECT MeetingId, MeetingDate FROM Meetings WHERE MeetingId NOT IN (SELECT MeetingId FROM MeetingResult)"
    '        Using cmd As New SqlCommand(query, conn)
    '            conn.Open()
    '            Dim reader As SqlDataReader = cmd.ExecuteReader()
    '            ddlCurrentMeetings.DataSource = reader
    '            ddlCurrentMeetings.DataTextField = "MeetingDate"
    '            ddlCurrentMeetings.DataValueField = "MeetingId"
    '            ddlCurrentMeetings.DataBind()
    '        End Using
    '    End Using
    '    ddlCurrentMeetings.Items.Insert(0, New ListItem("-- اختر اجتماعاً --", "0"))
    'End Sub
    Private Sub LoadCurrentMeetings()
        Using conn As New SqlConnection(connStr)
            Dim query As String = "SELECT DISTINCT CONVERT(VARCHAR(10), MeetingDate, 103) AS MeetingDate " &
                                  "FROM Meetings " &
                                  "WHERE MeetingId NOT IN (SELECT MeetingId FROM MeetingResult) " &
                                  "ORDER BY MeetingDate DESC"

            Using cmd As New SqlCommand(query, conn)
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                ddlCurrentMeetings.DataSource = reader
                ddlCurrentMeetings.DataTextField = "MeetingDate"
                ddlCurrentMeetings.DataValueField = "MeetingDate"
                ddlCurrentMeetings.DataBind()
            End Using
        End Using
        ddlCurrentMeetings.Items.Insert(0, New ListItem("-- اختر اجتماعاً --", ""))
    End Sub
    ' دالة عند الضغط على "عرض الاجتماع"
    Protected Sub btnViewCurrentMeeting_Click(sender As Object, e As EventArgs)
        ' هنا تقدر تضيف الكود اللي هيتنفذ لما تضغط على الزرار
        Dim selectedMeetingId As String = ddlCurrentMeetings.SelectedValue
        If Not String.IsNullOrEmpty(selectedMeetingId) Then
            Response.Redirect("CurrentMeetingReport.aspx?MeetingId=" & selectedMeetingId)
        Else
            ' في حالة عدم اختيار اجتماع
            lblMessage.Text = "يجب اختيار اجتماع."
        End If
    End Sub
    Protected Sub btnShowOldMeetings_Click(sender As Object, e As EventArgs)
        pnlOldMeetings.Visible = True
        LoadOldMeetings()
    End Sub

    Private Sub LoadOldMeetings()
        Dim connStr As String = ConfigurationManager.ConnectionStrings("MainConn").ConnectionString
        Dim query As String = "SELECT DISTINCT M.MeetingId, CONVERT(varchar(10), M.MeetingDate, 120) AS MeetingDateOnly, M.MeetingDate FROM Meetings M INNER JOIN MeetingResult R ON M.MeetingId = R.MeetingId ORDER BY M.MeetingDate DESC"

        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand(query, conn)
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                ddlOldMeetings.Items.Clear()
                ddlOldMeetings.Items.Add(New ListItem("اختر اجتماع", ""))

                While reader.Read()
                    Dim item As New ListItem(reader("MeetingDateOnly").ToString(), reader("MeetingId").ToString())
                    ddlOldMeetings.Items.Add(item)
                End While
            End Using
        End Using
    End Sub

    ' تسجيل الخروج
    Protected Sub btnLogout_Click(sender As Object, e As EventArgs)
        Session.Clear()
        Session.Abandon()
        Response.Redirect("Login.aspx")
    End Sub
End Class
