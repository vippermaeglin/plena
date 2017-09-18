'M4 Trading Platform - copyright Modulus Financial Engineering, Inc. - all rights reserved.
'http://www.modulusfe.com

Option Strict Off
Option Explicit On
Option Compare Text

Imports M4Admin.modulusfe.platform

Public Class frmAccounts
    'TODO: get the last price from your data API in the GetLastPrice function below

    Private WithEvents svc As New Service

    Private Structure Report
        Dim License As String
        Dim NumberOfTrades As Long
        Dim TotalPL As Double
        Dim PortfolioValue As Double
        Dim AccountCashBalance As Double
    End Structure

    Private reports As New List(Of Report)
    Private reportSummary As Report
    Private m_StartingBalance As Double
    Private licenseKeys() As Object
    Private dt As New DataTable

    Private Sub frmAccounts_Load (ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ListKeys()
        dt.Columns.Add ("OrderID")
        dt.Columns.Add ("Status")
        dt.Columns.Add ("Details")
        dt.Columns.Add ("Time")
        dt.Columns.Add ("Symbol")
        dt.Columns.Add ("Type")
        dt.Columns.Add ("Qty")
        dt.Columns.Add ("Entry")
        dt.Columns.Add ("Last")
        dt.Columns.Add ("DollarGain")
        dt.Columns.Add ("PercentGain")
        Panel1.Left = Width - Panel1.Width - 10
        Panel1.Top = Height - 90
        ListBox1.Height = Height - ListBox1.Top - 42 - 50
        txtReport.Width = Width - 16 - txtReport.Left
        txtReport.Height = ListBox1.Height
    End Sub

    Private Sub frmAccounts_Resize (ByVal sender As Object, ByVal e As EventArgs) Handles Me.Resize
        Panel1.Left = Width - Panel1.Width - 10
        Panel1.Top = Height - 90
        ListBox1.Height = Height - ListBox1.Top - 42 - 50
        txtReport.Width = Width - 16 - txtReport.Left
        txtReport.Height = ListBox1.Height
    End Sub

    Private Sub ListKeys()
        Try
            licenseKeys = svc.ListAllUserKeys (frmMain.ClientID, frmMain.ClientPassword)
        Catch ex As Exception
            MsgBox (ex.Message, MsgBoxStyle.Exclamation)
            Return
        End Try
        ListBox1.Items.Clear()
        For n As Integer = 0 To licenseKeys.Length - 1
            ListBox1.Items.Add (licenseKeys (n).ToString)
        Next
        Label1.Text = "Activated license keys (" & licenseKeys.Length & " total)"
    End Sub

    'Returns the number of trades, total P&L, portfolio value and account balance (cash) for one user.
    Private Sub GetReport (ByVal License As String)

        dt.Clear()
        reports.Clear()
        reportSummary.NumberOfTrades = 0
        reportSummary.License = License
        reportSummary.PortfolioValue = 0
        reportSummary.AccountCashBalance = 0
        reportSummary.TotalPL = 0

        'Retrieve available portfolios from the server for this license key
        Dim portfolios As New List(Of String)
'GlobalSettings.PortfoliosList

        'List all portfolios in the user's web service entry list
        Dim data As Object()
        Try
            data = svc.ListUserData (frmMain.ClientID, frmMain.ClientPassword, License)
        Catch se As Exception
            'There are no portfolios or the license key is expired
            Return
        End Try

        'Save the portfolio names related to the supplied license key
        For n As Integer = 0 To data.Length - 1
            If Mid (data (n), 1, Len ("Portfolio: ")) = "Portfolio: " Then
                portfolios.Add (Replace (data (n), "Portfolio: ", ""))
            End If
        Next

        'Now open each portfolio and run the report
        For n As Integer = 0 To portfolios.Count - 1
            LoadPortfolio (portfolios (n), License)
        Next

        SetReportSummary (License)

    End Sub


#Region "Portfolio Loading"


    Public Class Orders
        Public Enum Status
            Sending = 1
'Order is being transmitted
            Sent = 2
'Order has been routed
            Received = 3
'Order has been received
            CancelSending = 4
'Cancel request is being sent 
            CancelSent = 5
'Cancel request has been sent
            CancelReceived = 6
' Cancel request has been received 
            Out = 7
'Your order is out of the market 
            Filled = 8
'Order has been filled 
            PartialFill = 9
'Your order has been partially filled 
            Expired = 10
'Time limit of the order has expired 
            Rejected = 11
'Order was rejected
            Unknown = 12
'Indicates a problem - call your broker!
        End Enum

        Public Enum Side
            LongSide = 1
            ShortSide = 2
            Unknown = 3
        End Enum

        Public Shared Function SideToString (ByVal Value As Side) As String
            Return IIf (Value = Side.LongSide, "Long", "Short")
        End Function

        Public Shared Function SideFromString (ByVal Value As String) As Side
            Return IIf (Value = "Long", Side.LongSide, Side.ShortSide)
        End Function

        Public Shared Function StatusToString (ByVal Value As Status) As String
            Select Case Value
                Case Status.Sending
                    Return "Sending"
                Case Status.Sent
                    Return "Sent"
                Case Status.Received
                    Return "Received"
                Case Status.CancelSending
                    Return "Cancel Sending"
                Case Status.CancelSent
                    Return "Cancel Sent"
                Case Status.CancelReceived
                    Return "Cancel Received"
                Case Status.Out
                    Return "Out"
                Case Status.Filled
                    Return "Filled"
                Case Status.PartialFill
                    Return "Partial Fill"
                Case Status.Expired
                    Return "Expired"
                Case Status.Rejected
                    Return "Rejected"
                Case Status.Unknown
                    Return "Unknown"
            End Select
            Return "Unknown"
        End Function

        Public Shared Function StatusFromString (ByVal Value As String) As Status
            Select Case Value
                Case "Sending"
                    Return Status.Sending
                Case "Sent"
                    Return Status.Sent
                Case "Received"
                    Return Status.Received
                Case "Cancel Sending"
                    Return Status.CancelSending
                Case "Cancel Sent"
                    Return Status.CancelSent
                Case "Cancel Received"
                    Return Status.CancelReceived
                Case "Out"
                    Return Status.Out
                Case "Filled"
                    Return Status.Filled
                Case "Partial Fill"
                    Return Status.PartialFill
                Case "Expired"
                    Return Status.Expired
                Case "Rejected"
                    Return Status.Rejected
                Case "Unknown"
                    Return Status.Unknown
            End Select
            Return Status.Unknown
'Safest
        End Function
    End Class

    Public Class Order
        Public Enum OrderType
            Limit = 1
'Limit
            Market = 2
            StopLimit = 3
            StopMarket = 4
            Unknown = 5
        End Enum

        Public Enum Expiration
            GoodTillCanceled = 1
            GoodTillCanceledPlusAfterHours = 2
            DayOrder = 3
            DayOrderPlusAfterHours = 4
            Unknown = 5
        End Enum

        Public Shared Function OrderTypeToString (ByVal Value As OrderType) As String
            Select Case Value
                Case OrderType.Limit
                    Return "Limit"
                Case OrderType.Market
                    Return "Market"
                Case OrderType.StopLimit
                    Return "Stop Limit"
                Case OrderType.StopMarket
                    Return "Stop Market"
                Case Else
                    Return "Unknown"
            End Select
        End Function

        Public Shared Function OrderTypeFromString (ByVal Value As String) As OrderType
            Dim ret As OrderType
            Select Case Value
                Case "Limit"
                    ret = OrderType.Limit
                Case "Market"
                    ret = OrderType.Market
                Case "Stop Limit"
                    ret = OrderType.StopLimit
                Case "Stop Market"
                    ret = OrderType.StopMarket
                Case Else
                    ret = OrderType.Unknown
            End Select
            Return ret
        End Function

        Public Shared Function ExpirationToString (ByVal Value As Expiration) As String
            Select Case Value
                Case Expiration.DayOrder
                    Return "Day"
                Case Expiration.DayOrderPlusAfterHours
                    Return "Day+EXT"
                Case Expiration.GoodTillCanceled
                    Return "GTC"
                Case Expiration.GoodTillCanceledPlusAfterHours
                    Return "GTC+EXT"
                Case Else
                    Return "Unknown"
            End Select
        End Function

        Public Shared Function ExpirationFromString (ByVal Value As String) As Expiration
            Dim ret As Expiration
            Select Case Value
                Case "Day"
                    ret = Expiration.DayOrder
                Case "Day+EXT"
                    ret = Expiration.DayOrderPlusAfterHours
                Case "GTC"
                    ret = Expiration.GoodTillCanceled
                Case "GTC+EXT"
                    ret = Expiration.GoodTillCanceledPlusAfterHours
                Case Else
                    ret = Expiration.Unknown
            End Select
            Return ret
        End Function
    End Class


    'Loads an encrypted portfolio file
    Private Sub LoadPortfolio (ByVal PortfolioName As String, ByVal License As String)

        'Load the portfolio from the web service
        Dim portfolio As String = PortfolioName
        Dim encryptedPortfolio As String = ""
        Try
            encryptedPortfolio = _
                svc.GetUserData (frmMain.ClientID, frmMain.ClientPassword, License, "Portfolio: " & portfolio)
        Catch se As Exception
            MsgBox("Failed to load portfolio", MsgBoxStyle.Exclamation, " ")
            Return
        End Try

        'Descript the portfolio
        Dim secure As New Security
        Dim decryptedPortfolio As String = ""
        Try
            decryptedPortfolio = secure.Decrypt(encryptedPortfolio)
        Catch ex As Exception
            MsgBox("Failed to load portfolio", MsgBoxStyle.Exclamation, " ")
            Return
        End Try

        'The first two lines are portfolio name and starting balance
        Dim rows() As String = Split (decryptedPortfolio, vbCrLf)
        If rows.Length < 3 Then
            'No trades
            Return
        End If

        'm_portfolioName = rows(0)
        m_StartingBalance = rows (1)

        'Orderid, status, details time, symbol, type, qty, entry, last, $ gain, % gain        
        For row As Integer = 2 To rows.Length - 1
            Dim cols() As String = Split (rows (row), "|")
            dt.Rows.Add (cols)
        Next

        CalculatePortfolio (License)

    End Sub

#End Region


#Region "Calculations"

    ' Shows portfolio summary
    Private Sub CalculatePortfolio (ByVal License As String)
        GetLastPrices()
        CalculateGains()
        CalculatePortfolioValue (License)
    End Sub


    Private Sub CalculateGains()

        Dim status As Orders.Status

        For n As Integer = 0 To dt.Rows.Count - 1

            status = Orders.StatusFromString (dt.Rows (n).Item ("Status"))
            If status <> Orders.Status.CancelReceived And status <> Orders.Status.Rejected And _
               status <> Orders.Status.Expired Then

                Dim LastPrice As Double = dt.Rows (n).Item ("Last")
                Dim EntryPrice As Double = Val (dt.Rows (n).Item ("Entry"))
                Dim Quantity As Integer = Val (dt.Rows (n).Item ("Qty"))

                Dim valGain As Double
                Dim pctGain As Double

                If dt.Rows (n).Item ("Type") = "Long" Then 'Long position
                    valGain = LastPrice - EntryPrice
                    pctGain = Math.Round (LastPrice/EntryPrice - 1, 4)*100
                ElseIf dt.Rows (n).Item ("Type").Value = "Short" Then 'Short position
                    valGain = EntryPrice - LastPrice
                    pctGain = Math.Round (EntryPrice/LastPrice - 1, 4)*100
                Else
                    valGain = 0
                    pctGain = 0
                End If

                dt.Rows (n).Item ("DollarGain") = FormatCurrency (valGain*Quantity, 2)
                dt.Rows (n).Item ("PercentGain") = Math.Round (pctGain, 2) & "%"

            End If

        Next

    End Sub

    'Calculates values for entire portfolio (top label)
    Private Sub CalculatePortfolioValue (ByVal License As String)

        Dim cashUsed As Double
        Dim totalPnL As Double

        Dim status As Orders.Status

        For n As Integer = 0 To dt.Rows.Count - 1

            status = Orders.StatusFromString (dt.Rows (n).Item ("Status"))
            If status <> Orders.Status.CancelReceived And status <> Orders.Status.Rejected And _
               status <> Orders.Status.Expired Then
                cashUsed += (Val (dt.Rows (n).Item ("Entry"))*Val (dt.Rows (n).Item ("Qty")))
                totalPnL += CDbl (dt.Rows (n).Item ("DollarGain"))
            End If

        Next

        Dim ret As New Report
        ret.AccountCashBalance = FormatCurrency (m_StartingBalance - cashUsed, 2)
        ret.PortfolioValue = FormatCurrency (m_StartingBalance + totalPnL, 2)
        ret.TotalPL = FormatCurrency (totalPnL, 2)
        ret.NumberOfTrades = dt.Rows.Count
        ret.License = License
        reports.Add (ret)

    End Sub

    Private Sub GetLastPrices()
        Dim Symbol As String
        Dim status As Orders.Status
        For n As Integer = 0 To dt.Rows.Count - 1
            Symbol = dt.Rows (n).Item ("Symbol")
            'Update the price only if we're not out of the position
            status = Orders.StatusFromString (dt.Rows (n).Item ("Status"))
            If status <> Orders.Status.Out And status <> Orders.Status.CancelReceived Then
                dt.Rows (n).Item ("Last") = GetLastPrice (Symbol)
            End If
        Next
    End Sub


    Public Function GetLastPrice (ByVal Symbol As String) As Double
        'TODO: get the last price from your data API here
        Stop
        Return 0
    End Function


    'Returns a summary of all portfolios for the current account
    Private Sub SetReportSummary (ByVal License As String)
        reportSummary.NumberOfTrades = 0
        reportSummary.License = License
        reportSummary.PortfolioValue = 0
        reportSummary.AccountCashBalance = 0
        reportSummary.TotalPL = 0
        For n As Integer = 0 To reports.Count - 1
            reportSummary.PortfolioValue += reports (n).PortfolioValue
            reportSummary.AccountCashBalance += reports (n).AccountCashBalance
            reportSummary.NumberOfTrades += reports (n).NumberOfTrades
            reportSummary.TotalPL += reports (n).TotalPL
        Next
    End Sub


#End Region


    Private Sub cmdAllKeys_Click (ByVal sender As Object, ByVal e As EventArgs) Handles cmdAllKeys.Click

        txtReport.Text = ""

        For n As Integer = 0 To licenseKeys.Length - 1

            GetReport (CStr (licenseKeys (n)))

            If reportSummary.PortfolioValue > 0 Then
                txtReport.Text &= "Report for " & licenseKeys (n) & vbCrLf & vbCrLf
                txtReport.Text &= "Portfolio Value: " & reportSummary.PortfolioValue & vbCrLf
                txtReport.Text &= "Account Cash Balance: " & reportSummary.AccountCashBalance & vbCrLf
                txtReport.Text &= "Number of Trades: " & reportSummary.NumberOfTrades & vbCrLf
                txtReport.Text &= "Total P&L: " & reportSummary.TotalPL & vbCrLf & vbCrLf & _
                                  "-----------------------------------------------------------------------------" & _
                                  vbCrLf & vbCrLf
            End If
        Next

    End Sub

    Private Sub cmdSelected_Click (ByVal sender As Object, ByVal e As EventArgs) Handles cmdSelected.Click

        If ListBox1.Text = "" Then Exit Sub

        GetReport (ListBox1.Text)

        txtReport.Text = "Report for " & ListBox1.Text & vbCrLf & vbCrLf
        txtReport.Text &= "Portfolio Value: " & reportSummary.PortfolioValue & vbCrLf
        txtReport.Text &= "Account Cash Balance: " & reportSummary.AccountCashBalance & vbCrLf
        txtReport.Text &= "Number of Trades: " & reportSummary.NumberOfTrades & vbCrLf
        txtReport.Text &= "Total P&L: " & reportSummary.TotalPL & vbCrLf

    End Sub
End Class