Imports System.Text.RegularExpressions

''' <summary>
''' This Class represents a ComponentImporter that can be used to import a
''' component from another existing KiCad Project that contains a local library
''' </summary>
Public Class KicadToKicadComponentImporter
    Inherits ComponentImporter

    Private Property kiImportSrcPrj As KiCadProject

    Public Sub New(kicadPrj As KiCadProject, kicadImportSrcPrj As KiCadProject, log As Logger)
        MyBase.New("", kicadPrj, log)
        kiImportSrcPrj = kicadImportSrcPrj
    End Sub

    Public Sub ImportSelectedSymbols(ByVal selectedCompList As List(Of String))
        Dim importSymLibContent = IO.File.ReadAllLines(kiImportSrcPrj.PrjDir & "\Lib\0_project_symbols.lib")
        Dim destSymLibContent = IO.File.ReadAllText(KiPrj.PrjDir & "\Lib\0_project_symbols.lib")

        For Each compName As String In selectedCompList
            Dim compContent As String = "#" & vbCrLf
            Dim compBlockOpen = False

            Logger.Log("Importing symbol for component " & compName)
            For Each line In importSymLibContent
                If line.Contains("DEF " & compName) Then
                    compBlockOpen = True
                End If

                If compBlockOpen Then
                    compContent = compContent & line & vbCrLf

                    ' Get footprint name and copy
                    If line.Contains("F2 ") Then
                        Dim footprintName As String = Regex.Match(line, "(?<=\"")(.*?)(?=\"")").Value.Split(":").Last
                        If footprintName = "" Then
                            Logger.Log("No footprint detected for component '" & compName & "'")
                        Else
                            Logger.Log("Detected footprint '" & footprintName & "' for component '" & compName & "'")

                            Logger.Log("Saved footprint in 'Lib/0_project_footprints.pretty/" & footprintName & ".kicad_mod'")
                            IO.File.Copy(kiImportSrcPrj.PrjDir & "\Lib\0_project_footprints.pretty\" & footprintName & ".kicad_mod", KiPrj.PrjDir & "\Lib\0_project_footprints.pretty\" & footprintName & ".kicad_mod", True)

                        End If


                    End If
                End If

                If line.Contains("ENDDEF") Then
                    compBlockOpen = False
                End If
            Next

            ' Add component to destination lib
            If destSymLibContent.Contains(compName) = False Then
                Logger.Log("Merging extracted component with project symbol library")
                destSymLibContent = destSymLibContent.Insert(destSymLibContent.IndexOf("#End Library") - 1, compContent)

                Logger.Log("Saving '\Lib\0_project_symbols.lib'")
                IO.File.WriteAllText(KiPrj.PrjDir & "\Lib\0_project_symbols.lib", destSymLibContent)
            Else
                Logger.Log(compName & " already exists in '\Lib\0_project_symbols.lib'. Skipping")
            End If

            ' Import 3d
            If IO.File.Exists(kiImportSrcPrj.PrjDir & "\Lib\Package3d\" & compName & ".stp") Then
                Logger.Log("Imported 3d package (.stp) for component " & compName)
                IO.File.Copy(kiImportSrcPrj.PrjDir & "\Lib\Package3d\" & compName & ".stp", KiPrj.PrjDir & "\Lib\Package3d\" & compName & ".stp", True)
            End If

            If IO.File.Exists(kiImportSrcPrj.PrjDir & "\Lib\Package3d\" & compName & ".wrl") Then
                Logger.Log("Imported 3d package (.wrl) for component " & compName)
                IO.File.Copy(kiImportSrcPrj.PrjDir & "\Lib\Package3d\" & compName & ".wrl", KiPrj.PrjDir & "\Lib\Package3d\" & compName & ".wrl", True)
            End If
        Next
    End Sub

    Public Sub ImportAllSymbols()
        Dim importSymLibContent = IO.File.ReadAllLines(kiImportSrcPrj.PrjDir & "\Lib\0_project_symbols.lib")
        Dim destSymLibContent = IO.File.ReadAllText(KiPrj.PrjDir & "\Lib\0_project_symbols.lib")

        For Each compName As String In kiImportSrcPrj.GetAllCompInPrj
            Dim compContent As String = "#" & vbCrLf
            Dim compBlockOpen = False

            Logger.Log("Importing symbol for component " & compName)
            For Each line In importSymLibContent
                If line.Contains("DEF " & compName) Then
                    compBlockOpen = True
                End If

                If compBlockOpen Then
                    compContent = compContent & line & vbCrLf

                    ' Get footprint name and copy
                    If line.Contains("F2 ") Then
                        Dim footprintName As String = Regex.Match(line, "(?<=\"")(.*?)(?=\"")").Value.Split(":").Last
                        If footprintName = "" Then
                            Logger.Log("No footprint detected for component '" & compName & "'")
                        Else
                            Logger.Log("Detected footprint '" & footprintName & "' for component '" & compName & "'")

                            Logger.Log("Saved footprint in 'Lib/0_project_footprints.pretty/" & footprintName & ".kicad_mod'")
                            IO.File.Copy(kiImportSrcPrj.PrjDir & "\Lib\0_project_footprints.pretty\" & footprintName & ".kicad_mod", KiPrj.PrjDir & "\Lib\0_project_footprints.pretty\" & footprintName & ".kicad_mod", True)

                        End If


                    End If
                End If

                If line.Contains("ENDDEF") Then
                    compBlockOpen = False
                End If
            Next

            ' Add component to destination lib
            If destSymLibContent.Contains(compName) = False Then
                Logger.Log("Merging extracted component with project symbol library")
                destSymLibContent = destSymLibContent.Insert(destSymLibContent.IndexOf("#End Library") - 1, compContent)

                Logger.Log("Saving '\Lib\0_project_symbols.lib'")
                IO.File.WriteAllText(KiPrj.PrjDir & "\Lib\0_project_symbols.lib", destSymLibContent)
            Else
                Logger.Log(compName & " already exists in '\Lib\0_project_symbols.lib'. Skipping")
            End If

            ' Import 3d
            If IO.File.Exists(kiImportSrcPrj.PrjDir & "\Lib\Package3d\" & compName & ".stp") Then
                Logger.Log("Imported 3d package (.stp) for component " & compName)
                IO.File.Copy(kiImportSrcPrj.PrjDir & "\Lib\Package3d\" & compName & ".stp", KiPrj.PrjDir & "\Lib\Package3d\" & compName & ".stp", True)
            End If

            If IO.File.Exists(kiImportSrcPrj.PrjDir & "\Lib\Package3d\" & compName & ".wrl") Then
                Logger.Log("Imported 3d package (.wrl) for component " & compName)
                IO.File.Copy(kiImportSrcPrj.PrjDir & "\Lib\Package3d\" & compName & ".wrl", KiPrj.PrjDir & "\Lib\Package3d\" & compName & ".wrl", True)
            End If
        Next
    End Sub



End Class
