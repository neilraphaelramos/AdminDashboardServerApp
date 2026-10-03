Public Class CustomDesignGUI

    '=========================================================
    ' WEBSITE INPUT
    '=========================================================

    Public Class WebsiteInputResult
        Public Property Name As String
        Public Property Url As String
    End Class


    Public Shared Function WebsiteInputBox(title As String) As WebsiteInputResult

        Using frm As New Form()

            frm.Text = title
            frm.StartPosition = FormStartPosition.CenterParent
            frm.FormBorderStyle = FormBorderStyle.FixedDialog
            frm.ClientSize = New Size(360, 195)
            frm.MinimizeBox = False
            frm.MaximizeBox = False
            frm.BackColor = Color.FromArgb(30, 30, 30)
            frm.ForeColor = Color.White
            frm.ShowInTaskbar = False

            Dim lblname As New Label()
            lblname.Text = "Name of Website"
            lblname.Location = New Point(15, 15)
            lblname.AutoSize = True
            lblname.ForeColor = Color.White

            Dim txtname As New TextBox()
            txtname.Location = New Point(15, 40)
            txtname.Width = 330
            txtname.BackColor = Color.FromArgb(45, 45, 45)
            txtname.ForeColor = Color.White
            txtname.BorderStyle = BorderStyle.FixedSingle

            Dim lblUrl As New Label()
            lblUrl.Text = "Web URL"
            lblUrl.Location = New Point(15, 75)
            lblUrl.AutoSize = True
            lblUrl.ForeColor = Color.White

            Dim txtUrl As New TextBox()
            txtUrl.Location = New Point(15, 100)
            txtUrl.Width = 330
            txtUrl.BackColor = Color.FromArgb(45, 45, 45)
            txtUrl.ForeColor = Color.White
            txtUrl.BorderStyle = BorderStyle.FixedSingle

            Dim btnOK As New Button()
            btnOK.Text = "OK"
            btnOK.Location = New Point(180, 150)
            btnOK.Size = New Size(75, 30)
            btnOK.BackColor = Color.FromArgb(60, 60, 60)
            btnOK.ForeColor = Color.White
            btnOK.FlatStyle = FlatStyle.Flat
            btnOK.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80)
            btnOK.DialogResult = DialogResult.OK

            Dim btnCancel As New Button()
            btnCancel.Text = "Cancel"
            btnCancel.Location = New Point(270, 150)
            btnCancel.Size = New Size(75, 30)
            btnCancel.BackColor = Color.FromArgb(60, 60, 60)
            btnCancel.ForeColor = Color.White
            btnCancel.FlatStyle = FlatStyle.Flat
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80)
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
End Class

