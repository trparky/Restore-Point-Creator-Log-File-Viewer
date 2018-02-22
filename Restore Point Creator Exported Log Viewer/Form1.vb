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
    Private nullDate As New Date(1, 1, 1, 0, 0, 0, DateTimeKind.Local)

    Enum storedDateType
        unixTimestamp = 0
        windowsTimeString = 1
        mixed = 3
    End Enum

    ''' <summary>Creates a List View Item out of a log entry.</summary>
    ''' <param name="logEntry">An restorePointCreatorExportedLog Class instance.</param>
    ''' <returns>Returns a ListViewItem object.</returns>
    Function processLogEntry(ByRef logEntry As restorePointCreatorExportedLog, ByRef dateType As storedDateType) As eventLogListEntry
        Dim listViewItemObject As eventLogListEntry
        Dim entryDate As Date

        Select Case logEntry.logType
            Case EventLogEntryType.Error ' Error
                listViewItemObject = New eventLogListEntry("Error") With {.ImageIndex = 0}
            Case EventLogEntryType.Information ' Information
                listViewItemObject = New eventLogListEntry("Information") With {.ImageIndex = 1}
            Case EventLogEntryType.Warning ' Warning
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
                listViewItemObject.SubItems.Add(If(chkConvertTimes.Checked, entryDate.ToLocalTime.ToString, entryDate.ToString))
            End If
        ElseIf shortExportDataVersion = 3 Or shortExportDataVersion = 4 Then
            dateType = storedDateType.unixTimestamp

            entryDate = UNIXTimestampToDate(logEntry.unixTime)
            listViewItemObject.SubItems.Add(If(chkConvertTimes.Checked, entryDate.ToLocalTime.ToString, entryDate.ToString))
        ElseIf shortExportDataVersion = 5 Then
            dateType = storedDateType.mixed
            entryDate = If(logEntry.unixTime = 0, If(chkConvertTimes.Checked, logEntry.dateObject.ToLocalTime, logEntry.dateObject), UNIXTimestampToDate(logEntry.unixTime))
            listViewItemObject.SubItems.Add(entryDate.ToString)
        End If

        With listViewItemObject
            .SubItems.Add(formatNumber(logEntry.logID))
            .SubItems.Add(If(logEntry.boolException, "Yes", "No"))
            .SubItems.Add(logEntry.logSource)
            .SubItems.Add("")
        End With

        Return listViewItemObject
    End Function

    Sub openFile(strFileName As String, boolShowMessageBox As Boolean)
        CopyPathToClipboardToolStripMenuItem.Visible = True
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
            Dim strLineInFile As String

            Using fileHandle As New IO.StreamReader(strFileName, Encoding.UTF8)
                strLineInFile = fileHandle.ReadLine

                lblProgramVersion.Text = ""
                lblProgramVersion.Visible = False

                lblOSVersion.Text = ""
                lblOSVersion.Visible = False

                While strLineInFile IsNot Nothing
                    If strLineInFile.StartsWith("//") Then
                        If strLineInFile.StartsWith("// program version: ", StringComparison.OrdinalIgnoreCase) Then
                            lblProgramVersion.Text = strLineInFile.Replace("// ", "")
                            lblProgramVersion.Visible = True
                        ElseIf strLineInFile.StartsWith("// operating system: ", StringComparison.OrdinalIgnoreCase) Then
                            lblOSVersion.Text = strLineInFile.Replace("// ", "")
                            lblOSVersion.Visible = True
                        ElseIf strLineInFile.StartsWith("// export data version: ", StringComparison.OrdinalIgnoreCase) Then
                            shortExportDataVersion = Short.Parse(Regex.Match(strLineInFile, "// Export Data Version: (\d{1,2})", RegexOptions.IgnoreCase).Groups(1).Value)
                        End If
                    Else
                        logEntry = jsonEngine.Deserialize(strLineInFile, GetType(restorePointCreatorExportedLog))

                        If Not chkProgramClosingAndOpeningEvents.Checked AndAlso (logEntry.logData.Contains("started the program") Or logEntry.logData.Contains("closed the program")) Then
                            Continue While
                        Else
                            eventLogContents.Add(processLogEntry(logEntry, dateType))
                        End If
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
                    ElseIf shortExportDataVersion = 5 Then
                        .Text = "Date Type: Mixed"
                    End If
                End With

                jsonEngine = Nothing
            End Using
        ElseIf fileInfo.Extension.Equals(".reslogx", StringComparison.OrdinalIgnoreCase) Then
            If exportedLogFile Is Nothing Then
                exportedLogFile = New exportedLogFile()

                Using streamReader As New IO.StreamReader(strFileName)
                    Dim xmlSerializerObject As New Xml.Serialization.XmlSerializer(exportedLogFile.GetType)
                    exportedLogFile = xmlSerializerObject.Deserialize(streamReader)
                End Using

                lblProgramVersion.Text = "Program Version: " & exportedLogFile.programVersion
                lblProgramVersion.Visible = True
                lblOSVersion.Text = "Operating System: " & exportedLogFile.operatingSystem
                lblOSVersion.Visible = True
                lblLogFileType.Text = "Log File Type: XML"

                shortExportDataVersion = exportedLogFile.version

                If exportedLogFile.version = 5 Then
                    If isTheLogSourceColumnInTheList() Then eventLogList.Columns.Remove(colLogSource)
                    lblDateType.Text = "Date Type: Mixed"
                Else
                    lblDateType.Text = "Date Type: UNIX Timestamp"
                End If
            End If

            For Each logEntry As restorePointCreatorExportedLog In exportedLogFile.logsEntries
                If Not chkProgramClosingAndOpeningEvents.Checked AndAlso (logEntry.logData.Contains("started the program") Or logEntry.logData.Contains("closed the program")) Then
                    Continue For
                Else
                    eventLogContents.Add(processLogEntry(logEntry, dateType))
                End If
            Next
        ElseIf fileInfo.Extension.Equals(".log", StringComparison.OrdinalIgnoreCase) Then
            lblProgramVersion.Visible = False
            lblOSVersion.Visible = False
            lblLogFileType.Visible = False
            lblDateType.Text = "Date Type: Windows Timestamp"

            Dim restorePointCreatorExportedLogObject As New List(Of restorePointCreatorExportedLog)

            Using streamReader As New IO.StreamReader(strFileName)
                Dim xmlSerializerObject As New Xml.Serialization.XmlSerializer(restorePointCreatorExportedLogObject.GetType)
                restorePointCreatorExportedLogObject = xmlSerializerObject.Deserialize(streamReader)
            End Using

            shortExportDataVersion = 5
            For Each logEntry As restorePointCreatorExportedLog In restorePointCreatorExportedLogObject
                If Not chkProgramClosingAndOpeningEvents.Checked AndAlso (logEntry.logData.Contains("started the program") Or logEntry.logData.Contains("closed the program")) Then
                    Continue For
                Else
                    eventLogContents.Add(processLogEntry(logEntry, dateType))
                End If
            Next
        End If

        With eventLogList
            .Items.Clear()
            .Items.AddRange(eventLogContents.ToArray())
            .Sort()
        End With

        lblCount.Text = "Number of Logs: " & eventLogList.Items.Count.ToString("n0", Globalization.CultureInfo.InvariantCulture)
        lblExportVersion.Text = "Data Export Version: " & shortExportDataVersion
        lblFileSize.Text = "File Size: " & bytesToHumanSize(New IO.FileInfo(strFileName).Length)

        timeStamp.Stop()
        lblProcessed.Text = String.Format("Loaded in {0}ms.", timeStamp.ElapsedMilliseconds)

        btnClear.Enabled = False
        btnSearch.Enabled = True
        eventLogList.Enabled = True

        If boolShowMessageBox Then MsgBox("Event Log Entry File Import Complete.", MsgBoxStyle.Information, Me.Text)
    End Sub

    Private Function isTheLogSourceColumnInTheList() As Boolean
        Return If(eventLogList.Columns.Cast(Of ColumnHeader).ToList.Where(Function(item As ColumnHeader) item.Text.Equals("log source", StringComparison.OrdinalIgnoreCase)).Count = 0, False, True)
    End Function

    Private Sub btnBrowseForFile_Click(sender As Object, e As EventArgs) Handles btnBrowseForFile.Click
        OpenFileDialog1.Title = "Open Restore Point Creator Exported Log File"
        OpenFileDialog1.FileName = Nothing
        OpenFileDialog1.Filter = "Both Log File Types|*.reslog;*.reslogx;*.log|Restore Point Creator Exported Log File|*.reslog;*.reslogx|Raw Application Log File|*.log"

        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            If Not isTheLogSourceColumnInTheList() Then eventLogList.Columns.Add(colLogSource)

            btnRawView.Enabled = True
            exportedLogFile = Nothing
            openFile(OpenFileDialog1.FileName, True)
        End If
    End Sub

    Public Function formatNumber(strInput As String) As String
        Dim longInput As Long
        Return If(Long.TryParse(strInput, longInput), longInput.ToString("n0", Globalization.CultureInfo.InvariantCulture), strInput)
    End Function

    Private Function UNIXTimestampToDate(ByVal strUnixTime As ULong) As Date
        Return DateAdd(DateInterval.Second, strUnixTime, New DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).ToLocalTime
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
        chkConvertTimes.Checked = My.Settings.boolConvertTimes

        applySavedSorting()

        colEventType.Width = My.Settings.eventLogColumn1Size
        colDateTime.Width = My.Settings.eventLogColumn2Size
        colEventID.Width = My.Settings.eventLogColumn3Size
        colLogSource.Width = My.Settings.eventLogColumn4Size
        colException.Width = My.Settings.eventLogColumn5Size

        chkProgramClosingAndOpeningEvents.Checked = My.Settings.boolIncludeOpeningAndClosingEvents

        If Registry.CurrentUser.OpenSubKey("SOFTWARE\Classes\.reslog") IsNot Nothing And Registry.CurrentUser.OpenSubKey("SOFTWARE\Classes\.reslogx") IsNot Nothing Then chkAssociate.Checked = True

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
            My.Settings.eventLogColumn1Size = colEventType.Width
            My.Settings.eventLogColumn2Size = colDateTime.Width
            My.Settings.eventLogColumn3Size = colEventID.Width
            My.Settings.eventLogColumn4Size = colLogSource.Width
            My.Settings.eventLogColumn5Size = colException.Width
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
        m_SortingColumn.Text = If(sort_order = SortOrder.Ascending, "> " & m_SortingColumn.Text, "< " & m_SortingColumn.Text)

        ' Create a comparer.
        eventLogList.ListViewItemSorter = New ListViewComparer(e.Column, sort_order)

        ' Sort.
        eventLogList.Sort()
    End Sub

    Sub applySavedSorting()
        ' Some data validation.
        If My.Settings.sortingColumn < 0 Or My.Settings.sortingColumn > 4 Then My.Settings.sortingColumn = 0
        If My.Settings.sortingOrder <> 1 And My.Settings.sortingOrder <> 2 Then My.Settings.sortingOrder = 2
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
        m_SortingColumn.Text = If(sort_order = SortOrder.Ascending, "> " & m_SortingColumn.Text, "< " & m_SortingColumn.Text)

        ' Create a comparer.
        eventLogList.ListViewItemSorter = New ListViewComparer(My.Settings.sortingColumn, sort_order)

        ' Sort.
        eventLogList.Sort()
    End Sub

    Public Function removeSourceCodePathInfo(strInput As String) As String
        If strInput.regExSearch("(?:Google Drive|OneDrive|AppData)") Then
            strInput = Regex.Replace(strInput, "C:\\Users\\Tom\\(?:Google Drive|OneDrive)\\My Visual Studio Projects\\Projects\\", "", RegexOptions.IgnoreCase)
            strInput = strInput.caseInsensitiveReplace("C:\Users\Tom\AppData\Local\Temp\", "")
            Return strInput
        Else
            Return strInput
        End If
    End Function

    Private Sub eventLogList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles eventLogList.SelectedIndexChanged
        Try
            If eventLogList.SelectedItems.Count <> 0 Then
                eventLogText.Text = DirectCast(eventLogList.SelectedItems(0), eventLogListEntry).eventLogText.Replace(vbLf, vbCrLf)
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
                btnClear.Enabled = True
                eventLogList.Sort()
                Dim strEntriesFound As String = If(eventLogList.Items.Count = 1, "1 log entry was found.", eventLogList.Items.Count.ToString & " log entries were found.")
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
                If Environment.GetCommandLineArgs.Count <> 1 Then startInfo.Arguments = Environment.GetCommandLineArgs(1)
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
            Return If(principal.IsInRole(WindowsBuiltInRole.Administrator), True, False)
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

        btnClear.Enabled = False
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
                openFile(strPassedCommandLine, True)
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
        If boolFileLoaded Then openFile(strLoadedFile, False)
    End Sub

    Private Sub CopyPathToClipboardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyPathToClipboardToolStripMenuItem.Click
        Try
            Clipboard.SetDataObject(strLoadedFile, True, 5, 200)
        Catch ex As Exception
            MsgBox("Unable to open Windows Clipboard to copy text to it.", MsgBoxStyle.Critical, Me.Text)
        End Try
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.windowSize = Me.Size
        My.Settings.windowState = Me.WindowState
    End Sub

    Private Sub chkConvertTimes_Click(sender As Object, e As EventArgs) Handles chkConvertTimes.Click
        My.Settings.boolConvertTimes = chkConvertTimes.Checked
        If boolFileLoaded Then openFile(strLoadedFile, False)
    End Sub
End Class