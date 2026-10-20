Imports Microsoft.Web.WebView2.Core
Imports System.IO

Module WarningPageModule

    Private ReadOnly WarningFolder As String = DataPaths.WarningFolder

    Private ReadOnly CertWarningFile As String =
        Path.Combine(WarningFolder, "certificate-warning.html")

    Public Sub ShowCertificateWarning(
        core As CoreWebView2,
        Optional siteUrl As String = "",
        Optional theme As String = "light")

        If core Is Nothing Then Return

        If Not File.Exists(CertWarningFile) Then
            core.NavigateToString(GetFallbackHtml(siteUrl, theme))
            Return
        End If

        Dim fileUri As New Uri(CertWarningFile)
        Dim target As String = fileUri.AbsoluteUri

        Dim q As New List(Of String)
        If Not String.IsNullOrWhiteSpace(siteUrl) Then
            q.Add("url=" & Uri.EscapeDataString(siteUrl))
        End If
        q.Add("theme=" & Uri.EscapeDataString(theme.ToLower()))

        target &= "?" & String.Join("&", q)
        core.Navigate(target)
    End Sub

    Private Function GetFallbackHtml(siteUrl As String, theme As String) As String
        Dim isDark = theme.ToLower() <> "light"
        Dim bg = If(isDark, "#1e1e1e", "#f5f5f5")
        Dim fg = If(isDark, "#ffffff", "#222222")
        Return $"<html><body style='background:{bg};color:{fg};font-family:Segoe UI;padding:40px'>" &
               "<h2>Certificate Warning</h2>" &
               $"<p>{System.Net.WebUtility.HtmlEncode(siteUrl)}</p></body></html>"
    End Function

End Module