<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class maindashboard
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
	<System.Diagnostics.DebuggerStepThrough()>
	Private Sub InitializeComponent()
		MSMain = New MenuStrip()
		FileToolStripMenuItem = New ToolStripMenuItem()
		MSAddIP = New ToolStripMenuItem()
		MSSettings = New ToolStripMenuItem()
		WVDisplay = New WebView2()
		PnlTabs = New FlowLayoutPanel()
		MSMain.SuspendLayout()
		CType(WVDisplay, ComponentModel.ISupportInitialize).BeginInit()
		SuspendLayout()
		' 
		' MSMain
		' 
		MSMain.BackColor = Color.FromArgb(CByte(24), CByte(26), CByte(27))
		MSMain.Items.AddRange(New ToolStripItem() {FileToolStripMenuItem, MSSettings})
		MSMain.Location = New Point(0, 0)
		MSMain.Name = "MSMain"
		MSMain.Size = New Size(800, 24)
		MSMain.TabIndex = 0
		MSMain.Text = "MenuStrip1"
		' 
		' FileToolStripMenuItem
		' 
		FileToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {MSAddIP})
		FileToolStripMenuItem.ForeColor = Color.White
		FileToolStripMenuItem.Name = "FileToolStripMenuItem"
		FileToolStripMenuItem.Size = New Size(37, 20)
		FileToolStripMenuItem.Text = "File"
		' 
		' MSAddIP
		' 
		MSAddIP.BackColor = Color.FromArgb(CByte(24), CByte(26), CByte(27))
		MSAddIP.ForeColor = Color.White
		MSAddIP.Name = "MSAddIP"
		MSAddIP.Size = New Size(154, 22)
		MSAddIP.Text = "Add IP Address"
		' 
		' MSSettings
		' 
		MSSettings.ForeColor = SystemColors.ButtonHighlight
		MSSettings.Name = "MSSettings"
		MSSettings.Size = New Size(61, 20)
		MSSettings.Text = "Settings"
		' 
		' WVDisplay
		' 
		WVDisplay.AllowExternalDrop = True
		WVDisplay.CreationProperties = Nothing
		WVDisplay.DefaultBackgroundColor = Color.White
		WVDisplay.Dock = DockStyle.Fill
		WVDisplay.Location = New Point(0, 64)
		WVDisplay.Name = "WVDisplay"
		WVDisplay.Size = New Size(800, 386)
		WVDisplay.TabIndex = 1
		WVDisplay.ZoomFactor = 1R
		' 
		' PnlTabs
		' 
		PnlTabs.AutoScroll = True
		PnlTabs.Dock = DockStyle.Top
		PnlTabs.Location = New Point(0, 24)
		PnlTabs.Name = "PnlTabs"
		PnlTabs.Size = New Size(800, 40)
		PnlTabs.TabIndex = 2
		PnlTabs.WrapContents = False
		' 
		' maindashboard
		' 
		AutoScaleDimensions = New SizeF(7F, 15F)
		AutoScaleMode = AutoScaleMode.Font
		ClientSize = New Size(800, 450)
		Controls.Add(WVDisplay)
		Controls.Add(PnlTabs)
		Controls.Add(MSMain)
		MainMenuStrip = MSMain
		Name = "maindashboard"
		Text = "Admin Dashboard App"
		MSMain.ResumeLayout(False)
		MSMain.PerformLayout()
		CType(WVDisplay, ComponentModel.ISupportInitialize).EndInit()
		ResumeLayout(False)
		PerformLayout()
	End Sub

	Friend WithEvents MSMain As MenuStrip
	Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
	Friend WithEvents MSAddIP As ToolStripMenuItem
	Friend WithEvents MSSettings As ToolStripMenuItem
	Friend WithEvents WVDisplay As WebView2
	Friend WithEvents PnlTabs As FlowLayoutPanel

End Class
