<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.btnBrowseForFile = New System.Windows.Forms.Button()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.eventLogList = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.imageList = New System.Windows.Forms.ImageList(Me.components)
        Me.eventLogText = New System.Windows.Forms.TextBox()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.lblFileName = New System.Windows.Forms.Label()
        Me.chkAssociate = New System.Windows.Forms.CheckBox()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.lblCount = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblDateType = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblProcessed = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblProgramVersion = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblOSVersion = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblExportVersion = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblLogFileType = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblFileSize = New System.Windows.Forms.ToolStripStatusLabel()
        Me.chkProgramClosingAndOpeningEvents = New System.Windows.Forms.CheckBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnRawView = New System.Windows.Forms.Button()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnBrowseForFile
        '
        Me.btnBrowseForFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseForFile.Location = New System.Drawing.Point(3, 3)
        Me.btnBrowseForFile.Name = "btnBrowseForFile"
        Me.btnBrowseForFile.Size = New System.Drawing.Size(658, 23)
        Me.btnBrowseForFile.TabIndex = 2
        Me.btnBrowseForFile.Text = "Load Exported Log File"
        Me.btnBrowseForFile.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(12, 61)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.TableLayoutPanel2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.eventLogList)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.eventLogText)
        Me.SplitContainer1.Size = New System.Drawing.Size(830, 338)
        Me.SplitContainer1.SplitterDistance = 415
        Me.SplitContainer1.TabIndex = 6
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.btnSearch, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.btnClear, 1, 0)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 304)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(416, 29)
        Me.TableLayoutPanel2.TabIndex = 1
        '
        'btnSearch
        '
        Me.btnSearch.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSearch.Enabled = False
        Me.btnSearch.Location = New System.Drawing.Point(3, 3)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(202, 23)
        Me.btnSearch.TabIndex = 8
        Me.btnSearch.Text = "Search Event Log"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClear.Enabled = False
        Me.btnClear.Location = New System.Drawing.Point(211, 3)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(202, 23)
        Me.btnClear.TabIndex = 9
        Me.btnClear.Text = "Clear Search Results"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'eventLogList
        '
        Me.eventLogList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.eventLogList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4})
        Me.eventLogList.Enabled = False
        Me.eventLogList.FullRowSelect = True
        Me.eventLogList.Location = New System.Drawing.Point(3, 3)
        Me.eventLogList.MultiSelect = False
        Me.eventLogList.Name = "eventLogList"
        Me.eventLogList.Size = New System.Drawing.Size(407, 295)
        Me.eventLogList.SmallImageList = Me.imageList
        Me.eventLogList.TabIndex = 0
        Me.eventLogList.UseCompatibleStateImageBehavior = False
        Me.eventLogList.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Event Type"
        Me.ColumnHeader1.Width = 100
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Date & Time"
        Me.ColumnHeader2.Width = 100
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Event ID"
        Me.ColumnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Log Source"
        Me.ColumnHeader4.Width = 95
        '
        'imageList
        '
        Me.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.imageList.ImageSize = New System.Drawing.Size(16, 16)
        Me.imageList.TransparentColor = System.Drawing.Color.Transparent
        '
        'eventLogText
        '
        Me.eventLogText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.eventLogText.BackColor = System.Drawing.SystemColors.Window
        Me.eventLogText.Location = New System.Drawing.Point(3, 3)
        Me.eventLogText.Multiline = True
        Me.eventLogText.Name = "eventLogText"
        Me.eventLogText.ReadOnly = True
        Me.eventLogText.Size = New System.Drawing.Size(402, 331)
        Me.eventLogText.TabIndex = 1
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'lblFileName
        '
        Me.lblFileName.AutoSize = True
        Me.lblFileName.Location = New System.Drawing.Point(12, 45)
        Me.lblFileName.Name = "lblFileName"
        Me.lblFileName.Size = New System.Drawing.Size(80, 13)
        Me.lblFileName.TabIndex = 7
        Me.lblFileName.Text = "Log File: (none)"
        '
        'chkAssociate
        '
        Me.chkAssociate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkAssociate.AutoSize = True
        Me.chkAssociate.Location = New System.Drawing.Point(644, 44)
        Me.chkAssociate.Name = "chkAssociate"
        Me.chkAssociate.Size = New System.Drawing.Size(198, 17)
        Me.chkAssociate.TabIndex = 8
        Me.chkAssociate.Text = "Associate file types with this program"
        Me.chkAssociate.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblCount, Me.lblDateType, Me.lblProcessed, Me.lblProgramVersion, Me.lblOSVersion, Me.lblExportVersion, Me.lblLogFileType, Me.lblFileSize})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 402)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(854, 22)
        Me.StatusStrip1.TabIndex = 9
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'lblCount
        '
        Me.lblCount.Name = "lblCount"
        Me.lblCount.Size = New System.Drawing.Size(0, 17)
        '
        'lblDateType
        '
        Me.lblDateType.Margin = New System.Windows.Forms.Padding(30, 3, 0, 2)
        Me.lblDateType.Name = "lblDateType"
        Me.lblDateType.Size = New System.Drawing.Size(0, 17)
        '
        'lblProcessed
        '
        Me.lblProcessed.Margin = New System.Windows.Forms.Padding(30, 3, 0, 2)
        Me.lblProcessed.Name = "lblProcessed"
        Me.lblProcessed.Size = New System.Drawing.Size(0, 17)
        '
        'lblProgramVersion
        '
        Me.lblProgramVersion.Margin = New System.Windows.Forms.Padding(30, 3, 0, 2)
        Me.lblProgramVersion.Name = "lblProgramVersion"
        Me.lblProgramVersion.Size = New System.Drawing.Size(0, 17)
        '
        'lblOSVersion
        '
        Me.lblOSVersion.Margin = New System.Windows.Forms.Padding(30, 3, 0, 2)
        Me.lblOSVersion.Name = "lblOSVersion"
        Me.lblOSVersion.Size = New System.Drawing.Size(0, 17)
        '
        'lblExportVersion
        '
        Me.lblExportVersion.Margin = New System.Windows.Forms.Padding(30, 3, 0, 2)
        Me.lblExportVersion.Name = "lblExportVersion"
        Me.lblExportVersion.Size = New System.Drawing.Size(0, 17)
        '
        'lblLogFileType
        '
        Me.lblLogFileType.Margin = New System.Windows.Forms.Padding(30, 3, 0, 2)
        Me.lblLogFileType.Name = "lblLogFileType"
        Me.lblLogFileType.Size = New System.Drawing.Size(0, 17)
        '
        'lblFileSize
        '
        Me.lblFileSize.Margin = New System.Windows.Forms.Padding(30, 3, 0, 2)
        Me.lblFileSize.Name = "lblFileSize"
        Me.lblFileSize.Size = New System.Drawing.Size(0, 17)
        '
        'chkProgramClosingAndOpeningEvents
        '
        Me.chkProgramClosingAndOpeningEvents.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkProgramClosingAndOpeningEvents.AutoSize = True
        Me.chkProgramClosingAndOpeningEvents.Location = New System.Drawing.Point(403, 44)
        Me.chkProgramClosingAndOpeningEvents.Name = "chkProgramClosingAndOpeningEvents"
        Me.chkProgramClosingAndOpeningEvents.Size = New System.Drawing.Size(235, 17)
        Me.chkProgramClosingAndOpeningEvents.TabIndex = 10
        Me.chkProgramClosingAndOpeningEvents.Text = "Include program opening and closing events"
        Me.chkProgramClosingAndOpeningEvents.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnBrowseForFile, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnRawView, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 12)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(830, 30)
        Me.TableLayoutPanel1.TabIndex = 11
        '
        'btnRawView
        '
        Me.btnRawView.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRawView.Enabled = False
        Me.btnRawView.Location = New System.Drawing.Point(667, 3)
        Me.btnRawView.Name = "btnRawView"
        Me.btnRawView.Size = New System.Drawing.Size(160, 23)
        Me.btnRawView.TabIndex = 3
        Me.btnRawView.Text = "Raw View"
        Me.btnRawView.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(854, 424)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.chkProgramClosingAndOpeningEvents)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.chkAssociate)
        Me.Controls.Add(Me.lblFileName)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Icon = Global.Restore_Point_Creator_Exported_Log_Viewer.My.Resources.Resources.RestorePoint_noBackground_2
        Me.MinimumSize = New System.Drawing.Size(870, 386)
        Me.Name = "Form1"
        Me.Text = "Restore Point Creator Exported Log Viewer"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnBrowseForFile As Button
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents eventLogText As TextBox
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents imageList As ImageList
    Friend WithEvents eventLogList As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents ColumnHeader4 As ColumnHeader
    Friend WithEvents btnSearch As Button
    Friend WithEvents btnClear As Button
    Friend WithEvents lblFileName As Label
    Friend WithEvents chkAssociate As CheckBox
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents lblCount As ToolStripStatusLabel
    Friend WithEvents lblDateType As ToolStripStatusLabel
    Friend WithEvents lblProcessed As ToolStripStatusLabel
    Friend WithEvents lblProgramVersion As ToolStripStatusLabel
    Friend WithEvents lblOSVersion As ToolStripStatusLabel
    Friend WithEvents lblExportVersion As ToolStripStatusLabel
    Friend WithEvents chkProgramClosingAndOpeningEvents As CheckBox
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents btnRawView As Button
    Friend WithEvents lblLogFileType As ToolStripStatusLabel
    Friend WithEvents lblFileSize As ToolStripStatusLabel
End Class
