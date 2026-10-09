Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Net.Http
Imports System.Windows.Forms
Imports Microsoft.Web.WebView2.Core
Public Class maindashboard
	Private DraggedButton As Button = Nothing
	Private DragStartPoint As Point
	Private Shared ReadOnly FaviconClient As HttpClient = CreateFaviconClient()
	' One WebView2 per website (keeps login session)
	Private webViews As New Dictionary(Of String, Microsoft.Web.WebView2.WinForms.WebView2)
	Private currentWebView As Microsoft.Web.WebView2.WinForms.WebView2 = Nothing
	Private isAuthenticating As Boolean = False
	Private previousBounds As Rectangle
	Private PnlLoading As Panel
	Private LblLoading As Label
	Private loadingCreated As Boolean = False

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

		EnsureLoadingOverlay()
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
			AddHandler WVDisplay.CoreWebView2.BasicAuthenticationRequested,
				AddressOf CoreWebView2_BasicAuthenticationRequested
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
	Private Async Sub DefaultWeb_NavigationCompleted(sender As Object, e As CoreWebView2NavigationCompletedEventArgs)
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
		HideLoading()
		isAuthenticating = False
		If e.IsSuccess Then Return
		If e.WebErrorStatus = CoreWebView2WebErrorStatus.ValidAuthenticationCredentialsRequired OrElse
			e.WebErrorStatus = CoreWebView2WebErrorStatus.ValidProxyAuthenticationRequired Then
			Return
		End If
		If isAuthenticating Then Return
		Dim core As CoreWebView2 = TryCast(sender, CoreWebView2)
		If core Is Nothing Then Return
		Dim currentSource As String = core.Source
		If currentSource.Contains("Error") OrElse currentSource.Contains("index.html") Then
			Return
		End If
		ErrorPageModule.Show(core, CurrentLinkWeb)
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

	Private Sub EnsureLoadingOverlay()
		If loadingCreated Then Return

		PnlLoading = New Panel()
		PnlLoading.Dock = DockStyle.Fill
		PnlLoading.BackColor = Color.FromArgb(30, 30, 30)
		PnlLoading.Visible = False

		LblLoading = New Label()
		LblLoading.AutoSize = False
		LblLoading.Dock = DockStyle.Fill
		LblLoading.TextAlign = ContentAlignment.MiddleCenter
		LblLoading.ForeColor = Color.White
		LblLoading.Font = New Font("Segoe UI", 12.0F)
		LblLoading.Text = "Connecting, please wait..."
		LblLoading.BackColor = Color.Transparent

		PnlLoading.Controls.Add(LblLoading)

		' Add to the same container as WVDisplay
		If WVDisplay.Parent IsNot Nothing Then
			WVDisplay.Parent.Controls.Add(PnlLoading)
		Else
			Me.Controls.Add(PnlLoading)
		End If

		loadingCreated = True
	End Sub

	Private Sub ShowLoading(Optional siteName As String = "")
		EnsureLoadingOverlay()

		If String.IsNullOrWhiteSpace(siteName) Then
			LblLoading.Text = "Connecting, please wait..."
		Else
			LblLoading.Text = siteName & Environment.NewLine & "Connecting, please wait..."
		End If

		PnlLoading.Visible = True
		PnlLoading.BringToFront()
	End Sub

	Private Sub HideLoading()
		If PnlLoading IsNot Nothing Then
			PnlLoading.Visible = False
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
	Private Sub ApplyRoundedCorners(ctrl As Control, radius As Integer,
								Optional roundTopLeft As Boolean = False,
								Optional roundTopRight As Boolean = True)
		Dim w As Integer = ctrl.Width
		Dim h As Integer = ctrl.Height
		Dim d As Integer = radius * 2
		Using path As New GraphicsPath()
			' Top-left corner
			If roundTopLeft Then
				path.AddArc(0, 0, d, d, 180, 90)
			Else
				path.AddLine(0, h, 0, 0)
			End If
			' Top-right corner
			If roundTopRight Then
				path.AddArc(w - d, 0, d, d, 270, 90)
			Else
				path.AddLine(0, 0, w, 0)
			End If
			' Straight bottom corners
			path.AddLine(w, h, 0, h)
			path.CloseFigure()
			ctrl.Region = New Region(path)
		End Using
	End Sub
	Private Sub CoreWebView2_BasicAuthenticationRequested(sender As Object, e As CoreWebView2BasicAuthenticationRequestedEventArgs)
		isAuthenticating = True
		e.Cancel = False
	End Sub
	Private Sub AddWebsiteButton(website As WebsiteItem)
		Dim btn As New Button()
		btn.Text = website.name                  ' name only (left side)
		btn.Tag = website
		btn.AutoSize = False
		btn.Width = 160
		btn.Height = 36
		btn.FlatStyle = FlatStyle.Flat
		btn.FlatAppearance.BorderSize = 0
		btn.Margin = New Padding(2)
		btn.Cursor = Cursors.Hand
		btn.TextAlign = ContentAlignment.MiddleLeft
		btn.TextImageRelation = TextImageRelation.ImageBeforeText
		btn.ImageAlign = ContentAlignment.MiddleLeft
		btn.Padding = New Padding(6, 0, 28, 0)   ' leave space on the right for ↻

		ApplyRoundedCorners(btn, 12)
		AddHandler btn.Resize, Sub(s, e) ApplyRoundedCorners(DirectCast(s, Button), 8)

		' Draw the reload icon on the right
		AddHandler btn.Paint, AddressOf WebsiteButton_Paint

		' Handle open vs reload
		AddHandler btn.MouseClick, AddressOf WebsiteButton_MouseClick

		' Drag support
		AddHandler btn.MouseDown, AddressOf WebsiteButton_MouseDown
		AddHandler btn.MouseMove, AddressOf WebsiteButton_MouseMove
		AddHandler btn.MouseUp, AddressOf WebsiteButton_MouseUp

		PnlTabs.Controls.Add(btn)
		LoadFaviconAsync(btn, website.url)
	End Sub

	Private Sub WebsiteButton_Paint(sender As Object, e As PaintEventArgs)
		Dim btn As Button = DirectCast(sender, Button)

		Dim iconText As String = "↻"
		Using font As New Font("Segoe UI", 11, FontStyle.Regular)
			Dim size = e.Graphics.MeasureString(iconText, font)
			Dim x As Single = btn.Width - size.Width - 8
			Dim y As Single = (btn.Height - size.Height) / 2

			Using brush As New SolidBrush(btn.ForeColor)
				e.Graphics.DrawString(iconText, font, brush, x, y)
			End Using
		End Using
	End Sub

	Private Async Sub WebsiteButton_MouseClick(sender As Object, e As MouseEventArgs)
		Dim btn As Button = DirectCast(sender, Button)
		Dim website As WebsiteItem = TryCast(btn.Tag, WebsiteItem)
		If website Is Nothing Then Return

		CurrentLinkWeb = website.url
		AdminURLName = website.name

		' Right 28 pixels = reload
		If e.X >= btn.Width - 28 Then
			Await ReloadWebsite(website.url)
		Else
			Await ShowWebsite(website.url)
		End If

		HighlightActiveButton()
	End Sub

	Private Async Function ShowWebsite(url As String) As Task
		If currentWebView IsNot Nothing Then
			currentWebView.Visible = False
		End If
		If webViews.ContainsKey(url) Then
			currentWebView = webViews(url)
			currentWebView.Visible = True
			currentWebView.BringToFront()
			HideLoading()
			Return
		End If
		Dim wv As New Microsoft.Web.WebView2.WinForms.WebView2()
		wv.Dock = DockStyle.Fill
		wv.Visible = False
		Dim parent = If(WVDisplay.Parent, Me)
		parent.Controls.Add(wv)
		Await wv.EnsureCoreWebView2Async(Nothing)
		wv.CoreWebView2.Settings.AreDevToolsEnabled = SettingsModule.EnableDevTools
		wv.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = SettingsModule.EnableDevTools
		wv.CoreWebView2.Settings.AreDefaultContextMenusEnabled = SettingsModule.EnableContextMenu
		AddHandler wv.CoreWebView2.ContextMenuRequested, AddressOf CoreWebView2_ContextMenuRequested
		AddHandler wv.CoreWebView2.ServerCertificateErrorDetected, AddressOf WebView2_ServerCertificateErrorDetected
		AddHandler wv.CoreWebView2.BasicAuthenticationRequested, AddressOf CoreWebView2_BasicAuthenticationRequested
		AddHandler wv.CoreWebView2.NavigationCompleted, AddressOf WebView_NavigationCompleted
		AddHandler wv.CoreWebView2.WebMessageReceived, AddressOf WebView_WebMessageReceived
		webViews.Add(url, wv)
		currentWebView = wv
		wv.Visible = True
		wv.BringToFront()
		ShowLoading(AdminURLName)
		wv.CoreWebView2.Navigate(url)
	End Function
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

	Private Async Function ReloadWebsite(url As String) As Task
		If webViews.ContainsKey(url) Then
			Dim wv = webViews(url)

			If currentWebView IsNot Nothing Then currentWebView.Visible = False
			currentWebView = wv
			wv.Visible = True
			wv.BringToFront()
			ShowLoading(AdminURLName)
			If wv.CoreWebView2 IsNot Nothing Then
				wv.CoreWebView2.Navigate(url)
			End If
		Else
			' Not opened yet → just open it
			Await ShowWebsite(url)
		End If

		For Each ctrl As Control In PnlTabs.Controls
			Dim btn As Button = TryCast(ctrl, Button)
			If btn Is Nothing Then Continue For

			Dim website As WebsiteItem = TryCast(btn.Tag, WebsiteItem)
			If website IsNot Nothing AndAlso
			   String.Equals(website.url, url, StringComparison.OrdinalIgnoreCase) Then
				LoadFaviconAsync(btn, url)
				Exit For
			End If
		Next
	End Function

	Private Async Sub LoadFaviconAsync(btn As Button, websiteUrl As String)
		' Always set default first
		If btn.Image IsNot Nothing Then
			btn.Image.Dispose()
			btn.Image = Nothing
		End If
		btn.Image = Icons.CreateDefaultGlobeIcon()
		btn.Invalidate()

		Try
			If String.IsNullOrWhiteSpace(websiteUrl) Then Return

			Dim siteUri As New Uri(websiteUrl)
			Dim faviconUrl As String = Nothing

			' Try to find favicon from HTML
			Try
				Dim html As String = Await FaviconClient.GetStringAsync(websiteUrl)
				Dim patterns As String() = {
				"<link[^>]+rel\s*=\s*[""'][^""']*\bicon\b[^""']*[""'][^>]+href\s*=\s*[""']([^""']+)[""']",
				"<link[^>]+href\s*=\s*[""']([^""']+)[""'][^>]+rel\s*=\s*[""'][^""']*\bicon\b[^""']*[""']"
			}
				For Each pattern As String In patterns
					Dim match = System.Text.RegularExpressions.Regex.Match(
					html, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
					If match.Success Then
						faviconUrl = match.Groups(1).Value.Trim()
						If Not String.IsNullOrWhiteSpace(faviconUrl) Then Exit For
					End If
				Next
			Catch
				' ignore – will try /favicon.ico
			End Try

			Dim faviconUri As Uri = Nothing
			If Not String.IsNullOrWhiteSpace(faviconUrl) Then
				Uri.TryCreate(siteUri, faviconUrl, faviconUri)
			End If

			If faviconUri Is Nothing Then
				faviconUri = New Uri(siteUri.GetLeftPart(UriPartial.Authority) & "/favicon.ico")
			End If

			Dim data() As Byte = Await FaviconClient.GetByteArrayAsync(faviconUri.ToString())
			If data Is Nothing OrElse data.Length = 0 Then Return

			Using ms As New MemoryStream(data)
				Using original As Image = Image.FromStream(ms)
					Dim favicon As New Bitmap(original, New Size(20, 20))
					If btn.IsDisposed Then
						favicon.Dispose()
						Return
					End If

					Dim iconWithSpace As New Bitmap(25, 20)
					iconWithSpace.MakeTransparent()
					Using g As Graphics = Graphics.FromImage(iconWithSpace)
						g.Clear(Color.Transparent)
						g.CompositingMode = Drawing2D.CompositingMode.SourceCopy
						g.DrawImage(favicon, 0, 0, 20, 20)
					End Using
					favicon.Dispose()

					If btn.Image IsNot Nothing Then
						btn.Image.Dispose()
					End If
					btn.Image = iconWithSpace
					btn.Invalidate()
				End Using
			End Using

		Catch
			' Keep the default globe icon
		End Try
	End Sub

	Private Sub MSAddIP_Click(
		sender As Object,
		e As EventArgs) Handles MSAddIP.Click
		Dim result As CustomDesignGUI.WebsiteInputResult =
			CustomDesignGUI.WebsiteInputBox("Add Website",,, True)
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