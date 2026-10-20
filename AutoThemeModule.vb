Module AutoThemeModule
	Public Function ResolveTheme() As String
		If SettingsModule.AutoTheme Then
			Return If(IsAutoThemeDarkNow(), "dark", "light")
		End If
		Return SettingsModule.Theme
	End Function

	Public Function isDarkNow() As Boolean
		Dim hour As Integer = DateTime.Now.Hour
		Dim darkStart As Integer = SettingsModule.AutoThemeDarkHour
		Dim lightStart As Integer = SettingsModule.AutoThemeLightHour

		If darkStart = lightStart Then
			Return True
		End If

		If darkStart > lightStart Then
			Return hour >= darkStart OrElse hour < lightStart
		Else
			Return hour >= darkStart AndAlso hour < lightStart
		End If
	End Function

	Public Function IsResolvedDark() As Boolean
		Dim t = ResolveTheme().ToLower()
		If t = "dark" Then Return True
		If t = "light" Then Return False
		Return IsWindowsDarkMode()
	End Function

	Private Function IsWindowsDarkMode() As Boolean
		Try
			Using key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
				"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize")
				If key Is Nothing Then Return False
				Dim value = key.GetValue("AppsUseLightTheme")
				Return value IsNot Nothing AndAlso Convert.ToInt32(value) = 0
			End Using
		Catch
			Return False
		End Try
	End Function
End Module
