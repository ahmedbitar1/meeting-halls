Imports System.Data.SqlClient

Partial Class Committees
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("Username") Is Nothing Then
            Response.Redirect("Login.aspx")
        End If
    End Sub

    Protected Sub btnCrisisCommittee_Click(sender As Object, e As EventArgs)
        ' نخزن رقم لجنة الأزمات في الـ Session
        Session("CommitteeId") = 1  ' 1 هو الـ ID بتاع لجنة الأزمات في جدول Committees
        Response.Redirect("Main.aspx")
    End Sub
End Class
