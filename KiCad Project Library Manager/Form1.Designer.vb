<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.FolderBrowserDialogProjetFold = New System.Windows.Forms.FolderBrowserDialog()
        Me.ButtonChoosePrjFold = New System.Windows.Forms.Button()
        Me.OpenFileDialogPrjFold = New System.Windows.Forms.OpenFileDialog()
        Me.ButtonCreatePrjLib = New System.Windows.Forms.Button()
        Me.ButtonAddCompFromZip = New System.Windows.Forms.Button()
        Me.OpenFileDialogCompZip = New System.Windows.Forms.OpenFileDialog()
        Me.TextBoxLog = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBoxPrjFoldPath = New System.Windows.Forms.TextBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.ButtonImportComponents = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ListBoxImportComp = New System.Windows.Forms.ListBox()
        Me.ButtonChoosePrjFoldImport = New System.Windows.Forms.Button()
        Me.TextBoxPrjFoldImportPath = New System.Windows.Forms.TextBox()
        Me.OpenFileDialogPrjFoldImport = New System.Windows.Forms.OpenFileDialog()
        Me.ButtonImportAllComponents = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ButtonChoosePrjFold
        '
        Me.ButtonChoosePrjFold.Location = New System.Drawing.Point(616, 17)
        Me.ButtonChoosePrjFold.Name = "ButtonChoosePrjFold"
        Me.ButtonChoosePrjFold.Size = New System.Drawing.Size(154, 23)
        Me.ButtonChoosePrjFold.TabIndex = 0
        Me.ButtonChoosePrjFold.Text = "Choose projet base file (.pro)"
        Me.ButtonChoosePrjFold.UseVisualStyleBackColor = True
        '
        'OpenFileDialogPrjFold
        '
        Me.OpenFileDialogPrjFold.AddExtension = False
        Me.OpenFileDialogPrjFold.DefaultExt = "pro"
        Me.OpenFileDialogPrjFold.FileName = "*"
        Me.OpenFileDialogPrjFold.ValidateNames = False
        '
        'ButtonCreatePrjLib
        '
        Me.ButtonCreatePrjLib.Enabled = False
        Me.ButtonCreatePrjLib.Location = New System.Drawing.Point(6, 45)
        Me.ButtonCreatePrjLib.Name = "ButtonCreatePrjLib"
        Me.ButtonCreatePrjLib.Size = New System.Drawing.Size(165, 49)
        Me.ButtonCreatePrjLib.TabIndex = 2
        Me.ButtonCreatePrjLib.Text = "Create projet library"
        Me.ButtonCreatePrjLib.UseVisualStyleBackColor = True
        '
        'ButtonAddCompFromZip
        '
        Me.ButtonAddCompFromZip.Enabled = False
        Me.ButtonAddCompFromZip.Location = New System.Drawing.Point(6, 118)
        Me.ButtonAddCompFromZip.Name = "ButtonAddCompFromZip"
        Me.ButtonAddCompFromZip.Size = New System.Drawing.Size(158, 62)
        Me.ButtonAddCompFromZip.TabIndex = 3
        Me.ButtonAddCompFromZip.Text = "Import component " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "from Samacsys" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "ComponentSearchEngine" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "archive (.zip)"
        Me.ButtonAddCompFromZip.UseVisualStyleBackColor = True
        '
        'OpenFileDialogCompZip
        '
        Me.OpenFileDialogCompZip.DefaultExt = "zip"
        Me.OpenFileDialogCompZip.FileName = "OpenFileDialog1"
        '
        'TextBoxLog
        '
        Me.TextBoxLog.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxLog.Location = New System.Drawing.Point(6, 22)
        Me.TextBoxLog.Multiline = True
        Me.TextBoxLog.Name = "TextBoxLog"
        Me.TextBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBoxLog.Size = New System.Drawing.Size(764, 100)
        Me.TextBoxLog.TabIndex = 4
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.TextBoxPrjFoldPath)
        Me.GroupBox1.Controls.Add(Me.ButtonChoosePrjFold)
        Me.GroupBox1.Controls.Add(Me.ButtonCreatePrjLib)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 119)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(776, 100)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Project Selection and local project library creation"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(177, 50)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(570, 30)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = resources.GetString("Label1.Text")
        '
        'TextBoxPrjFoldPath
        '
        Me.TextBoxPrjFoldPath.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.KiCad_Project_Library_Manager.My.MySettings.Default, "TB_prj_saved", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.TextBoxPrjFoldPath.Location = New System.Drawing.Point(6, 19)
        Me.TextBoxPrjFoldPath.Name = "TextBoxPrjFoldPath"
        Me.TextBoxPrjFoldPath.Size = New System.Drawing.Size(604, 20)
        Me.TextBoxPrjFoldPath.TabIndex = 1
        Me.TextBoxPrjFoldPath.Text = Global.KiCad_Project_Library_Manager.My.MySettings.Default.TB_prj_saved
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.LinkLabel1)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.ButtonAddCompFromZip)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 225)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(377, 186)
        Me.GroupBox2.TabIndex = 6
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Component import"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(6, 70)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(286, 45)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Warning : Please be aware that the symbol/footprint" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "must be manually inspected t" &
    "o check if they have" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " pinout/dimension errors"
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Location = New System.Drawing.Point(6, 54)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(188, 13)
        Me.LinkLabel1.TabIndex = 5
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "https://componentsearchengine.com/"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(6, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(336, 30)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "This tool allows to automatically import a component " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(symbol/footprint/3d packa" &
    "ge) into the selected project library"
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.TextBoxLog)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 417)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(776, 128)
        Me.GroupBox3.TabIndex = 7
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Log"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.ButtonImportAllComponents)
        Me.GroupBox4.Controls.Add(Me.ButtonImportComponents)
        Me.GroupBox4.Controls.Add(Me.Label4)
        Me.GroupBox4.Controls.Add(Me.ListBoxImportComp)
        Me.GroupBox4.Controls.Add(Me.ButtonChoosePrjFoldImport)
        Me.GroupBox4.Controls.Add(Me.TextBoxPrjFoldImportPath)
        Me.GroupBox4.Location = New System.Drawing.Point(395, 225)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(393, 186)
        Me.GroupBox4.TabIndex = 8
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Transfert component from existing project"
        '
        'ButtonImportComponents
        '
        Me.ButtonImportComponents.Location = New System.Drawing.Point(132, 67)
        Me.ButtonImportComponents.Name = "ButtonImportComponents"
        Me.ButtonImportComponents.Size = New System.Drawing.Size(95, 48)
        Me.ButtonImportComponents.TabIndex = 7
        Me.ButtonImportComponents.Text = "Import selected component(s)"
        Me.ButtonImportComponents.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 46)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(147, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Components in source project"
        '
        'ListBoxImportComp
        '
        Me.ListBoxImportComp.FormattingEnabled = True
        Me.ListBoxImportComp.Location = New System.Drawing.Point(6, 67)
        Me.ListBoxImportComp.Name = "ListBoxImportComp"
        Me.ListBoxImportComp.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.ListBoxImportComp.Size = New System.Drawing.Size(120, 108)
        Me.ListBoxImportComp.TabIndex = 5
        '
        'ButtonChoosePrjFoldImport
        '
        Me.ButtonChoosePrjFoldImport.Location = New System.Drawing.Point(233, 17)
        Me.ButtonChoosePrjFoldImport.Name = "ButtonChoosePrjFoldImport"
        Me.ButtonChoosePrjFoldImport.Size = New System.Drawing.Size(154, 23)
        Me.ButtonChoosePrjFoldImport.TabIndex = 4
        Me.ButtonChoosePrjFoldImport.Text = "Choose projet to import from"
        Me.ButtonChoosePrjFoldImport.UseVisualStyleBackColor = True
        '
        'TextBoxPrjFoldImportPath
        '
        Me.TextBoxPrjFoldImportPath.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.KiCad_Project_Library_Manager.My.MySettings.Default, "TB_prj_import_saved", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.TextBoxPrjFoldImportPath.Location = New System.Drawing.Point(6, 19)
        Me.TextBoxPrjFoldImportPath.Name = "TextBoxPrjFoldImportPath"
        Me.TextBoxPrjFoldImportPath.Size = New System.Drawing.Size(221, 20)
        Me.TextBoxPrjFoldImportPath.TabIndex = 4
        Me.TextBoxPrjFoldImportPath.Text = Global.KiCad_Project_Library_Manager.My.MySettings.Default.TB_prj_import_saved
        '
        'OpenFileDialogPrjFoldImport
        '
        Me.OpenFileDialogPrjFoldImport.AddExtension = False
        Me.OpenFileDialogPrjFoldImport.DefaultExt = "pro"
        Me.OpenFileDialogPrjFoldImport.FileName = "*"
        Me.OpenFileDialogPrjFoldImport.ValidateNames = False
        '
        'ButtonImportAllComponents
        '
        Me.ButtonImportAllComponents.Location = New System.Drawing.Point(132, 118)
        Me.ButtonImportAllComponents.Name = "ButtonImportAllComponents"
        Me.ButtonImportAllComponents.Size = New System.Drawing.Size(95, 48)
        Me.ButtonImportAllComponents.TabIndex = 8
        Me.ButtonImportAllComponents.Text = "Import all components"
        Me.ButtonImportAllComponents.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(12, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(93, 87)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 9
        Me.PictureBox1.TabStop = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(111, 12)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(690, 90)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = resources.GetString("Label5.Text")
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 572)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "KiCad Project Local Library Manager v0.0.1"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents FolderBrowserDialogProjetFold As FolderBrowserDialog
    Friend WithEvents ButtonChoosePrjFold As Button
    Friend WithEvents TextBoxPrjFoldPath As TextBox
    Friend WithEvents OpenFileDialogPrjFold As OpenFileDialog
    Friend WithEvents ButtonCreatePrjLib As Button
    Friend WithEvents ButtonAddCompFromZip As Button
    Friend WithEvents OpenFileDialogCompZip As OpenFileDialog
    Friend WithEvents TextBoxLog As TextBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents LinkLabel1 As LinkLabel
    Friend WithEvents Label2 As Label
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents Label3 As Label
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents ButtonChoosePrjFoldImport As Button
    Friend WithEvents TextBoxPrjFoldImportPath As TextBox
    Friend WithEvents OpenFileDialogPrjFoldImport As OpenFileDialog
    Friend WithEvents ListBoxImportComp As ListBox
    Friend WithEvents Label4 As Label
    Friend WithEvents ButtonImportComponents As Button
    Friend WithEvents ButtonImportAllComponents As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Label5 As Label
End Class
