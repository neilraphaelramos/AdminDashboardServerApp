Public Class CustomDesignGUI

    '=========================================================
    ' WEBSITE INPUT
    '=========================================================

    Public Class WebsiteInputResult
        Public Property Name As String
        Public Property Url As String
    End Class


    Public Shared Function WebsiteInputBox(title As String, Optional defaultname As String = "", Optional defaultUrl As String = "", Optional updateorok As Boolean = False, Optional themeSet As String = Nothing) As WebsiteInputResult

        Dim setText As String = ""

        Dim theme As String = If(String.IsNullOrWhiteSpace(themeSet), AutoThemeModule.ResolveTheme(), themeSet).ToLower()

        Dim isDark As Boolean

        If theme = "dark" Then
            isDark = True
        ElseIf theme = "light" Then
            isDark = False
        Else
            isDark = AutoThemeModule.IsResolvedDark()
        End If

        If updateorok Then
            setText = "Add"
        Else
            setText = "Update"
        End If

        Dim formBack As Color = If(isDark, Color.FromArgb(30, 30, 30), Color.FromArgb(245, 245, 245))
        Dim formFore As Color = If(isDark, Color.White, Color.FromArgb(30, 30, 30))
        Dim inputBack As Color = If(isDark, Color.FromArgb(45, 45, 45), Color.White)
        Dim inputFore As Color = If(isDark, Color.White, Color.FromArgb(30, 30, 30))
        Dim btnBack As Color = If(isDark, Color.FromArgb(60, 60, 60), Color.FromArgb(230, 230, 230))
        Dim btnBorder As Color = If(isDark, Color.FromArgb(80, 80, 80), Color.FromArgb(180, 180, 180))

        Using frm As New Form()

            frm.Text = title
            frm.StartPosition = FormStartPosition.CenterParent
            frm.FormBorderStyle = FormBorderStyle.FixedDialog
            frm.ClientSize = New Size(360, 195)
            frm.MinimizeBox = False
            frm.MaximizeBox = False
            frm.BackColor = formBack
            frm.ForeColor = formFore
            frm.ShowInTaskbar = False

            Dim lblname As New Label()
            lblname.Text = "Name of Website"
            lblname.Location = New Point(15, 15)
            lblname.AutoSize = True
            lblname.ForeColor = formFore

            Dim txtname As New TextBox()
            txtname.Location = New Point(15, 40)
            txtname.Width = 330
            txtname.BackColor = inputBack
            txtname.ForeColor = inputFore
            txtname.BorderStyle = BorderStyle.FixedSingle
            txtname.Text = defaultname

            Dim lblUrl As New Label()
            lblUrl.Text = "Web URL"
            lblUrl.Location = New Point(15, 75)
            lblUrl.AutoSize = True
            lblUrl.ForeColor = formFore

            Dim txtUrl As New TextBox()
            txtUrl.Location = New Point(15, 100)
            txtUrl.Width = 330
            txtUrl.BackColor = inputBack
            txtUrl.ForeColor = inputFore
            txtUrl.BorderStyle = BorderStyle.FixedSingle
            txtUrl.Text = defaultUrl

            Dim btnOK As New Button()
            btnOK.Text = setText
            btnOK.Location = New Point(180, 150)
            btnOK.Size = New Size(75, 30)
            btnOK.BackColor = inputBack
            btnOK.ForeColor = formFore
            btnOK.FlatStyle = FlatStyle.Flat
            btnOK.FlatAppearance.BorderColor = btnBorder
            btnOK.DialogResult = DialogResult.OK

            Dim btnCancel As New Button()
            btnCancel.Text = "Cancel"
            btnCancel.Location = New Point(270, 150)
            btnCancel.Size = New Size(75, 30)
            btnCancel.BackColor = inputBack
            btnCancel.ForeColor = formFore
            btnCancel.FlatStyle = FlatStyle.Flat
            btnCancel.FlatAppearance.BorderColor = btnBorder
            btnCancel.DialogResult = DialogResult.Cancel

            frm.Controls.Add(lblname)
            frm.Controls.Add(txtname)
            frm.Controls.Add(lblUrl)
            frm.Controls.Add(txtUrl)
            frm.Controls.Add(btnOK)
            frm.Controls.Add(btnCancel)

            frm.AcceptButton = btnOK
            frm.CancelButton = btnCancel

            txtname.Select()

            If frm.ShowDialog() = DialogResult.OK Then

                If String.IsNullOrWhiteSpace(txtname.Text) Then
                    Return Nothing
                End If

                If String.IsNullOrWhiteSpace(txtUrl.Text) Then
                    Return Nothing
                End If

                Return New WebsiteInputResult With {
                    .Name = txtname.Text.Trim(),
                    .Url = txtUrl.Text.Trim()
                }

            End If

            Return Nothing

        End Using

    End Function

    Public Shared Function ThemedMessageBox(
    text As String,
    Optional title As String = "Confirm",
    Optional buttons As MessageBoxButtons = MessageBoxButtons.YesNo,
    Optional icon As MessageBoxIcon = MessageBoxIcon.Question,
    Optional themeSet As String = "system"
) As DialogResult

        Using frm As New Form()
            '---------- Form ----------
            frm.Text = title
            frm.StartPosition = FormStartPosition.CenterParent
            frm.FormBorderStyle = FormBorderStyle.FixedDialog
            frm.ClientSize = New Size(380, 160)
            frm.MinimizeBox = False
            frm.MaximizeBox = False
            frm.ShowInTaskbar = False
            frm.Font = New Font("Segoe UI", 9.0F)

            ' Colors
            Dim backColor As Color
            Dim foreColor As Color
            Dim buttonBack As Color
            Dim buttonBorder As Color

            If themeSet = "system" OrElse themeSet = "dark" Then
                backColor = Color.FromArgb(30, 30, 30)
                foreColor = Color.White
                buttonBack = Color.FromArgb(60, 60, 60)
                buttonBorder = Color.FromArgb(80, 80, 80)
            Else
                backColor = Color.FromArgb(245, 245, 245)
                foreColor = Color.Black
                buttonBack = Color.FromArgb(230, 230, 230)
                buttonBorder = Color.FromArgb(180, 180, 180)
            End If

            frm.BackColor = backColor
            frm.ForeColor = foreColor

            '---------- Message Label ----------
            Dim lblMessage As New Label()
            lblMessage.Text = text
            lblMessage.Location = New Point(20, 25)
            lblMessage.Size = New Size(340, 60)
            lblMessage.ForeColor = foreColor
            lblMessage.BackColor = Color.Transparent

            '---------- Buttons ----------
            Dim btn1 As New Button()
            Dim btn2 As New Button()

            btn1.Size = New Size(85, 30)
            btn2.Size = New Size(85, 30)
            btn1.FlatStyle = FlatStyle.Flat
            btn2.FlatStyle = FlatStyle.Flat
            btn1.BackColor = buttonBack
            btn2.BackColor = buttonBack
            btn1.ForeColor = foreColor
            btn2.ForeColor = foreColor
            btn1.FlatAppearance.BorderColor = buttonBorder
            btn2.FlatAppearance.BorderColor = buttonBorder

            Select Case buttons
                Case MessageBoxButtons.YesNo
                    btn1.Text = "Yes"
                    btn1.DialogResult = DialogResult.Yes
                    btn1.Location = New Point(180, 110)

                    btn2.Text = "No"
                    btn2.DialogResult = DialogResult.No
                    btn2.Location = New Point(275, 110)

                    frm.Controls.Add(btn1)
                    frm.Controls.Add(btn2)
                    frm.AcceptButton = btn1
                    frm.CancelButton = btn2

                Case MessageBoxButtons.OKCancel
                    btn1.Text = "OK"
                    btn1.DialogResult = DialogResult.OK
                    btn1.Location = New Point(180, 110)

                    btn2.Text = "Cancel"
                    btn2.DialogResult = DialogResult.Cancel
                    btn2.Location = New Point(275, 110)

                    frm.Controls.Add(btn1)
                    frm.Controls.Add(btn2)
                    frm.AcceptButton = btn1
                    frm.CancelButton = btn2

                Case Else ' OK only
                    btn1.Text = "OK"
                    btn1.DialogResult = DialogResult.OK
                    btn1.Location = New Point(275, 110)

                    frm.Controls.Add(btn1)
                    frm.AcceptButton = btn1
            End Select

            frm.Controls.Add(lblMessage)

            Return frm.ShowDialog()
        End Using
    End Function
End Class

