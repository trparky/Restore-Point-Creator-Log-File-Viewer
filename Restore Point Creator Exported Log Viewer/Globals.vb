Module Globals
    Public rawViewInstance As Raw_View

    ''' <summary>Converts the number of Bytes to a nice way of saying it, like MBs, GBs, etc.</summary>
    ''' <param name="size">The amount of data in Bytes.</param>
    ''' <param name="roundToNearestWholeNumber">Tells the function if it should round the number to the nearest whole number.</param>
    ''' <returns>A String value such as 100 MBs or 100 GBs.</returns>
    Public Function bytesToHumanSize(ByVal size As ULong, Optional roundToNearestWholeNumber As Boolean = False) As String
        Dim result As String

        If size <= (2 ^ 10) Then
            result = size & " Bytes"
        ElseIf size > (2 ^ 10) And size <= (2 ^ 20) Then
            If roundToNearestWholeNumber = True Then
                result = Math.Round(size / (2 ^ 10), 0) & " KBs"
            Else
                result = Math.Round(size / (2 ^ 10), 2) & " KBs"
            End If
        ElseIf size > (2 ^ 20) And size <= (2 ^ 30) Then
            If roundToNearestWholeNumber = True Then
                result = Math.Round(size / (2 ^ 20), 0) & " MBs"
            Else
                result = Math.Round(size / (2 ^ 20), 2) & " MBs"
            End If
        ElseIf size > (2 ^ 30) And size <= (2 ^ 40) Then
            If roundToNearestWholeNumber = True Then
                result = Math.Round(size / (2 ^ 30), 0) & " GBs"
            Else
                result = Math.Round(size / (2 ^ 30), 2) & " GBs"
            End If
        ElseIf size > (2 ^ 40) And size <= (2 ^ 50) Then
            If roundToNearestWholeNumber = True Then
                result = Math.Round(size / (2 ^ 40), 0) & " TBs"
            Else
                result = Math.Round(size / (2 ^ 40), 2) & " TBs"
            End If
        ElseIf size > (2 ^ 50) And size <= (2 ^ 60) Then
            If roundToNearestWholeNumber = True Then
                result = Math.Round(size / (2 ^ 50), 0) & " PBs"
            Else
                result = Math.Round(size / (2 ^ 50), 2) & " PBs"
            End If
        Else
            result = "(None)"
        End If

        Return result
    End Function
End Module