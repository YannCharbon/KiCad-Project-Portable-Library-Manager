Imports System.IO.Compression
Imports System.Text.RegularExpressions

''' <summary>
''' This Class represents a generic component importer
''' </summary>
Public Class ComponentImporter

    Protected Friend Property ZipPath As String
    Protected Friend Property KiPrj As KiCadProject
    Protected Friend Property Logger As Logger

    Public Sub New(zipFilePath As String, kicadPrj As KiCadProject, log As Logger)
        ZipPath = zipFilePath
        KiPrj = kicadPrj
        Logger = log
    End Sub

    Protected Friend Sub _ExtractComponentZip()
        _InitTmpFolder()

        Logger.Log("Extracting '" & ZipPath & "' folder to 'Lib/tmp/' folder")
        ZipFile.ExtractToDirectory(ZipPath, KiPrj.PrjDir & "\Lib\tmp\")
    End Sub

    Protected Friend Sub _InitTmpFolder()
        If IO.Directory.Exists(KiPrj.PrjDir & "\Lib\tmp") Then
            Logger.Log("Clearing 'Lib/tmp/' folder")
            IO.Directory.Delete(KiPrj.PrjDir & "\Lib\tmp", True)
        End If

        Logger.Log("Creating 'Lib/tmp/' folder")
        IO.Directory.CreateDirectory(KiPrj.PrjDir & "\Lib\tmp")
    End Sub

    Protected Friend Sub _DeleteTmpFolder()
        Logger.Log("Deleting 'Lib/tmp/' folder")
        If IO.Directory.Exists(KiPrj.PrjDir & "\Lib\tmp") Then
            IO.Directory.Delete(KiPrj.PrjDir & "\Lib\tmp", True)
        End If
    End Sub

    Protected Friend Sub _ImportCompFromLibFile(ByVal compName As String, ByVal compFileName As String, ByRef prevSymLibContent As String)
        Dim srComp As IO.StreamReader = New IO.StreamReader(compFileName)
        Dim compSymLibContent As String = srComp.ReadToEnd
        srComp.Close()

        Logger.Log("Extracting component symbol from archive")
        Dim compOnly As String = ""
        Dim isInCompBlock = False
        For Each line In compSymLibContent.Split(vbLf)
            If line.Contains("#End Library") Then
                isInCompBlock = False
            End If

            If isInCompBlock Then
                If line.Contains("F2") Then
                    ' Modifying footprintname
                    Dim footprint As String = Regex.Match(line, "(?<=\"")(.*?)(?=\"")").Value
                    line = line.Replace(footprint, "0_project_footprints:" & footprint)
                End If
                compOnly = compOnly & line.Trim & vbCrLf
            End If

            If line.Contains("#encoding utf-8") Then
                isInCompBlock = True
            End If
        Next

        ' Add component to lib
        If prevSymLibContent.Contains(compName) = False Then
            Logger.Log("Merging extracted component with project symbol library")
            prevSymLibContent = prevSymLibContent.Insert(prevSymLibContent.IndexOf("#End Library") - 1, compOnly)

            Logger.Log("Saving '\Lib\0_project_symbols.lib'")
            IO.File.WriteAllText(KiPrj.PrjDir & "\Lib\0_project_symbols.lib", prevSymLibContent)
        Else
            Logger.Log(compName & " already exists in '\Lib\0_project_symbols.lib'. Skipping")
        End If
    End Sub

End Class
