Imports System.Text

Public Class Raw_View
    Public strFileToLoad As String

    Private Sub Raw_View_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.rawViewLocation = Me.Location
        My.Settings.rawViewSize = Me.Size
        My.Settings.Save()

        rawViewInstance.Dispose()
        rawViewInstance = Nothing
    End Sub

    Private Sub Raw_View_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblFileName.Text = "Log File: " & strFileToLoad & "         File Size: " & bytesToHumanSize(New IO.FileInfo(strFileToLoad).Length)

        Dim fileHandle As New IO.StreamReader(strFileToLoad, Encoding.UTF8)
        txtRawFileView.Text = fileHandle.ReadToEnd.Trim
        fileHandle.Close()
        fileHandle.Dispose()
    End Sub

    Private Sub Raw_View_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        My.Settings.rawViewSize = Me.Size
    End Sub
End Class