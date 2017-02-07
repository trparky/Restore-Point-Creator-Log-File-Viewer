Public Class Search_Event_Log
    Public Enum userResponse
        abortSearch = 0
        doSearch = 1
    End Enum

    Public Enum searceType
        typeAny = 0
        typeInfo = 1
        typeError = 2
    End Enum

    Public searchType As searceType
    Public searchTerms As String = ""
    Public boolCaseInsensitive As Boolean = False
    Public boolUseRegEx As Boolean = False
    Private boolButtonPushed As Boolean = False
    Public dialogResponse As userResponse = userResponse.abortSearch
    Public previousSearchType As searceType

    Public Function boolTestRegExPattern(strPattern As String) As Boolean
        Try
            Dim testRegExPattern As New Text.RegularExpressions.Regex(strPattern)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        If chkRegEx.Checked And Not boolTestRegExPattern(txtSearchTerms.Text.Trim) Then
            MsgBox("There was an error detected in your Regular Expression pattern. Please try again.", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        searchTerms = txtSearchTerms.Text.Trim
        boolCaseInsensitive = chkCaseInsensitive.Checked
        boolUseRegEx = chkRegEx.Checked

        If radAny.Checked Then
            searchType = searceType.typeAny
        ElseIf radError.Checked Then
            searchType = searceType.typeError
        ElseIf radInfo.Checked Then
            searchType = searceType.typeInfo
        End If

        dialogResponse = userResponse.doSearch
        boolButtonPushed = True
        Me.Close()
    End Sub

    Private Sub Search_Event_Log_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If Not boolButtonPushed Then dialogResponse = userResponse.abortSearch
    End Sub

    Private Sub txtSearchTerms_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearchTerms.KeyUp
        If Not String.IsNullOrEmpty(txtSearchTerms.Text.Trim) And e.KeyCode = Keys.Enter Then
            btnSearch.PerformClick()
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtSearchTerms.Text = Nothing
    End Sub

    Private Sub txtSearchTerms_TextChanged(sender As Object, e As EventArgs) Handles txtSearchTerms.TextChanged
        If String.IsNullOrEmpty(txtSearchTerms.Text) Then
            btnSearch.Enabled = False
            btnClear.Enabled = False
        Else
            btnSearch.Enabled = True
            btnClear.Enabled = True
        End If
    End Sub

    Private Sub Search_Event_Log_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If previousSearchType.Equals(searceType.typeAny) Then
            radAny.Checked = True
        ElseIf previousSearchType.Equals(searceType.typeError) Then
            radError.Checked = True
        ElseIf previousSearchType.Equals(searceType.typeInfo) Then
            radInfo.Checked = True
        End If

        radError.Text = EventLogEntryType.Error.ToString
        radInfo.Text = EventLogEntryType.Information.ToString
    End Sub
End Class