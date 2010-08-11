Imports System.Runtime.InteropServices
Imports System.Text

Public NotInheritable Class HidDevices

#Region "--> Constructor "

    Private Sub New()
    End Sub

#End Region

#Region "--> Public Methods "

    Public Shared Function IsConnected(ByVal DevicePath As String) As Boolean

        Dim Devices As HidDevice() = Enumerate(DevicePath)

        Return Devices.Length > 0

    End Function

    Public Shared Function GetDevice(ByVal DevicePath As String) As HidDevice

        Dim Devices As HidDevice() = Enumerate(DevicePath)

        If Devices.Length > 0 Then

            Return Devices(0)

        Else

            Return Nothing

        End If

    End Function

    Public Shared Function Enumerate() As HidDevice()

        Return Enumerate(New HidDeviceIdentifier)

    End Function

    Public Shared Function Enumerate(ByVal VendorId As Integer, ByVal ProductId As Integer) As HidDevice()

        Return Enumerate(New HidDeviceIdentifier(VendorId, ProductId))

    End Function

    Public Shared Function Enumerate(ByVal DevicePath As String) As HidDevice()

        Return Enumerate(New HidDeviceIdentifier(DevicePath))

    End Function

    Public Shared Function Enumerate(ByVal VendorId As Integer) As HidDevice()

        Return Enumerate(New HidDeviceIdentifier(VendorId))

    End Function

#End Region

#Region "--> Private Methods "

    Public Class DeviceInfo
        Private _path As String
        Private _description As String
        Private _connected As Boolean

        Public Sub New(ByVal path As String, ByVal description As String, ByVal connected As Boolean)
            _path = path
            _description = description
            _connected = connected
        End Sub

        Public ReadOnly Property Path() As String
            Get
                Return _path
            End Get
        End Property
        Public ReadOnly Property Description() As String
            Get
                Return _description
            End Get
        End Property
        Public ReadOnly Property Connected() As Boolean
            Get
                Return _connected
            End Get
        End Property
    End Class

    Public Shared Function EnumerateTest() As DeviceInfo()

        Dim DeviceInfoSet As IntPtr
        Dim Devices As New ArrayList

        DeviceInfoSet = NativeMethods.SetupDiGetClassDevs(HidClassGUID, vbNullString, 0, NativeMethods.DIGCF_DEVICEINTERFACE)

        If DeviceInfoSet.ToInt32 <> NativeMethods.INVALID_HANDLE_VALUE Then

            Dim DeviceInfoData As NativeMethods.SP_DEVINFO_DATA = CreateDeviceInfoData()
            Dim DeviceIndex As Integer = 0

            While NativeMethods.SetupDiEnumDeviceInfo(DeviceInfoSet, DeviceIndex, DeviceInfoData) = True

                Dim DeviceInterfaceData As New NativeMethods.SP_DEVICE_INTERFACE_DATA
                Dim DeviceInterfaceIndex As Integer = 0

                DeviceIndex += 1

                DeviceInterfaceData.cbSize = Marshal.SizeOf(DeviceInterfaceData)

                While NativeMethods.SetupDiEnumDeviceInterfaces(DeviceInfoSet, DeviceInfoData, HidClassGUID, DeviceInterfaceIndex, DeviceInterfaceData) = True

                    Dim DevicePath As String = GetDevicePath(DeviceInfoSet, DeviceInterfaceData)

                    DeviceInterfaceIndex += 1

                    If Not DevicePath Is Nothing Then

                        Dim RequiredSize As Integer
                        Dim RegType As Integer
                        Dim Buffer As Byte()
                        Dim Description As String

                        ReDim Buffer(255)

                        If (NativeMethods.SetupDiGetDeviceRegistryProperty(DeviceInfoSet, DeviceInfoData, NativeMethods.SPDRP_DEVICEDESC, RegType, Buffer, 255, RequiredSize)) Then

                            Description = Text.ASCIIEncoding.ASCII.GetString(Buffer, 0, RequiredSize - 1)

                        Else

                            Description = String.Empty

                        End If

                        Dim Device As New DeviceInfo(DevicePath, Description, True)
                        Devices.Add(Device)
                    End If

                End While

            End While

            NativeMethods.SetupDiDestroyDeviceInfoList(DeviceInfoSet)

        End If

        Return CType(Devices.ToArray(GetType(DeviceInfo)), DeviceInfo())

    End Function

    Private Shared Function Enumerate(ByVal Identifier As HidDeviceIdentifier) As HidDevice()

        Dim DeviceInfoSet As IntPtr
        Dim Devices As New ArrayList

        DeviceInfoSet = NativeMethods.SetupDiGetClassDevs(HidClassGUID, vbNullString, 0, NativeMethods.DIGCF_PRESENT Or NativeMethods.DIGCF_DEVICEINTERFACE)

        If DeviceInfoSet.ToInt32 <> NativeMethods.INVALID_HANDLE_VALUE Then

            Dim DeviceInfoData As NativeMethods.SP_DEVINFO_DATA = CreateDeviceInfoData()
            Dim DeviceIndex As Integer = 0

            While NativeMethods.SetupDiEnumDeviceInfo(DeviceInfoSet, DeviceIndex, DeviceInfoData) = True

                Dim DeviceInterfaceData As New NativeMethods.SP_DEVICE_INTERFACE_DATA
                Dim DeviceInterfaceIndex As Integer = 0

                DeviceIndex += 1

                DeviceInterfaceData.cbSize = Marshal.SizeOf(DeviceInterfaceData)

                While NativeMethods.SetupDiEnumDeviceInterfaces(DeviceInfoSet, DeviceInfoData, HidClassGUID, DeviceInterfaceIndex, DeviceInterfaceData) = True

                    Dim DevicePath As String = GetDevicePath(DeviceInfoSet, DeviceInterfaceData)

                    DeviceInterfaceIndex += 1

                    If Not DevicePath Is Nothing Then

                        Dim RequiredSize As Integer
                        Dim RegType As Integer
                        Dim Buffer As Byte()
                        Dim Description As String

                        ReDim Buffer(255)

                        If (NativeMethods.SetupDiGetDeviceRegistryProperty(DeviceInfoSet, DeviceInfoData, NativeMethods.SPDRP_DEVICEDESC, RegType, Buffer, 255, RequiredSize)) Then

                            Description = Text.ASCIIEncoding.ASCII.GetString(Buffer, 0, RequiredSize - 1)

                        Else

                            Description = String.Empty

                        End If

                        Dim Device As New HidDevice(DevicePath, Description)

                        If Identifier.Type = HidDeviceIdentifier.IdentifierTypes.Any OrElse _
                        (Identifier.Type = HidDeviceIdentifier.IdentifierTypes.DevicePath AndAlso Identifier.Equals(DevicePath)) OrElse _
                        (Identifier.Type = HidDeviceIdentifier.IdentifierTypes.VendorId AndAlso Identifier.Equals(Device.Attributes.VendorId)) OrElse _
                        (Identifier.Type = HidDeviceIdentifier.IdentifierTypes.VendorAndProductId AndAlso Identifier.Equals(Device.Attributes.VendorId, Device.Attributes.ProductId)) Then

                            Devices.Add(Device)

                        End If

                    End If

                End While

            End While

            NativeMethods.SetupDiDestroyDeviceInfoList(DeviceInfoSet)

        End If

        Return CType(Devices.ToArray(GetType(HidDevice)), HidDevice())

    End Function

    Private Shared Function CreateDeviceInfoData() As NativeMethods.SP_DEVINFO_DATA

        Dim DeviceInfoData As New NativeMethods.SP_DEVINFO_DATA

        DeviceInfoData.cbSize = 28
        DeviceInfoData.DevInst = 0
        DeviceInfoData.ClassGuid = System.Guid.Empty
        DeviceInfoData.Reserved = 0

        Return DeviceInfoData

    End Function

    Private Shared Function GetDevicePath(ByVal DeviceInfoSet As IntPtr, ByVal DeviceInterfaceData As NativeMethods.SP_DEVICE_INTERFACE_DATA) As String

        Dim DetailDataBuffer As IntPtr
        Dim BufferSize As Integer
        Dim DevicePath As String = Nothing

        NativeMethods.SetupDiGetDeviceInterfaceDetail(DeviceInfoSet, DeviceInterfaceData, IntPtr.Zero, 0, BufferSize, IntPtr.Zero)

        DetailDataBuffer = Marshal.AllocHGlobal(BufferSize)

        Marshal.WriteInt32(DetailDataBuffer, 4 + Marshal.SystemDefaultCharSize)

        If NativeMethods.SetupDiGetDeviceInterfaceDetail(DeviceInfoSet, DeviceInterfaceData, DetailDataBuffer, BufferSize, 0, IntPtr.Zero) = True Then

            Dim DevicePathPointer As IntPtr = New IntPtr(DetailDataBuffer.ToInt32 + 4)

            DevicePath = Marshal.PtrToStringAuto(DevicePathPointer)

        End If

        Marshal.FreeHGlobal(DetailDataBuffer)

        Return DevicePath

    End Function

#End Region

#Region "--> Private Properties "

    Private Shared ReadOnly Property HidClassGUID() As Guid
        Get

            Static ClassGUID As Guid

            If ClassGUID.Equals(Guid.Empty) = True Then NativeMethods.HidD_GetHidGuid(ClassGUID)

            Return ClassGUID

        End Get
    End Property

#End Region

End Class
