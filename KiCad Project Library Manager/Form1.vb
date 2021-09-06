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
            Me.Text = Me.Text & " - " & TextBoxPrjFoldPath.Text.Split("\").Last.Split(".").First
            LabelPrj.Text = "Current project : " & TextBoxPrjFoldPath.Text.Split("\").Last.Split(".").First
            LabelPrj.BackColor = Color.Teal
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
            ButtonAddCompFromUltraLibZip.Enabled = True
            ButtonAddCompFromSnapEdaZip.Enabled = True
            Me.Text = Me.Text & " - " & TextBoxPrjFoldPath.Text.Split("\").Last.Split(".").First
            LabelPrj.Text = "Current project : " & TextBoxPrjFoldPath.Text.Split("\").Last.Split(".").First
            LabelPrj.BackColor = Color.Teal

            GetCurrentPrjComp()
        Else
            ButtonCreatePrjLib.Enabled = False
            ButtonAddCompFromZip.Enabled = False
            ButtonAddCompFromUltraLibZip.Enabled = False
            ButtonAddCompFromSnapEdaZip.Enabled = False
            Me.Text = Me.Text.Split("-").First
            LabelPrj.Text = "No project selected"
            LabelPrj.BackColor = Color.Peru
        End If

        If TextBoxPrjFoldImportPath.Text.Contains(".pro") Then
            Dim compList As List(Of String) = kicadImportSrcPrj.GetAllCompInPrj()
            For Each comp In compList
                ListBoxImportComp.Items.Add(comp)
            Next
        End If
    End Sub

    Public Sub GetCurrentPrjComp()
        ListViewCompInPrj.Items.Clear()

        Dim compList As List(Of String) = kicadPrj.GetAllCompInPrj()
        For Each comp In compList
            Dim itm As New ListViewItem

            itm.Text = comp

            If kicadPrj.CheckIfCompHas3dModel(comp) Then
                itm.SubItems.Add("Yes")
            Else
                itm.SubItems.Add("No")
            End If


            ListViewCompInPrj.Items.Add(itm)
        Next

        ListViewCompInPrj.Refresh()
    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

    End Sub

    ''' <summary>
    ''' Add comp from Samacsys
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
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

    ''' <summary>
    ''' Add Comp from Ultra Librarian
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ButtonAddCompFromUltraLibZip_Click(sender As Object, e As EventArgs) Handles ButtonAddCompFromUltraLibZip.Click
        logger.Log("Choosing 'Ultra Librarian' component archive to deploy to project lib")
        If OpenFileDialogCompZip.ShowDialog() = DialogResult.OK Then
            Dim zipFilePath = OpenFileDialogCompZip.FileName
            logger.Log("Selected archive located at " & zipFilePath)

            Dim compImporter As UltraLibComponentImporter = New UltraLibComponentImporter(zipFilePath, kicadPrj, logger)

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
            ButtonAddCompFromUltraLibZip.Enabled = True
            ButtonAddCompFromSnapEdaZip.Enabled = True
            kicadPrj = New KiCadProject(TextBoxPrjFoldPath.Text.Remove(TextBoxPrjFoldPath.Text.LastIndexOf("\"), TextBoxPrjFoldPath.Text.Length - TextBoxPrjFoldPath.Text.LastIndexOf("\")), logger)
            Me.Text = Me.Text & " - " & TextBoxPrjFoldPath.Text.Split("\").Last.Split(".").First
            LabelPrj.Text = "Current project : " & TextBoxPrjFoldPath.Text.Split("\").Last.Split(".").First
            LabelPrj.BackColor = Color.Teal

            GetCurrentPrjComp()
        Else
            ButtonCreatePrjLib.Enabled = False
            ButtonAddCompFromZip.Enabled = False
            ButtonAddCompFromUltraLibZip.Enabled = False
            ButtonAddCompFromSnapEdaZip.Enabled = False
            Me.Text = Me.Text.Split("-").First
            LabelPrj.Text = "No project selected"
            LabelPrj.BackColor = Color.Peru
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

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        System.Diagnostics.Process.Start("https://componentsearchengine.com/")
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        System.Diagnostics.Process.Start("https://www.ultralibrarian.com/")
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        System.Diagnostics.Process.Start("https://www.snapeda.com/")
    End Sub

    Private Sub ButtonAddCompFromSnapEdaZip_Click(sender As Object, e As EventArgs) Handles ButtonAddCompFromSnapEdaZip.Click
        logger.Log("Choosing 'Ultra Librarian' component archive to deploy to project lib")
        If OpenFileDialogCompZip.ShowDialog() = DialogResult.OK Then
            Dim zipFilePath = OpenFileDialogCompZip.FileName
            logger.Log("Selected archive located at " & zipFilePath)

            Dim compImporter As SnapEdaComponentImporter = New SnapEdaComponentImporter(zipFilePath, kicadPrj, logger)

            compImporter.ImportComponent()

            logger.Log("done")
        Else
            logger.Log("Aborted")
        End If
    End Sub

    Private Sub ButtonAdd3dToComp_Click(sender As Object, e As EventArgs) Handles ButtonAdd3dToComp.Click
        If ListViewCompInPrj.SelectedIndices.Count = 1 Then
            logger.Log("Choosing component 3D model (.stp or .wrl) to add to select component")
            If OpenFileDialogCompZip.ShowDialog() = DialogResult.OK Then
                Dim model3dFilePath = OpenFileDialogCompZip.FileName
                logger.Log("Selected file located at " & model3dFilePath)

                kicadPrj.Add3dModel(ListViewCompInPrj.SelectedItems(0).Text, model3dFilePath)

                logger.Log("done")

                GetCurrentPrjComp()
            Else
                logger.Log("Aborted")
            End If
        End If
    End Sub

    Private Sub ButtonAbout_Click(sender As Object, e As EventArgs) Handles ButtonAbout.Click
        MsgBox("Project link : https://github.com/YannCharbon/KiCad-Project-Portable-Library-Manager", MsgBoxStyle.Information)
    End Sub
End Class
