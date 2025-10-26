Imports System.Data.SqlClient
Imports System.Data

Public Class New_Meeting
    Inherits System.Web.UI.Page

    ' ✅ التخزين في Session
    Private Property dtMeetingPlan As DataTable
        Get
            If Session("dtMeetingPlan") Is Nothing Then
                Dim dt As New DataTable()
                dt.Columns.AddRange({
                    New DataColumn("MeetingId", GetType(Integer)),
                    New DataColumn("LocationName", GetType(String)),
                    New DataColumn("TopicTitle", GetType(String)),
                    New DataColumn("Notes", GetType(String)),
                    New DataColumn("DiscussionTime", GetType(Integer)),
                    New DataColumn("LocationId", GetType(Integer)),
                    New DataColumn("TopicId", GetType(Integer))
                })
                Session("dtMeetingPlan") = dt
            End If
            Return CType(Session("dtMeetingPlan"), DataTable)
        End Get
        Set(value As DataTable)
            Session("dtMeetingPlan") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
        Response.Cache.SetNoStore()

        If Session("Username") Is Nothing Then
            Response.Redirect("Login.aspx")
        End If

        If Not IsPostBack Then
            LoadLocations()

            If Session("MeetingId") Is Nothing Then
                Dim nextId As Integer = GetNextMeetingIdFromDatabase()
                Session("MeetingId") = nextId
            End If

            lblMeetingId.Text = Session("MeetingId").ToString()
        End If

        ' ✅ تحميل الجريد دايمًا من السيشن
        gvMeetings.DataSource = dtMeetingPlan
        gvMeetings.DataBind()
    End Sub

    Private Function GetNextMeetingIdFromDatabase() As Integer
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConn").ConnectionString)
        Dim cmd As New SqlCommand("SELECT ISNULL(MAX(MeetingId), 0) + 1 FROM Meetings", con)
        con.Open()
        Dim result As Integer = Convert.ToInt32(cmd.ExecuteScalar())
        con.Close()
        Return result
    End Function

    Private Sub LoadLocations()
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConn").ConnectionString)
        Dim cmd As New SqlCommand("SELECT LocationId, LocationName FROM Locations", con)
        con.Open()
        ddlLocation.DataSource = cmd.ExecuteReader()
        ddlLocation.DataTextField = "LocationName"
        ddlLocation.DataValueField = "LocationId"
        ddlLocation.DataBind()
        ddlLocation.Items.Insert(0, New ListItem("-- اختر الموضوع --", ""))
        con.Close()
    End Sub

    Protected Sub ddlLocation_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlTopic.Items.Clear()
        ddlTopic.Items.Add(New ListItem("-- اختر النقاط --", ""))

        If ddlLocation.SelectedValue <> "" Then
            Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConn").ConnectionString)
            Dim cmd As New SqlCommand("SELECT TopicId, TopicTitle FROM Topics WHERE LocationId = @LocationId AND CommitteeId = @CommitteeId", con)
            cmd.Parameters.AddWithValue("@LocationId", ddlLocation.SelectedValue)
            cmd.Parameters.AddWithValue("@CommitteeId", Session("CommitteeId"))
            con.Open()
            ddlTopic.DataSource = cmd.ExecuteReader()
            ddlTopic.DataTextField = "TopicTitle"
            ddlTopic.DataValueField = "TopicId"
            ddlTopic.DataBind()
            ddlTopic.Items.Insert(0, New ListItem("-- اختر النقاط --", ""))
            con.Close()
        End If
    End Sub

    Protected Sub cvDate_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Dim inputDate As Date
        If Date.TryParse(txtDate.Text, inputDate) Then
            args.IsValid = inputDate >= Date.Today
        Else
            args.IsValid = False
        End If
    End Sub

    Protected Sub btnLogout_Click(sender As Object, e As EventArgs)
        Session.Clear()
        Session.Abandon()
        Response.Redirect("Login.aspx")
    End Sub

    Protected Sub btnAddToGrid_Click(sender As Object, e As EventArgs)
        If Page.IsValid Then
            ' 🧠 تحميل الـ dtMeetingPlan من السيشن
            Dim dtMeetingPlan As DataTable = TryCast(Session("dtMeetingPlan"), DataTable)
            If dtMeetingPlan Is Nothing Then
                dtMeetingPlan = CreateMeetingPlanTable() ' جدول جديد لو مش موجود
            End If

            ' ✅ التحقق من التكرار
            For Each row As DataRow In dtMeetingPlan.Rows
                If row("TopicId").ToString() = ddlTopic.SelectedValue Then
                    lblMessage.Text = "⚠️ هذه النقطه مضافه بالفعل."
                    Return
                End If
            Next

            ' ✅ إضافة السطر الجديد
            Dim meetingId As Integer = Integer.Parse(Session("MeetingId").ToString())
            Dim newRow As DataRow = dtMeetingPlan.NewRow()
            newRow("MeetingId") = meetingId
            newRow("LocationId") = Integer.Parse(ddlLocation.SelectedValue)
            newRow("LocationName") = ddlLocation.SelectedItem.Text
            newRow("TopicId") = Integer.Parse(ddlTopic.SelectedValue)
            newRow("TopicTitle") = ddlTopic.SelectedItem.Text
            newRow("Notes") = txtNotes.Text.Trim()
            newRow("DiscussionTime") = Integer.Parse(txtDuration.Text.Trim())
            dtMeetingPlan.Rows.Add(newRow)

            ' 🧠 حفظه في السيشن
            Session("dtMeetingPlan") = dtMeetingPlan

            ' ✅ تحميله في الجريد
            gvMeetings.DataSource = dtMeetingPlan
            gvMeetings.DataBind()

            ' 🧹 تفريغ الحقول
            txtNotes.Text = ""
            txtDuration.Text = ""
            ddlLocation.ClearSelection()
            ddlTopic.Items.Clear()
            ddlTopic.Items.Insert(0, New ListItem("-- اختر النقاط --", ""))

            lblMessage.Text = "✅ تم إضافة النقطه إلى الجدول."
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs)
        ' 🧠 تحميل البيانات من السيشن
        Dim dtMeetingPlan As DataTable = TryCast(Session("dtMeetingPlan"), DataTable)
        If dtMeetingPlan Is Nothing OrElse dtMeetingPlan.Rows.Count = 0 Then
            lblMessage.Text = "⚠️ لا يوجد نقاط مضافة للحفظ."
            Return
        End If

        If String.IsNullOrWhiteSpace(txtDate.Text) Then
            lblMessage.Text = "⚠️ يرجى إدخال تاريخ الاجتماع قبل الحفظ."
            Return
        End If

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConn").ConnectionString)
        con.Open()
        Dim trans As SqlTransaction = con.BeginTransaction()

        Try
            ' ✅ إضافة الوقت الحالي إلى التاريخ
            Dim selectedDate As Date = Date.Parse(txtDate.Text)
            Dim meetingDate As DateTime = selectedDate.Date + DateTime.Now.TimeOfDay

            ' ✅ إدخال الاجتماع
            Dim cmdMeeting As New SqlCommand("INSERT INTO Meetings (MeetingDate) VALUES (@MeetingDate); SELECT SCOPE_IDENTITY();", con, trans)
            cmdMeeting.Parameters.AddWithValue("@MeetingDate", meetingDate)
            Dim newMeetingId As Integer = Convert.ToInt32(cmdMeeting.ExecuteScalar())

            ' ✅ إدخال المواضيع
            For Each row As DataRow In dtMeetingPlan.Rows
                Dim cmdPlan As New SqlCommand("INSERT INTO MeetingPlan (MeetingId, TopicId, Notes, DiscussionTime) VALUES (@MeetingId, @TopicId, @Notes, @DiscussionTime)", con, trans)
                cmdPlan.Parameters.AddWithValue("@MeetingId", newMeetingId)
                cmdPlan.Parameters.AddWithValue("@TopicId", row("TopicId"))
                cmdPlan.Parameters.AddWithValue("@Notes", row("Notes"))
                cmdPlan.Parameters.AddWithValue("@DiscussionTime", row("DiscussionTime"))
                cmdPlan.ExecuteNonQuery()
            Next

            trans.Commit()
            lblMessage.Text = "✅ تم حفظ الاجتماع بنجاح."

            ' 🧹 تفريغ كل حاجة
            Session("dtMeetingPlan") = Nothing
            gvMeetings.DataSource = Nothing
            gvMeetings.DataBind()
            txtDate.Text = ""
            txtNotes.Text = ""
            txtDuration.Text = ""
            ddlLocation.ClearSelection()
            ddlTopic.Items.Clear()
            ddlTopic.Items.Insert(0, New ListItem("-- اختر النقاط --", ""))

            ' ✅ تحميل MeetingId جديد
            Dim nextId As Integer = GetNextMeetingIdFromDatabase()
            Session("MeetingId") = nextId
            lblMeetingId.Text = nextId.ToString()

        Catch ex As Exception
            trans.Rollback()
            lblMessage.Text = "❌ حدث خطأ أثناء الحفظ: " & ex.Message
        Finally
            con.Close()
        End Try
    End Sub
    Private Function CreateMeetingPlanTable() As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("MeetingId", GetType(Integer))
        dt.Columns.Add("LocationId", GetType(Integer))
        dt.Columns.Add("LocationName", GetType(String))
        dt.Columns.Add("TopicId", GetType(Integer))
        dt.Columns.Add("TopicTitle", GetType(String))
        dt.Columns.Add("Notes", GetType(String))
        dt.Columns.Add("DiscussionTime", GetType(Integer))
        Return dt
    End Function
    Protected Sub btnGoMain_Click(sender As Object, e As EventArgs)
        Response.Redirect("Main.aspx")
    End Sub

End Class
