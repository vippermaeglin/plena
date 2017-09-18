'M4 Trading Platform - copyright Modulus Financial Engineering, Inc. - all rights reserved.
'http://www.modulusfe.com

Public Class frmLicenseKeys

    Private Sub Form1_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        SaveSetting("keygen", "settings", "number", TextBox1.Text)
        SaveSetting("keygen", "settings", "expires", txtExpires.Text)
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        TextBox1.Text = GetSetting("keygen", "settings", "number", "10")
        txtExpires.Text = GetSetting("keygen", "settings", "expires", "30")        
    End Sub

    Private Sub TextBox1_GotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles TextBox1.GotFocus
        TextBox1.SelectAll()
    End Sub

    Private Sub txtExpires_GotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles txtExpires.GotFocus
        txtExpires.SelectAll()
    End Sub

    Private Sub TextBox2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TextBox2.Click
        TextBox2.SelectAll()
    End Sub

    Private Sub TextBox2_GotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles TextBox2.GotFocus
        TextBox2.SelectAll()
    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click

        If Not IsNumeric(TextBox1.Text) Then
            MsgBox("Please enter a number", MsgBoxStyle.Exclamation)
            Return
        End If

        Dim expires As Integer = txtExpires.Text
        'Note: For non-expiring keys, set expires to 100000

        TextBox2.Text = ""
        For n As Integer = 1 To CInt(TextBox1.Text)
            TextBox2.Text &= CreateLicense(expires) & vbCrLf
        Next

        'To copy to the clipboard:
        'Clipboard.Clear()
        'Clipboard.SetText(TextBox2.Text)

    End Sub

    Private Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
        Dim key As String = InputBox("License key:")
        If key = "" Then Exit Sub
        MsgBox(IIf(VerifyLicense(key), "License key is valid!", "License key is INVALID!"))
    End Sub

    Function CreateLicense(ByVal Expires As Integer)
        Dim s As String = ""
        Randomize(Now.Month + Now.Day + Now.Second)
        Dim v As Integer
        Dim checksum As Integer
        For n As Integer = 1 To 47
            If Rnd() > 0.5 Then
                v = (Rnd() * (90 - 65)) + 65
                s &= Chr(v)
            Else
                v = (Rnd() * (57 - 48)) + 48
                s &= Chr(v)
            End If
        Next
        Expires = Expires * 2
        Mid(s, 5, 1) = Len(CStr(Expires))
        Mid(s, 30, Len(CStr(Expires))) = Expires
        For n As Integer = 1 To 43
            checksum += Asc(Mid(s, n, 1))
        Next
        checksum += 1000
        Mid(s, 44, Len(CStr(checksum))) = checksum
        Return s
    End Function

    'Checks to see if the license has been altered
    Function VerifyLicense(ByVal License As String) As Boolean
        Dim s As String = ""
        If Len(License) <> 47 Then Return False
        Dim checksum As Integer
        For n As Integer = 1 To 43
            checksum += Asc(Mid(License, n, 1))
        Next
        checksum += 1000
        Dim a As String = Mid(License, 44, Len(CStr(checksum)))
        Return Mid(License, 44, Len(CStr(checksum))) = checksum
    End Function


End Class