Imports System.Windows.Forms

Module DebugModule

    Public Sub Show(message As String, Optional title As String = "Debug")
        If Not SettingsModule.DebugMode Then Return

        MessageBox.Show(
            message,
            title,
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)
    End Sub

    Public Sub Trace(message As String, Optional showMessageBox As Boolean = False)
        If Not SettingsModule.DebugMode Then Return

        LoggerModule.Debug(message)

        If showMessageBox Then
            Show(message)
        End If
    End Sub

    Public Sub Time(label As String, action As Action)
        If Not SettingsModule.DebugMode Then
            action()
            Return
        End If

        Dim sw As New Stopwatch()
        sw.Start()
        Try
            action()
        Finally
            sw.Stop()
            LoggerModule.Debug($"{label} took {sw.ElapsedMilliseconds} ms")
        End Try
    End Sub

    Public Function IsEnabled() As Boolean
        Return SettingsModule.DebugMode
    End Function

End Module