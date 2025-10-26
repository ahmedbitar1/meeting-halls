Imports System.Data.SqlClient
Imports System.Configuration

Public Class DatabaseHelper
    Private Shared ReadOnly connectionString As String = ConfigurationManager.ConnectionStrings("MainConn").ConnectionString

    Public Shared Function GetConnection() As SqlConnection
        Dim conn As New SqlConnection(connectionString)
        conn.Open()
        Return conn
    End Function

    Public Shared Function ExecuteQuery(query As String, Optional parameters As List(Of SqlParameter) = Nothing) As DataTable
        Dim dt As New DataTable()

        Using conn As SqlConnection = GetConnection()
            Using cmd As New SqlCommand(query, conn)
                If parameters IsNot Nothing Then
                    cmd.Parameters.AddRange(parameters.ToArray())
                End If

                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using

        Return dt
    End Function

    Public Shared Function ExecuteNonQuery(query As String, Optional parameters As List(Of SqlParameter) = Nothing) As Integer
        Using conn As SqlConnection = GetConnection()
            Using cmd As New SqlCommand(query, conn)
                If parameters IsNot Nothing Then
                    cmd.Parameters.AddRange(parameters.ToArray())
                End If
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function
End Class
