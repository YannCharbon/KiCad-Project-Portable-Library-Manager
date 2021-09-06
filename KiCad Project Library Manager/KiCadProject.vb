Imports System.Text.RegularExpressions

Public Class KiCadProject
    Public Property PrjDir As String
    Private Property Logger As Logger

    Public Sub New(prjDirectory As String, log As Logger)
        PrjDir = prjDirectory
        Logger = log
    End Sub

    Public Sub CreateOrCompleteSymbolTable()
        If IO.File.Exists(PrjDir & "\sym-lib-table") = False Then
            Logger.Log("Creating 'sym-lib-table' file")
            IO.File.WriteAllBytes(PrjDir & "\sym-lib-table", My.Resources.sym_lib_table)
        Else
            Logger.Log("'sym-lib-table' file already exists. Adding '0_project_symbols' lib to it.")

            Dim symlibtableContent = IO.File.ReadAllText(PrjDir & "\sym-lib-table")
            Dim contentWithNewLib As String = ""

            If symlibtableContent.Contains("0_project_symbols") Then
                Logger.Log("'0_project_symbols' lib already exists in 'sym-lib-table'. No modification needed.")
            Else
                contentWithNewLib = symlibtableContent.Insert(symlibtableContent.LastIndexOf(")") - 2, vbCrLf & "  (lib (name 0_project_symbols)(type Legacy)(uri ${KIPRJMOD}/Lib/0_project_symbols.lib)(options "")(descr ""))")
            End If

            IO.File.WriteAllText(PrjDir & "\sym-lib-table", contentWithNewLib)

        End If
    End Sub

    Public Sub CreateOrCompleteFootprintTable()
        If IO.File.Exists(PrjDir & "\fp-lib-table") = False Then
            Logger.Log("Creating 'fp-lib-table' file")
            IO.File.WriteAllBytes(PrjDir & "\fp-lib-table", My.Resources.fp_lib_table)
        Else

            Logger.Log("'fp-lib-table' file already exists. Adding '0_project_footprints' lib to it.")

            Dim fplibtableContent = IO.File.ReadAllText(PrjDir & "\fp-lib-table")
            Dim contentWithNewLib As String = ""

            If fplibtableContent.Contains("0_project_footprints") Then
                Logger.Log("'0_project_footprints' lib already exists in 'fp-lib-table'. No modification needed.")
            Else
                contentWithNewLib = fplibtableContent.Insert(fplibtableContent.LastIndexOf(")") - 2, vbCrLf & "  (lib (name 0_project_footprints)(type KiCad)(uri ${KIPRJMOD}/Lib/0_project_footprints.pretty)(options "")(descr ""))")
            End If

            IO.File.WriteAllText(PrjDir & "\fp-lib-table", contentWithNewLib)
        End If
    End Sub

    Public Sub InitLibDirectory()
        If IO.Directory.Exists(PrjDir & "\Lib") = False Then
            Logger.Log("Creating 'Lib/' folder")

            IO.Directory.CreateDirectory(PrjDir & "\Lib")
            IO.Directory.CreateDirectory(PrjDir & "\Lib\0_project_footprints.pretty")
            IO.Directory.CreateDirectory(PrjDir & "\Lib\Package3d")

            IO.File.WriteAllBytes(PrjDir & "\Lib\0_project_symbols.dcm", My.Resources._0_project_symbols_dcm)
            IO.File.WriteAllBytes(PrjDir & "\Lib\0_project_symbols.lib", My.Resources._0_project_symbols_lib)
        Else
            Logger.Log("'\Lib' folder already exists. Skipping. (Manually delete to redeploy empty library)")
        End If
    End Sub

    Public Function GetAllCompInPrj() As List(Of String)
        Dim lines = IO.File.ReadAllLines(PrjDir & "\Lib\0_project_symbols.lib")
        Dim compList As List(Of String) = New List(Of String)
        For Each line As String In lines
            If line.Contains("DEF ") And Not line.Contains("ENDDEF") Then
                Dim compName As String = Regex.Match(line, "(?<=DEF\s)(.*?)(?=\s)").Value
                compList.Add(compName)
                'Logger.Log("Detected component '" & compName & "' in import project")
            End If
        Next

        Return compList
    End Function

    Public Function CheckIfCompHas3dModel(ByVal compName As String) As Boolean
        If IO.File.Exists(PrjDir & "\Lib\Package3d\" & compName & ".stp") Then
            Return True
        End If

        If IO.File.Exists(PrjDir & "\Lib\Package3d\" & compName & ".wrl") Then
            Return True
        End If

        Return False
    End Function

    Public Sub Add3dModel(ByVal compName As String, ByVal model3dFilePath As String)
        If model3dFilePath.Split(".").Last = "stp" Or model3dFilePath.Split(".").Last = "step" Then
            If IO.File.Exists(PrjDir & "\Lib\Package3d\" & compName & ".stp") = False Then
                IO.File.Copy(model3dFilePath, PrjDir & "\Lib\Package3d\" & compName & ".stp")
                Logger.Log("Added 3D Model to '" & compName & "'")
            Else
                Logger.Log("3D model (.stp) already exists. Skipping.")
            End If
        End If

        If model3dFilePath.Split(".").Last = "wrl" Then
            If IO.File.Exists(PrjDir & "\Lib\Package3d\" & compName & ".wrl") = False Then
                IO.File.Copy(model3dFilePath, PrjDir & "\Lib\Package3d\" & compName & ".wrl")
                Logger.Log("Added 3D Model to '" & compName & "'")
            Else
                Logger.Log("3D model (.wrl) already exists. Skipping.")
            End If
        End If
    End Sub
End Class
