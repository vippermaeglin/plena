Imports System.Windows.Forms.Form
Imports System.Windows.Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLogin
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtUsername = New System.Windows.Forms.TextBox
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me._lblCaption_0 = New System.Windows.Forms.Label
        Me._lblCaption_1 = New System.Windows.Forms.Label
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.NButton1 = New System.Windows.Forms.Button
        Me._Line1_1 = New System.Windows.Forms.Label
        Me.Image1 = New System.Windows.Forms.PictureBox
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        CType(Me.Image1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtUsername
        '
        Me.txtUsername.Location = New System.Drawing.Point(126, 77)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(159, 20)
        Me.txtUsername.TabIndex = 0
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(126, 101)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(159, 20)
        Me.txtPassword.TabIndex = 1
        '
        '_lblCaption_0
        '
        Me._lblCaption_0.BackColor = System.Drawing.Color.Transparent
        Me._lblCaption_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCaption_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCaption_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCaption_0.Location = New System.Drawing.Point(31, 78)
        Me._lblCaption_0.Name = "_lblCaption_0"
        Me._lblCaption_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCaption_0.Size = New System.Drawing.Size(73, 17)
        Me._lblCaption_0.TabIndex = 10
        Me._lblCaption_0.Text = "&User name:"
        '
        '_lblCaption_1
        '
        Me._lblCaption_1.BackColor = System.Drawing.Color.Transparent
        Me._lblCaption_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCaption_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCaption_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCaption_1.Location = New System.Drawing.Point(31, 102)
        Me._lblCaption_1.Name = "_lblCaption_1"
        Me._lblCaption_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCaption_1.Size = New System.Drawing.Size(73, 17)
        Me._lblCaption_1.TabIndex = 9
        Me._lblCaption_1.Text = "&Password:"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 37)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(200, 100)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'NButton1
        '
        Me.NButton1.Location = New System.Drawing.Point(3, 40)
        Me.NButton1.Name = "NButton1"
        Me.NButton1.Size = New System.Drawing.Size(77, 23)
        Me.NButton1.TabIndex = 1
        Me.NButton1.Text = "&OK"
        '
        '_Line1_1
        '
        Me._Line1_1.BackColor = System.Drawing.SystemColors.Window
        Me._Line1_1.Location = New System.Drawing.Point(-15, 138)
        Me._Line1_1.Name = "_Line1_1"
        Me._Line1_1.Size = New System.Drawing.Size(392, 1)
        Me._Line1_1.TabIndex = 23
        '
        'Image1
        '
        Me.Image1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Image1.ErrorImage = Nothing
        Me.Image1.Image = Global.M4Admin.My.Resources.Resources.Login
        Me.Image1.InitialImage = Nothing
        Me.Image1.Location = New System.Drawing.Point(-1, -3)
        Me.Image1.Name = "Image1"
        Me.Image1.Size = New System.Drawing.Size(343, 60)
        Me.Image1.TabIndex = 25
        Me.Image1.TabStop = False
        '
        'cmdCancel
        '
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(235, 153)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
        Me.cmdCancel.TabIndex = 27
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.Location = New System.Drawing.Point(149, 153)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(77, 23)
        Me.cmdOK.TabIndex = 26
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'frmLogin
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(327, 192)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.Image1)
        Me.Controls.Add(Me._Line1_1)
        Me.Controls.Add(Me._lblCaption_0)
        Me.Controls.Add(Me._lblCaption_1)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.txtUsername)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLogin"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "M4 Admin Login"
        CType(Me.Image1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Public WithEvents _lblCaption_0 As System.Windows.Forms.Label
    Public WithEvents _lblCaption_1 As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents NButton1 As System.Windows.Forms.Button
    Public WithEvents _Line1_1 As System.Windows.Forms.Label
    Public WithEvents Image1 As System.Windows.Forms.PictureBox
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button

End Class
