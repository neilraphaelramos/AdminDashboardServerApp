Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Public Class CustomLeftTabControl
    Inherits TabControl

    '==============================
    ' Theme Colors
    '==============================
    Private _stripBackColor As Color = Color.FromArgb(32, 32, 32)
    Private _selectedTabColor As Color = Color.FromArgb(55, 55, 55)
    Private _unselectedTabColor As Color = Color.FromArgb(32, 32, 32)
    Private _selectedTextColor As Color = Color.White
    Private _unselectedTextColor As Color = Color.Gainsboro
    Private _borderColor As Color = Color.FromArgb(60, 60, 60)

    <Category("Appearance")>
    <DefaultValue(GetType(Color), "32, 32, 32")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Property StripBackColor As Color
        Get
            Return _stripBackColor
        End Get
        Set(value As Color)
            _stripBackColor = value
            Me.BackColor = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")>
    <DefaultValue(GetType(Color), "55, 55, 55")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Property SelectedTabColor As Color
        Get
            Return _selectedTabColor
        End Get
        Set(value As Color)
            _selectedTabColor = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")>
    <DefaultValue(GetType(Color), "32, 32, 32")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Property UnselectedTabColor As Color
        Get
            Return _unselectedTabColor
        End Get
        Set(value As Color)
            _unselectedTabColor = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")>
    <DefaultValue(GetType(Color), "White")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Property SelectedTextColor As Color
        Get
            Return _selectedTextColor
        End Get
        Set(value As Color)
            _selectedTextColor = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")>
    <DefaultValue(GetType(Color), "Gainsboro")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Property UnselectedTextColor As Color
        Get
            Return _unselectedTextColor
        End Get
        Set(value As Color)
            _unselectedTextColor = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")>
    <DefaultValue(GetType(Color), "60, 60, 60")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Property BorderColor As Color
        Get
            Return _borderColor
        End Get
        Set(value As Color)
            _borderColor = value
            Me.Invalidate()
        End Set
    End Property

    Public Sub New()
        MyBase.New()

        Me.Alignment = TabAlignment.Left
        Me.DrawMode = TabDrawMode.OwnerDrawFixed
        Me.SizeMode = TabSizeMode.Fixed
        Me.ItemSize = New Size(28, 140)          ' Height of each tab , Width of the strip
        Me.Multiline = True
        Me.BackColor = _stripBackColor

        Me.SetStyle(ControlStyles.UserPaint Or
                    ControlStyles.AllPaintingInWmPaint Or
                    ControlStyles.OptimizedDoubleBuffer Or
                    ControlStyles.ResizeRedraw, True)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        ' Fill entire background
        Using brush As New SolidBrush(StripBackColor)
            e.Graphics.FillRectangle(brush, Me.ClientRectangle)
        End Using

        ' Draw all tabs
        For i As Integer = 0 To Me.TabCount - 1
            DrawTab(e.Graphics, i)
        Next

        ' Vertical line exactly between tabs and content
        Using pen As New Pen(BorderColor)
            Dim lineX As Integer = Me.DisplayRectangle.Left - 1
            e.Graphics.DrawLine(pen, lineX, 0, lineX, Me.Height)
        End Using
    End Sub

    Private Sub DrawTab(g As Graphics, index As Integer)
        Dim tabRect As Rectangle = Me.GetTabRect(index)
        Dim isSelected As Boolean = (index = Me.SelectedIndex)

        ' Tab background
        Dim backColor As Color = If(isSelected, SelectedTabColor, UnselectedTabColor)
        Using brush As New SolidBrush(backColor)
            g.FillRectangle(brush, tabRect)
        End Using

        ' Optional border
        Using pen As New Pen(BorderColor)
            g.DrawRectangle(pen, tabRect)
        End Using

        ' ========== HORIZONTAL TEXT ==========
        Dim text As String = Me.TabPages(index).Text
        Dim textColor As Color = If(isSelected, SelectedTextColor, UnselectedTextColor)

        Using font As New Font("Segoe UI", 9.0F, FontStyle.Regular)
            Dim textSize As SizeF = g.MeasureString(text, font)

            Dim x As Single = tabRect.X + (tabRect.Width - textSize.Width) / 2
            Dim y As Single = tabRect.Y + (tabRect.Height - textSize.Height) / 2

            Using brush As New SolidBrush(textColor)
                g.DrawString(text, font, brush, x, y)
            End Using
        End Using
    End Sub

    Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
        ' Prevent system from painting white background
    End Sub

    ' Call this when theme changes
    Public Sub ApplyTheme(isDark As Boolean)
        If isDark Then
            StripBackColor = Color.FromArgb(32, 32, 32)
            SelectedTabColor = Color.FromArgb(55, 55, 55)
            UnselectedTabColor = Color.FromArgb(32, 32, 32)
            SelectedTextColor = Color.White
            UnselectedTextColor = Color.Gainsboro
            BorderColor = Color.FromArgb(60, 60, 60)
        Else
            StripBackColor = Color.FromArgb(240, 240, 240)
            SelectedTabColor = Color.DodgerBlue
            UnselectedTabColor = Color.FromArgb(230, 230, 230)
            SelectedTextColor = Color.White
            UnselectedTextColor = Color.Black
            BorderColor = Color.FromArgb(180, 180, 180)
        End If
    End Sub

End Class