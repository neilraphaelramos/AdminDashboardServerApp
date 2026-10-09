
Public Class Icons
    Public Shared Function CreateDefaultGlobeIcon() As Image
        Dim bmp As New Bitmap(25, 20)
        bmp.MakeTransparent()

        Using g As Graphics = Graphics.FromImage(bmp)
            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            g.Clear(Color.Transparent)

            ' Simple globe circle
            Using pen As New Pen(Color.FromArgb(180, 180, 180), 1.5F)
                g.DrawEllipse(pen, 2, 1, 16, 16)
                g.DrawEllipse(pen, 6, 1, 8, 16)           ' vertical oval
                g.DrawLine(pen, 2, 9, 18, 9)              ' horizontal line
            End Using
        End Using

        Return bmp
    End Function
End Class
