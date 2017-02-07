' This class extends the ListViewItem so that I can add more properties to it for my purposes.
Public Class eventLogListEntry
    Inherits ListViewItem
    Private longEventLogEntryID As Long, strEventLogSource, strEventLogText, _strLevel As String, levelType As Short

    Public Sub New(strInput As String)
        Me.Text = strInput
        Me.strEventLogLevel = strInput
    End Sub

    Public Property strEventLogLevel() As String
        Get
            Return _strLevel
        End Get
        Set(value As String)
            _strLevel = value
        End Set
    End Property

    Public Property eventLogText() As String
        Get
            Return strEventLogText
        End Get
        Set(value As String)
            strEventLogText = value
        End Set
    End Property

    Public Property eventLogEntryID() As Long
        Get
            Return longEventLogEntryID
        End Get
        Set(value As Long)
            longEventLogEntryID = value
        End Set
    End Property

    Public Property eventLogSource() As String
        Get
            Return strEventLogSource
        End Get
        Set(value As String)
            strEventLogSource = value
        End Set
    End Property

    Public Property eventLogLevel() As Short
        Get
            Return levelType
        End Get
        Set(value As Short)
            levelType = value
        End Set
    End Property
End Class