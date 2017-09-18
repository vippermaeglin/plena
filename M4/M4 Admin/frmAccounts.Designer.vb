<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAccounts
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
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtReport = New System.Windows.Forms.TextBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.cmdAllKeys = New System.Windows.Forms.Button
        Me.cmdSelected = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(12, 28)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(327, 303)
        Me.ListBox1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(113, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Activated license keys"
        '
        'txtReport
        '
        Me.txtReport.Location = New System.Drawing.Point(345, 28)
        Me.txtReport.Multiline = True
        Me.txtReport.Name = "txtReport"
        Me.txtReport.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtReport.Size = New System.Drawing.Size(305, 299)
        Me.txtReport.TabIndex = 33
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.cmdAllKeys)
        Me.Panel1.Controls.Add(Me.cmdSelected)
        Me.Panel1.Location = New System.Drawing.Point(235, 337)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(415, 43)
        Me.Panel1.TabIndex = 35
        '
        'cmdAllKeys
        '
        Me.cmdAllKeys.Location = New System.Drawing.Point(210, 6)
        Me.cmdAllKeys.Name = "cmdAllKeys"
        Me.cmdAllKeys.Size = New System.Drawing.Size(187, 30)
        Me.cmdAllKeys.TabIndex = 36
        Me.cmdAllKeys.Text = "Run Report for All Keys"
        Me.cmdAllKeys.UseVisualStyleBackColor = True
        '
        'cmdSelected
        '
        Me.cmdSelected.Location = New System.Drawing.Point(17, 6)
        Me.cmdSelected.Name = "cmdSelected"
        Me.cmdSelected.Size = New System.Drawing.Size(187, 30)
        Me.cmdSelected.TabIndex = 35
        Me.cmdSelected.Text = "Run Report for Selected Key"
        Me.cmdSelected.UseVisualStyleBackColor = True
        '
        'frmAccounts
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(850, 441)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.txtReport)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ListBox1)
        Me.Name = "frmAccounts"
        Me.Text = "Active License Keys"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtReport As System.Windows.Forms.TextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents cmdAllKeys As System.Windows.Forms.Button
    Friend WithEvents cmdSelected As System.Windows.Forms.Button
End Class
