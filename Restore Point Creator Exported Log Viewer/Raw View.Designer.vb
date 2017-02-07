<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Raw_View
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
        Me.lblFileName = New System.Windows.Forms.Label()
        Me.txtRawFileView = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'lblFileName
        '
        Me.lblFileName.AutoSize = True
        Me.lblFileName.Location = New System.Drawing.Point(12, 9)
        Me.lblFileName.Name = "lblFileName"
        Me.lblFileName.Size = New System.Drawing.Size(80, 13)
        Me.lblFileName.TabIndex = 8
        Me.lblFileName.Text = "Log File: (none)"
        '
        'txtRawFileView
        '
        Me.txtRawFileView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRawFileView.Location = New System.Drawing.Point(15, 25)
        Me.txtRawFileView.Multiline = True
        Me.txtRawFileView.Name = "txtRawFileView"
        Me.txtRawFileView.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtRawFileView.Size = New System.Drawing.Size(705, 397)
        Me.txtRawFileView.TabIndex = 9
        '
        'Raw_View
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(732, 434)
        Me.Controls.Add(Me.txtRawFileView)
        Me.Controls.Add(Me.lblFileName)
        Me.Icon = Global.Restore_Point_Creator_Exported_Log_Viewer.My.Resources.Resources.RestorePoint_noBackground_2
        Me.MinimumSize = New System.Drawing.Size(748, 473)
        Me.Name = "Raw_View"
        Me.Text = "Raw View"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblFileName As Label
    Friend WithEvents txtRawFileView As TextBox
End Class
