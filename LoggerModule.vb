Imports System.IO
Imports System.Text

Module LoggerModule

    Private ReadOnly LogFolder As String =
        Path.Combine(Application.StartupPath, DataPaths.LogsFolder)

    Private ReadOnly LogFile As String =
        Path.Combine(DataPaths.LogsFolder, $"app_{DateTime.Now:yyyy-MM-dd}.log")

    Private ReadOnly SyncLockObj As New Object()

    Public Sub Info(message As String)
        Write("INFO", message)
    End Sub

    Public Sub Warn(message As String)
        Write("WARN", message)
    End Sub

    Public Sub [Error](message As String, Optional ex As Exception = Nothing)
        Dim full As String = message
        If ex IsNot Nothing Then
            full &= Environment.NewLine & ex.ToString()
        End If
        Write("ERROR", full)
    End Sub
    Public Sub Debug(message As String)
        If Not SettingsModule.DebugMode Then Return
        Write("DEBUG", message)
    End Sub

    Private Sub Write(level As String, message As String)
        If Not SettingsModule.EnableLogging Then Return

        Try
            If Not Directory.Exists(LogFolder) Then
                Directory.CreateDirectory(LogFolder)
            End If

            Dim line As String =
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] {message}"

            SyncLock SyncLockObj
                File.AppendAllText(LogFile, line & Environment.NewLine, Encoding.UTF8)
            End SyncLock

        Catch
            ' Logging must never crash the app
        End Try
    End Sub

    Public Sub OpenLogFolder()
        Try
            If Not Directory.Exists(LogFolder) Then
                Directory.CreateDirectory(LogFolder)
            End If
            Process.Start("explorer.exe", LogFolder)
        Catch
        End Try
    End Sub

End Module