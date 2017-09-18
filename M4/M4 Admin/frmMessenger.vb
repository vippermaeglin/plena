'M4 Trading Platform - copyright Modulus Financial Engineering, Inc. - all rights reserved.
'http://www.modulusfe.com

Option Strict Off
Option Explicit On
Option Compare Text

Imports M4Admin.modulusfe.platform

Public Class frmMessenger

    Private WithEvents svc As New Service

    Private licenseKeys() As Object    

    Private Sub frmAccounts_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ListKeys()
        Panel1.Left = Width - Panel1.Width - 10
        Panel1.Top = Height - 90
        ListBox1.Height = Height - ListBox1.Top - 42 - 50
        txtMessage.Width = Width - 16 - txtMessage.Left
        txtMessage.Height = ListBox1.Height
    End Sub

    Private Sub frmAccounts_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Resize
        Panel1.Left = Width - Panel1.Width - 10
        Panel1.Top = Height - 90
        ListBox1.Height = Height - ListBox1.Top - 42 - 50
        txtMessage.Width = Width - 16 - txtMessage.Left
        txtMessage.Height = ListBox1.Height
    End Sub

    Private Sub ListKeys()
        Try
            licenseKeys = svc.ListAllUserKeys(frmMain.ClientID, frmMain.ClientPassword)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
            Return
        End Try
        ListBox1.Items.Clear()
        For n As Integer = 0 To licenseKeys.Length - 1
            ListBox1.Items.Add(licenseKeys(n).ToString)
        Next
        Label1.Text = "Activated license keys (" & licenseKeys.Length & " total)"
    End Sub

    Private Function SendMessage(ByVal License As String) As Boolean
        Dim key As String = "ALERT|" & Now() & "|SCRIPT|MESSAGE|0"
        Dim data As String = Now & "|" & txtMessage.Text
        Try
            svc.SetUserData(frmMain.ClientID, frmMain.ClientPassword, License, key, data)
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function
 
    Private Sub cmdSelected_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSelected.Click
        If ListBox1.Text = "" Then Exit Sub
        cmdSendAll.Enabled = False
        cmdSelected.Enabled = False
        If SendMessage(ListBox1.Text) Then
            MsgBox("Message sent!", MsgBoxStyle.Information)
        Else
            MsgBox("Failed to send message!", MsgBoxStyle.Exclamation)
        End If
        cmdSendAll.Enabled = True
        cmdSelected.Enabled = True
    End Sub

    Private Sub cmdSendAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSendAll.Click
        cmdSendAll.Enabled = False
        cmdSelected.Enabled = False
        Dim sent As Integer
        For n As Integer = 0 To ListBox1.Items.Count - 1
            If SendMessage(ListBox1.Items(n)) Then
                sent += 1
            End If
        Next
        MsgBox(sent & " out of " & ListBox1.Items.Count - 1 & " messages received!", MsgBoxStyle.Information)
        cmdSendAll.Enabled = True
        cmdSelected.Enabled = True
    End Sub

End Class