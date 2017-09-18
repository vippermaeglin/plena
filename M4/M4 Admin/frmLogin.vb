'M4 Trading Platform - copyright Modulus Financial Engineering, Inc. - all rights reserved.
'http://www.modulusfe.com

Public Class frmLogin

    Private m_ExitOnCancel As Boolean
    Private m_ok As Boolean

    Sub New(ByVal ExitOnCancel As Boolean) 'Flag to exit the application if the login fails
        InitializeComponent()
        m_ExitOnCancel = ExitOnCancel
    End Sub

    Private Sub frmLogin_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        If Not m_ok Then e.Cancel = True
    End Sub

    Private Sub frmLogin_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        txtUsername.Text = GetSetting("M4Admin", "Settings", "Username", "Admin") 'TODO
        txtPassword.Text = GetSetting("M4Admin", "Settings", "Password", "")
    End Sub

    Private Sub cmdOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOK.Click        
        m_ok = True
        SaveSetting("M4Admin", "Settings", "Username", txtUsername.Text)
        SaveSetting("M4Admin", "Settings", "Password", txtPassword.Text)

        'TODO:
        If txtUsername.Text = "Admin" And txtPassword.Text = "password" Then
            DialogResult = System.Windows.Forms.DialogResult.OK
            Close()
            Application.DoEvents()
        Else
            MsgBox("Incorrect login!", MsgBoxStyle.Exclamation)
        End If

    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        m_ok = True
        If m_ExitOnCancel Then
            End
        Else
            DialogResult = System.Windows.Forms.DialogResult.Cancel
            Close()
        End If
    End Sub

    Private Sub Text_GotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles txtPassword.GotFocus, txtUsername.GotFocus
        Dim text As TextBox = DirectCast(sender, TextBox)
        text.SelectAll()
    End Sub

End Class