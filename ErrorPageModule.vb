Imports System.IO
Imports Microsoft.Web.WebView2.Core

Module ErrorPageModule

	Private ReadOnly ErrorFolder As String = Path.Combine(Application.StartupPath, "Error")
	Private ReadOnly ErrorHtmlPath As String = Path.Combine(ErrorFolder, "index.html")

	Public Sub Show(webView As CoreWebView2, failedUrl As String, Optional errorMessage As String = "took too long to respond")
		Try
			If webView Is Nothing Then Return

			If Not File.Exists(ErrorHtmlPath) Then
				webView.NavigateToString(GetFallbackHtml(failedUrl, errorMessage))
				Return
			End If

			' Avoid stacking handlers – remove any previous one first
			RemoveHandler webView.NavigationCompleted, AddressOf ErrorPage_NavigationCompleted

			Dim fileUri As New Uri(ErrorHtmlPath)
			webView.Navigate(fileUri.AbsoluteUri)

			' Store the values so the handler can use them
			_lastFailedUrl = failedUrl
			_lastErrorMessage = errorMessage

			AddHandler webView.NavigationCompleted, AddressOf ErrorPage_NavigationCompleted

		Catch
		End Try
	End Sub

	Private _lastFailedUrl As String = ""
	Private _lastErrorMessage As String = ""

	Private Sub ErrorPage_NavigationCompleted(sender As Object, e As CoreWebView2NavigationCompletedEventArgs)
		Dim webView = TryCast(sender, CoreWebView2)
		If webView Is Nothing Then Return

		' Always remove the handler after it runs once
		RemoveHandler webView.NavigationCompleted, AddressOf ErrorPage_NavigationCompleted

		If e.IsSuccess Then
			Dim isDark As Boolean = IsDarkTheme()
			Dim json As String =
			$"{{""url"":""{EscapeJson(_lastFailedUrl)}"",""message"":""{EscapeJson(_lastErrorMessage)}"",""dark"":{isDark.ToString().ToLower()}}}"
			webView.PostWebMessageAsJson(json)
		End If
	End Sub

	Private Function EscapeJson(value As String) As String
		If String.IsNullOrEmpty(value) Then Return ""
		Return value.Replace("\", "\\").Replace("""", "\""")
	End Function

	Private Function IsDarkTheme() As Boolean
		Select Case SettingsModule.Theme.ToLower()
			Case "dark"
				Return True
			Case "light"
				Return False
			Case Else
				' System theme
				Try
					Using key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
						"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize")
						If key Is Nothing Then Return True
						Dim value = key.GetValue("AppsUseLightTheme")
						If value Is Nothing Then Return True
						Return Convert.ToInt32(value) = 0
					End Using
				Catch
					Return True
				End Try
		End Select
	End Function

	Private Function GetFallbackHtml(url As String, message As String) As String
		Dim isDark = IsDarkTheme()
		Dim bg = If(isDark, "#1e1e1e", "#f5f5f5")
		Dim fg = If(isDark, "#ffffff", "#222222")

		Return $"
			<html>
				<body style='background:{bg};color:{fg};font-family:Segoe UI;text-align:center;padding-top:100px;'>
					<h2>Hmmm... can't reach this page</h2>
					<p>{url}</p>
					<p>{message}</p>
				</body>
			</html>"
	End Function

	Public Sub HandleWebMessage(message As String)
		If String.IsNullOrWhiteSpace(message) Then Return

		Select Case message.ToLower()
			Case "troubleshoot"
				MessageBox.Show(
					"Please check:" & vbCrLf &
					"• Is the device powered on?" & vbCrLf &
					"• Are you on the same network?" & vbCrLf &
					"• Is the IP address correct?" & vbCrLf &
					"• Is there a firewall blocking the connection?",
					"Troubleshoot Connection",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information)

			Case "refresh"
				' Optional
		End Select
	End Sub

	Public Sub ApplyThemeErrorPage(webView As CoreWebView2)
		Try
			If webView Is Nothing Then Return

			Dim isDark As Boolean = IsDarkTheme()
			Dim json As String = $"{{""dark"":{isDark.ToString().ToLower()}}}"
			webView.PostWebMessageAsJson(json)

		Catch
		End Try
	End Sub

End Module