Public Class SettingsForm
	Private Sub SettingsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		TCSettings.Alignment = TabAlignment.Left
		TCSettings.DrawMode = TabDrawMode.OwnerDrawFixed
		TCSettings.SizeMode = TabSizeMode.Fixed
		TCSettings.ItemSize = New Size(23, 150)
	End Sub

	Private Sub TCSetting_DrawItem(sender As Object, e As DrawItemEventArgs) Handles TCSettings.DrawItem
		Dim tabcontrol As TabControl = DirectCast(sender, TabControl)
		Dim tabPage As TabPage = TabControl.TabPages(e.Index)
		Dim tabRect As Rectangle = tabcontrol.GetTabRect(e.Index)

		If e.Index = tabcontrol.SelectedIndex Then
			e.Graphics.FillRectangle(Brushes.DodgerBlue, tabRect)
		Else
			e.Graphics.FillRectangle(Brushes.LightGray, tabRect)
		End If

		Dim textColor As Color = If(e.Index = tabcontrol.SelectedIndex, Color.White, Color.Black)
		Using font As New Font("Segoe UI", 9, FontStyle.Regular)
			Dim textSize As SizeF = e.Graphics.MeasureString(tabPage.Text, font)
			Dim x As Single = tabRect.X + (tabRect.Width - textSize.Width) / 2
			Dim y As Single = tabRect.Y + (tabRect.Height - textSize.Height) / 2

			Using brush As New SolidBrush(textColor)
				e.Graphics.DrawString(tabPage.Text, font, brush, x, y)
			End Using
		End Using
	End Sub
End Class