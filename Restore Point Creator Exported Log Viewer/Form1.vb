Imports System.ComponentModel
Imports System.Security.Principal
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.Win32

Public Class Form1
    Private exportedLogFile As exportedLogFile
    Private oldSplitterDifference As Integer
    Private m_SortingColumn As ColumnHeader
    Private boolDoneLoading As Boolean = False
    Private shortExportDataVersion As Short = 1
    Private boolFileLoaded As Boolean = False
    Private strLoadedFile As String = Nothing

    Private Const strApplication As String = "Application"
    Private Const strRestorePointCreator As String = "Restore Point Creator"
    Private Const strSystemRestorePointCreator As String = "System Restore Point Creator"

    Private rawSearchTerms As String = Nothing, previousSearchType As Search_Event_Log.searceType
    Private regexStartedOrEndEventCheck As New Regex("(?:started the program|closed the program)", RegexOptions.Compiled + RegexOptions.IgnoreCase + RegexOptions.Multiline)
    Private eventLogContents As New List(Of eventLogListEntry)

    Enum storedDateType
        unixTimestamp = 0
        windowsTimeString = 1
    End Enum

    ''' <summary>Creates a List View Item out of a log entry.</summary>
    ''' <param name="logEntry">An restorePointCreatorExportedLog Class instance.</param>
    ''' <returns>Returns a ListViewItem object.</returns>
    Function processLogEntry(ByRef logEntry As restorePointCreatorExportedLog, ByRef dateType As storedDateType) As eventLogListEntry
        Dim listViewItemObject As eventLogListEntry
        Dim entryDate As Date

        Select Case logEntry.logType
            Case Eventing.Reader.StandardEventLevel.Error ' Error
                listViewItemObject = New eventLogListEntry("Error") With {.ImageIndex = 0}
            Case Eventing.Reader.StandardEventLevel.Informational ' Information
                listViewItemObject = New eventLogListEntry("Information") With {.ImageIndex = 1}
            Case Eventing.Reader.StandardEventLevel.Warning ' Warning
                listViewItemObject = New eventLogListEntry("Warning") With {.ImageIndex = 2}
            Case Else
                listViewItemObject = New eventLogListEntry("Unknown")
        End Select

        With listViewItemObject
            .eventLogSource = logEntry.logSource
            .eventLogLevel = logEntry.logType
            .eventLogEntryID = logEntry.logID
            .eventLogText = logEntry.logData
        End With

        If shortExportDataVersion = 1 Then
            If logEntry.unixTime = 0 Then
                dateType = storedDateType.windowsTimeString

                If logEntry.logDate Is Nothing Then
                    listViewItemObject.SubItems.Add("(No date entry found)")
                Else
                    If Date.TryParse(logEntry.logDate, entryDate) Then
                        listViewItemObject.SubItems.Add(entryDate.ToLocalTime.ToString)
                    Else
                        listViewItemObject.SubItems.Add("(Error in date parsing)")
                    End If
                End If
            Else
                dateType = storedDateType.unixTimestamp

                entryDate = UNIXTimestampToDate(logEntry.unixTime)
                listViewItemObject.SubItems.Add(entryDate.ToLocalTime.ToString)
            End If
        ElseIf shortExportDataVersion = 3 Or shortExportDataVersion = 4 Then
            dateType = storedDateType.unixTimestamp

            entryDate = UNIXTimestampToDate(logEntry.unixTime)
            listViewItemObject.SubItems.Add(entryDate.ToLocalTime.ToString)
        End If

        With listViewItemObject
            .SubItems.Add(formatNumber(logEntry.logID))
            .SubItems.Add(logEntry.logSource)
            .SubItems.Add("")
        End With

        Return listViewItemObject
    End Function

    Sub openFile(strFileName As String)
        lblFileName.Text = "Log File: " & strFileName
        strLoadedFile = strFileName

        boolFileLoaded = True

        Dim fileInfo As New IO.FileInfo(strFileName)
        Dim timeStamp As New Stopwatch
        Dim dateType As storedDateType

        eventLogContents.Clear()
        timeStamp.Start()

        If fileInfo.Extension.Equals(".reslog", StringComparison.OrdinalIgnoreCase) Then
            lblLogFileType.Text = "Log File Type: Legacy JSON"
            Dim jsonEngine As New Web.Script.Serialization.JavaScriptSerializer
            Dim logEntry As restorePointCreatorExportedLog

            Dim fileHandle As New IO.StreamReader(strFileName, Encoding.UTF8)
            Dim strLineInFile As String = fileHandle.ReadLine

            lblProgramVersion.Text = ""
            lblProgramVersion.Visible = False

            lblOSVersion.Text = ""
            lblOSVersion.Visible = False

            While strLineInFile IsNot Nothing
                If strLineInFile.StartsWith("//") Then
                    If strLineInFile.ToLower.StartsWith("// program version: ") Then
                        lblProgramVersion.Text = strLineInFile.Replace("// ", "")
                        lblProgramVersion.Visible = True
                    ElseIf strLineInFile.ToLower.StartsWith("// operating system: ") Then
                        lblOSVersion.Text = strLineInFile.Replace("// ", "")
                        lblOSVersion.Visible = True
                    ElseIf strLineInFile.ToLower.StartsWith("// export data version: ") Then
                        shortExportDataVersion = Short.Parse(Regex.Match(strLineInFile, "// Export Data Version: (\d{1,2})", RegexOptions.IgnoreCase).Groups(1).Value)
                    End If
                Else
                    logEntry = jsonEngine.Deserialize(strLineInFile, GetType(restorePointCreatorExportedLog))
                    eventLogContents.Add(processLogEntry(logEntry, dateType))

                    'If chkProgramClosingAndOpeningEvents.Checked Then
                    '    itemsToPutInToList.Add(processLogEntry(logEntry, dateType))
                    'Else
                    '    If Not regexStartedOrEndEventCheck.IsMatch(logEntry.logData) Then
                    '        itemsToPutInToList.Add(processLogEntry(logEntry, dateType))
                    '    End If
                    'End If
                End If

                strLineInFile = fileHandle.ReadLine
            End While

            With lblDateType
                If shortExportDataVersion = 1 Then
                    If dateType = storedDateType.windowsTimeString Then
                        .Text = "Date Type: Date String"
                    ElseIf dateType = storedDateType.unixTimestamp Then
                        .Text = "Date Type: UNIX Timestamp"
                    End If
                ElseIf shortExportDataVersion = 3 Then
                    .Text = "Date Type: UNIX Timestamp"
                End If
            End With

            jsonEngine = Nothing
            fileHandle.Close()
            fileHandle.Dispose()
        ElseIf fileInfo.Extension.Equals(".reslogx", StringComparison.OrdinalIgnoreCase) Then
            If exportedLogFile Is Nothing Then
                Dim streamReader As New IO.StreamReader(strFileName)
                exportedLogFile = New exportedLogFile()
                Dim xmlSerializerObject As New Xml.Serialization.XmlSerializer(exportedLogFile.GetType)
                exportedLogFile = xmlSerializerObject.Deserialize(streamReader)
                streamReader.Close()
                streamReader.Dispose()

                lblProgramVersion.Text = "Program Version: " & exportedLogFile.programVersion
                lblProgramVersion.Visible = True
                lblOSVersion.Text = "Operating System: " & exportedLogFile.operatingSystem
                lblOSVersion.Visible = True
                lblLogFileType.Text = "Log File Type: XML"

                shortExportDataVersion = exportedLogFile.version

                lblDateType.Text = "Date Type: UNIX Timestamp"
            End If

            For Each logEntry As restorePointCreatorExportedLog In exportedLogFile.logsEntries
                eventLogContents.Add(processLogEntry(logEntry, dateType))

                'If chkProgramClosingAndOpeningEvents.Checked Then
                '    itemsToPutInToList.Add(processLogEntry(item, dateType))
                'Else
                '    If Not regexStartedOrEndEventCheck.IsMatch(item.logData) Then
                '        itemsToPutInToList.Add(processLogEntry(item, dateType))
                '    End If
                'End If
            Next
        End If

        With eventLogList
            .Items.Clear()
            .Items.AddRange(eventLogContents.ToArray())
            .Sort()
        End With

        lblCount.Text = "Number of Logs: " & eventLogList.Items.Count
        lblExportVersion.Text = "Data Export Version: " & shortExportDataVersion
        lblFileSize.Text = "File Size: " & bytesToHumanSize(New IO.FileInfo(strFileName).Length)

        timeStamp.Stop()
        lblProcessed.Text = String.Format("Event Log Loaded and Processed in {0}ms ({1} seconds).", timeStamp.ElapsedMilliseconds, Math.Round(timeStamp.Elapsed.TotalSeconds, 3))

        btnClear.Enabled = True
        btnSearch.Enabled = True
        eventLogList.Enabled = True

        MsgBox("Event Log Entry File Import Complete.", MsgBoxStyle.Information, Me.Text)
    End Sub

    Private Sub btnBrowseForFile_Click(sender As Object, e As EventArgs) Handles btnBrowseForFile.Click
        OpenFileDialog1.Title = "Open Restore Point Creator Exported Log File"
        OpenFileDialog1.FileName = Nothing
        OpenFileDialog1.Filter = "Restore Point Creator Exported Log File|*.reslog;*.reslogx"

        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            btnRawView.Enabled = True
            exportedLogFile = Nothing
            openFile(OpenFileDialog1.FileName)
        End If
    End Sub

    Public Function formatNumber(strInput As String) As String
        Dim longInput As Long
        If Long.TryParse(strInput, longInput) Then
            Return longInput.ToString("n0", Globalization.CultureInfo.InvariantCulture)
        Else
            Return strInput
        End If
    End Function

    Private Function UNIXTimestampToDate(ByVal strUnixTime As ULong) As Date
        'Dim tmpDate As Date = DateAdd(DateInterval.Second, strUnixTime, #1/1/1970#)
        Dim tmpDate As Date = DateAdd(DateInterval.Second, strUnixTime, New DateTime(1970, 1, 1, 0, 0, 0, 0))

        If tmpDate.IsDaylightSavingTime Then
            tmpDate = DateAdd(DateInterval.Hour, 1, tmpDate)
        End If

        Return tmpDate
    End Function

    Private Sub registerFileExtension(extension As String)
        Dim registryKey As RegistryKey

        registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Classes", True)

        If registryKey IsNot Nothing Then
            registryKey.CreateSubKey(extension)
            registryKey.Close()
            registryKey.Dispose()
        End If

        registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Classes\" & extension, True)

        If registryKey IsNot Nothing Then
            registryKey.CreateSubKey("DefaultIcon").SetValue(vbNullString, String.Format("{0}{1}{0},0", Chr(34), Windows.Forms.Application.ExecutablePath))
            registryKey.CreateSubKey("shell").CreateSubKey("open")
            registryKey.Close()
            registryKey.Dispose()
        End If

        registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Classes\" & extension & "\shell\open", True)

        If registryKey IsNot Nothing Then
            registryKey.SetValue(vbNullString, String.Format("{0}{1}{0} {0}%1{0}", Chr(34), Windows.Forms.Application.ExecutablePath))
            registryKey.Close()
            registryKey.Dispose()
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        imageList.Images.Add("Error", My.Resources.errorIcon)
        imageList.Images.Add("Information", My.Resources.informationIcon)
        imageList.Images.Add("Warning", My.Resources.warningIcon)

        Me.Size = My.Settings.windowSize
        Me.WindowState = My.Settings.windowState

        applySavedSorting()

        ColumnHeader1.Width = My.Settings.eventLogColumn1Size
        ColumnHeader2.Width = My.Settings.eventLogColumn2Size
        ColumnHeader3.Width = My.Settings.eventLogColumn3Size
        ColumnHeader4.Width = My.Settings.eventLogColumn4Size

        chkProgramClosingAndOpeningEvents.Checked = My.Settings.boolIncludeOpeningAndClosingEvents

        If Registry.CurrentUser.OpenSubKey("SOFTWARE\Classes\.reslog") IsNot Nothing And Registry.CurrentUser.OpenSubKey("SOFTWARE\Classes\.reslogx") IsNot Nothing Then
            chkAssociate.Checked = True
        End If

        boolDoneLoading = True
    End Sub

    Private Sub SplitContainer1_SplitterMoved(sender As Object, e As SplitterEventArgs) Handles SplitContainer1.SplitterMoved
        If SplitContainer1.SplitterDistance < 418 Then
            SplitContainer1.SplitterDistance = 418
            Exit Sub
        End If

        Try
            If boolDoneLoading Then My.Settings.splitDistance = SplitContainer1.SplitterDistance
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Form1_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        My.Settings.windowSize = Me.Size
        My.Settings.windowState = Me.WindowState
        My.Settings.Save()
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Try
            SplitContainer1.SplitterDistance = oldSplitterDifference
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Form1_ResizeBegin(sender As Object, e As EventArgs) Handles Me.ResizeBegin
        oldSplitterDifference = SplitContainer1.SplitterDistance
    End Sub

    Private Sub eventLogList_ColumnWidthChanged(sender As Object, e As ColumnWidthChangedEventArgs) Handles eventLogList.ColumnWidthChanged
        If boolDoneLoading Then
            My.Settings.eventLogColumn1Size = ColumnHeader1.Width
            My.Settings.eventLogColumn2Size = ColumnHeader2.Width
            My.Settings.eventLogColumn3Size = ColumnHeader3.Width
            My.Settings.eventLogColumn4Size = ColumnHeader4.Width
            My.Settings.Save()
        End If
    End Sub

    Private Sub eventLogList_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles eventLogList.ColumnClick
        ' Get the new sorting column.
        Dim new_sorting_column As ColumnHeader = eventLogList.Columns(e.Column)
        My.Settings.sortingColumn = e.Column

        ' Figure out the new sorting order.
        Dim sort_order As SortOrder
        If (m_SortingColumn Is Nothing) Then
            ' New column. Sort ascending.
            sort_order = SortOrder.Ascending
            My.Settings.sortingOrder = SortOrder.Ascending
        Else
            ' See if this is the same column.
            If new_sorting_column.Equals(m_SortingColumn) Then
                ' Same column. Switch the sort order.
                If m_SortingColumn.Text.StartsWith("> ") Then
                    sort_order = SortOrder.Descending
                    My.Settings.sortingOrder = SortOrder.Descending
                Else
                    sort_order = SortOrder.Ascending
                    My.Settings.sortingOrder = SortOrder.Ascending
                End If
            Else
                ' New column. Sort ascending.
                sort_order = SortOrder.Ascending
                My.Settings.sortingOrder = SortOrder.Ascending
            End If

            ' Remove the old sort indicator.
            m_SortingColumn.Text = m_SortingColumn.Text.Substring(2)
        End If

        ' Display the new sort order.
        m_SortingColumn = new_sorting_column
        If sort_order = SortOrder.Ascending Then
            m_SortingColumn.Text = "> " & m_SortingColumn.Text
        Else
            m_SortingColumn.Text = "< " & m_SortingColumn.Text
        End If

        ' Create a comparer.
        eventLogList.ListViewItemSorter = New ListViewComparer(e.Column, sort_order)

        ' Sort.
        eventLogList.Sort()
    End Sub

    Sub applySavedSorting()
        ' Some data validation.
        If My.Settings.sortingColumn < 0 Or My.Settings.sortingColumn > 4 Then
            My.Settings.sortingColumn = 0
        End If

        If My.Settings.sortingOrder <> 1 And My.Settings.sortingOrder <> 2 Then
            My.Settings.sortingOrder = 2
        End If
        ' Some data validation.

        ' Get the new sorting column.
        Dim new_sorting_column As ColumnHeader = eventLogList.Columns(My.Settings.sortingColumn)
        Dim sort_order As SortOrder = My.Settings.sortingOrder

        ' Figure out the new sorting order.
        If (m_SortingColumn IsNot Nothing) Then
            ' See if this is the same column.
            If new_sorting_column.Equals(m_SortingColumn) Then
                ' Same column. Switch the sort order.
                If m_SortingColumn.Text.StartsWith("> ") Then
                    sort_order = SortOrder.Descending
                    My.Settings.sortingOrder = SortOrder.Descending
                Else
                    sort_order = SortOrder.Ascending
                    My.Settings.sortingOrder = SortOrder.Ascending
                End If
            Else
                ' New column. Sort ascending.
                sort_order = SortOrder.Ascending
                My.Settings.sortingOrder = SortOrder.Ascending
            End If

            ' Remove the old sort indicator.
            m_SortingColumn.Text = m_SortingColumn.Text.Substring(2)
        End If

        ' Display the new sort order.
        m_SortingColumn = new_sorting_column
        If sort_order = SortOrder.Ascending Then
            m_SortingColumn.Text = "> " & m_SortingColumn.Text
        Else
            m_SortingColumn.Text = "< " & m_SortingColumn.Text
        End If

        ' Create a comparer.
        eventLogList.ListViewItemSorter = New ListViewComparer(My.Settings.sortingColumn, sort_order)

        ' Sort.
        eventLogList.Sort()
    End Sub

    Public Function removeSourceCodePathInfo(strInput As String) As String
        If strInput.ToLower.caseInsensitiveContains("Google Drive") Then
            Return Regex.Replace(strInput, Regex.Escape("C:\Users\Tom\OneDrive\My Visual Studio Projects\Projects\"), "", RegexOptions.IgnoreCase)
        Else
            Return strInput
        End If
    End Function

    Private Sub eventLogList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles eventLogList.SelectedIndexChanged
        Try
            If eventLogList.SelectedItems.Count <> 0 Then
                eventLogText.Text = DirectCast(eventLogList.SelectedItems(0), eventLogListEntry).eventLogText.Replace(vbLf, vbCrLf)
                Debug.WriteLine(eventLogText.Text)
            End If
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>Tests to see if the RegEx pattern is valid or not.</summary>
    ''' <param name="strPattern">A RegEx pattern.</param>
    ''' <returns>A Boolean value. If True, the RegEx pattern is valid. If False, the RegEx pattern is not valid.</returns>
    Private Function boolTestRegExPattern(strPattern As String) As Boolean
        Try
            Dim testRegExPattern As New Regex(strPattern)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        For Each item As ListViewItem In eventLogList.Items
            item.SubItems(4).Text = ""
            item.BackColor = eventLogList.BackColor
        Next

        Dim searchWindow As New Search_Event_Log
        With searchWindow
            .StartPosition = FormStartPosition.CenterParent
            .txtSearchTerms.Text = rawSearchTerms
            .previousSearchType = previousSearchType
            .ShowDialog()
        End With

        If searchWindow.dialogResponse = Search_Event_Log.userResponse.doSearch Then
            Dim searchTerms As String = searchWindow.searchTerms

            With searchWindow
                rawSearchTerms = .searchTerms
                previousSearchType = .searchType
            End With

            Dim boolCaseInsensitive As Boolean = searchWindow.boolCaseInsensitive
            Dim boolUseRegEx As Boolean = searchWindow.boolUseRegEx
            Dim searchType As Search_Event_Log.searceType = searchWindow.searchType

            If boolUseRegEx And Not boolTestRegExPattern(searchTerms) Then
                MsgBox("Invalid RegEx Pattern.", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If

            searchWindow.Dispose()
            searchWindow = Nothing

            eventLogList.Items.Clear()

            For Each item As eventLogListEntry In eventLogContents
                With item
                    If boolUseRegEx Then
                        If searchType = Search_Event_Log.searceType.typeAny And .eventLogText.regExSearch(searchTerms) Then
                            eventLogList.Items.Add(item)
                        ElseIf searchType = Search_Event_Log.searceType.typeError And .strEventLogLevel = EventLogEntryType.Error.ToString And .eventLogText.regExSearch(searchTerms) Then
                            eventLogList.Items.Add(item)
                        ElseIf searchType = Search_Event_Log.searceType.typeInfo And .strEventLogLevel = EventLogEntryType.Information.ToString And .eventLogText.regExSearch(searchTerms) Then
                            eventLogList.Items.Add(item)
                        End If
                    ElseIf boolCaseInsensitive Then
                        If searchType = Search_Event_Log.searceType.typeAny And .eventLogText.caseInsensitiveContains(searchTerms) Then
                            eventLogList.Items.Add(item)
                        ElseIf searchType = Search_Event_Log.searceType.typeError And .strEventLogLevel = EventLogEntryType.Error.ToString And .eventLogText.caseInsensitiveContains(searchTerms) Then
                            eventLogList.Items.Add(item)
                        ElseIf searchType = Search_Event_Log.searceType.typeInfo And .strEventLogLevel = EventLogEntryType.Information.ToString And .eventLogText.caseInsensitiveContains(searchTerms) Then
                            eventLogList.Items.Add(item)
                        End If
                    Else
                        If searchType = Search_Event_Log.searceType.typeAny And .eventLogText.Contains(searchTerms) Then
                            eventLogList.Items.Add(item)
                        ElseIf searchType = Search_Event_Log.searceType.typeError And .strEventLogLevel = EventLogEntryType.Error.ToString And .eventLogText.Contains(searchTerms) Then
                            eventLogList.Items.Add(item)
                        ElseIf searchType = Search_Event_Log.searceType.typeInfo And .strEventLogLevel = EventLogEntryType.Information.ToString And .eventLogText.Contains(searchTerms) Then
                            eventLogList.Items.Add(item)
                        End If
                    End If
                End With
            Next

            If eventLogList.Items.Count <> 0 Then
                eventLogList.Sort()

                Dim strEntriesFound As String
                If eventLogList.Items.Count = 1 Then
                    strEntriesFound = "1 log entry was found."
                Else
                    strEntriesFound = eventLogList.Items.Count.ToString & " log entries were found."
                End If

                MsgBox("Search complete. " & strEntriesFound, MsgBoxStyle.Information, Me.Text)
            Else
                MsgBox("Search complete. No results found.", MsgBoxStyle.Information, Me.Text)
            End If
        End If
    End Sub

    Public Sub reRunWithAdminUserRights(Optional strCommandLineArguments As String = Nothing)
        Try
            Dim startInfo As New ProcessStartInfo

            Dim executablePathPathInfo As New IO.FileInfo(Application.ExecutablePath)
            startInfo.FileName = executablePathPathInfo.FullName
            executablePathPathInfo = Nothing

            If strCommandLineArguments = Nothing Then
                If Environment.GetCommandLineArgs.Count <> 1 Then
                    startInfo.Arguments = Environment.GetCommandLineArgs(1)
                End If
            Else
                startInfo.Arguments = strCommandLineArguments
            End If

            If Not areWeAnAdministrator() Then startInfo.Verb = "runas"

            Process.Start(startInfo)
        Catch ex As Win32Exception
            MsgBox("There was an error while attempting to elevate the process, please make sure that when the Windows UAC prompt appears asking you to run the program with elevated privileges that you say ""Yes"" to the UAC prompt." & vbCrLf & vbCrLf & "The program will now terminate.", MsgBoxStyle.Critical, Me.Text)
        End Try
    End Sub

    Public Function areWeAnAdministrator() As Boolean
        Try
            Dim principal As WindowsPrincipal = New WindowsPrincipal(WindowsIdentity.GetCurrent())

            If principal.IsInRole(WindowsBuiltInRole.Administrator) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        rawSearchTerms = Nothing

        With eventLogList
            .Items.Clear()
            .Items.AddRange(eventLogContents.ToArray())
            .Sort()
        End With
    End Sub

    Private Sub chkAssociate_Click(sender As Object, e As EventArgs) Handles chkAssociate.Click
        If chkAssociate.Checked Then
            registerFileExtension(".reslog")
            registerFileExtension(".reslogx")
        Else
            Registry.CurrentUser.OpenSubKey("SOFTWARE\Classes", True).DeleteSubKeyTree(".reslog", False)
            Registry.CurrentUser.OpenSubKey("SOFTWARE\Classes", True).DeleteSubKeyTree(".reslogx", False)
        End If
    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        If My.Application.CommandLineArgs.Count = 1 Then
            Dim strPassedCommandLine As String = My.Application.CommandLineArgs(0).Trim

            If IO.File.Exists(strPassedCommandLine) And (strPassedCommandLine.EndsWith(".reslog", StringComparison.OrdinalIgnoreCase) Or strPassedCommandLine.EndsWith(".reslogx", StringComparison.OrdinalIgnoreCase)) Then
                btnRawView.Enabled = True
                openFile(strPassedCommandLine)
            End If
        End If
    End Sub

    Private Sub btnRawView_Click(sender As Object, e As EventArgs) Handles btnRawView.Click
        If IO.File.Exists(strLoadedFile) Then
            If rawViewInstance Is Nothing Then
                rawViewInstance = New Raw_View With {
                    .Size = My.Settings.rawViewSize,
                    .strFileToLoad = strLoadedFile
                }
                rawViewInstance.Show()
                rawViewInstance.Location = My.Settings.rawViewLocation
            Else
                rawViewInstance.BringToFront()
            End If
        Else
            MsgBox("The Raw View Tool cannot be loaded, the source file no longer exists.", MsgBoxStyle.Critical, Me.Text)
        End If
    End Sub

    Private Sub chkProgramClosingAndOpeningEvents_Click(sender As Object, e As EventArgs) Handles chkProgramClosingAndOpeningEvents.Click
        My.Settings.boolIncludeOpeningAndClosingEvents = chkProgramClosingAndOpeningEvents.Checked
        If boolFileLoaded Then openFile(strLoadedFile)
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.windowSize = Me.Size
        My.Settings.windowState = Me.WindowState
    End Sub
End Class

Public Class restorePointCreatorExportedLog
    Public logType As Short
    Public logID As Long
    Public unixTime As ULong = 0
    Public logDate, logData, logSource As String
End Class

Public Class exportedLogFile
    Public programVersion, operatingSystem As String, version As Short
    Public logsEntries As List(Of restorePointCreatorExportedLog)
End Class