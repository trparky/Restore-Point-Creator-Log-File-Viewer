Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions

Module Module1
    ''' <summary>This function operates a lot like Contains() but is case-InSeNsItIvE.</summary>
    ''' <param name="needle">The String containing what you want to search for.</param>
    ''' <return>Returns a Boolean value.</return>
    <Extension()>
    Public Function caseInsensitiveContains(haystack As String, needle As String) As Boolean
        Dim index As Integer = haystack.IndexOf(needle, StringComparison.OrdinalIgnoreCase)
        Return (index >= 0)

        'Try
        '    If boolDoEscaping = True Then needle = Regex.Escape(needle)
        '    Return Regex.IsMatch(haystack, needle, RegexOptions.IgnoreCase)
        'Catch ex As Exception
        '    Return False
        'End Try
    End Function

    ''' <summary>This function performs RegEx search on a String. This function operates a lot like Contains().</summary>
    ''' <param name="needle">The String containing what you want to search for.</param>
    ''' <param name="boolDoEscaping">A Boolean value. Normally you would pass a correct RegEx pattern to this function so normally this value would be False.</param>
    ''' <return>Returns a Boolean value.</return>
    <Extension()>
    Public Function regExSearch(haystack As String, needle As String, Optional boolDoEscaping As Boolean = False) As Boolean
        Try
            If boolDoEscaping = True Then needle = Regex.Escape(needle)
            Return Regex.IsMatch(haystack, needle, RegexOptions.IgnoreCase)
        Catch ex As Exception
            Return False
        End Try
    End Function
End Module