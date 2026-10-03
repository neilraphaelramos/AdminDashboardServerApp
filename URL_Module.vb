Imports System.IO
Imports System.Text.Json

Public Class WebsiteItem

    Public Property id As Integer
    Public Property name As String
    Public Property url As String

End Class


Module URL_Module

    Private ReadOnly ConfigFile As String =
        Path.Combine(
            Application.StartupPath,
            "server.json"
        )


    Public Property CurrentLinkWeb As String = ""

    Public Property AdminURLName As String = ""


    ' ============================================================
    ' GET ALL WEBSITES
    ' ============================================================

    Public Function GetWebsites() As List(Of WebsiteItem)

        If Not File.Exists(ConfigFile) Then

            Return New List(Of WebsiteItem)

        End If

        Try

            Dim json As String =
                File.ReadAllText(ConfigFile)

            If String.IsNullOrWhiteSpace(json) Then

                Return New List(Of WebsiteItem)

            End If

            Dim websites =
                JsonSerializer.Deserialize(Of List(Of WebsiteItem))(json)

            If websites Is Nothing Then

                Return New List(Of WebsiteItem)

            End If

            Return websites

        Catch ex As Exception

            Return New List(Of WebsiteItem)

        End Try

    End Function


    ' ============================================================
    ' SAVE ALL WEBSITES
    ' ============================================================

    Public Sub SaveWebsites(
        websites As List(Of WebsiteItem))

        Try

            Dim options As New JsonSerializerOptions With {
                .WriteIndented = True
            }

            Dim output As String =
                JsonSerializer.Serialize(
                    websites,
                    options
                )

            File.WriteAllText(
                ConfigFile,
                output
            )

        Catch ex As Exception

            MessageBox.Show(
                ex.Message,
                "Save Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    ' ============================================================
    ' LOAD FIRST WEBSITE
    ' ============================================================

    Public Sub LoadIP()

        Dim websites As List(Of WebsiteItem) =
            GetWebsites()

        If websites.Count = 0 Then

            CurrentLinkWeb = ""
            AdminURLName = ""

            Return

        End If

        CurrentLinkWeb =
            websites(0).url

        AdminURLName =
            websites(0).name

    End Sub


    ' ============================================================
    ' OLD SAVEIP SUPPORT
    ' ============================================================

    Public Sub SaveIP()

        Dim websites As List(Of WebsiteItem) =
            GetWebsites()

        Dim nextId As Integer = 0

        If websites.Count > 0 Then

            nextId =
                websites.Max(Function(x) x.id) + 1

        End If

        websites.Add(
            New WebsiteItem With {
                .id = nextId,
                .name = AdminURLName,
                .url = CurrentLinkWeb
            }
        )

        SaveWebsites(websites)

    End Sub

End Module