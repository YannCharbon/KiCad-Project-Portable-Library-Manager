Public Class UltraLibComponentImporter
    Inherits ComponentImporter

    Private Property CompFolder As String
    Private Property CompName As String

    Public Sub New(zipFilePath As String, kicadPrj As KiCadProject, log As Logger)
        MyBase.New(zipFilePath, kicadPrj, log)
    End Sub

    Public Sub ImportComponent()
        _ExtractComponentZip()

        If _CheckIfFormatIsValid() Then
            CompFolder = KiPrj.PrjDir & "\Lib\tmp\"

            CompName = GetCompName()
            If CompName.Contains("/") Then
                CompName = CompName.Replace("/", "-")
            End If

            _FindAndCopyFootprint()

            _FindAndCopyPackage3d()

            _FindAndCopySymbol()
        End If

        _DeleteTmpFolder()

    End Sub

    Private Function GetCompName() As String
        Dim compLibFile = IO.Directory.GetFiles(CompFolder & "\KiCAD\", "*.lib", IO.SearchOption.AllDirectories)(0)
        Dim sr As IO.StreamReader = New IO.StreamReader(compLibFile)
        For Each line In sr.ReadToEnd.Split(vbCrLf)
            If line.Contains("DEF ") Then
                sr.Close()
                Return line.Split(" ")(1)
            End If
        Next
        sr.Close()
        Return ""
    End Function

    Private Function _CheckIfFormatIsValid() As Boolean

        For Each dir As String In IO.Directory.GetDirectories(KiPrj.PrjDir & "\Lib\tmp\")
            If dir.Split("\").Last = "KiCAD" Then
                Return True
            End If
        Next

        MsgBox("Component ZIP contains multiples folders")
        Logger.Log("ERROR - Component archive contains multiples subfolders. Cannot determine which one to use for current component")
        Return False

    End Function

    Private Sub _FindAndCopyFootprint()
        Logger.Log("Importing footprint")

        Dim files() As String
        files = IO.Directory.GetFiles(CompFolder & "\KiCAD\", "*.kicad_mod", IO.SearchOption.AllDirectories)
        For Each FileName As String In files
            Dim footprintName As String = FileName.Split("\").Last.Split(".").First
            Logger.Log("Found '" & footprintName & "' footprint")

            Dim srComp As IO.StreamReader = New IO.StreamReader(FileName)
            Dim compFootprintContent As String = srComp.ReadToEnd
            Dim compFootprintContentSplit = compFootprintContent.Split(vbCr)
            srComp.Close()

            ' Getting correct footprint name
            For Each line In compFootprintContentSplit
                If line.Contains("(module") Then
                    footprintName = line.Split(" ")(1).Replace("""", "")
                End If
            Next

            ' Modify 3dpackage name
            Dim footprintContains3dModel As Boolean = False
            Dim i = 0
            For Each line In compFootprintContentSplit
                compFootprintContentSplit(i) = compFootprintContentSplit(i).TrimStart(vbLf)
                If line.Contains("model") Then
                    footprintContains3dModel = True
                    compFootprintContentSplit(i) = "  (model ${KIPRJMOD}/Lib/Package3d/" & CompName & ".stp"
                    Logger.Log("Adjusted 3d package model name from '" & line.Substring(line.IndexOf("model") + "model".Length, line.Length - line.IndexOf("model") - "model".Length) & "' to  '" & "${KIPRJMOD}/Lib/Package3d/" & CompName & ".stp")
                End If
                i += 1
            Next

            ' Add 3d model if footprint does not already contains one
            If footprintContains3dModel = False Then
                compFootprintContentSplit(i - 2) = "  (model ${KIPRJMOD}/Lib/Package3d/" & CompName & ".stp"
                compFootprintContentSplit(i - 1) = "    (at (xyz 0 0 0))"
                ReDim Preserve compFootprintContentSplit(i + 3)
                compFootprintContentSplit(i) = "    (scale (xyz 1 1 1))"
                compFootprintContentSplit(i + 1) = "    (rotate (xyz 0 0 0))"
                compFootprintContentSplit(i + 2) = "  )"
                compFootprintContentSplit(i + 3) = ")"
            End If


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
            files = IO.Directory.GetFiles(CompFolder, "*.stp", IO.SearchOption.AllDirectories)
        Catch ex As Exception
            Logger.Log("Could not find STP file in folder '" & CompFolder & "\3D\'")
        End Try

        If files.Length = 0 Then
            Try
                files = IO.Directory.GetFiles(CompFolder, "*.step", IO.SearchOption.AllDirectories)
            Catch ex As Exception
                Logger.Log("Could not find STEP file in folder '" & CompFolder & "\3D\'")
            End Try
        End If

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
            files = IO.Directory.GetFiles(CompFolder, "*.wrl", IO.SearchOption.AllDirectories)
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
        files = IO.Directory.GetFiles(CompFolder & "\KiCAD\", "*.lib", IO.SearchOption.AllDirectories)
        For Each CompFileName As String In files
            Logger.Log("Found '" & CompFileName.Split("\").Last & "' symbol library")

            _ImportCompFromLibFile(CompName, CompFileName, symLibContent)

        Next
    End Sub

End Class
