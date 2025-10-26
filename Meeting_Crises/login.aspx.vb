Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Text
Imports System.Configuration

Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("Username") IsNot Nothing AndAlso Session("Role") IsNot Nothing Then
                Response.Redirect("Committees.aspx")
            End If
        End If
    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim username As String = txtUsername.Text.Trim()
        Dim password As String = txtPassword.Text.Trim()

        If username = "" OrElse password = "" Then
            lblMessage.ForeColor = Drawing.Color.Red
            lblMessage.Text = "يرجى إدخال اسم المستخدم وكلمة المرور"
            Exit Sub
        End If

        Dim hashedPassword As String = ComputeSha256Hash(password)

        Dim connString As String = ConfigurationManager.ConnectionStrings("MainConn").ConnectionString
        Dim query As String = "SELECT * FROM Users WHERE Username = @Username AND PasswordHash = @Password"

        Using conn As New SqlConnection(connString)
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@Username", username)
            cmd.Parameters.AddWithValue("@Password", hashedPassword)

            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                If reader.HasRows Then
                    While reader.Read()
                        Session("UserID") = reader("UserID").ToString()
                        Session("Username") = reader("Username").ToString()
                        Session("Role") = reader("Role").ToString()
                    End While

                    lblMessage.ForeColor = Drawing.Color.Green
                    lblMessage.Text = "تم تسجيل الدخول بنجاح"

                    ' تأجيل التحويل باستخدام JavaScript بعد 2 ثانية
                    ClientScript.RegisterStartupScript(Me.GetType(), "redirect", "setTimeout(function(){ window.location='Committees.aspx'; }, 2000);", True)

                Else
                    lblMessage.ForeColor = Drawing.Color.Red
                    lblMessage.Text = "اسم المستخدم أو كلمة المرور غير صحيحة"
                End If

            Catch ex As Exception
                lblMessage.ForeColor = Drawing.Color.Red
                lblMessage.Text = "حدث خطأ في الاتصال بقاعدة البيانات. يرجى المحاولة لاحقًا."
                ' للتصحيح أثناء التطوير: يمكن عرض تفاصيل الخطأ
                ' lblMessage.Text &= "<br/>" & ex.Message
            Finally
                conn.Close()
            End Try
        End Using
    End Sub


    ' دالة الهاش المستخدمة لتحويل كلمة المرور إلى hash
    Private Function ComputeSha256Hash(rawData As String) As String
        Using sha256Hash As SHA256 = SHA256.Create()
            ' حساب الـ hash
            Dim bytes As Byte() = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData))
            Dim builder As New StringBuilder()

            ' تحويل البايتات إلى سلسلة hex
            For Each b As Byte In bytes
                builder.Append(b.ToString("x2"))
            Next

            ' إرجاع الـ hash كـ string
            Return builder.ToString()
        End Using
    End Function
End Class
