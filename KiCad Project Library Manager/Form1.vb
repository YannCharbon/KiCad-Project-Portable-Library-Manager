Imports System.ComponentModel
Imports System.IO.Compression
Imports System.Text.RegularExpressions

Public Class Form1

    Dim prjDir As String = ""
    Dim prjImportDir As String = ""

    Sub Log(ByVal str As String)
        TextBoxLog.Text = TextBoxLog.Text & Now & "  -  " & str & vbCrLf
        TextBoxLog.SelectionStart = TextBoxLog.Text.Length
        TextBoxLog.ScrollToCaret()
    End Sub

    Private Sub ButtonChoosePrjFold_Click(sender As Object, e As EventArgs) Handles ButtonChoosePrjFold.Click
        Log("Selecting project")
        If OpenFileDialogPrjFold.ShowDialog() = DialogResult.OK Then
            TextBoxPrjFoldPath.Text = OpenFileDialogPrjFold.FileName
            Log("Selected valid project located at " & TextBoxPrjFoldPath.Text)
            prjDir = TextBoxPrjFoldPath.Text.Remove(TextBoxPrjFoldPath.Text.LastIndexOf("\"), TextBoxPrjFoldPath.Text.Length - TextBoxPrjFoldPath.Text.LastIndexOf("\"))
        Else
            Log("Aborted")
        End If
    End Sub

    Private Sub ButtonCreatePrjLib_Click(sender As Object, e As EventArgs) Handles ButtonCreatePrjLib.Click

        If IO.File.Exists(prjDir & "\sym-lib-table") = False Then
            Log("Creating 'sym-lib-table' file")
            IO.File.WriteAllBytes(prjDir & "\sym-lib-table", My.Resources.sym_lib_table)
        Else
            Log("'sym-lib-table' file already exists. Skipping. (Manually delete to redeploy empty library)")
        End If
        If IO.File.Exists(prjDir & "\fp-lib-table") = False Then
            Log("Creating 'fp-lib-table' file")
            IO.File.WriteAllBytes(prjDir & "\fp-lib-table", My.Resources.fp_lib_table)
        Else
            Log("'fp-lib-table' file already exists. Skipping. (Manually delete to redeploy empty library)")
        End If
        If IO.Directory.Exists(prjDir & "\Lib") = False Then
            Log("Creating 'Lib/' folder")

            IO.Directory.CreateDirectory(prjDir & "\Lib")
            IO.Directory.CreateDirectory(prjDir & "\Lib\0_project_footprints.pretty")
            IO.Directory.CreateDirectory(prjDir & "\Lib\Package3d")

            IO.File.WriteAllBytes(prjDir & "\Lib\0_project_symbols.dcm", My.Resources._0_project_symbols_dcm)
            IO.File.WriteAllBytes(prjDir & "\Lib\0_project_symbols.lib", My.Resources._0_project_symbols_lib)
        Else
            Log("'\Lib' folder already exists. Skipping. (Manually delete to redeploy empty library)")
        End If

        Log("done")
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            prjDir = TextBoxPrjFoldPath.Text.Remove(TextBoxPrjFoldPath.Text.LastIndexOf("\"), TextBoxPrjFoldPath.Text.Length - TextBoxPrjFoldPath.Text.LastIndexOf("\"))
        Catch ex As Exception
            Log("Base project path is not valid")
        End Try

        Try
            prjImportDir = TextBoxPrjFoldImportPath.Text.Remove(TextBoxPrjFoldImportPath.Text.LastIndexOf("\"), TextBoxPrjFoldImportPath.Text.Length - TextBoxPrjFoldImportPath.Text.LastIndexOf("\"))
        Catch ex As Exception
            Log("Import source project path is not valid")
        End Try

        If TextBoxPrjFoldPath.Text.Contains(".pro") Then
            ButtonCreatePrjLib.Enabled = True
            ButtonAddCompFromZip.Enabled = True
        Else
            ButtonCreatePrjLib.Enabled = False
            ButtonAddCompFromZip.Enabled = False
        End If

        If TextBoxPrjFoldPath.Text.Contains(".pro") Then
            ListAllCompInImportPrj()
        End If
    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

    End Sub

    Private Sub ButtonAddCompFromZip_Click(sender As Object, e As EventArgs) Handles ButtonAddCompFromZip.Click
        Log("Choosing 'Samacsys - ComponentSearchEngine' component archive to deploy to project lib")
        If OpenFileDialogCompZip.ShowDialog() = DialogResult.OK Then
            Dim zipFilePath = OpenFileDialogCompZip.FileName
            Log("Selected archive located at " & zipFilePath)

            If IO.Directory.Exists(prjDir & "\Lib\tmp") Then
                Log("Clearing 'Lib/tmp/' folder")
                IO.Directory.Delete(prjDir & "\Lib\tmp", True)
            End If

            Log("Creating 'Lib/tmp/' folder")
            IO.Directory.CreateDirectory(prjDir & "\Lib\tmp")

            Log("Extracting '" & zipFilePath & "' folder to 'Lib/tmp/' folder")
            ZipFile.ExtractToDirectory(zipFilePath, prjDir & "\Lib\tmp\")

            If IO.Directory.GetDirectories(prjDir & "\Lib\tmp\").Length = 1 Then
                Dim compFold As String = IO.Directory.GetDirectories(prjDir & "\Lib\tmp\")(0)
                Dim compName = compFold.Split("\").Last

                Log("Importing footprint")
                FindAndCopyFootprint(compFold, compName)
                Log("Importing 3d package")
                FindAndCopyPackage3d(compFold, compName)
                Log("Importing symbol")
                FindAndCopySymbol(compFold, compName)
                'FindAndCopySymbolDcm(compFold, compName)

            Else
                MsgBox("Component ZIP contains multiples folders")
                Log("ERROR - Component archive contains multiples subfolders. Cannot determine which one to use for current component")
            End If

            Log("Deleting 'Lib/tmp/' folder")
            IO.Directory.Delete(prjDir & "\Lib\tmp", True)

            Log("done")
        Else
            Log("Aborted")
        End If
    End Sub

    Sub FindAndCopyFootprint(ByVal compFold As String, ByVal compName As String)
        Dim files() As String
        files = IO.Directory.GetFiles(compFold & "\KiCad\", "*.kicad_mod", IO.SearchOption.TopDirectoryOnly)
        For Each FileName As String In files
            Dim footprintName As String = FileName.Split("\").Last.Split(".").First
            Log("Found '" & footprintName & "' footprint")

            ' Modify 3dpackage
            Dim srComp As IO.StreamReader = New IO.StreamReader(FileName)
            Dim compFootprintContent As String = srComp.ReadToEnd
            srComp.Close()

            Dim i = 0
            Dim compFootprintContentSplit = compFootprintContent.Split(vbCr)
            For Each line In compFootprintContentSplit
                compFootprintContentSplit(i) = compFootprintContentSplit(i).TrimStart(vbLf)
                If line.Contains("model") Then
                    compFootprintContentSplit(i) = "  (model ${KIPRJMOD}/Lib/Package3d/" & compName & ".stp"
                    Log("Adjusted 3d package model name from '" & line.Substring(line.IndexOf("model") + "model".Length, line.Length - line.IndexOf("model") - "model".Length) & "' to  '" & "${KIPRJMOD}/Lib/Package3d/" & compName & ".stp")
                End If
                i += 1
            Next

            Log("Saved footprint in 'Lib/0_project_footprints.pretty/" & footprintName & ".kicad_mod'")
            IO.File.WriteAllLines(prjDir & "\Lib\0_project_footprints.pretty\" & footprintName & ".kicad_mod", compFootprintContentSplit)
        Next
    End Sub

    Sub FindAndCopyPackage3d(ByVal compFold As String, ByVal compName As String)
        Dim files() As String
        ' STP
        Log("Importing '.stp' 3d packages")
        files = IO.Directory.GetFiles(compFold & "\3D\", "*.stp", IO.SearchOption.TopDirectoryOnly)
        For Each FileName As String In files
            Log("Importing '" & FileName.Split("\").Last & "' as '\Lib\Package3d\" & compName & ".stp'")
            IO.File.Copy(FileName, prjDir & "\Lib\Package3d\" & compName & ".stp", True)
        Next

        'WRL
        Log("Importing '.wrl' 3d packages")
        files = IO.Directory.GetFiles(compFold & "\3D\", "*.wrl", IO.SearchOption.TopDirectoryOnly)
        For Each FileName As String In files
            Log("Importing '" & FileName.Split("\").Last & "' as '\Lib\Package3d\" & compName & ".wrl'")
            IO.File.Copy(FileName, prjDir & "\Lib\Package3d\" & compName & ".wrl", True)
        Next
    End Sub

    Sub FindAndCopySymbol(ByVal compFold As String, ByVal compName As String)
        ' Get previous symbol lib content
        Dim sr As IO.StreamReader = New IO.StreamReader(prjDir & "\Lib\0_project_symbols.lib")
        Dim symLibContent As String = sr.ReadToEnd
        sr.Close()

        Dim files() As String
        files = IO.Directory.GetFiles(compFold & "\KiCad\", "*.lib", IO.SearchOption.TopDirectoryOnly)
        For Each FileName As String In files
            Log("Found '" & FileName.Split("\").Last & "' symbol library")


            Dim srComp As IO.StreamReader = New IO.StreamReader(FileName)
            Dim compSymLibContent As String = srComp.ReadToEnd
            srComp.Close()

            Log("Extracting component symbol from archive")
            Dim compOnly As String = ""
            Dim isInCompBlock = False
            For Each line In compSymLibContent.Split(vbCrLf)
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
            If symLibContent.Contains(compName) = False Then
                Log("Merging extracted component with project symbol library")
                symLibContent = symLibContent.Insert(symLibContent.IndexOf("#End Library") - 1, compOnly)

                Log("Saving '\Lib\0_project_symbols.lib'")
                IO.File.WriteAllText(prjDir & "\Lib\0_project_symbols.lib", symLibContent)
            Else
                Log(compName & " already exists in '\Lib\0_project_symbols.lib'. Skipping")
            End If

        Next
    End Sub



    Private Sub TextBoxPrjFoldPath_TextChanged(sender As Object, e As EventArgs) Handles TextBoxPrjFoldPath.TextChanged
        If TextBoxPrjFoldPath.Text.Contains(".pro") Then
            ButtonCreatePrjLib.Enabled = True
            ButtonAddCompFromZip.Enabled = True
            prjDir = TextBoxPrjFoldPath.Text.Remove(TextBoxPrjFoldPath.Text.LastIndexOf("\"), TextBoxPrjFoldPath.Text.Length - TextBoxPrjFoldPath.Text.LastIndexOf("\"))
        Else
            ButtonCreatePrjLib.Enabled = False
            ButtonAddCompFromZip.Enabled = False
        End If
    End Sub

    Private Sub ButtonChoosePrjFoldImport_Click(sender As Object, e As EventArgs) Handles ButtonChoosePrjFoldImport.Click
        ListBoxImportComp.Items.Clear()
        Log("Selecting project to import component from")
        If OpenFileDialogPrjFoldImport.ShowDialog() = DialogResult.OK Then
            TextBoxPrjFoldImportPath.Text = OpenFileDialogPrjFoldImport.FileName
            Log("Selected valid project located at " & TextBoxPrjFoldImportPath.Text)

            prjImportDir = TextBoxPrjFoldImportPath.Text.Remove(TextBoxPrjFoldImportPath.Text.LastIndexOf("\"), TextBoxPrjFoldImportPath.Text.Length - TextBoxPrjFoldImportPath.Text.LastIndexOf("\"))

            ListAllCompInImportPrj()
        Else
            Log("Aborted")
        End If
    End Sub

    Sub ListAllCompInImportPrj()
        Dim lines = IO.File.ReadAllLines(prjImportDir & "\Lib\0_project_symbols.lib")
        For Each line As String In lines
            If line.Contains("DEF ") And Not line.Contains("ENDDEF") Then
                Dim compName As String = Regex.Match(line, "(?<=DEF\s)(.*?)(?=\s)").Value
                ListBoxImportComp.Items.Add(compName)
                Log("Detected component '" & compName & "' in import project")
            End If
        Next
    End Sub

    Private Sub ButtonImportComponents_Click(sender As Object, e As EventArgs) Handles ButtonImportComponents.Click
        ImportSelectedSymbols()
    End Sub

    Sub ImportSelectedSymbols()
        Dim importSymLibContent = IO.File.ReadAllLines(prjImportDir & "\Lib\0_project_symbols.lib")
        Dim destSymLibContent = IO.File.ReadAllText(prjDir & "\Lib\0_project_symbols.lib")

        For Each compName As String In ListBoxImportComp.SelectedItems
            Dim compContent As String = "#" & vbCrLf
            Dim compBlockOpen = False

            Log("Importing symbol for component " & compName)
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
                            Log("No footprint detected for component '" & compName & "'")
                        Else
                            Log("Detected footprint '" & footprintName & "' for component '" & compName & "'")

                            Log("Saved footprint in 'Lib/0_project_footprints.pretty/" & footprintName & ".kicad_mod'")
                            IO.File.Copy(prjImportDir & "\Lib\0_project_footprints.pretty\" & footprintName & ".kicad_mod", prjDir & "\Lib\0_project_footprints.pretty\" & footprintName & ".kicad_mod", True)

                        End If


                    End If
                End If

                If line.Contains("ENDDEF") Then
                    compBlockOpen = False
                End If
            Next

            ' Add component to destination lib
            If destSymLibContent.Contains(compName) = False Then
                Log("Merging extracted component with project symbol library")
                destSymLibContent = destSymLibContent.Insert(destSymLibContent.IndexOf("#End Library") - 1, compContent)

                Log("Saving '\Lib\0_project_symbols.lib'")
                IO.File.WriteAllText(prjDir & "\Lib\0_project_symbols.lib", destSymLibContent)
            Else
                Log(compName & " already exists in '\Lib\0_project_symbols.lib'. Skipping")
            End If

            ' Import 3d
            If IO.File.Exists(prjImportDir & "\Lib\Package3d\" & compName & ".stp") Then
                Log("Imported 3d package (.stp) for component " & compName)
                IO.File.Copy(prjImportDir & "\Lib\Package3d\" & compName & ".stp", prjDir & "\Lib\Package3d\" & compName & ".stp", True)
            End If

            If IO.File.Exists(prjImportDir & "\Lib\Package3d\" & compName & ".wrl") Then
                Log("Imported 3d package (.wrl) for component " & compName)
                IO.File.Copy(prjImportDir & "\Lib\Package3d\" & compName & ".wrl", prjDir & "\Lib\Package3d\" & compName & ".wrl", True)
            End If
        Next
    End Sub

    Sub ImportAllSymbols()
        Dim importSymLibContent = IO.File.ReadAllLines(prjImportDir & "\Lib\0_project_symbols.lib")
        Dim destSymLibContent = IO.File.ReadAllText(prjDir & "\Lib\0_project_symbols.lib")

        For Each compName As String In ListBoxImportComp.Items
            Dim compContent As String = "#" & vbCrLf
            Dim compBlockOpen = False

            Log("Importing symbol for component " & compName)
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
                            Log("No footprint detected for component '" & compName & "'")
                        Else
                            Log("Detected footprint '" & footprintName & "' for component '" & compName & "'")

                            Log("Saved footprint in 'Lib/0_project_footprints.pretty/" & footprintName & ".kicad_mod'")
                            IO.File.Copy(prjImportDir & "\Lib\0_project_footprints.pretty\" & footprintName & ".kicad_mod", prjDir & "\Lib\0_project_footprints.pretty\" & footprintName & ".kicad_mod", True)

                        End If


                    End If
                End If

                If line.Contains("ENDDEF") Then
                    compBlockOpen = False
                End If
            Next

            ' Add component to destination lib
            If destSymLibContent.Contains(compName) = False Then
                Log("Merging extracted component with project symbol library")
                destSymLibContent = destSymLibContent.Insert(destSymLibContent.IndexOf("#End Library") - 1, compContent)

                Log("Saving '\Lib\0_project_symbols.lib'")
                IO.File.WriteAllText(prjDir & "\Lib\0_project_symbols.lib", destSymLibContent)
            Else
                Log(compName & " already exists in '\Lib\0_project_symbols.lib'. Skipping")
            End If

            ' Import 3d
            If IO.File.Exists(prjImportDir & "\Lib\Package3d\" & compName & ".stp") Then
                Log("Imported 3d package (.stp) for component " & compName)
                IO.File.Copy(prjImportDir & "\Lib\Package3d\" & compName & ".stp", prjDir & "\Lib\Package3d\" & compName & ".stp", True)
            End If

            If IO.File.Exists(prjImportDir & "\Lib\Package3d\" & compName & ".wrl") Then
                Log("Imported 3d package (.wrl) for component " & compName)
                IO.File.Copy(prjImportDir & "\Lib\Package3d\" & compName & ".wrl", prjDir & "\Lib\Package3d\" & compName & ".wrl", True)
            End If
        Next
    End Sub

    Private Sub ButtonImportAllComponents_Click(sender As Object, e As EventArgs) Handles ButtonImportAllComponents.Click
        ImportAllSymbols()
    End Sub
End Class
