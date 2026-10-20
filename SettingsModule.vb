Imports System.IO
Imports System.Xml.Linq

Module SettingsModule

    Private ReadOnly ConfigFolder As String =
        Path.Combine(Application.StartupPath, "Config")

    Private ReadOnly ConfigFile As String =
        Path.Combine(ConfigFolder, "settings.config")


    '========================================
    ' Settings
    '========================================

    Public Property Theme As String = "system"

    Public Property IsFullScreen As Boolean = False

    Public Property StartWithDefaultPage As Boolean = True

    Public Property OpenLastWebsite As Boolean = False

    Public Property EnableDevTools As Boolean = False

    Public Property EnableContextMenu As Boolean = False

    Public Property AllowInvalidCertificates As Boolean = True

    Public Property EnableLogging As Boolean = False

    Public Property DebugMode As Boolean = False

    Public Property AutoTheme As Boolean = False

    Public Property AutoThemeDarkHour As Integer = 18

    Public Property AutoThemeLightHour As Integer = 6


    '========================================
    ' Load Settings
    '========================================

    Public Sub LoadSettings()

        Try

            If Not File.Exists(ConfigFile) Then

                CreateDefaultConfig()
                Return

            End If


            Dim doc As XDocument =
                XDocument.Load(ConfigFile)

            Dim root As XElement =
                doc.Element("Settings")


            If root Is Nothing Then

                CreateDefaultConfig()
                Return

            End If


            '====================================
            ' General
            '====================================

            Dim general As XElement =
                root.Element("General")

            If general IsNot Nothing Then

                StartWithDefaultPage =
                    GetBoolean(
                        general,
                        "StartWithDefaultPage",
                        True
                    )

                OpenLastWebsite =
                    GetBoolean(
                        general,
                        "OpenLastWebsite",
                        False
                    )

                IsFullScreen =
                    GetBoolean(
                        general,
                        "IsFullScreen",
                        False
                    )

            End If


            '====================================
            ' Appearance
            '====================================

            Dim appearance As XElement =
                root.Element("Appearance")

            If appearance IsNot Nothing Then

                Theme =
                    GetString(
                        appearance,
                        "Theme",
                        "system"
                    )

                AutoTheme = GetBoolean(appearance, "AutoTheme", False)
                AutoThemeDarkHour = GetInteger(appearance, "AutoThemeDarkHour", 18)
                AutoThemeLightHour = GetInteger(appearance, "AutoThemeLightHour", 18)
            End If


            '====================================
            ' WebView
            '====================================

            Dim webView As XElement =
                root.Element("WebView")

            If webView IsNot Nothing Then

                EnableDevTools =
                    GetBoolean(
                        webView,
                        "EnableDevTools",
                        False
                    )

                EnableContextMenu =
                    GetBoolean(
                        webView,
                        "EnableContextMenu",
                        False
                    )

                AllowInvalidCertificates =
                    GetBoolean(
                        webView,
                        "AllowInvalidCertificates",
                        False
                    )

            End If


            '====================================
            ' Advanced
            '====================================

            Dim advanced As XElement =
                root.Element("Advanced")

            If advanced IsNot Nothing Then

                EnableLogging =
                    GetBoolean(
                        advanced,
                        "EnableLogging",
                        False
                    )

                DebugMode =
                    GetBoolean(
                        advanced,
                        "DebugMode",
                        False
                    )

            End If


        Catch ex As Exception

            MessageBox.Show(
                "Unable to load settings." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Settings Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

        End Try

    End Sub


    '========================================
    ' Save Settings
    '========================================

    Public Sub SaveSettings()

        Try

            If Not Directory.Exists(ConfigFolder) Then
                Directory.CreateDirectory(ConfigFolder)
            End If


            ' Root
            Dim settings As New XElement("Settings")


            ' General
            Dim general As New XElement("General")

            general.Add(
                New XElement(
                    "StartWithDefaultPage",
                    StartWithDefaultPage
                ),
                New XElement(
                    "OpenLastWebsite",
                    OpenLastWebsite
                ),
                New XElement(
                    "IsFullScreen",
                    IsFullScreen
                )
            )

            settings.Add(general)


            ' Appearance
            Dim appearance As New XElement("Appearance")

            appearance.Add(
                New XElement(
                    "Theme",
                    Theme
                ),
                New XElement("AutoTheme", AutoTheme),
                New XElement("AutoThemeDarkHour", AutoThemeDarkHour),
                New XElement("AutoThemeLightHour", AutoThemeLightHour)
            )

            settings.Add(appearance)


            ' WebView
            Dim webView As New XElement("WebView")

            webView.Add(
                New XElement(
                    "EnableDevTools",
                    EnableDevTools
                ),
                New XElement(
                    "EnableContextMenu",
                    EnableContextMenu
                ),
                New XElement(
                    "AllowInvalidCertificates",
                    AllowInvalidCertificates
                )
            )

            settings.Add(webView)


            ' Advanced
            Dim advanced As New XElement("Advanced")

            advanced.Add(
                New XElement(
                    "EnableLogging",
                    EnableLogging
                ),
                New XElement(
                    "DebugMode",
                    DebugMode
                )
            )

            settings.Add(advanced)


            ' XML document
            Dim doc As New XDocument(
                New XDeclaration(
                    "1.0",
                    "utf-8",
                    "yes"
                ),
                settings
            )


            ' Save
            doc.Save(ConfigFile)


        Catch ex As Exception

            MessageBox.Show(
                "Unable to save settings." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Settings Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '========================================
    ' Create Default Config
    '========================================

    Private Sub CreateDefaultConfig()

        Theme = "system"

        IsFullScreen = False

        StartWithDefaultPage = True

        OpenLastWebsite = False

        EnableDevTools = False

        EnableContextMenu = False

        AllowInvalidCertificates = True

        EnableLogging = False

        DebugMode = False

        AutoTheme = False

        AutoThemeDarkHour = 18

        AutoThemeLightHour = 6

        SaveSettings()

    End Sub


    '========================================
    ' Get String
    '========================================

    Private Function GetString(
        parent As XElement,
        name As String,
        defaultValue As String
    ) As String

        Dim element As XElement =
            parent.Element(name)

        If element Is Nothing Then
            Return defaultValue
        End If

        If String.IsNullOrWhiteSpace(element.Value) Then
            Return defaultValue
        End If

        Return element.Value

    End Function


    '========================================
    ' Get Boolean
    '========================================

    Private Function GetBoolean(
        parent As XElement,
        name As String,
        defaultValue As Boolean
    ) As Boolean

        Dim element As XElement =
            parent.Element(name)

        If element Is Nothing Then
            Return defaultValue
        End If

        Dim result As Boolean

        If Boolean.TryParse(
            element.Value,
            result
        ) Then

            Return result

        End If

        Return defaultValue

    End Function

    Private Function GetInteger(parent As XElement, name As String, defaultValue As Integer) As Integer
        Dim element As XElement = parent.Element(name)
        If element Is Nothing Then Return defaultValue
        Dim result As Integer
        If Integer.TryParse(element.Value, result) Then Return result
        Return defaultValue
    End Function

    Public Function IsAutoThemeDarkNow() As Boolean
        Dim hour As Integer = DateTime.Now.Hour
        ' Example: Dark from 18:00 to 05:59, Light from 06:00 to 17:59
        If AutoThemeDarkHour > AutoThemeLightHour Then
            Return hour >= AutoThemeDarkHour OrElse hour < AutoThemeLightHour
        Else
            Return hour >= AutoThemeDarkHour AndAlso hour < AutoThemeLightHour
        End If
    End Function


    '========================================
    ' Reset Settings
    '========================================

    Public Sub ResetSettingsConfig()
        Try
            Theme = "system"
            IsFullScreen = False
            StartWithDefaultPage = True
            OpenLastWebsite = False
            EnableDevTools = False
            EnableContextMenu = False
            AllowInvalidCertificates = True
            EnableLogging = False
            DebugMode = False
            AutoTheme = False
            AutoThemeDarkHour = 18
            AutoThemeLightHour = 6

            If File.Exists(ConfigFile) Then
                File.Delete(ConfigFile)
            End If



            CreateDefaultConfig()
        Catch ex As Exception
            MessageBox.Show(
                "Unable to reset settings." & Environment.NewLine & Environment.NewLine & ex.Message,
                "Reset Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)
        End Try
    End Sub
End Module