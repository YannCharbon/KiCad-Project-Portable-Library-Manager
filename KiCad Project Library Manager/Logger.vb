Public Class Logger

    Dim callerInstance As Form1

    Public Sub New(ByVal form As Form1)
        callerInstance = form
    End Sub

    Public Sub Log(ByVal str As String)
        callerInstance.TextBoxLog.Text = callerInstance.TextBoxLog.Text & Now & "  -  " & str & vbCrLf
        callerInstance.TextBoxLog.SelectionStart = callerInstance.TextBoxLog.Text.Length
        callerInstance.TextBoxLog.ScrollToCaret()
    End Sub

End Class
