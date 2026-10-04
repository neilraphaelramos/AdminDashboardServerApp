<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SettingsForm
	Inherits System.Windows.Forms.Form

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		Try
			If disposing AndAlso components IsNot Nothing Then
				components.Dispose()
			End If
		Finally
			MyBase.Dispose(disposing)
		End Try
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
		Label3 = New Label()
		CHKOpenLast = New CheckBox()
		Label2 = New Label()
		CHKStartDefault = New CheckBox()
		Label1 = New Label()
		CHKShowNavigation = New CheckBox()
		Label4 = New Label()
		lblThemeDescription = New Label()
		CMBTheme = New ComboBox()
		lblTheme = New Label()
		Label7 = New Label()
		CHKInvalidCertificates = New CheckBox()
		Label6 = New Label()
		CHKContextMenu = New CheckBox()
		CHKDevTools = New CheckBox()
		Label5 = New Label()
		BtnMoveDown = New Button()
		BtnMoveUp = New Button()
		BtnRemoveWebsite = New Button()
		BtnEditWebsite = New Button()
		BtnAddWebsite = New Button()
		LVWebsites = New ListView()
		CHName = New ColumnHeader()
		CHUrl = New ColumnHeader()
		Label8 = New Label()
		BtnResetSettings = New Button()
		Label11 = New Label()
		Label10 = New Label()
		CHKDebug = New CheckBox()
		CHKLogging = New CheckBox()
		Label9 = New Label()
		pnlHeader = New Panel()
		lblDescription = New Label()
		lblTitle = New Label()
		pnlButtons = New Panel()
		BtnOK = New Button()
		BtnApply = New Button()
		CTCSettings = New CustomLeftTabControl()
		TPGeneral = New TabPage()
		Label12 = New Label()
		CHKFullScreen = New CheckBox()
		TPAppearances = New TabPage()
		TPWebView = New TabPage()
		TPWebsites = New TabPage()
		TPAdvanced = New TabPage()
		pnlHeader.SuspendLayout()
		pnlButtons.SuspendLayout()
		CTCSettings.SuspendLayout()
		TPGeneral.SuspendLayout()
		TPAppearances.SuspendLayout()
		TPWebView.SuspendLayout()
		TPWebsites.SuspendLayout()
		TPAdvanced.SuspendLayout()
		SuspendLayout()
		' 
		' Label3
		' 
		Label3.AutoSize = True
		Label3.Location = New Point(6, 115)
		Label3.Name = "Label3"
		Label3.Size = New Size(244, 15)
		Label3.TabIndex = 4
		Label3.Text = "Automatically open the last selected website."
		' 
		' CHKOpenLast
		' 
		CHKOpenLast.AutoSize = True
		CHKOpenLast.Location = New Point(6, 93)
		CHKOpenLast.Name = "CHKOpenLast"
		CHKOpenLast.Size = New Size(119, 19)
		CHKOpenLast.TabIndex = 3
		CHKOpenLast.Text = "Open last website"
		CHKOpenLast.UseVisualStyleBackColor = True
		' 
		' Label2
		' 
		Label2.AutoSize = True
		Label2.Location = New Point(6, 64)
		Label2.Name = "Label2"
		Label2.Size = New Size(291, 15)
		Label2.TabIndex = 2
		Label2.Text = "Show the local dashboard when the application starts."
		' 
		' CHKStartDefault
		' 
		CHKStartDefault.AutoSize = True
		CHKStartDefault.Location = New Point(6, 42)
		CHKStartDefault.Name = "CHKStartDefault"
		CHKStartDefault.Size = New Size(145, 19)
		CHKStartDefault.TabIndex = 1
		CHKStartDefault.Text = "Start with default page"
		CHKStartDefault.UseVisualStyleBackColor = True
		' 
		' Label1
		' 
		Label1.AutoSize = True
		Label1.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
		Label1.Location = New Point(6, 3)
		Label1.Name = "Label1"
		Label1.Size = New Size(89, 20)
		Label1.TabIndex = 0
		Label1.Text = "Application"
		' 
		' CHKShowNavigation
		' 
		CHKShowNavigation.AutoSize = True
		CHKShowNavigation.Location = New Point(9, 122)
		CHKShowNavigation.Name = "CHKShowNavigation"
		CHKShowNavigation.Size = New Size(157, 19)
		CHKShowNavigation.TabIndex = 5
		CHKShowNavigation.Text = "Show website navigation"
		CHKShowNavigation.UseVisualStyleBackColor = True
		' 
		' Label4
		' 
		Label4.AutoSize = True
		Label4.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
		Label4.Location = New Point(6, 99)
		Label4.Name = "Label4"
		Label4.Size = New Size(72, 20)
		Label4.TabIndex = 4
		Label4.Text = "Interface"
		' 
		' lblThemeDescription
		' 
		lblThemeDescription.AutoSize = True
		lblThemeDescription.Location = New Point(9, 34)
		lblThemeDescription.Name = "lblThemeDescription"
		lblThemeDescription.Size = New Size(233, 15)
		lblThemeDescription.TabIndex = 3
		lblThemeDescription.Text = "Choose how the dashboard should appear."
		' 
		' CMBTheme
		' 
		CMBTheme.BackColor = SystemColors.Window
		CMBTheme.DropDownStyle = ComboBoxStyle.DropDownList
		CMBTheme.FormattingEnabled = True
		CMBTheme.Items.AddRange(New Object() {"System", "Light", "Dark"})
		CMBTheme.Location = New Point(9, 52)
		CMBTheme.Name = "CMBTheme"
		CMBTheme.Size = New Size(181, 23)
		CMBTheme.TabIndex = 2
		' 
		' lblTheme
		' 
		lblTheme.AutoSize = True
		lblTheme.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
		lblTheme.Location = New Point(6, 3)
		lblTheme.Name = "lblTheme"
		lblTheme.Size = New Size(57, 20)
		lblTheme.TabIndex = 1
		lblTheme.Text = "Theme"
		' 
		' Label7
		' 
		Label7.AutoSize = True
		Label7.ForeColor = Color.DarkOrange
		Label7.Location = New Point(6, 166)
		Label7.Name = "Label7"
		Label7.Size = New Size(193, 30)
		Label7.TabIndex = 7
		Label7.Text = "⚠ Only enable this for trusted local" & vbLf & "  server services."
		' 
		' CHKInvalidCertificates
		' 
		CHKInvalidCertificates.AutoSize = True
		CHKInvalidCertificates.Location = New Point(6, 144)
		CHKInvalidCertificates.Name = "CHKInvalidCertificates"
		CHKInvalidCertificates.Size = New Size(191, 19)
		CHKInvalidCertificates.TabIndex = 6
		CHKInvalidCertificates.Text = "Allow invalid HTTPS certificates"
		CHKInvalidCertificates.UseVisualStyleBackColor = True
		' 
		' Label6
		' 
		Label6.AutoSize = True
		Label6.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
		Label6.Location = New Point(6, 105)
		Label6.Name = "Label6"
		Label6.Size = New Size(65, 20)
		Label6.TabIndex = 5
		Label6.Text = "Security"
		' 
		' CHKContextMenu
		' 
		CHKContextMenu.AutoSize = True
		CHKContextMenu.Location = New Point(6, 60)
		CHKContextMenu.Name = "CHKContextMenu"
		CHKContextMenu.Size = New Size(140, 19)
		CHKContextMenu.TabIndex = 4
		CHKContextMenu.Text = "Enable Context Menu"
		CHKContextMenu.UseVisualStyleBackColor = True
		' 
		' CHKDevTools
		' 
		CHKDevTools.AutoSize = True
		CHKDevTools.Location = New Point(6, 35)
		CHKDevTools.Name = "CHKDevTools"
		CHKDevTools.Size = New Size(147, 19)
		CHKDevTools.TabIndex = 3
		CHKDevTools.Text = "Enable Developer Tools"
		CHKDevTools.UseVisualStyleBackColor = True
		' 
		' Label5
		' 
		Label5.AutoSize = True
		Label5.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
		Label5.Location = New Point(6, 3)
		Label5.Name = "Label5"
		Label5.Size = New Size(67, 20)
		Label5.TabIndex = 2
		Label5.Text = "Browser"
		' 
		' BtnMoveDown
		' 
		BtnMoveDown.FlatStyle = FlatStyle.Popup
		BtnMoveDown.Location = New Point(236, 39)
		BtnMoveDown.Name = "BtnMoveDown"
		BtnMoveDown.Size = New Size(75, 23)
		BtnMoveDown.TabIndex = 9
		BtnMoveDown.Text = "Down"
		BtnMoveDown.UseVisualStyleBackColor = True
		' 
		' BtnMoveUp
		' 
		BtnMoveUp.FlatStyle = FlatStyle.Popup
		BtnMoveUp.Location = New Point(155, 39)
		BtnMoveUp.Name = "BtnMoveUp"
		BtnMoveUp.Size = New Size(75, 23)
		BtnMoveUp.TabIndex = 8
		BtnMoveUp.Text = "Up"
		BtnMoveUp.UseVisualStyleBackColor = True
		' 
		' BtnRemoveWebsite
		' 
		BtnRemoveWebsite.FlatStyle = FlatStyle.Popup
		BtnRemoveWebsite.Location = New Point(236, 264)
		BtnRemoveWebsite.Name = "BtnRemoveWebsite"
		BtnRemoveWebsite.Size = New Size(75, 23)
		BtnRemoveWebsite.TabIndex = 7
		BtnRemoveWebsite.Text = "Remove"
		BtnRemoveWebsite.UseVisualStyleBackColor = True
		' 
		' BtnEditWebsite
		' 
		BtnEditWebsite.FlatStyle = FlatStyle.Popup
		BtnEditWebsite.Location = New Point(143, 264)
		BtnEditWebsite.Name = "BtnEditWebsite"
		BtnEditWebsite.Size = New Size(75, 23)
		BtnEditWebsite.TabIndex = 6
		BtnEditWebsite.Text = "Edit"
		BtnEditWebsite.UseVisualStyleBackColor = True
		' 
		' BtnAddWebsite
		' 
		BtnAddWebsite.FlatStyle = FlatStyle.Popup
		BtnAddWebsite.Location = New Point(46, 264)
		BtnAddWebsite.Name = "BtnAddWebsite"
		BtnAddWebsite.Size = New Size(75, 23)
		BtnAddWebsite.TabIndex = 5
		BtnAddWebsite.Text = "Add"
		BtnAddWebsite.UseVisualStyleBackColor = True
		' 
		' LVWebsites
		' 
		LVWebsites.Columns.AddRange(New ColumnHeader() {CHName, CHUrl})
		LVWebsites.FullRowSelect = True
		LVWebsites.GridLines = True
		LVWebsites.Location = New Point(6, 68)
		LVWebsites.Name = "LVWebsites"
		LVWebsites.Size = New Size(305, 190)
		LVWebsites.TabIndex = 4
		LVWebsites.UseCompatibleStateImageBehavior = False
		LVWebsites.View = View.Details
		' 
		' CHName
		' 
		CHName.Text = "Name"
		CHName.Width = 120
		' 
		' CHUrl
		' 
		CHUrl.Text = "URL"
		CHUrl.Width = 180
		' 
		' Label8
		' 
		Label8.AutoSize = True
		Label8.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
		Label8.Location = New Point(6, 3)
		Label8.Name = "Label8"
		Label8.Size = New Size(148, 20)
		Label8.TabIndex = 3
		Label8.Text = "Configured Services"
		' 
		' BtnResetSettings
		' 
		BtnResetSettings.BackColor = SystemColors.Control
		BtnResetSettings.FlatStyle = FlatStyle.Popup
		BtnResetSettings.Location = New Point(6, 179)
		BtnResetSettings.Name = "BtnResetSettings"
		BtnResetSettings.Size = New Size(127, 23)
		BtnResetSettings.TabIndex = 9
		BtnResetSettings.Text = "Reset Settings"
		BtnResetSettings.UseVisualStyleBackColor = False
		' 
		' Label11
		' 
		Label11.AutoSize = True
		Label11.Location = New Point(6, 152)
		Label11.Name = "Label11"
		Label11.Size = New Size(172, 15)
		Label11.TabIndex = 8
		Label11.Text = "Reset the application's settings." & vbLf
		' 
		' Label10
		' 
		Label10.AutoSize = True
		Label10.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
		Label10.Location = New Point(6, 123)
		Label10.Name = "Label10"
		Label10.Size = New Size(48, 20)
		Label10.TabIndex = 7
		Label10.Text = "Reset"
		' 
		' CHKDebug
		' 
		CHKDebug.AutoSize = True
		CHKDebug.Location = New Point(6, 65)
		CHKDebug.Name = "CHKDebug"
		CHKDebug.Size = New Size(95, 19)
		CHKDebug.TabIndex = 6
		CHKDebug.Text = "Debug mode"
		CHKDebug.UseVisualStyleBackColor = True
		' 
		' CHKLogging
		' 
		CHKLogging.AutoSize = True
		CHKLogging.Location = New Point(6, 40)
		CHKLogging.Name = "CHKLogging"
		CHKLogging.Size = New Size(105, 19)
		CHKLogging.TabIndex = 5
		CHKLogging.Text = "Enable logging"
		CHKLogging.UseVisualStyleBackColor = True
		' 
		' Label9
		' 
		Label9.AutoSize = True
		Label9.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
		Label9.Location = New Point(6, 3)
		Label9.Name = "Label9"
		Label9.Size = New Size(90, 20)
		Label9.TabIndex = 4
		Label9.Text = "Diagnostics"
		' 
		' pnlHeader
		' 
		pnlHeader.Controls.Add(lblDescription)
		pnlHeader.Controls.Add(lblTitle)
		pnlHeader.Dock = DockStyle.Top
		pnlHeader.Location = New Point(0, 0)
		pnlHeader.Name = "pnlHeader"
		pnlHeader.Size = New Size(466, 65)
		pnlHeader.TabIndex = 0
		' 
		' lblDescription
		' 
		lblDescription.AutoSize = True
		lblDescription.Location = New Point(22, 38)
		lblDescription.Name = "lblDescription"
		lblDescription.Size = New Size(186, 15)
		lblDescription.TabIndex = 1
		lblDescription.Text = "Configure your Admin Dashboard"
		' 
		' lblTitle
		' 
		lblTitle.AutoSize = True
		lblTitle.Font = New Font("Segoe UI", 18F, FontStyle.Bold)
		lblTitle.Location = New Point(20, 10)
		lblTitle.Name = "lblTitle"
		lblTitle.Size = New Size(106, 32)
		lblTitle.TabIndex = 0
		lblTitle.Text = "Settings"
		' 
		' pnlButtons
		' 
		pnlButtons.Controls.Add(BtnOK)
		pnlButtons.Controls.Add(BtnApply)
		pnlButtons.Dock = DockStyle.Bottom
		pnlButtons.Location = New Point(0, 395)
		pnlButtons.Name = "pnlButtons"
		pnlButtons.Size = New Size(466, 55)
		pnlButtons.TabIndex = 1
		' 
		' BtnOK
		' 
		BtnOK.BackColor = SystemColors.Control
		BtnOK.FlatStyle = FlatStyle.Popup
		BtnOK.Location = New Point(371, 13)
		BtnOK.Name = "BtnOK"
		BtnOK.Size = New Size(80, 30)
		BtnOK.TabIndex = 2
		BtnOK.Text = "Close"
		BtnOK.UseVisualStyleBackColor = False
		' 
		' BtnApply
		' 
		BtnApply.FlatStyle = FlatStyle.Popup
		BtnApply.Location = New Point(285, 13)
		BtnApply.Name = "BtnApply"
		BtnApply.Size = New Size(80, 30)
		BtnApply.TabIndex = 0
		BtnApply.Text = "Apply"
		BtnApply.UseVisualStyleBackColor = True
		' 
		' CTCSettings
		' 
		CTCSettings.Alignment = TabAlignment.Left
		CTCSettings.Controls.Add(TPGeneral)
		CTCSettings.Controls.Add(TPAppearances)
		CTCSettings.Controls.Add(TPWebView)
		CTCSettings.Controls.Add(TPWebsites)
		CTCSettings.Controls.Add(TPAdvanced)
		CTCSettings.Dock = DockStyle.Fill
		CTCSettings.DrawMode = TabDrawMode.OwnerDrawFixed
		CTCSettings.ItemSize = New Size(32, 140)
		CTCSettings.Location = New Point(0, 65)
		CTCSettings.Multiline = True
		CTCSettings.Name = "CTCSettings"
		CTCSettings.SelectedIndex = 0
		CTCSettings.Size = New Size(466, 330)
		CTCSettings.SizeMode = TabSizeMode.Fixed
		CTCSettings.TabIndex = 2
		' 
		' TPGeneral
		' 
		TPGeneral.Controls.Add(Label12)
		TPGeneral.Controls.Add(CHKFullScreen)
		TPGeneral.Controls.Add(Label3)
		TPGeneral.Controls.Add(Label1)
		TPGeneral.Controls.Add(CHKOpenLast)
		TPGeneral.Controls.Add(CHKStartDefault)
		TPGeneral.Controls.Add(Label2)
		TPGeneral.Location = New Point(144, 4)
		TPGeneral.Name = "TPGeneral"
		TPGeneral.Padding = New Padding(3)
		TPGeneral.Size = New Size(318, 322)
		TPGeneral.TabIndex = 0
		TPGeneral.Text = "General"
		TPGeneral.UseVisualStyleBackColor = True
		' 
		' Label12
		' 
		Label12.AutoSize = True
		Label12.Location = New Point(6, 168)
		Label12.Name = "Label12"
		Label12.Size = New Size(142, 15)
		Label12.TabIndex = 6
		Label12.Text = "Set to Full Screen window"
		' 
		' CHKFullScreen
		' 
		CHKFullScreen.AutoSize = True
		CHKFullScreen.Location = New Point(6, 146)
		CHKFullScreen.Name = "CHKFullScreen"
		CHKFullScreen.Size = New Size(80, 19)
		CHKFullScreen.TabIndex = 5
		CHKFullScreen.Text = "FullScreen"
		CHKFullScreen.UseVisualStyleBackColor = True
		' 
		' TPAppearances
		' 
		TPAppearances.Controls.Add(CHKShowNavigation)
		TPAppearances.Controls.Add(lblTheme)
		TPAppearances.Controls.Add(Label4)
		TPAppearances.Controls.Add(CMBTheme)
		TPAppearances.Controls.Add(lblThemeDescription)
		TPAppearances.Location = New Point(144, 4)
		TPAppearances.Name = "TPAppearances"
		TPAppearances.Padding = New Padding(3)
		TPAppearances.Size = New Size(318, 322)
		TPAppearances.TabIndex = 1
		TPAppearances.Text = "Appearance"
		TPAppearances.UseVisualStyleBackColor = True
		' 
		' TPWebView
		' 
		TPWebView.Controls.Add(Label7)
		TPWebView.Controls.Add(Label5)
		TPWebView.Controls.Add(CHKInvalidCertificates)
		TPWebView.Controls.Add(CHKDevTools)
		TPWebView.Controls.Add(Label6)
		TPWebView.Controls.Add(CHKContextMenu)
		TPWebView.Location = New Point(144, 4)
		TPWebView.Name = "TPWebView"
		TPWebView.Size = New Size(318, 322)
		TPWebView.TabIndex = 2
		TPWebView.Text = "WebView"
		TPWebView.UseVisualStyleBackColor = True
		' 
		' TPWebsites
		' 
		TPWebsites.Controls.Add(BtnMoveDown)
		TPWebsites.Controls.Add(Label8)
		TPWebsites.Controls.Add(BtnMoveUp)
		TPWebsites.Controls.Add(LVWebsites)
		TPWebsites.Controls.Add(BtnRemoveWebsite)
		TPWebsites.Controls.Add(BtnAddWebsite)
		TPWebsites.Controls.Add(BtnEditWebsite)
		TPWebsites.Location = New Point(144, 4)
		TPWebsites.Name = "TPWebsites"
		TPWebsites.Size = New Size(318, 322)
		TPWebsites.TabIndex = 3
		TPWebsites.Text = "Websites"
		TPWebsites.UseVisualStyleBackColor = True
		' 
		' TPAdvanced
		' 
		TPAdvanced.Controls.Add(BtnResetSettings)
		TPAdvanced.Controls.Add(Label9)
		TPAdvanced.Controls.Add(Label11)
		TPAdvanced.Controls.Add(CHKLogging)
		TPAdvanced.Controls.Add(Label10)
		TPAdvanced.Controls.Add(CHKDebug)
		TPAdvanced.Location = New Point(144, 4)
		TPAdvanced.Name = "TPAdvanced"
		TPAdvanced.Size = New Size(318, 322)
		TPAdvanced.TabIndex = 4
		TPAdvanced.Text = "Advanced"
		TPAdvanced.UseVisualStyleBackColor = True
		' 
		' SettingsForm
		' 
		AutoScaleDimensions = New SizeF(7F, 15F)
		AutoScaleMode = AutoScaleMode.Font
		ClientSize = New Size(466, 450)
		Controls.Add(CTCSettings)
		Controls.Add(pnlButtons)
		Controls.Add(pnlHeader)
		FormBorderStyle = FormBorderStyle.FixedToolWindow
		Name = "SettingsForm"
		StartPosition = FormStartPosition.CenterScreen
		Text = "Settings"
		pnlHeader.ResumeLayout(False)
		pnlHeader.PerformLayout()
		pnlButtons.ResumeLayout(False)
		CTCSettings.ResumeLayout(False)
		TPGeneral.ResumeLayout(False)
		TPGeneral.PerformLayout()
		TPAppearances.ResumeLayout(False)
		TPAppearances.PerformLayout()
		TPWebView.ResumeLayout(False)
		TPWebView.PerformLayout()
		TPWebsites.ResumeLayout(False)
		TPWebsites.PerformLayout()
		TPAdvanced.ResumeLayout(False)
		TPAdvanced.PerformLayout()
		ResumeLayout(False)
	End Sub
	Friend WithEvents pnlHeader As Panel
	Friend WithEvents lblTitle As Label
	Friend WithEvents CHKStartDefault As CheckBox
	Friend WithEvents Label1 As Label
	Friend WithEvents lblDescription As Label
	Friend WithEvents Label3 As Label
	Friend WithEvents CHKOpenLast As CheckBox
	Friend WithEvents Label2 As Label
	Friend WithEvents CHKShowNavigation As CheckBox
	Friend WithEvents Label4 As Label
	Friend WithEvents lblThemeDescription As Label
	Friend WithEvents CMBTheme As ComboBox
	Friend WithEvents lblTheme As Label
	Friend WithEvents Label7 As Label
	Friend WithEvents CHKInvalidCertificates As CheckBox
	Friend WithEvents Label6 As Label
	Friend WithEvents CHKContextMenu As CheckBox
	Friend WithEvents CHKDevTools As CheckBox
	Friend WithEvents Label5 As Label
	Friend WithEvents BtnMoveDown As Button
	Friend WithEvents BtnMoveUp As Button
	Friend WithEvents BtnRemoveWebsite As Button
	Friend WithEvents BtnEditWebsite As Button
	Friend WithEvents BtnAddWebsite As Button
	Friend WithEvents LVWebsites As ListView
	Friend WithEvents CHName As ColumnHeader
	Friend WithEvents CHUrl As ColumnHeader
	Friend WithEvents Label8 As Label
	Friend WithEvents Label10 As Label
	Friend WithEvents CHKDebug As CheckBox
	Friend WithEvents CHKLogging As CheckBox
	Friend WithEvents Label9 As Label
	Friend WithEvents BtnResetSettings As Button
	Friend WithEvents Label11 As Label
	Friend WithEvents pnlButtons As Panel
	Friend WithEvents BtnApply As Button
	Friend WithEvents BtnOK As Button
	Friend WithEvents CTCSettings As CustomLeftTabControl
	Friend WithEvents TPGeneral As TabPage
	Friend WithEvents TPAppearances As TabPage
	Friend WithEvents TPWebView As TabPage
	Friend WithEvents TPWebsites As TabPage
	Friend WithEvents TPAdvanced As TabPage
	Friend WithEvents Label12 As Label
	Friend WithEvents CHKFullScreen As CheckBox
End Class
