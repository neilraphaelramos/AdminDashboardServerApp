Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.Web.WebView2.Core
Imports System.Linq

Module DefaultWebModule

    'Theme: light dark system
    Private CurrentTheme As String = "system"

    Public Function GetDefaultWebPath() As String
        Return Path.Combine(
            Application.StartupPath, "Default Web", "index.html"
        )
    End Function

    Public Function DefaultWebExists() As Boolean
        Return File.Exists(GetDefaultWebPath())
    End Function

    Public Sub SetTheme(theme As String)
        Select Case theme.ToLower()
            Case "light"
                CurrentTheme = "light"
            Case "dark"
                CurrentTheme = "dark"
            Case Else
                CurrentTheme = "system"
        End Select
    End Sub

    Public Function GetTheme() As String
        Return CurrentTheme
    End Function

    Public Sub ShowDefaultWeb(webView As CoreWebView2)

        If webView Is Nothing Then
            Return
        End If

        Dim htmlPath As String = GetDefaultWebPath()

        If Not File.Exists(htmlPath) Then

            MessageBox.Show(
                "Default web page was not found:" &
                Environment.NewLine &
                htmlPath,
                "Default Web",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return

        End If

        Dim fileUri As New Uri(htmlPath)

        webView.Navigate(fileUri.AbsoluteUri)

    End Sub


    Public Async Function ApplyTheme(webView As CoreWebView2) As Task
        If webView Is Nothing Then
            Return
        End If

        Dim theme As String = CurrentTheme.ToLower()

        Dim script As String = $"document.documentElement.setAttribute('data-theme','{theme}');"

        Await webView.ExecuteScriptAsync(script)
    End Function

    Public Sub ApplyWinFormsTheme(form As Form)

        If form Is Nothing Then Return

        Dim isDark As Boolean = IsDarkTheme()

        Dim backgroundColor As Color
        Dim foregroundColor As Color
        Dim controlColor As Color

        If isDark Then

            backgroundColor = Color.FromArgb(32, 32, 32)
            foregroundColor = Color.White
            controlColor = Color.FromArgb(45, 45, 45)

        Else

            backgroundColor = SystemColors.Control
            foregroundColor = SystemColors.ControlText
            controlColor = SystemColors.Window

        End If


        '========================================
        ' FORM
        '========================================

        form.BackColor = backgroundColor
        form.ForeColor = foregroundColor


        '========================================
        ' CONTROLS
        '========================================

        ApplyControlTheme(
        form.Controls,
        isDark,
        backgroundColor,
        foregroundColor,
        controlColor
    )


        '========================================
        ' MENUSTRIP
        '========================================

        Dim menuStrip As MenuStrip =
        FindMenuStrip(form)

        If menuStrip IsNot Nothing Then

            If isDark Then

                menuStrip.Renderer =
                New DarkMenuRenderer()

                menuStrip.BackColor =
                Color.FromArgb(32, 32, 32)

                menuStrip.ForeColor =
                Color.White

            Else

                menuStrip.Renderer =
                New ToolStripProfessionalRenderer()

                menuStrip.BackColor =
                SystemColors.Control

                menuStrip.ForeColor =
                SystemColors.ControlText

            End If

            menuStrip.Refresh()

        End If

    End Sub

    Private Function FindMenuStrip(
    parent As Control
) As MenuStrip

        For Each control As Control In parent.Controls

            If TypeOf control Is MenuStrip Then

                Dim menu As MenuStrip =
                DirectCast(control, MenuStrip)

                If menu.Name = "MSMain" Then
                    Return menu
                End If

            End If


            If control.Controls.Count > 0 Then

                Dim result As MenuStrip =
                FindMenuStrip(control)

                If result IsNot Nothing Then
                    Return result
                End If

            End If

        Next

        Return Nothing

    End Function

    Private Sub ApplyToolStripItemTheme(
    item As ToolStripItem,
    isDark As Boolean)

        If isDark Then

            item.BackColor =
                Color.FromArgb(32, 32, 32)

            item.ForeColor =
                Color.White

        Else

            item.BackColor =
                SystemColors.Control

            item.ForeColor =
                SystemColors.ControlText

        End If


        Dim menuItem As ToolStripMenuItem =
            TryCast(item, ToolStripMenuItem)

        If menuItem IsNot Nothing Then

            For Each child As ToolStripItem In
                menuItem.DropDownItems

                ApplyToolStripItemTheme(
                    child,
                    isDark
                )

            Next

        End If

    End Sub


    Private Sub ApplyControlTheme(
    controls As Control.ControlCollection,
    isDark As Boolean,
    backgroundColor As Color,
    foregroundColor As Color,
    controlColor As Color
)

        For Each control As Control In controls

            '========================================
            ' DEFAULT
            '========================================

            control.ForeColor = foregroundColor


            '========================================
            ' PANEL
            '========================================

            If TypeOf control Is Panel Then

                control.BackColor = backgroundColor


                '========================================
                ' TAB CONTROL
                '========================================

            ElseIf TypeOf control Is TabControl Then

                Dim tabControl As TabControl =
                DirectCast(control, TabControl)

                tabControl.BackColor =
                backgroundColor

                tabControl.ForeColor =
                foregroundColor


                '------------------------------------
                ' TAB PAGES
                '------------------------------------

                For Each page As TabPage In tabControl.TabPages

                    page.BackColor =
                    backgroundColor

                    page.ForeColor =
                    foregroundColor

                    ' Recursively theme controls
                    ApplyControlTheme(
                    page.Controls,
                    isDark,
                    backgroundColor,
                    foregroundColor,
                    controlColor
                )

                Next


                '========================================
                ' BUTTON
                '========================================

            ElseIf TypeOf control Is Button Then

                Dim button As Button =
                DirectCast(control, Button)

                ' IMPORTANT:
                ' Don't leave the button transparent.
                button.UseVisualStyleBackColor = False

                If isDark Then

                    button.BackColor =
                    Color.FromArgb(55, 55, 55)

                    button.ForeColor =
                    Color.White

                Else

                    button.BackColor =
                    SystemColors.Control

                    button.ForeColor =
                    SystemColors.ControlText

                End If


                '========================================
                ' CHECKBOX
                '========================================

            ElseIf TypeOf control Is CheckBox Then

                Dim checkBox As CheckBox =
                DirectCast(control, CheckBox)

                checkBox.BackColor =
                backgroundColor

                checkBox.ForeColor =
                foregroundColor


                '========================================
                ' RADIO BUTTON
                '========================================

            ElseIf TypeOf control Is RadioButton Then

                Dim radioButton As RadioButton =
                DirectCast(control, RadioButton)

                radioButton.BackColor =
                backgroundColor

                radioButton.ForeColor =
                foregroundColor


                '========================================
                ' LABEL
                '========================================

            ElseIf TypeOf control Is Label Then

                control.BackColor =
                backgroundColor

                control.ForeColor =
                foregroundColor


                '========================================
                ' TEXTBOX
                '========================================

            ElseIf TypeOf control Is TextBox Then

                Dim textBox As TextBox =
                DirectCast(control, TextBox)

                If isDark Then

                    textBox.BackColor =
                    Color.FromArgb(45, 45, 45)

                    textBox.ForeColor =
                    Color.White

                Else

                    textBox.BackColor =
                    SystemColors.Window

                    textBox.ForeColor =
                    SystemColors.WindowText

                End If


                '========================================
                ' COMBOBOX
                '========================================

            ElseIf TypeOf control Is ComboBox Then

                Dim comboBox As ComboBox =
                DirectCast(control, ComboBox)

                If isDark Then

                    comboBox.BackColor =
                    Color.FromArgb(45, 45, 45)

                    comboBox.ForeColor =
                    Color.White

                Else

                    comboBox.BackColor =
                    SystemColors.Window

                    comboBox.ForeColor =
                    SystemColors.WindowText

                End If


                '========================================
                ' LISTVIEW
                '========================================

            ElseIf TypeOf control Is ListView Then

                Dim listView As ListView =
                DirectCast(control, ListView)

                If isDark Then

                    listView.BackColor =
                    Color.FromArgb(45, 45, 45)

                    listView.ForeColor =
                    Color.White

                Else

                    listView.BackColor =
                    SystemColors.Window

                    listView.ForeColor =
                    SystemColors.WindowText

                End If


                '========================================
                ' GROUPBOX
                '========================================

            ElseIf TypeOf control Is GroupBox Then

                Dim groupBox As GroupBox =
                DirectCast(control, GroupBox)

                groupBox.BackColor =
                backgroundColor

                groupBox.ForeColor =
                foregroundColor


                '========================================
                ' OTHER CONTROLS
                '========================================

            Else

                ' Don't leave normal controls transparent
                ' unless they specifically need transparency.

                If control.BackColor = Color.Transparent Then
                    control.BackColor = backgroundColor
                End If

            End If


            '========================================
            ' RECURSIVE CHILD CONTROLS
            '========================================

            If control.Controls.Count > 0 AndAlso
           Not TypeOf control Is TabControl Then

                ApplyControlTheme(
                control.Controls,
                isDark,
                backgroundColor,
                foregroundColor,
                controlColor
            )

            End If

        Next

    End Sub


    Private Function IsDarkTheme() As Boolean

        Select Case SettingsModule.Theme.ToLower()

            Case "dark"
                Return True

            Case "light"
                Return False

            Case Else
                ' System theme
                Return IsWindowsDarkMode()

        End Select

    End Function


    Private Function IsWindowsDarkMode() As Boolean

        Try

            Using key As Microsoft.Win32.RegistryKey =
                Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                    "Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"
                )

                If key Is Nothing Then
                    Return False
                End If

                Dim value As Object =
                    key.GetValue("AppsUseLightTheme")

                If value Is Nothing Then
                    Return False
                End If

                Return Convert.ToInt32(value) = 0

            End Using

        Catch

            Return False

        End Try

    End Function
End Module