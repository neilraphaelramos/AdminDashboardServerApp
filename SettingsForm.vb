Imports System.Runtime

Public Class SettingsForm

    Private Sub SettingsForm_Load(
        sender As Object,
        e As EventArgs
    ) Handles MyBase.Load

        '========================================
        ' THEME COMBOBOX
        '========================================
        CMBTheme.Items.Clear()
        CMBTheme.Items.Add("System")
        CMBTheme.Items.Add("Light")
        CMBTheme.Items.Add("Dark")

        '========================================
        ' LOAD SETTINGS
        '========================================
        SettingsModule.LoadSettings()
        GetWebsites()

        CHKStartDefault.Checked = SettingsModule.StartWithDefaultPage
        CHKOpenLast.Checked = SettingsModule.OpenLastWebsite
        CHKFullScreen.Checked = SettingsModule.IsFullScreen

        Select Case SettingsModule.Theme.ToLower()
            Case "light"
                CMBTheme.SelectedItem = "Light"
            Case "dark"
                CMBTheme.SelectedItem = "Dark"
            Case Else
                CMBTheme.SelectedItem = "System"
        End Select

        CHKDevTools.Checked = SettingsModule.EnableDevTools
        CHKContextMenu.Checked = SettingsModule.EnableContextMenu
        CHKInvalidCertificates.Checked = SettingsModule.AllowInvalidCertificates
        CHKLogging.Checked = SettingsModule.EnableLogging
        CHKDebug.Checked = SettingsModule.DebugMode

        LoadWebsiteList()

        '========================================
        ' APPLY CURRENT THEME TO SETTINGS FORM
        '========================================
        ApplySettingsTheme()
    End Sub

    '========================================================
    ' THEME
    '========================================================
    Private Sub ApplySettingsTheme()
        ApplyWinFormsTheme(Me)

        If CTCSettings IsNot Nothing Then
            CTCSettings.ApplyTheme(IsCurrentDarkTheme())
        End If
    End Sub

    '========================================================
    ' DETERMINE CURRENT THEME
    '========================================================
    Private Function IsCurrentDarkTheme() As Boolean
        Select Case SettingsModule.Theme.ToLower()
            Case "dark"
                Return True
            Case "light"
                Return False
            Case Else
                Return IsWindowsDarkMode()
        End Select
    End Function

    '========================================================
    ' WINDOWS SYSTEM DARK MODE
    '========================================================
    Private Function IsWindowsDarkMode() As Boolean
        Try
            Using key As Microsoft.Win32.RegistryKey =
                Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                    "Software\Microsoft\Windows\CurrentVersion\Themes\Personalize")

                If key Is Nothing Then Return False

                Dim value As Object = key.GetValue("AppsUseLightTheme")
                If value Is Nothing Then Return False

                Return Convert.ToInt32(value) = 0
            End Using
        Catch
            Return False
        End Try
    End Function

    '========================================================
    ' APPLY BUTTON
    '========================================================
    Private Sub BtnApply_Click(sender As Object, e As EventArgs) Handles BtnApply.Click
        ApplySettings()
    End Sub

    '========================================================
    ' CLOSE BUTTON
    '========================================================
    Private Sub BtnOK_Click(sender As Object, e As EventArgs) Handles BtnOK.Click
        Me.Close()
    End Sub

    '========================================================
    ' APPLY ALL SETTINGS
    '========================================================
    Private Sub ApplySettings()
        '------------------------------------------
        ' General
        '------------------------------------------
        SettingsModule.StartWithDefaultPage = CHKStartDefault.Checked
        SettingsModule.OpenLastWebsite = CHKOpenLast.Checked
        SettingsModule.IsFullScreen = CHKFullScreen.Checked

        '------------------------------------------
        ' Appearance
        '------------------------------------------
        If CMBTheme.SelectedItem IsNot Nothing Then
            SettingsModule.Theme = CMBTheme.SelectedItem.ToString().ToLower()
        End If

        '------------------------------------------
        ' WebView
        '------------------------------------------
        SettingsModule.EnableDevTools = CHKDevTools.Checked
        SettingsModule.EnableContextMenu = CHKContextMenu.Checked
        SettingsModule.AllowInvalidCertificates = CHKInvalidCertificates.Checked

        '------------------------------------------
        ' Advanced
        '------------------------------------------
        SettingsModule.EnableLogging = CHKLogging.Checked
        SettingsModule.DebugMode = CHKDebug.Checked

        '------------------------------------------
        ' SAVE TO FILE
        '------------------------------------------
        SettingsModule.SaveSettings()

        '------------------------------------------
        ' APPLY TO MAIN DASHBOARD
        '------------------------------------------
        Dim dashboard As maindashboard =
            TryCast(Application.OpenForms("maindashboard"), maindashboard)

        If dashboard IsNot Nothing Then
            dashboard.ApplyApplicationTheme()
        End If

        '------------------------------------------
        ' APPLY TO THIS SETTINGS WINDOW
        '------------------------------------------
        ApplySettingsTheme()

        MessageBox.Show(
            "Settings applied successfully.",
            "Settings",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)
    End Sub

    '========================================================
    ' WEBSITE LIST
    '========================================================
    Private Sub LoadWebsiteList()
        LVWebsites.Items.Clear()

        Dim websites As List(Of WebsiteItem) = GetWebsites()

        For Each website As WebsiteItem In websites
            Dim item As New ListViewItem(website.name)
            item.SubItems.Add(website.url)
            item.Tag = website
            LVWebsites.Items.Add(item)
        Next
    End Sub

End Class