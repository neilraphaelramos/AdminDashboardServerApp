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
		TCSettings = New TabControl()
		TPGeneral = New TabPage()
		TPAppearance = New TabPage()
		TPWebView = New TabPage()
		TPWebsites = New TabPage()
		TPAdvanced = New TabPage()
		TCSettings.SuspendLayout()
		SuspendLayout()
		' 
		' TCSettings
		' 
		TCSettings.Alignment = TabAlignment.Left
		TCSettings.Controls.Add(TPGeneral)
		TCSettings.Controls.Add(TPAppearance)
		TCSettings.Controls.Add(TPWebView)
		TCSettings.Controls.Add(TPWebsites)
		TCSettings.Controls.Add(TPAdvanced)
		TCSettings.Dock = DockStyle.Fill
		TCSettings.DrawMode = TabDrawMode.OwnerDrawFixed
		TCSettings.ItemSize = New Size(23, 150)
		TCSettings.Location = New Point(0, 0)
		TCSettings.Multiline = True
		TCSettings.Name = "TCSettings"
		TCSettings.SelectedIndex = 0
		TCSettings.Size = New Size(800, 450)
		TCSettings.SizeMode = TabSizeMode.Fixed
		TCSettings.TabIndex = 0
		' 
		' TPGeneral
		' 
		TPGeneral.Location = New Point(154, 4)
		TPGeneral.Name = "TPGeneral"
		TPGeneral.Padding = New Padding(3)
		TPGeneral.Size = New Size(642, 442)
		TPGeneral.TabIndex = 0
		TPGeneral.Text = "General"
		TPGeneral.UseVisualStyleBackColor = True
		' 
		' TPAppearance
		' 
		TPAppearance.Location = New Point(154, 4)
		TPAppearance.Name = "TPAppearance"
		TPAppearance.Padding = New Padding(3)
		TPAppearance.Size = New Size(642, 442)
		TPAppearance.TabIndex = 1
		TPAppearance.Text = "Appearance"
		TPAppearance.UseVisualStyleBackColor = True
		' 
		' TPWebView
		' 
		TPWebView.Location = New Point(154, 4)
		TPWebView.Name = "TPWebView"
		TPWebView.Size = New Size(642, 442)
		TPWebView.TabIndex = 2
		TPWebView.Text = "WebView"
		TPWebView.UseVisualStyleBackColor = True
		' 
		' TPWebsites
		' 
		TPWebsites.Location = New Point(154, 4)
		TPWebsites.Name = "TPWebsites"
		TPWebsites.Size = New Size(642, 442)
		TPWebsites.TabIndex = 3
		TPWebsites.Text = "Websites"
		TPWebsites.UseVisualStyleBackColor = True
		' 
		' TPAdvanced
		' 
		TPAdvanced.Location = New Point(154, 4)
		TPAdvanced.Name = "TPAdvanced"
		TPAdvanced.Size = New Size(642, 442)
		TPAdvanced.TabIndex = 4
		TPAdvanced.Text = "Advanced"
		TPAdvanced.UseVisualStyleBackColor = True
		' 
		' SettingsForm
		' 
		AutoScaleDimensions = New SizeF(7F, 15F)
		AutoScaleMode = AutoScaleMode.Font
		ClientSize = New Size(800, 450)
		Controls.Add(TCSettings)
		FormBorderStyle = FormBorderStyle.FixedToolWindow
		Name = "SettingsForm"
		StartPosition = FormStartPosition.CenterScreen
		Text = "Settings"
		TCSettings.ResumeLayout(False)
		ResumeLayout(False)
	End Sub

	Friend WithEvents TCSettings As TabControl
	Friend WithEvents TPGeneral As TabPage
	Friend WithEvents TPAppearance As TabPage
	Friend WithEvents TPWebView As TabPage
	Friend WithEvents TPWebsites As TabPage
	Friend WithEvents TPAdvanced As TabPage
End Class
