Imports System.ComponentModel
Imports System.IO.Compression
Imports System.Text.RegularExpressions

Public Class Form1


    Dim kicadPrj As KiCadProject
    Dim kicadImportSrcPrj As KiCadProject
    Dim logger As Logger

    Private Sub ButtonChoosePrjFold_Click(sender As Object, e As EventArgs) Handles ButtonChoosePrjFold.Click
        logger.Log("Selecting project")
        If OpenFileDialogPrjFold.ShowDialog() = DialogResult.OK Then
            TextBoxPrjFoldPath.Text = OpenFileDialogPrjFold.FileName
            logger.Log("Selected valid project located at " & TextBoxPrjFoldPath.Text)
            kicadPrj = New KiCadProject(TextBoxPrjFoldPath.Text.Remove(TextBoxPrjFoldPath.Text.LastIndexOf("\"), TextBoxPrjFoldPath.Text.Length - TextBoxPrjFoldPath.Text.LastIndexOf("\")), logger)
        Else
            logger.Log("Aborted")
        End If
    End Sub

    Private Sub ButtonCreatePrjLib_Click(sender As Object, e As EventArgs) Handles ButtonCreatePrjLib.Click

        kicadPrj.CreateOrCompleteSymbolTable()

        kicadPrj.CreateOrCompleteFootprintTable()

        kicadPrj.InitLibDirectory()

        logger.Log("done")
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        logger = New Logger(Me)

        Try
            kicadPrj = New KiCadProject(TextBoxPrjFoldPath.Text.Remove(TextBoxPrjFoldPath.Text.LastIndexOf("\"), TextBoxPrjFoldPath.Text.Length - TextBoxPrjFoldPath.Text.LastIndexOf("\")), logger)
        Catch ex As Exception
            logger.Log("Base project path is not valid")
        End Try

        Try
            kicadImportSrcPrj = New KiCadProject(TextBoxPrjFoldImportPath.Text.Remove(TextBoxPrjFoldImportPath.Text.LastIndexOf("\"), TextBoxPrjFoldImportPath.Text.Length - TextBoxPrjFoldImportPath.Text.LastIndexOf("\")), logger)
        Catch ex As Exception
            logger.Log("Import source project path is not valid")
        End Try

        If TextBoxPrjFoldPath.Text.Contains(".pro") Then
            ButtonCreatePrjLib.Enabled = True
            ButtonAddCompFromZip.Enabled = True
        Else
            ButtonCreatePrjLib.Enabled = False
            ButtonAddCompFromZip.Enabled = False
        End If

        If TextBoxPrjFoldImportPath.Text.Contains(".pro") Then
            Dim compList As List(Of String) = kicadImportSrcPrj.GetAllCompInPrj()
            For Each comp In compList
                ListBoxImportComp.Items.Add(comp)
            Next
        End If
    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

    End Sub

    Private Sub ButtonAddCompFromZip_Click(sender As Object, e As EventArgs) Handles ButtonAddCompFromZip.Click
        logger.Log("Choosing 'Samacsys - ComponentSearchEngine' component archive to deploy to project lib")
        If OpenFileDialogCompZip.ShowDialog() = DialogResult.OK Then
            Dim zipFilePath = OpenFileDialogCompZip.FileName
            logger.Log("Selected archive located at " & zipFilePath)

            Dim compImporter As SamacsysComponentImporter = New SamacsysComponentImporter(zipFilePath, kicadPrj, logger)

            compImporter.ImportComponent()

            logger.Log("done")
        Else
            logger.Log("Aborted")
        End If
    End Sub


    Private Sub TextBoxPrjFoldPath_TextChanged(sender As Object, e As EventArgs) Handles TextBoxPrjFoldPath.TextChanged
        If TextBoxPrjFoldPath.Text.Contains(".pro") Then
            ButtonCreatePrjLib.Enabled = True
            ButtonAddCompFromZip.Enabled = True
            kicadPrj = New KiCadProject(TextBoxPrjFoldPath.Text.Remove(TextBoxPrjFoldPath.Text.LastIndexOf("\"), TextBoxPrjFoldPath.Text.Length - TextBoxPrjFoldPath.Text.LastIndexOf("\")), logger)
        Else
            ButtonCreatePrjLib.Enabled = False
            ButtonAddCompFromZip.Enabled = False
        End If
    End Sub

    Private Sub ButtonChoosePrjFoldImport_Click(sender As Object, e As EventArgs) Handles ButtonChoosePrjFoldImport.Click
        ListBoxImportComp.Items.Clear()
        logger.Log("Selecting project to import component from")
        If OpenFileDialogPrjFoldImport.ShowDialog() = DialogResult.OK Then
            TextBoxPrjFoldImportPath.Text = OpenFileDialogPrjFoldImport.FileName
            logger.Log("Selected valid project located at " & TextBoxPrjFoldImportPath.Text)

            kicadImportSrcPrj = New KiCadProject(TextBoxPrjFoldImportPath.Text.Remove(TextBoxPrjFoldImportPath.Text.LastIndexOf("\"), TextBoxPrjFoldImportPath.Text.Length - TextBoxPrjFoldImportPath.Text.LastIndexOf("\")), logger)
            Dim compList As List(Of String) = kicadImportSrcPrj.GetAllCompInPrj()
            For Each comp In compList
                ListBoxImportComp.Items.Add(comp)
            Next
        Else
            logger.Log("Aborted")
        End If
    End Sub



    Private Sub ButtonImportComponents_Click(sender As Object, e As EventArgs) Handles ButtonImportComponents.Click
        Dim selectedCompList As List(Of String) = New List(Of String)

        For Each compName As String In ListBoxImportComp.SelectedItems
            selectedCompList.Add(compName)
        Next

        Dim compImport As KicadToKicadComponentImporter = New KicadToKicadComponentImporter(kicadPrj, kicadImportSrcPrj, logger)

        compImport.ImportSelectedSymbols(selectedCompList)

    End Sub



    Private Sub ButtonImportAllComponents_Click(sender As Object, e As EventArgs) Handles ButtonImportAllComponents.Click
        Dim compImport As KicadToKicadComponentImporter = New KicadToKicadComponentImporter(kicadPrj, kicadImportSrcPrj, logger)

        compImport.ImportAllSymbols()
    End Sub
End Class
