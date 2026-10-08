Imports Microsoft.Web.WebView2.Core
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Net.Http
Imports System.IO

Public Class maindashboard

	Private DraggedButton As Button = Nothing
	Private DragStartPoint As Point
	Private Shared ReadOnly FaviconClient As HttpClient = CreateFaviconClient()

	Private previousBounds As Rectangle

	'Private ReadOnly ConnectionTimeoutMs As Integer = 5000
	'Private WithEvents TimeoutTimer As New Timer()

	Private Shared Function CreateFaviconClient() As HttpClient

		Dim handler As New HttpClientHandler()

		' Useful for trusted local HTTPS dashboards
		handler.ServerCertificateCustomValidationCallback =
		Function(sender, certificate, chain, sslPolicyErrors)
			Return True
		End Function

		Dim client As New HttpClient(handler)

		client.Timeout = TimeSpan.FromSeconds(5)
		client.DefaultRequestHeaders.UserAgent.ParseAdd(
		"Mozilla/5.0 WebDashboard"
	)

		Return client

	End Function

	Private Sub maindashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		SettingsModule.LoadSettings()

		ApplyWinFormsTheme(Me)

		ApplyFullScreen()

		LoadIP()

		' Create website buttons
		LoadWebsiteButtons()

		' Start WebView2
		Initialize()

	End Sub

	Private Async Sub Initialize()

		Try

			Await WVDisplay.EnsureCoreWebView2Async(Nothing)

			WVDisplay.CoreWebView2.Settings.AreDevToolsEnabled = SettingsModule.EnableDevTools
			WVDisplay.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = SettingsModule.EnableDevTools
			WVDisplay.CoreWebView2.Settings.AreDefaultContextMenusEnabled = SettingsModule.EnableContextMenu

			AddHandler WVDisplay.CoreWebView2.ContextMenuRequested,
				AddressOf CoreWebView2_ContextMenuRequested

			AddHandler WVDisplay.CoreWebView2.ServerCertificateErrorDetected,
				AddressOf WebView2_ServerCertificateErrorDetected

			AddHandler WVDisplay.CoreWebView2.NavigationCompleted,
				AddressOf WebView_NavigationCompleted
			AddHandler WVDisplay.CoreWebView2.WebMessageReceived,
				AddressOf WebView_WebMessageReceived

			If SettingsModule.StartWithDefaultPage Then
				CurrentLinkWeb = ""
				AdminURLName = ""
				SetTheme(SettingsModule.Theme)
				ShowDefaultWeb(WVDisplay.CoreWebView2)
				AddHandler WVDisplay.NavigationCompleted, AddressOf DefaultWeb_NavigationCompleted

			ElseIf SettingsModule.OpenLastWebsite AndAlso Not String.IsNullOrWhiteSpace(CurrentLinkWeb) Then
				NavigateToServer()

			Else
				CurrentLinkWeb = ""
				SetTheme(SettingsModule.Theme)
				ShowDefaultWeb(WVDisplay.CoreWebView2)
				AddHandler WVDisplay.NavigationCompleted, AddressOf DefaultWeb_NavigationCompleted
			End If

		Catch ex As Exception
			MessageBox.Show(ex.Message, "WebView2 Error", MessageBoxButtons.OK, MessageBoxIcon.Error
			)

		End Try

	End Sub

	Public Async Sub ApplyApplicationTheme()

		ApplyWinFormsTheme(Me)

		SetTheme(SettingsModule.Theme)

		ApplyFullScreen()

		If WVDisplay.CoreWebView2 Is Nothing Then
			Return
		End If

		If String.IsNullOrWhiteSpace(CurrentLinkWeb) Then

			Await ApplyTheme(
			WVDisplay.CoreWebView2
		)

		End If

		ErrorPageModule.ApplyThemeErrorPage(WVDisplay.CoreWebView2)
	End Sub

	Private Async Sub DefaultWeb_NavigationCompleted(
	sender As Object,
	e As CoreWebView2NavigationCompletedEventArgs)

		If Not e.IsSuccess Then
			Return
		End If

		If WVDisplay.CoreWebView2 Is Nothing Then
			Return
		End If

		Await ApplyTheme(
		WVDisplay.CoreWebView2
	)

	End Sub

	Private Sub WebView_NavigationCompleted(sender As Object, e As CoreWebView2NavigationCompletedEventArgs)
		If e.IsSuccess Then Return
		Dim currentSource = ""
		If WVDisplay.CoreWebView2 IsNot Nothing Then
			currentSource = WVDisplay.CoreWebView2.Source
		End If

		If currentSource.Contains("Error") OrElse currentSource.Contains("index.html") Then
			Return
		End If

		ErrorPageModule.Show(WVDisplay.CoreWebView2, CurrentLinkWeb)
	End Sub

	Private Sub WebView_WebMessageReceived(sender As Object, e As CoreWebView2WebMessageReceivedEventArgs)
		Dim message As String = e.TryGetWebMessageAsString()
		ErrorPageModule.HandleWebMessage(message)
	End Sub

	Private Sub NavigateToServer()

		If WVDisplay.CoreWebView2 Is Nothing Then
			Return
		End If

		If String.IsNullOrWhiteSpace(CurrentLinkWeb) Then
			Return
		End If

		WVDisplay.CoreWebView2.Navigate(CurrentLinkWeb)

	End Sub

	Private Sub ApplyFullScreen()
		Dim isFullScreen = SettingsModule.IsFullScreen

		If isFullScreen Then
			EnterFullScreen()
		Else
			ExitFullScreen()
		End If
	End Sub

	Private Sub CoreWebView2_ContextMenuRequested(sender As Object, e As CoreWebView2ContextMenuRequestedEventArgs)
		If Not SettingsModule.EnableContextMenu Then
			e.Handled = True
		End If
	End Sub

	Private Sub EnterFullScreen()
		If Me.FormBorderStyle = FormBorderStyle.None Then Return

		previousBounds = Me.Bounds

		Me.FormBorderStyle = FormBorderStyle.None
		Me.WindowState = FormWindowState.Normal
		Me.Bounds = Screen.FromControl(Me).Bounds
	End Sub

	Private Sub ExitFullScreen()
		If Me.FormBorderStyle <> FormBorderStyle.None Then Return

		Me.FormBorderStyle = FormBorderStyle.Sizable
		Me.WindowState = FormWindowState.Maximized

		If previousBounds.Width > 0 AndAlso previousBounds.Height > 0 Then
			Me.Bounds = previousBounds
		Else
			Dim screenBounds = Screen.FromControl(Me).WorkingArea
			Me.Location = New Point(
				screenBounds.Left + (screenBounds.Width - Me.Width) \ 2,
				screenBounds.Top + (screenBounds.Height - Me.Height) \ 2
			)
		End If

	End Sub

	Private Sub WebView2_ServerCertificateErrorDetected(sender As Object, e As CoreWebView2ServerCertificateErrorDetectedEventArgs)

		If Not String.IsNullOrWhiteSpace(CurrentLinkWeb) AndAlso e.RequestUri.StartsWith(CurrentLinkWeb, StringComparison.OrdinalIgnoreCase) Then

			e.Action =
				CoreWebView2ServerCertificateErrorAction.AlwaysAllow

		Else

			e.Action =
				CoreWebView2ServerCertificateErrorAction.Cancel

		End If

	End Sub

	Public Sub LoadWebsiteButtons()

		PnlTabs.SuspendLayout()

		' Dispose existing buttons and their images
		For Each control As Control In PnlTabs.Controls

			Dim btn As Button = TryCast(control, Button)

			If btn IsNot Nothing Then

				If btn.Image IsNot Nothing Then
					btn.Image.Dispose()
					btn.Image = Nothing
				End If

				btn.Dispose()

			End If

		Next

		PnlTabs.Controls.Clear()

		Dim websites As List(Of WebsiteItem) = GetWebsites()

		For Each website As WebsiteItem In websites

			AddWebsiteButton(website)

		Next

		PnlTabs.ResumeLayout()

		' Select first website
		If websites.Count > 0 Then
			PnlTabs.Visible = True
		Else
			PnlTabs.Visible = False
		End If

	End Sub



	Private Sub AddWebsiteButton(website As WebsiteItem)

		Dim btn As New Button()

		btn.Text = website.name
		btn.Tag = website

		btn.AutoSize = False
		btn.Width = 150
		btn.Height = 36

		btn.FlatStyle = FlatStyle.Flat
		btn.FlatAppearance.BorderSize = 0

		btn.Margin = New Padding(2, 2, 2, 2)

		btn.Cursor = Cursors.Hand


		' Favicon + text layout
		btn.TextImageRelation = TextImageRelation.ImageBeforeText
		btn.ImageAlign = ContentAlignment.MiddleLeft
		btn.TextAlign = ContentAlignment.MiddleLeft
		btn.Padding = New Padding(0, 0, 0, 2)

		' Store website
		AddHandler btn.Click, AddressOf WebsiteButton_Click

		' Drag events
		AddHandler btn.MouseDown, AddressOf WebsiteButton_MouseDown
		AddHandler btn.MouseMove, AddressOf WebsiteButton_MouseMove
		AddHandler btn.MouseUp, AddressOf WebsiteButton_MouseUp

		PnlTabs.Controls.Add(btn)

		LoadFaviconAsync(btn, website.url)

	End Sub

	Private Sub WebsiteButton_Click(sender As Object, e As EventArgs)

		Dim btn As Button = DirectCast(sender, Button)

		Dim website As WebsiteItem =
			TryCast(btn.Tag, WebsiteItem)

		If website Is Nothing Then
			Return
		End If

		CurrentLinkWeb = website.url
		AdminURLName = website.name

		' Navigate WebView
		If WVDisplay.CoreWebView2 IsNot Nothing Then

			WVDisplay.CoreWebView2.Navigate(website.url)

		End If

		HighlightActiveButton()

	End Sub

	Public Sub HighlightActiveButton()
		Dim setDark As String = SettingsModule.Theme

		Dim normalBack As Color
		Dim normalFore As Color

		If setDark = "system" Or setDark = "dark" Then
			normalBack = Color.FromArgb(45, 45, 45)
			normalFore = Color.White
		Else
			normalBack = SystemColors.Control
			normalFore = SystemColors.ControlText
		End If

		For Each control As Control In PnlTabs.Controls

			Dim btn As Button = TryCast(control, Button)

			If btn Is Nothing Then
				Continue For
			End If

			Dim website As WebsiteItem =
				TryCast(btn.Tag, WebsiteItem)

			If website Is Nothing Then
				Continue For
			End If

			If String.Equals(
				website.url,
				CurrentLinkWeb,
				StringComparison.OrdinalIgnoreCase) Then

				' Active tab
				btn.BackColor = Color.DodgerBlue
				btn.ForeColor = Color.White

			Else

				' Normal tab
				btn.BackColor = normalBack
				btn.ForeColor = normalFore

			End If

		Next

	End Sub

	Private Sub WebsiteButton_MouseDown(
		sender As Object,
		e As MouseEventArgs)

		If e.Button <> MouseButtons.Left Then
			Return
		End If

		DraggedButton = TryCast(sender, Button)

		If DraggedButton Is Nothing Then
			Return
		End If

		DragStartPoint = e.Location

	End Sub

	Private Sub WebsiteButton_MouseMove(
		sender As Object,
		e As MouseEventArgs)

		If DraggedButton Is Nothing Then
			Return
		End If

		If e.Button <> MouseButtons.Left Then
			Return
		End If

		Dim distance As Integer =
			Math.Abs(e.X - DragStartPoint.X)

		If distance < 10 Then
			Return
		End If

		Dim mousePosition As Point =
			PnlTabs.PointToClient(Cursor.Position)

		For Each control As Control In PnlTabs.Controls

			If control Is DraggedButton Then
				Continue For
			End If

			Dim centerX As Integer =
				control.Left + (control.Width \ 2)

			If mousePosition.X < centerX Then

				Dim oldIndex As Integer =
					PnlTabs.Controls.IndexOf(DraggedButton)

				Dim newIndex As Integer =
					PnlTabs.Controls.IndexOf(control)

				If oldIndex <> newIndex Then

					PnlTabs.Controls.SetChildIndex(
						DraggedButton,
						newIndex
					)

				End If

				Exit For

			End If

		Next

	End Sub

	Private Sub WebsiteButton_MouseUp(
		sender As Object,
		e As MouseEventArgs)

		If e.Button <> MouseButtons.Left Then
			Return
		End If

		If DraggedButton Is Nothing Then
			Return
		End If

		' Save new order
		SaveWebsiteOrder()

		DraggedButton = Nothing

	End Sub

	Private Sub SaveWebsiteOrder()

		Dim websites As New List(Of WebsiteItem)

		For Each control As Control In PnlTabs.Controls

			Dim btn As Button = TryCast(control, Button)

			If btn Is Nothing Then
				Continue For
			End If

			Dim website As WebsiteItem =
				TryCast(btn.Tag, WebsiteItem)

			If website IsNot Nothing Then

				websites.Add(
					New WebsiteItem With {
						.id = website.id,
						.name = website.name,
						.url = website.url
					}
				)

			End If

		Next

		SaveWebsites(websites)

	End Sub

	Private Async Sub LoadFaviconAsync(btn As Button, websiteUrl As String)

		Try

			If String.IsNullOrWhiteSpace(websiteUrl) Then
				Return
			End If

			Dim siteUri As New Uri(websiteUrl)

			' --------------------------------------------------
			' 1. Try to find favicon from website HTML
			' --------------------------------------------------

			Dim faviconUrl As String = Nothing

			Try

				Dim html As String =
				Await FaviconClient.GetStringAsync(websiteUrl)

				' Look for:
				' <link rel="icon" href="...">
				' <link rel="shortcut icon" href="...">
				' <link rel="apple-touch-icon" href="...">

				Dim patterns As String() = {
				"<link[^>]+rel\s*=\s*[""'][^""']*\bicon\b[^""']*[""'][^>]+href\s*=\s*[""']([^""']+)[""']",
				"<link[^>]+href\s*=\s*[""']([^""']+)[""'][^>]+rel\s*=\s*[""'][^""']*\bicon\b[^""']*[""']"
			}

				For Each pattern As String In patterns

					Dim match As System.Text.RegularExpressions.Match =
					System.Text.RegularExpressions.Regex.Match(
						html,
						pattern,
						System.Text.RegularExpressions.RegexOptions.IgnoreCase
					)

					If match.Success Then

						faviconUrl = match.Groups(1).Value.Trim()

						If Not String.IsNullOrWhiteSpace(faviconUrl) Then
							Exit For
						End If

					End If

				Next

			Catch
				' HTML request failed.
				' We will fall back to /favicon.ico.
			End Try


			' --------------------------------------------------
			' 2. Resolve favicon URL
			' --------------------------------------------------

			Dim faviconUri As Uri = Nothing

			If Not String.IsNullOrWhiteSpace(faviconUrl) Then

				' Handles:
				' /favicon.ico
				' /admin/img/favicon.ico
				' favicon.ico
				' ../favicon.ico
				' https://example.com/favicon.ico

				If Uri.TryCreate(
				siteUri,
				faviconUrl,
				faviconUri) = False Then

					faviconUri = Nothing

				End If

			End If


			' --------------------------------------------------
			' 3. Fallback to /favicon.ico
			' --------------------------------------------------

			If faviconUri Is Nothing Then

				faviconUri = New Uri(
				siteUri.GetLeftPart(UriPartial.Authority) &
				"/favicon.ico"
			)

			End If


			' --------------------------------------------------
			' 4. Download favicon
			' --------------------------------------------------

			Dim data() As Byte =
			Await FaviconClient.GetByteArrayAsync(
				faviconUri.ToString()
			)

			If data Is Nothing OrElse data.Length = 0 Then
				Return
			End If


			' --------------------------------------------------
			' 5. Create 20x20 icon
			' --------------------------------------------------

			Using ms As New MemoryStream(data)

				Using original As Image = Image.FromStream(ms)

					Dim favicon As New Bitmap(
					original,
					New Size(20, 20)
				)

					If btn.IsDisposed Then

						favicon.Dispose()
						Return

					End If


					' --------------------------------------------------
					' 6. Add 5px spacing after icon
					' --------------------------------------------------

					Dim iconWithSpace As New Bitmap(25, 20)

					iconWithSpace.MakeTransparent()

					Using g As Graphics =
					Graphics.FromImage(iconWithSpace)

						g.Clear(Color.Transparent)

						g.CompositingMode = Drawing2D.CompositingMode.SourceCopy

						g.DrawImage(favicon, 0, 0, 20, 20)

					End Using

					favicon.Dispose()


					' Dispose previous image
					If btn.Image IsNot Nothing Then
						btn.Image.Dispose()
					End If


					btn.Image = iconWithSpace

					' Make sure layout is refreshed
					btn.Invalidate()

				End Using

			End Using


		Catch ex As Exception

			' Favicon failure should never break the dashboard.

		End Try

	End Sub

	Private Sub MSAddIP_Click(
		sender As Object,
		e As EventArgs) Handles MSAddIP.Click

		Dim result As CustomDesignGUI.WebsiteInputResult =
			CustomDesignGUI.WebsiteInputBox("Add Website")

		If result Is Nothing Then
			Return
		End If

		Dim website As New WebsiteItem With {
			.name = result.Name,
			.url = result.Url
		}

		' Get next ID
		Dim websites As List(Of WebsiteItem) =
			GetWebsites()

		If websites.Count > 0 Then

			website.id =
				websites.Max(Function(x) x.id) + 1

		Else

			website.id = 0

		End If

		websites.Add(website)

		SaveWebsites(websites)

		PnlTabs.Visible = True

		' Add button immediately
		AddWebsiteButton(website)

		' Select the new website
		CurrentLinkWeb = website.url
		AdminURLName = website.name

		If WVDisplay.CoreWebView2 IsNot Nothing Then

			WVDisplay.CoreWebView2.Navigate(
				website.url
			)

		End If

		HighlightActiveButton()

	End Sub

	Private Sub MSSettings_Click(sender As Object, e As EventArgs) Handles MSSettings.Click
		SettingsForm.ShowDialog()
	End Sub

	Private Sub MSExit_Click(sender As Object, e As EventArgs) Handles MSExit.Click
		Dim result = CustomDesignGUI.ThemedMessageBox(
			"Are you sure you want to exit?",
			"Exit",
			MessageBoxButtons.YesNo,
			MessageBoxIcon.Question,
			themeSet:=SettingsModule.Theme
		)

		If result = DialogResult.Yes Then
			Application.Exit()
		End If
	End Sub
End Class