﻿Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions

Module Module1
    ''' <summary>This function operates a lot like Contains() but is case-InSeNsItIvE.</summary>
    ''' <param name="needle">The String containing what you want to search for.</param>
    ''' <return>Returns a Boolean value.</return>
    <Extension()>
    Public Function caseInsensitiveContains(haystack As String, needle As String) As Boolean
        Dim index As Integer = haystack.IndexOf(needle, StringComparison.OrdinalIgnoreCase)
        Return (index >= 0)
    End Function

    ''' <summary>This function operates a lot like Replace() but is case-InSeNsItIvE.</summary>
    ''' <param name="source">The source String, aka the String where the data will be replaced in.</param>
    ''' <param name="replace">What you want to replace in the String.</param>
    ''' <param name="replaceWith">What you want to replace with in the String.</param>
    ''' <param name="boolEscape">This is an optional parameter, the default is True. This parameter gives you far more control over how the function works. With this parameter set to True the function automatically properly escapes the "replace" parameter for use in the RegEx replace function that operates inside this function. If this parameter is set to False it is up to you, the programmer, to properly escape the value of the "replace" parameter or this function will throw an exception.</param>
    ''' <return>Returns a String value.</return>
    <Extension()>
    Public Function caseInsensitiveReplace(source As String, replace As String, replaceWith As String, Optional boolEscape As Boolean = True) As String
        If boolEscape Then replace = Regex.Escape(replace)
        Return Regex.Replace(source, replace, replaceWith, RegexOptions.IgnoreCase)
    End Function

    ''' <summary>This function performs RegEx search on a String. This function operates a lot like Contains().</summary>
    ''' <param name="needle">The String containing what you want to search for.</param>
    ''' <param name="boolDoEscaping">A Boolean value. Normally you would pass a correct RegEx pattern to this function so normally this value would be False.</param>
    ''' <return>Returns a Boolean value.</return>
    <Extension()>
    Public Function regExSearch(haystack As String, needle As String, Optional boolDoEscaping As Boolean = False) As Boolean
        Try
            If boolDoEscaping Then needle = Regex.Escape(needle)
            Return Regex.IsMatch(haystack, needle, RegexOptions.IgnoreCase)
        Catch ex As Exception
            Return False
        End Try
    End Function
End Module