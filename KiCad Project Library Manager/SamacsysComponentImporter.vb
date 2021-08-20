Imports System.Text.RegularExpressions

''' <summary>
''' This Class represents a ComponentImporter for components that can be downloaded
''' in ZIP format from Samacsys - ComponentSearchEngine
''' 
''' </summary>
Public Class SamacsysComponentImporter
    Inherits ComponentImporter

    Private Property CompFolder As String
    Private Property CompName As String

    Public Sub New(zipFilePath As String, kicadPrj As KiCadProject, log As Logger)
        MyBase.New(zipFilePath, kicadPrj, log)
    End Sub

    Public Sub ImportComponent()
        _ExtractComponentZip()

        If _CheckIfFormatIsValid() Then
            CompFolder = IO.Directory.GetDirectories(KiPrj.PrjDir & "\Lib\tmp\")(0)
            CompName = CompFolder.Split("\").Last

            _FindAndCopyFootprint()

            _FindAndCopyPackage3d()

            _FindAndCopySymbol()
        End If

        _DeleteTmpFolder()

    End Sub

    Private Function _CheckIfFormatIsValid() As Boolean
        If IO.Directory.GetDirectories(KiPrj.PrjDir & "\Lib\tmp\").Length = 1 Then
            Return True
        Else
            MsgBox("Component ZIP contains multiples folders")
            Logger.Log("ERROR - Component archive contains multiples subfolders. Cannot determine which one to use for current component")
            Return False
        End If
    End Function

    Private Sub _FindAndCopyFootprint()
        Logger.Log("Importing footprint")

        Dim files() As String
        files = IO.Directory.GetFiles(CompFolder & "\KiCad\", "*.kicad_mod", IO.SearchOption.TopDirectoryOnly)
        For Each FileName As String In files
            Dim footprintName As String = FileName.Split("\").Last.Split(".").First
            Logger.Log("Found '" & footprintName & "' footprint")

            ' Modify 3dpackage
            Dim srComp As IO.StreamReader = New IO.StreamReader(FileName)
            Dim compFootprintContent As String = srComp.ReadToEnd
            srComp.Close()

            Dim i = 0
            Dim compFootprintContentSplit = compFootprintContent.Split(vbCr)
            For Each line In compFootprintContentSplit
                compFootprintContentSplit(i) = compFootprintContentSplit(i).TrimStart(vbLf)
                If line.Contains("model") Then
                    compFootprintContentSplit(i) = "  (model ${KIPRJMOD}/Lib/Package3d/" & CompName & ".stp"
                    Logger.Log("Adjusted 3d package model name from '" & line.Substring(line.IndexOf("model") + "model".Length, line.Length - line.IndexOf("model") - "model".Length) & "' to  '" & "${KIPRJMOD}/Lib/Package3d/" & CompName & ".stp")
                End If
                i += 1
            Next

            Logger.Log("Saved footprint in 'Lib/0_project_footprints.pretty/" & footprintName & ".kicad_mod'")
            IO.File.WriteAllLines(KiPrj.PrjDir & "\Lib\0_project_footprints.pretty\" & footprintName & ".kicad_mod", compFootprintContentSplit)
        Next
    End Sub

    Private Sub _FindAndCopyPackage3d()
        Logger.Log("Importing 3d package")

        Dim files() As String
        ' STP
        Logger.Log("Importing '.stp' 3d packages")
        Try
            files = IO.Directory.GetFiles(CompFolder & "\3D\", "*.stp", IO.SearchOption.TopDirectoryOnly)
        Catch ex As Exception
            Logger.Log("Could not find STP file in folder '" & CompFolder & "\3D\'")
        End Try

        If files IsNot Nothing Then
            For Each FileName As String In files
                Logger.Log("Importing '" & FileName.Split("\").Last & "' as '\Lib\Package3d\" & CompName & ".stp'")
                IO.File.Copy(FileName, KiPrj.PrjDir & "\Lib\Package3d\" & CompName & ".stp", True)
            Next
        Else
            Logger.Log("No STP 3d package found. Skipping")
        End If


        'WRL
        Logger.Log("Importing '.wrl' 3d packages")
        Try
            files = IO.Directory.GetFiles(CompFolder & "\3D\", "*.wrl", IO.SearchOption.TopDirectoryOnly)
        Catch ex As Exception
            Logger.Log("Could not find WRL file in folder '" & CompFolder & "\3D\'")
        End Try

        If files IsNot Nothing Then
            For Each FileName As String In files
                Logger.Log("Importing '" & FileName.Split("\").Last & "' as '\Lib\Package3d\" & CompName & ".wrl'")
                IO.File.Copy(FileName, KiPrj.PrjDir & "\Lib\Package3d\" & CompName & ".wrl", True)
            Next
        Else
            Logger.Log("No WRL 3d package found. Skipping")
        End If

    End Sub

    Private Sub _FindAndCopySymbol()
        Logger.Log("Importing symbol")

        ' Get previous symbol lib content
        Dim sr As IO.StreamReader = New IO.StreamReader(KiPrj.PrjDir & "\Lib\0_project_symbols.lib")
        Dim symLibContent As String = sr.ReadToEnd
        sr.Close()

        Dim files() As String
        files = IO.Directory.GetFiles(CompFolder & "\KiCad\", "*.lib", IO.SearchOption.TopDirectoryOnly)
        For Each CompFileName As String In files
            Logger.Log("Found '" & CompFileName.Split("\").Last & "' symbol library")

            _ImportCompFromLibFile(CompName, CompFileName, symLibContent)

        Next
    End Sub

End Class
