Imports System.Runtime
Imports System.Drawing

Public Class SettingsForm
	Private tempWebsite As List(Of WebsiteItem)
	Private websitesChanged As Boolean = False

	Private Sub SettingsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

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

		BtnApply.Enabled = False
		BtnEditWebsite.Enabled = False
		BtnRemoveWebsite.Enabled = False
		BtnMoveUp.Enabled = False
		BtnMoveDown.Enabled = False

		CHKDevTools.Checked = SettingsModule.EnableDevTools
		CHKContextMenu.Checked = SettingsModule.EnableContextMenu
		CHKInvalidCertificates.Checked = SettingsModule.AllowInvalidCertificates
		CHKLogging.Checked = SettingsModule.EnableLogging
		CHKDebug.Checked = SettingsModule.DebugMode

		tempWebsite = GetWebsites()
		websitesChanged = False
		LoadWebsiteList()

		CheckIfNeedRestart()

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

	Private Sub CheckIfNeedRestart()
		Dim changeTextBtn As Boolean = (CHKDevTools.Checked <> SettingsModule.EnableDevTools) OrElse (CHKContextMenu.Checked <> SettingsModule.EnableContextMenu)

		lblRestartInfo.Visible = (CHKDevTools.Checked <> SettingsModule.EnableDevTools) OrElse (CHKContextMenu.Checked <> SettingsModule.EnableContextMenu)

		If changeTextBtn Then
			BtnApply.Text = "Apply & Restart"
			BtnApply.AutoSize = True
			BtnApply.Location = New Point(275, 13)
		Else
			BtnApply.Text = "Apply"
			BtnApply.AutoSize = False
			BtnApply.Location = New Point(285, 13)
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
		Dim needRestart As Boolean = (CHKDevTools.Checked <> SettingsModule.EnableDevTools) OrElse (CHKContextMenu.Checked <> SettingsModule.EnableContextMenu)

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

		ApplySettingsTheme()

		Dim dashboard As maindashboard =
			TryCast(Application.OpenForms("maindashboard"), maindashboard)

		If needRestart Then
			Application.Restart()
		Else
			If dashboard IsNot Nothing Then
				dashboard.ApplyApplicationTheme()
				dashboard.LoadWebsiteButtons()
				dashboard.HighlightActiveButton()
			End If

			MessageBox.Show(
				"Settings applied successfully.",
				"Settings",
				MessageBoxButtons.OK,
				MessageBoxIcon.Information)
		End If

		If websitesChanged Then
			SaveWebsites(tempWebsite)
			websitesChanged = False
		End If

		BtnApply.Enabled = False
	End Sub

	'========================================================
	' WEBSITE LIST
	'========================================================
	Private Sub LoadWebsiteList()
		LVWebsites.Items.Clear()

		If tempWebsite Is Nothing Then
			tempWebsite = New List(Of WebsiteItem)
		End If

		For Each website As WebsiteItem In tempWebsite
			Dim item As New ListViewItem(website.name)
			item.SubItems.Add(website.url)
			item.Tag = website
			LVWebsites.Items.Add(item)
		Next
	End Sub

	Private Function HasSettingsChanged() As Boolean
		If websitesChanged Then Return True

		Dim currentTheme As String = "system"
		If CMBTheme.SelectedItem IsNot Nothing Then
			currentTheme = CMBTheme.SelectedItem.ToString().ToLower()
		End If
		If currentTheme <> SettingsModule.Theme.ToLower() Then Return True

		If CHKStartDefault.Checked <> SettingsModule.StartWithDefaultPage Then Return True
		If CHKOpenLast.Checked <> SettingsModule.OpenLastWebsite Then Return True
		If CHKFullScreen.Checked <> SettingsModule.IsFullScreen Then Return True
		If CHKDevTools.Checked <> SettingsModule.EnableDevTools Then Return True
		If CHKContextMenu.Checked <> SettingsModule.EnableContextMenu Then Return True
		If CHKInvalidCertificates.Checked <> SettingsModule.AllowInvalidCertificates Then Return True
		If CHKLogging.Checked <> SettingsModule.EnableLogging Then Return True
		If CHKDebug.Checked <> SettingsModule.DebugMode Then Return True

		Return False
	End Function

	Private Sub UpdateApplyButtonState()
		BtnApply.Enabled = HasSettingsChanged()
		CheckIfNeedRestart()
	End Sub

	Private Sub CHKStartDefault_CheckedChanged(sender As Object, e As EventArgs) Handles CHKStartDefault.CheckedChanged
		UpdateApplyButtonState()
	End Sub

	Private Sub CHKOpenLast_CheckedChanged(sender As Object, e As EventArgs) Handles CHKOpenLast.CheckedChanged
		UpdateApplyButtonState()
	End Sub

	Private Sub CHKFullScreen_CheckedChanged(sender As Object, e As EventArgs) Handles CHKFullScreen.CheckedChanged
		UpdateApplyButtonState()
	End Sub

	Private Sub CMBTheme_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CMBTheme.SelectedIndexChanged
		UpdateApplyButtonState()
	End Sub

	Private Sub CHKDevTools_CheckedChanged(sender As Object, e As EventArgs) Handles CHKDevTools.CheckedChanged
		BtnApply.Enabled = HasSettingsChanged()
		CheckIfNeedRestart()
	End Sub

	Private Sub CHKContextMenu_CheckedChanged(sender As Object, e As EventArgs) Handles CHKContextMenu.CheckedChanged
		BtnApply.Enabled = HasSettingsChanged()
		CheckIfNeedRestart()
	End Sub

	Private Sub CHKInvalidCertificates_CheckedChanged(sender As Object, e As EventArgs) Handles CHKInvalidCertificates.CheckedChanged
		UpdateApplyButtonState()
	End Sub

	Private Sub CHKLogging_CheckedChanged(sender As Object, e As EventArgs) Handles CHKLogging.CheckedChanged
		UpdateApplyButtonState()
	End Sub

	Private Sub CHKDebug_CheckedChanged(sender As Object, e As EventArgs) Handles CHKDebug.CheckedChanged
		UpdateApplyButtonState()
	End Sub

	Private Sub BtnAddWebsite_Click(sender As Object, e As EventArgs) Handles BtnAddWebsite.Click
		Dim result = CustomDesignGUI.WebsiteInputBox("Add Website", , , True)
		If result Is Nothing Then Return

		Dim newId As Integer = 0
		If tempWebsite.Count > 0 Then
			newId = tempWebsite.Max(Function(x) x.id) + 1
		End If

		tempWebsite.Add(New WebsiteItem With {
			.id = newId,
			.name = result.Name,
			.url = result.Url
		})

		websitesChanged = True
		LoadWebsiteList()
		UpdateApplyButtonState()
	End Sub

	Private Sub BtnEditWebsite_Click(sender As Object, e As EventArgs) Handles BtnEditWebsite.Click
		If LVWebsites.SelectedItems.Count = 0 Then
			MessageBox.Show("Please select a website to edit.", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Return
		End If

		Dim selectedItem = LVWebsites.SelectedItems(0)
		Dim website = TryCast(selectedItem.Tag, WebsiteItem)
		If website Is Nothing Then Return

		' Pass current values so they appear in the dialog
		Dim result = CustomDesignGUI.WebsiteInputBox("Edit Website", website.name, website.url, False)
		If result Is Nothing Then Return

		Dim websites = GetWebsites()
		Dim existing = tempWebsite.FirstOrDefault(Function(x) x.id = website.id)

		If existing IsNot Nothing Then
			existing.name = result.Name
			existing.url = result.Url
			websitesChanged = True
			LoadWebsiteList()
			UpdateApplyButtonState()
		End If
	End Sub

	Private Sub BtnMoveUp_Click(sender As Object, e As EventArgs) Handles BtnMoveUp.Click
		If LVWebsites.SelectedItems.Count = 0 Then Return

		Dim index = LVWebsites.SelectedIndices(0)
		If index <= 0 Then Return

		Dim item = tempWebsite(index)
		tempWebsite.RemoveAt(index)
		tempWebsite.Insert(index - 1, item)

		websitesChanged = True

		LoadWebsiteList()

		' Keep selection
		LVWebsites.Items(index - 1).Selected = True
		UpdateApplyButtonState()
	End Sub

	Private Sub BtnMoveDown_Click(sender As Object, e As EventArgs) Handles BtnMoveDown.Click
		If LVWebsites.SelectedItems.Count = 0 Then Return

		Dim index = LVWebsites.SelectedIndices(0)
		If index >= LVWebsites.Items.Count - 1 Then Return

		Dim item = tempWebsite(index)
		tempWebsite.RemoveAt(index)
		tempWebsite.Insert(index + 1, item)

		websitesChanged = True

		LoadWebsiteList()

		' Keep selection
		LVWebsites.Items(index + 1).Selected = True
		UpdateApplyButtonState()
	End Sub

	Private Sub BtnRemoveWebsite_Click(sender As Object, e As EventArgs) Handles BtnRemoveWebsite.Click
		If LVWebsites.SelectedItems.Count = 0 Then
			MessageBox.Show("Please select a website to delete.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Return
		End If

		Dim selectedItem = LVWebsites.SelectedItems(0)
		Dim website = TryCast(selectedItem.Tag, WebsiteItem)
		If website Is Nothing Then Return

		' Confirm
		Dim confirm = MessageBox.Show(
			$"Are you sure you want to delete '{website.name}'?",
			"Delete Website",
			MessageBoxButtons.YesNo,
			MessageBoxIcon.Question)

		If confirm <> DialogResult.Yes Then Return

		' Remove from list
		Dim itemToRemove = tempWebsite.FirstOrDefault(Function(x) x.id = website.id)

		If itemToRemove IsNot Nothing Then
			tempWebsite.Remove(itemToRemove)
			websitesChanged = True
			LoadWebsiteList()
			UpdateApplyButtonState()
		End If
	End Sub

	Private Sub BtnResetSettings_Click(sender As Object, e As EventArgs) Handles BtnResetSettings.Click
		Dim result = MessageBox.Show(
			"Are you sure you want to reset all settings to default?",
			"Reset Settings",
			MessageBoxButtons.YesNo,
			MessageBoxIcon.Warning)

		If result <> DialogResult.Yes Then Return

		SettingsModule.ResetSettingsConfig()

		Application.Restart()
		Me.Close()
	End Sub

	Private Sub LVWebsites_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LVWebsites.SelectedIndexChanged
		Dim hasSelection As Boolean = LVWebsites.SelectedItems.Count > 0

		BtnEditWebsite.Enabled = hasSelection
		BtnRemoveWebsite.Enabled = hasSelection
		BtnMoveUp.Enabled = hasSelection
		BtnMoveDown.Enabled = hasSelection
	End Sub
End Class