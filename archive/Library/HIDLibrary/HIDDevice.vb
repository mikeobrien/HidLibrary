Imports System.Runtime.InteropServices

Public Class HidDevice

    Implements IDisposable

#Region "--> Public Events "

    Public Event Inserted()
    Public Event Removed()

#End Region

#Region "--> Public Enums "

    Public Enum DeviceMode
        NonOverlapped = 0
        Overlapped = 1
    End Enum

#End Region

#Region "--> Private Fields "

    Private _DevicePath As String
    Private _Description As String

    Private _DeviceAttributes As HidDeviceAttributes
    Private _DeviceCapabilities As HidDeviceCapabilities

    Private _HidReadHandle As Integer
    Private _HidWriteHandle As Integer
    Private _DeviceReadMode As DeviceMode = DeviceMode.NonOverlapped
    Private _DeviceWriteMode As DeviceMode = DeviceMode.NonOverlapped
    Private _DeviceOpen As Boolean = False

    Private WithEvents _DeviceEventMonitor As New HidDeviceEventMonitor(Me)

#End Region

#Region "--> Private Delegates "

    Private Delegate Function _ReadDelegate() As HidDeviceData
    Private Delegate Function _ReadReportDelegate() As HidReport
    Private Delegate Function _WriteDelegate(ByVal Data As Byte()) As Boolean
    Private Delegate Function _WriteReportDelegate(ByVal Report As HidReport) As Boolean

#End Region

#Region "--> Public Delegates "

    Public Delegate Sub ReadCallback(ByVal Data As HidDeviceData)
    Public Delegate Sub ReadReportCallback(ByVal Report As HidReport)

    Public Delegate Sub WriteCallback(ByVal Success As Boolean)

#End Region

#Region "--> Constructor "

    Friend Sub New(ByVal DevicePath As String, ByVal Description As String)

        _DevicePath = DevicePath
        _Description = Description

        Try

            Dim HidHandle As Integer

            HidHandle = Me.OpenDeviceIO(_DevicePath, NativeMethods.ACCESS_NONE)

            _DeviceAttributes = GetDeviceAttributes(HidHandle)
            _DeviceCapabilities = GetDeviceCapabilities(HidHandle)

            Me.CloseDeviceIO(HidHandle)

        Catch Exception As Exception
        End Try

    End Sub

#End Region

#Region "--> Public Methods "

    Public Overrides Function ToString() As String

        Return String.Format("{0} ({1}, {2}, {3})", _Description, _DeviceAttributes.VendorHexId, _DeviceAttributes.ProductHexId, _DeviceAttributes.Version)

    End Function

    Public Sub Open()

        Open(DeviceMode.NonOverlapped, DeviceMode.NonOverlapped)

    End Sub

    Public Sub Open(ByVal ReadMode As DeviceMode, ByVal WriteMode As DeviceMode)

        If IsOpen = True Then Exit Sub

        _DeviceReadMode = ReadMode
        _DeviceWriteMode = WriteMode

        Try

            _HidReadHandle = Me.OpenDeviceIO(_DevicePath, ReadMode, NativeMethods.GENERIC_READ)
            _HidWriteHandle = Me.OpenDeviceIO(_DevicePath, WriteMode, NativeMethods.GENERIC_WRITE)

            _DeviceEventMonitor.Enabled = True

        Catch Exception As Exception

            _DeviceOpen = False
            Return

        End Try

        _DeviceOpen = _HidReadHandle <> NativeMethods.INVALID_HANDLE_VALUE And _HidWriteHandle <> NativeMethods.INVALID_HANDLE_VALUE

    End Sub

    Public Sub Close()

        Close(True)

    End Sub

    Public Function Read() As HidDeviceData

        Return Read(0)

    End Function

    Public Sub Read(ByVal Callback As ReadCallback)

        Dim ReadDelegate As New _ReadDelegate(AddressOf Read)
        Dim AsyncState As New HidAsyncState(ReadDelegate, Callback)

        ReadDelegate.BeginInvoke(AddressOf EndRead, AsyncState)

    End Sub

    Public Function Read(ByVal Timeout As Integer) As HidDeviceData

        If IsConnected = True Then

            If IsOpen = False Then Open()

            Try

                Return ReadData(Timeout)

            Catch Exception As Exception

                Return New HidDeviceData(HidDeviceData.ReadStatus.ReadError)

            End Try

        Else

            Return New HidDeviceData(HidDeviceData.ReadStatus.NotConnected)

        End If

    End Function

    Public Sub ReadReport(ByVal Callback As ReadReportCallback)

        Dim ReadReportDelegate As New _ReadReportDelegate(AddressOf ReadReport)
        Dim AsyncState As New HidAsyncState(ReadReportDelegate, Callback)

        ReadReportDelegate.BeginInvoke(AddressOf EndReadReport, AsyncState)

    End Sub

    Public Function ReadReport(ByVal Timeout As Integer) As HidReport

        Return New HidReport(Capabilities.InputReportByteLength, Read(Timeout))

    End Function

    Public Function ReadReport() As HidReport

        Return ReadReport(0)

    End Function

    Public Sub Write(ByVal Data As Byte(), ByVal Callback As WriteCallback)

        Dim WriteDelegate As New _WriteDelegate(AddressOf Write)
        Dim AsyncState As New HidAsyncState(WriteDelegate, Callback)

        WriteDelegate.BeginInvoke(Data, AddressOf EndWrite, AsyncState)

    End Sub

    Public Function Write(ByVal Data As Byte()) As Boolean

        Return Write(Data, 0)

    End Function

    Public Function Write(ByVal Data As Byte(), ByVal Timeout As Integer) As Boolean

        If IsConnected = True Then

            If IsOpen = False Then Open()

            Try

                Return WriteData(Data, Timeout)

            Catch Exception As Exception

                Return False

            End Try

        Else

            Return False

        End If

    End Function

    Public Sub WriteReport(ByVal Report As HidReport, ByVal Callback As WriteCallback)

        Dim WriteReportDelegate As New _WriteReportDelegate(AddressOf WriteReport)
        Dim AsyncState As New HidAsyncState(WriteReportDelegate, Callback)

        WriteReportDelegate.BeginInvoke(Report, AddressOf EndWriteReport, AsyncState)

    End Sub

    Public Function WriteReport(ByVal Report As HidReport) As Boolean

        Return WriteReport(Report, 0)

    End Function

    Public Function WriteReport(ByVal Report As HidReport, ByVal Timeout As Integer) As Boolean

        Return Write(Report.GetBytes, Timeout)

    End Function

    Public Function CreateReport() As HidReport

        Return New HidReport(Me.Capabilities.OutputReportByteLength)

    End Function

#End Region

#Region "--> Public Properties "

    Public ReadOnly Property ReadHandle() As Integer
        Get

            Return _HidReadHandle

        End Get
    End Property

    Public ReadOnly Property WriteHandle() As Integer
        Get

            Return _HidReadHandle

        End Get
    End Property

    Public ReadOnly Property IsOpen() As Boolean
        Get

            Return _DeviceOpen

        End Get
    End Property

    Public ReadOnly Property IsConnected() As Boolean
        Get

            Return _DeviceEventMonitor.Connected

        End Get
    End Property

    Public ReadOnly Property Description() As String
        Get

            Return _Description

        End Get
    End Property

    Public ReadOnly Property Capabilities() As HidDeviceCapabilities
        Get

            Return _DeviceCapabilities

        End Get
    End Property

    Public ReadOnly Property Attributes() As HidDeviceAttributes
        Get

            Return _DeviceAttributes

        End Get
    End Property

    Public ReadOnly Property DevicePath() As String
        Get

            Return _DevicePath

        End Get
    End Property

#End Region

#Region "--> Private Methods "

    Private Sub EndRead(ByVal ar As IAsyncResult)

        Dim HidAsyncState As HidAsyncState = ar.AsyncState
        Dim CallerDelegate As _ReadDelegate = HidAsyncState.CallerDelegate
        Dim CallbackDelegate As ReadCallback = HidAsyncState.CallbackDelegate
        Dim Data As HidDeviceData = CallerDelegate.EndInvoke(ar)

        If Not CallbackDelegate Is Nothing Then CallbackDelegate.Invoke(Data)

    End Sub

    Private Sub EndReadReport(ByVal ar As IAsyncResult)

        Dim HidAsyncState As HidAsyncState = ar.AsyncState
        Dim CallerDelegate As _ReadReportDelegate = HidAsyncState.CallerDelegate
        Dim CallbackDelegate As ReadReportCallback = HidAsyncState.CallbackDelegate
        Dim Report As HidReport = CallerDelegate.EndInvoke(ar)

        If Not CallbackDelegate Is Nothing Then CallbackDelegate.Invoke(Report)

    End Sub

    Private Sub EndWrite(ByVal ar As IAsyncResult)

        Dim HidAsyncState As HidAsyncState = ar.AsyncState
        Dim CallerDelegate As _WriteDelegate = HidAsyncState.CallerDelegate
        Dim CallbackDelegate As WriteCallback = HidAsyncState.CallbackDelegate
        Dim Result As Boolean = CallerDelegate.EndInvoke(ar)

        If Not CallbackDelegate Is Nothing Then CallbackDelegate.Invoke(Result)

    End Sub

    Private Sub EndWriteReport(ByVal ar As IAsyncResult)

        Dim HidAsyncState As HidAsyncState = ar.AsyncState
        Dim CallerDelegate As _WriteReportDelegate = HidAsyncState.CallerDelegate
        Dim CallbackDelegate As WriteCallback = HidAsyncState.CallbackDelegate
        Dim Result As Boolean = CallerDelegate.EndInvoke(ar)

        If Not CallbackDelegate Is Nothing Then CallbackDelegate.Invoke(Result)

    End Sub

    Private Sub Close(ByVal DisableDeviceMonitor As Boolean)

        If DisableDeviceMonitor = True Then _DeviceEventMonitor.Enabled = False

        If IsOpen = False Then Exit Sub

        Try

            Me.CloseDeviceIO(_HidReadHandle)
            Me.CloseDeviceIO(_HidWriteHandle)

        Catch Exception As Exception
        Finally

            _DeviceOpen = False

        End Try

    End Sub

    Private Function CreateInputBuffer() As Byte()

        Return CreateBuffer(Capabilities.InputReportByteLength - 1)

    End Function

    Private Function CreateOutputBuffer() As Byte()

        Return CreateBuffer(Capabilities.OutputReportByteLength - 1)

    End Function

    Private Function CreateBuffer(ByVal Length As Integer) As Byte()

        Dim Buffer As Byte()

        ReDim Buffer(Length)

        Return Buffer

    End Function

    Private Function GetDeviceAttributes(ByVal HidHandle As Integer) As HidDeviceAttributes

        Dim DeviceAttributes As NativeMethods.HIDD_ATTRIBUTES

        DeviceAttributes.Size = Marshal.SizeOf(DeviceAttributes)

        NativeMethods.HidD_GetAttributes(HidHandle, DeviceAttributes)

        Return New HidDeviceAttributes(DeviceAttributes)

    End Function

    Private Function GetDeviceCapabilities(ByVal HidHandle As Integer) As HidDeviceCapabilities

        Dim Capabilities As NativeMethods.HIDP_CAPS
        Dim PreparsedDataPointer As IntPtr
        Dim Result As Integer

        If NativeMethods.HidD_GetPreparsedData(HidHandle, PreparsedDataPointer) = True Then

            Result = NativeMethods.HidP_GetCaps(PreparsedDataPointer, Capabilities)

            NativeMethods.HidD_FreePreparsedData(PreparsedDataPointer)

        End If

        Return New HidDeviceCapabilities(Capabilities)

    End Function

    Private Function WriteData(ByVal Data As Byte(), ByVal Timeout As Integer) As Boolean

        If _DeviceCapabilities.OutputReportByteLength > 0 Then

            Dim Buffer As Byte() = CreateOutputBuffer()
            Dim BytesWritten As Integer
            Dim Result As Integer

            Array.Copy(Data, 0, Buffer, 0, LowestValue(Data.Length, _DeviceCapabilities.OutputReportByteLength))

            If _DeviceWriteMode = DeviceMode.Overlapped Then

                Dim Security As New NativeMethods.SECURITY_ATTRIBUTES
                Dim Overlapped As New NativeMethods.OVERLAPPED
                Dim OverlapTimeout As Integer

                If Timeout <= 0 Then

                    OverlapTimeout = NativeMethods.WAIT_INFINITE

                Else

                    OverlapTimeout = Timeout

                End If

                Security.lpSecurityDescriptor = 0
                Security.bInheritHandle = CInt(True)
                Security.nLength = Len(Security)

                Overlapped.Offset = 0
                Overlapped.OffsetHigh = 0
                Overlapped.hEvent = NativeMethods.CreateEvent(Security, CInt(False), CInt(True), "")

                Try

                    NativeMethods.WriteFileOverlapped(_HidWriteHandle, Buffer(0), Buffer.Length, BytesWritten, Overlapped)

                Catch Exception As Exception

                    Return False

                End Try

                Result = NativeMethods.WaitForSingleObject(Overlapped.hEvent, OverlapTimeout)

                Select Case Result
                    Case NativeMethods.WAIT_OBJECT_0
                        Return True
                    Case NativeMethods.WAIT_TIMEOUT
                        Return False
                    Case NativeMethods.WAIT_FAILED
                        Return False
                    Case Else
                        Return False
                End Select

            Else

                Try

                    Return NativeMethods.WriteFile(_HidWriteHandle, Buffer(0), Buffer.Length, BytesWritten, 0)

                Catch Exception As Exception

                    Return False

                End Try

            End If

        Else

            Return False

        End If

    End Function

    Private Function ReadData(ByVal Timeout As Integer) As HidDeviceData

        Dim Buffer As Byte() = {}
        Dim Status As HidDeviceData.ReadStatus = HidDeviceData.ReadStatus.NoDataRead

        If _DeviceCapabilities.InputReportByteLength > 0 Then


            Dim BytesRead As Integer
            Dim Result As Integer

            Buffer = CreateInputBuffer()

            If _DeviceReadMode = DeviceMode.Overlapped Then

                Dim Security As New NativeMethods.SECURITY_ATTRIBUTES
                Dim Overlapped As New NativeMethods.OVERLAPPED
                Dim OverlapTimeout As Integer

                If Timeout <= 0 Then

                    OverlapTimeout = NativeMethods.WAIT_INFINITE

                Else

                    OverlapTimeout = Timeout

                End If

                Security.lpSecurityDescriptor = 0
                Security.bInheritHandle = CInt(True)
                Security.nLength = Len(Security)

                Overlapped.Offset = 0
                Overlapped.OffsetHigh = 0
                Overlapped.hEvent = NativeMethods.CreateEvent(Security, CInt(False), CInt(True), "")

                Try

                    NativeMethods.ReadFileOverlapped(_HidReadHandle, Buffer(0), Buffer.Length, BytesRead, Overlapped)

                    Result = NativeMethods.WaitForSingleObject(Overlapped.hEvent, OverlapTimeout)

                    Select Case Result
                        Case NativeMethods.WAIT_OBJECT_0
                            Status = HidDeviceData.ReadStatus.Success
                        Case NativeMethods.WAIT_TIMEOUT
                            Status = HidDeviceData.ReadStatus.WaitTimedOut
                            ReDim Buffer(-1)
                        Case NativeMethods.WAIT_FAILED
                            Status = HidDeviceData.ReadStatus.WaitFail
                            ReDim Buffer(-1)
                        Case Else
                            Status = HidDeviceData.ReadStatus.NoDataRead
                            ReDim Buffer(-1)
                    End Select

                Catch Exception As Exception

                    Status = HidDeviceData.ReadStatus.ReadError

                End Try

            Else

                Try

                    NativeMethods.ReadFile(_HidReadHandle, Buffer(0), Buffer.Length, BytesRead, IntPtr.Zero)
                    Status = HidDeviceData.ReadStatus.Success

                Catch Exception As Exception

                    Status = HidDeviceData.ReadStatus.ReadError

                End Try

            End If

        End If

        Return New HidDeviceData(Buffer, Status)

    End Function

    Private Function OpenDeviceIO(ByVal DevicePath As String, ByVal DeviceAccess As Integer) As Integer

        Return OpenDeviceIO(DevicePath, DeviceMode.NonOverlapped, DeviceAccess)

    End Function

    Private Function OpenDeviceIO(ByVal DevicePath As String, ByVal DeviceMode As DeviceMode, ByVal DeviceAccess As Integer) As Integer

        Dim Security As New NativeMethods.SECURITY_ATTRIBUTES
        Dim FileHandle As Integer
        Dim Flags As Integer = 0

        If DeviceMode = DeviceMode.Overlapped Then Flags = NativeMethods.FILE_FLAG_OVERLAPPED

        Security.lpSecurityDescriptor = 0
        Security.bInheritHandle = CInt(True)
        Security.nLength = Len(Security)

        FileHandle = NativeMethods.CreateFile(DevicePath, DeviceAccess, NativeMethods.FILE_SHARE_READ Or NativeMethods.FILE_SHARE_WRITE, Security, NativeMethods.OPEN_EXISTING, Flags, 0)

        Return FileHandle

    End Function

    Private Sub CloseDeviceIO(ByVal Handle As Integer)

        NativeMethods.CloseHandle(Handle)

    End Sub

    Private Function LowestValue(ByVal Value1 As Integer, ByVal Value2 As Integer) As Integer

        If Value1 > Value2 Then Return Value2 Else Return Value1

    End Function

#End Region

#Region "--> IDisposable "

    Public Sub Dispose() Implements System.IDisposable.Dispose

        If IsOpen Then Close()

    End Sub

#End Region

#Region "--> DeviceEventMonitor Events "

    Private Sub _DeviceEventMonitor_Inserted() Handles _DeviceEventMonitor.Inserted

        If _DeviceOpen = False Then Open()

        RaiseEvent Inserted()

    End Sub

    Private Sub _DeviceEventMonitor_Removed() Handles _DeviceEventMonitor.Removed

        If _DeviceOpen = True Then Close(False)

        RaiseEvent Removed()

    End Sub

#End Region

End Class