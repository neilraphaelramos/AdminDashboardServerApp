Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.Web.WebView2.Core

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
End Module