Imports System.Drawing
Imports System.Windows.Forms

Public Class DarkMenuRenderer
    Inherits ToolStripProfessionalRenderer

    Public Sub New()
        MyBase.New(New DarkMenuColorTable())
    End Sub

    Protected Overrides Sub OnRenderItemText(
        e As ToolStripItemTextRenderEventArgs)

        e.TextColor = Color.White
        MyBase.OnRenderItemText(e)
    End Sub

    Protected Overrides Sub OnRenderArrow(
        e As ToolStripArrowRenderEventArgs)

        e.ArrowColor = Color.White
        MyBase.OnRenderArrow(e)
    End Sub
End Class


Public Class DarkMenuColorTable
    Inherits ProfessionalColorTable

    ' Main MenuStrip
    Public Overrides ReadOnly Property MenuStripGradientBegin As Color
        Get
            Return Color.FromArgb(32, 32, 32)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuStripGradientEnd As Color
        Get
            Return Color.FromArgb(32, 32, 32)
        End Get
    End Property

    ' Dropdown background
    Public Overrides ReadOnly Property ToolStripDropDownBackground As Color
        Get
            Return Color.FromArgb(32, 32, 32)
        End Get
    End Property

    ' Left icon/image margin
    Public Overrides ReadOnly Property ImageMarginGradientBegin As Color
        Get
            Return Color.FromArgb(32, 32, 32)
        End Get
    End Property

    Public Overrides ReadOnly Property ImageMarginGradientMiddle As Color
        Get
            Return Color.FromArgb(32, 32, 32)
        End Get
    End Property

    Public Overrides ReadOnly Property ImageMarginGradientEnd As Color
        Get
            Return Color.FromArgb(32, 32, 32)
        End Get
    End Property

    ' Border
    Public Overrides ReadOnly Property MenuBorder As Color
        Get
            Return Color.FromArgb(70, 70, 70)
        End Get
    End Property

    ' Normal / selected item
    Public Overrides ReadOnly Property MenuItemSelected As Color
        Get
            Return Color.FromArgb(55, 55, 55)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemSelectedGradientBegin As Color
        Get
            Return Color.FromArgb(55, 55, 55)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemSelectedGradientEnd As Color
        Get
            Return Color.FromArgb(55, 55, 55)
        End Get
    End Property

    ' Clicked/pressed item
    Public Overrides ReadOnly Property MenuItemPressedGradientBegin As Color
        Get
            Return Color.FromArgb(45, 45, 45)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemPressedGradientMiddle As Color
        Get
            Return Color.FromArgb(45, 45, 45)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemPressedGradientEnd As Color
        Get
            Return Color.FromArgb(45, 45, 45)
        End Get
    End Property
End Class