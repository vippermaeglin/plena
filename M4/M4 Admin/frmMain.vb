'M4 Trading Platform - copyright Modulus Financial Engineering, Inc. - all rights reserved.
'http://www.modulusfe.com
Imports M4Admin.modulusfe.platform

Public Class frmMain

    Public Shared ClientID As String = "FROEDE" 'TODO: enter your Modulus client id here
    Public Shared ClientPassword As String = "847263854" 'TODO: enter your Modulus client password here
    Public Shared ClientTitle As String = "M4" 'TODO: enter the title of your application here

    Private WithEvents svc As New Service

    Private licenseKeys As List(Of String) 'Gets populated with all activated license keys

    Sub New()
        InitializeComponent()
        'Show the login (username and password are set inside the form)

        'Stop 'TODO: Ensure you have set ClientID and ClientPassword above!

        Dim login As New frmLogin(True)
        If login.ShowDialog <> System.Windows.Forms.DialogResult.OK Then
            End
        End If

    End Sub

    'Displays the license key form
    Private Sub ToolStripButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ToolStripButton1.Click
        Dim keys As New frmLicenseKeys
        keys.MdiParent = Me
        keys.Show()
    End Sub

    'Runs reports on one or all accounts
    Private Sub ToolStripButton4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ToolStripButton4.Click
        Dim accounts As New frmAccounts
        accounts.MdiParent = Me
        accounts.Show()
    End Sub

    'Deactivates a license key on the server (the M4 desktop or mobile client will not run again).
    Private Sub ToolStripButton2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ToolStripButton2.Click
        Dim licKey As String = InputBox("License key:", , "")
        If licKey.Length <> 47 Then Exit Sub
        Try
            If svc.RemoveUser(ClientID, ClientPassword, licKey) Then
                MsgBox("License key removed!", MsgBoxStyle.Information)
            Else
                MsgBox("Failed to remove liecnse key.", MsgBoxStyle.Exclamation)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        End Try
    End Sub

    Private Sub ToolStripButton3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ToolStripButton3.Click
        Dim messages As New frmMessenger
        messages.MdiParent = Me
        messages.Show()
    End Sub

End Class
