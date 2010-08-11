Imports System.Runtime.InteropServices
Imports System.Threading
Imports HIDLibrary

Public Class IT4XXX

#Region "--> Private Constants "

    Private Const IT4XXX_VENDOR_ID As Integer = &H536

    Private Const HID_REPORT_ID As Integer = &H4
    Private Const IT4XXX_ERROR_BEEP As Integer = &H20
    Private Const IT4XXX_SUCCESS_BEEP As Integer = &H40
    Private Const IT4XXX_START_SCAN As Integer = &H4
    Private Const IT4XXX_END_SCAN As Integer = &H1

#End Region

#Region "--> Public Events "

    Public Event Inserted()
    Public Event Removed()
    Public Event DataRecieved(ByVal Data As Byte())

#End Region

#Region "--> Private Fields "

    Private WithEvents _IT4XXX As HidDevice
    Private _IsListening As Boolean = False
    Private _IsReading As Integer = 0
    Private _SupportedProductIds As Integer()

#End Region

#Region "--> Constructor "

    Public Sub New(ByVal DevicePath As String, ByVal SupportedProductIds As Integer())

        _SupportedProductIds = SupportedProductIds

        _IT4XXX = HidDevices.GetDevice(DevicePath)

        _IT4XXX.Open()

        BeginReadReport()

    End Sub

    Public Sub New(ByVal HidDevice As HidDevice, ByVal SupportedProductIds As Integer())

        _SupportedProductIds = SupportedProductIds

        _IT4XXX = HidDevice

        If _IT4XXX.IsOpen = False Then _IT4XXX.Open()

        BeginReadReport()

    End Sub

#End Region

#Region "--> Public Shared Methods "

    Public Shared Function Enumerate(ByVal ProductIds As Integer()) As IT4XXX()

        Dim IT4XXXDevices As New ArrayList
        Dim Devices As HidDevice()
        Dim Device As HidDevice
        Dim ProductId As Integer

        For Each ProductId In ProductIds

            Devices = HidDevices.Enumerate(IT4XXX_VENDOR_ID, ProductId)

            For Each Device In Devices

                IT4XXXDevices.Add(New IT4XXX(Device, ProductIds))

            Next

        Next

        Return CType(IT4XXXDevices.ToArray(GetType(IT4XXX)), IT4XXX())

    End Function

#End Region

#Region "--> Public Properties "

    Public ReadOnly Property DevicePath() As String
        Get

            Return _IT4XXX.DevicePath

        End Get
    End Property

    Public ReadOnly Property DeviceReadHandle() As Integer
        Get

            Return _IT4XXX.ReadHandle

        End Get
    End Property

    Public ReadOnly Property DeviceWriteHandle() As Integer
        Get

            Return _IT4XXX.WriteHandle

        End Get
    End Property

    Public ReadOnly Property IsListening() As Boolean
        Get

            Return _IsListening

        End Get
    End Property

    Public ReadOnly Property IsConnected() As Boolean
        Get

            Return _IT4XXX.IsConnected

        End Get
    End Property

#End Region

#Region "--> Public Methods "

    Public Sub ErrorBeep()

        Dim Report As HidReport = _IT4XXX.CreateReport

        Report.ReportId = HID_REPORT_ID
        Report.Data(0) = IT4XXX_ERROR_BEEP

        _IT4XXX.WriteReport(Report)

    End Sub

    Public Sub SuccessBeep()

        Dim Report As HidReport = _IT4XXX.CreateReport

        Report.ReportId = HID_REPORT_ID
        Report.Data(0) = IT4XXX_SUCCESS_BEEP

        _IT4XXX.WriteReport(Report)

    End Sub

    Public Sub StartScan()

        Dim Report As HidReport = _IT4XXX.CreateReport

        Report.ReportId = HID_REPORT_ID
        Report.Data(0) = IT4XXX_START_SCAN

        _IT4XXX.WriteReport(Report)

    End Sub

    Public Sub EndScan()

        Dim Report As HidReport = _IT4XXX.CreateReport

        Report.ReportId = HID_REPORT_ID
        Report.Data(0) = IT4XXX_END_SCAN

        _IT4XXX.WriteReport(Report)

    End Sub

    Public Sub StartListen()

        _IsListening = True

    End Sub

    Public Sub StopListen()

        _IsListening = False

    End Sub

#End Region

#Region "--> Private Methods "

    Private Sub BeginReadReport()

        If Interlocked.CompareExchange(_IsReading, 1, 0) = 1 Then Exit Sub

        _IT4XXX.ReadReport(AddressOf ReadReport)

    End Sub

    Private Sub ReadReport(ByVal Report As HidReport)

        Dim IT4XXXReport As New IT4XXXReport(Report)
        Dim Data As Byte() = {}
        Dim CurrentPosition As Integer = 0
        Dim ReadRequired As Boolean = False

        If IT4XXXReport.Length > 0 And IT4XXXReport.ReadStatus = HidDeviceData.ReadStatus.Success Then

            Do

                If IT4XXXReport.MoreData = True And ReadRequired = True Then

                    IT4XXXReport = New IT4XXXReport(_IT4XXX.ReadReport())

                    Debug.WriteLine("   Length: " & IT4XXXReport.Data.Length)

                Else

                    ReadRequired = True

                End If

                If IT4XXXReport.Exists = True Then

                    ReDim Preserve Data(Data.GetUpperBound(0) + IT4XXXReport.Data.Length)
                    Array.Copy(IT4XXXReport.Data, 0, Data, CurrentPosition, IT4XXXReport.Data.Length)

                    CurrentPosition += IT4XXXReport.Data.Length

                End If

            Loop While IT4XXXReport.MoreData = True And IT4XXXReport.Exists = True

            If _IsListening = True Then

                If Data.Length > 0 Then RaiseEvent DataRecieved(Data)

            End If

        End If

        If IT4XXXReport.ReadStatus <> HidDeviceData.ReadStatus.NotConnected Then

            _IT4XXX.ReadReport(AddressOf ReadReport)

        Else

            _IsReading = 0

        End If

    End Sub

#End Region

#Region "--> Protected Methods "

    Protected Overrides Sub Finalize()

        _IT4XXX.Close()

        MyBase.Finalize()

    End Sub

#End Region

#Region "--> IT4600 Events "

    Private Sub _IT4XXX_Inserted() Handles _IT4XXX.Inserted

        BeginReadReport()

        RaiseEvent Inserted()

    End Sub

    Private Sub _IT4XXX_Removed() Handles _IT4XXX.Removed

        RaiseEvent Removed()

    End Sub

#End Region

End Class
