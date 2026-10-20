Imports System.IO

Module DataPaths
    Public ReadOnly Property DataRoot As String =
        Path.Combine(Application.StartupPath, "Data")

    Public ReadOnly Property ConfigFolder As String =
        Path.Combine(DataRoot, "Config")

    Public ReadOnly Property ConfigFile As String =
        Path.Combine(ConfigFolder, "settings.config")

    Public ReadOnly Property ServerFile As String =
        Path.Combine(ConfigFolder, "server.json")

    Public ReadOnly Property ContentFolder As String =
        Path.Combine(DataRoot, "Content")

    Public ReadOnly Property DefaultWebFolder As String =
        Path.Combine(ContentFolder, "Default Web")

    Public ReadOnly Property ErrorFolder As String =
        Path.Combine(ContentFolder, "Error")

    Public ReadOnly Property WarningFolder As String =
        Path.Combine(ContentFolder, "Warning")

    Public ReadOnly Property LogsFolder As String =
        Path.Combine(DataRoot, "Logs")

    Public Sub EnsureDataFolders()
        Directory.CreateDirectory(ConfigFolder)
        Directory.CreateDirectory(DefaultWebFolder)
        Directory.CreateDirectory(ErrorFolder)
        Directory.CreateDirectory(WarningFolder)
        Directory.CreateDirectory(LogsFolder)
    End Sub
End Module
