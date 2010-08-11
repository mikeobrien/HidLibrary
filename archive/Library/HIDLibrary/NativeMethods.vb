Imports System.Runtime.InteropServices

Friend NotInheritable Class NativeMethods

#Region "--> Constructor "

    Private Sub New()
    End Sub

#End Region

#Region "--> File IO "

    Friend Const FILE_FLAG_OVERLAPPED As Integer = &H40000000
    Friend Const FILE_SHARE_READ As Short = &H1S
    Friend Const FILE_SHARE_WRITE As Short = &H2S
    Friend Const GENERIC_READ As Integer = &H80000000
    Friend Const GENERIC_WRITE As Integer = &H40000000
    Friend Const ACCESS_NONE As Integer = 0
    Friend Const INVALID_HANDLE_VALUE As Integer = -1
    Friend Const OPEN_EXISTING As Short = 3
    Friend Const WAIT_TIMEOUT As Integer = &H102
    Friend Const WAIT_OBJECT_0 As Short = 0
    Friend Const WAIT_FAILED As Integer = &HFFFFFFFF
    Friend Const WAIT_INFINITE As Integer = &HFFFF

    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure OVERLAPPED
        Dim Internal As Integer
        Dim InternalHigh As Integer
        Dim Offset As Integer
        Dim OffsetHigh As Integer
        Dim hEvent As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure SECURITY_ATTRIBUTES
        Dim nLength As Integer
        Dim lpSecurityDescriptor As Integer
        Dim bInheritHandle As Integer
    End Structure

    <DllImport("kernel32.dll")> _
    Friend Shared Function CancelIo(ByVal hFile As Integer) As Integer
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Friend Shared Function CloseHandle(ByVal hObject As Integer) As Boolean
    End Function

    <DllImport("kernel32.dll", CharSet:=CharSet.Auto)> _
    Friend Shared Function CreateEvent(ByRef SecurityAttributes As SECURITY_ATTRIBUTES, ByVal bManualReset As Integer, ByVal bInitialState As Integer, ByVal lpName As String) As Integer
    End Function

    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Friend Shared Function CreateFile(ByVal lpFileName As String, ByVal dwDesiredAccess As Integer, ByVal dwShareMode As Integer, ByRef lpSecurityAttributes As SECURITY_ATTRIBUTES, ByVal dwCreationDisposition As Integer, ByVal dwFlagsAndAttributes As Integer, ByVal hTemplateFile As Integer) As Integer
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Friend Shared Function ReadFile(ByVal hFile As Integer, ByRef lpBuffer As Byte, ByVal nNumberOfBytesToRead As Integer, ByRef lpNumberOfBytesRead As Integer, ByVal lpOverlapped As IntPtr) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True, EntryPoint:="ReadFile")> _
    Friend Shared Function ReadFileOverlapped(ByVal hFile As Integer, ByRef lpBuffer As Byte, ByVal nNumberOfBytesToRead As Integer, ByRef lpNumberOfBytesRead As Integer, ByRef lpOverlapped As OVERLAPPED) As Boolean
    End Function

    <DllImport("kernel32.dll")> _
    Friend Shared Function WaitForSingleObject(ByVal hHandle As Integer, ByVal dwMilliseconds As Integer) As Integer
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Friend Shared Function WriteFileOverlapped(ByVal hFile As Integer, ByRef lpBuffer As Byte, ByVal nNumberOfBytesToWrite As Integer, ByRef lpNumberOfBytesWritten As Integer, ByRef lpOverlapped As OVERLAPPED) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Friend Shared Function WriteFile(ByVal hFile As Integer, ByRef lpBuffer As Byte, ByVal nNumberOfBytesToWrite As Integer, ByRef lpNumberOfBytesWritten As Integer, ByVal lpOverlapped As Integer) As Boolean
    End Function

#End Region

#Region "--> Device Management "

    Friend Const DBT_DEVICEARRIVAL As Integer = &H8000
    Friend Const DBT_DEVICEREMOVECOMPLETE As Integer = &H8004
    Friend Const DBT_DEVTYP_DEVICEINTERFACE As Integer = 5
    Friend Const DBT_DEVTYP_HANDLE As Integer = 6
    Friend Const DEVICE_NOTIFY_ALL_INTERFACE_CLASSES As Integer = 4
    Friend Const DEVICE_NOTIFY_SERVICE_HANDLE As Integer = 1
    Friend Const DEVICE_NOTIFY_WINDOW_HANDLE As Integer = 0
    Friend Const WM_DEVICECHANGE As Integer = &H219
    Friend Const DIGCF_PRESENT As Short = &H2S
    Friend Const DIGCF_DEVICEINTERFACE As Short = &H10S
    Friend Const DIGCF_ALLCLASSES As Integer = &H4
    Friend Const MAX_DEV_LEN As Integer = 1000

    Friend Const SPDRP_ADDRESS As Integer = &H1C
    Friend Const SPDRP_BUSNUMBER As Integer = &H15
    Friend Const SPDRP_BUSTYPEGUID As Integer = &H13
    Friend Const SPDRP_CAPABILITIES As Integer = &HF
    Friend Const SPDRP_CHARACTERISTICS As Integer = &H1B
    Friend Const SPDRP_CLASS As Integer = 7
    Friend Const SPDRP_CLASSGUID As Integer = 8
    Friend Const SPDRP_COMPATIBLEIDS As Integer = 2
    Friend Const SPDRP_CONFIGFLAGS As Integer = &HA
    Friend Const SPDRP_DEVICE_POWER_DATA As Integer = &H1E
    Friend Const SPDRP_DEVICEDESC As Integer = 0
    Friend Const SPDRP_DEVTYPE As Integer = &H19
    Friend Const SPDRP_DRIVER As Integer = 9
    Friend Const SPDRP_ENUMERATOR_NAME As Integer = &H16
    Friend Const SPDRP_EXCLUSIVE As Integer = &H1A
    Friend Const SPDRP_FRIENDLYNAME As Integer = &HC
    Friend Const SPDRP_HARDWAREID As Integer = 1
    Friend Const SPDRP_LEGACYBUSTYPE As Integer = &H14
    Friend Const SPDRP_LOCATION_INFORMATION As Integer = &HD
    Friend Const SPDRP_LOWERFILTERS As Integer = &H12
    Friend Const SPDRP_MFG As Integer = &HB
    Friend Const SPDRP_PHYSICAL_DEVICE_OBJECT_NAME As Integer = &HE
    Friend Const SPDRP_REMOVAL_POLICY As Integer = &H1F
    Friend Const SPDRP_REMOVAL_POLICY_HW_DEFAULT As Integer = &H20
    Friend Const SPDRP_REMOVAL_POLICY_OVERRIDE As Integer = &H21
    Friend Const SPDRP_SECURITY As Integer = &H17
    Friend Const SPDRP_SECURITY_SDS As Integer = &H18
    Friend Const SPDRP_SERVICE As Integer = 4
    Friend Const SPDRP_UI_NUMBER As Integer = &H10
    Friend Const SPDRP_UI_NUMBER_DESC_FORMAT As Integer = &H1D
    Friend Const SPDRP_UPPERFILTERS As Integer = &H11

    <StructLayout(LayoutKind.Sequential)> _
    Friend Class DEV_BROADCAST_DEVICEINTERFACE
        Friend dbcc_size As Integer
        Friend dbcc_devicetype As Integer
        Friend dbcc_reserved As Integer
        Friend dbcc_classguid As Guid
        Friend dbcc_name As Short
    End Class

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
    Friend Class DEV_BROADCAST_DEVICEINTERFACE_1
        Friend dbcc_size As Integer
        Friend dbcc_devicetype As Integer
        Friend dbcc_reserved As Integer
        <MarshalAs(UnmanagedType.ByValArray, ArraySubType:=UnmanagedType.U1, SizeConst:=16)> _
        Friend dbcc_classguid() As Byte
        <MarshalAs(UnmanagedType.ByValArray, sizeconst:=255)> _
        Friend dbcc_name() As Char
    End Class

    <StructLayout(LayoutKind.Sequential)> _
    Friend Class DEV_BROADCAST_HANDLE
        Friend dbch_size As Integer
        Friend dbch_devicetype As Integer
        Friend dbch_reserved As Integer
        Friend dbch_handle As Integer
        Friend dbch_hdevnotify As Integer
    End Class

    <StructLayout(LayoutKind.Sequential)> _
    Friend Class DEV_BROADCAST_HDR
        Friend dbch_size As Integer
        Friend dbch_devicetype As Integer
        Friend dbch_reserved As Integer
    End Class

    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure SP_DEVICE_INTERFACE_DATA
        Friend cbSize As Integer
        Friend InterfaceClassGuid As System.Guid
        Friend Flags As Integer
        Friend Reserved As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure SP_DEVICE_INTERFACE_DETAIL_DATA
        Friend cbSize As Integer
        Friend DevicePath As String
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure SP_DEVINFO_DATA
        Friend cbSize As Integer
        Friend ClassGuid As System.Guid
        Friend DevInst As Integer
        Friend Reserved As Integer
    End Structure

    <DllImport("setupapi.dll", EntryPoint:="SetupDiGetDeviceRegistryProperty")> _
    Public Shared Function SetupDiGetDeviceRegistryProperty(ByVal DeviceInfoSet As IntPtr, ByRef DeviceInfoData As SP_DEVINFO_DATA, ByVal PropertyVal As Integer, ByRef PropertyRegDataType As Integer, ByVal PropertyBuffer As Byte(), ByVal PropertyBufferSize As Integer, ByRef RequiredSize As Integer) As Boolean
    End Function

    <DllImport("setupapi.dll")> _
    Friend Shared Function SetupDiEnumDeviceInfo(ByVal DeviceInfoSet As IntPtr, ByVal MemberIndex As Integer, ByRef DeviceInfoData As SP_DEVINFO_DATA) As Boolean
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto)> _
    Friend Shared Function RegisterDeviceNotification(ByVal hRecipient As IntPtr, ByVal NotificationFilter As IntPtr, ByVal Flags As Int32) As IntPtr
    End Function

    Friend Shared Function SetupDiCreateDeviceInfoList(ByRef ClassGuid As System.Guid, ByVal hwndParent As Integer) As Integer
    End Function

    <DllImport("setupapi.dll")> _
    Friend Shared Function SetupDiDestroyDeviceInfoList(ByVal DeviceInfoSet As IntPtr) As Integer
    End Function

    <DllImport("setupapi.dll")> _
    Friend Shared Function SetupDiEnumDeviceInterfaces(ByVal DeviceInfoSet As IntPtr, ByRef DeviceInfoData As SP_DEVINFO_DATA, ByRef InterfaceClassGuid As System.Guid, ByVal MemberIndex As Integer, ByRef DeviceInterfaceData As SP_DEVICE_INTERFACE_DATA) As Boolean
    End Function

    <DllImport("setupapi.dll", CharSet:=CharSet.Auto)> _
    Friend Shared Function SetupDiGetClassDevs(ByRef ClassGuid As System.Guid, ByVal Enumerator As String, ByVal hwndParent As Integer, ByVal Flags As Integer) As IntPtr
    End Function

    <DllImport("setupapi.dll", CharSet:=CharSet.Auto)> _
    Friend Shared Function SetupDiGetDeviceInterfaceDetail(ByVal DeviceInfoSet As IntPtr, ByRef DeviceInterfaceData As SP_DEVICE_INTERFACE_DATA, ByVal DeviceInterfaceDetailData As IntPtr, ByVal DeviceInterfaceDetailDataSize As Integer, ByRef RequiredSize As Integer, ByVal DeviceInfoData As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll")> _
    Friend Shared Function UnregisterDeviceNotification(ByVal Handle As IntPtr) As Boolean
    End Function

#End Region

#Region "--> HID "

    Friend Const HIDP_INPUT As Short = 0
    Friend Const HIDP_OUTPUT As Short = 1
    Friend Const HIDP_FEATURE As Short = 2

    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure HIDD_ATTRIBUTES
        Friend Size As Integer
        Friend VendorID As UShort
        Friend ProductID As UShort
        Friend VersionNumber As Short
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure HIDP_CAPS
        Friend Usage As Short
        Friend UsagePage As Short
        Friend InputReportByteLength As Short
        Friend OutputReportByteLength As Short
        Friend FeatureReportByteLength As Short
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=17)> _
        Friend Reserved() As Short
        Friend NumberLinkCollectionNodes As Short
        Friend NumberInputButtonCaps As Short
        Friend NumberInputValueCaps As Short
        Friend NumberInputDataIndices As Short
        Friend NumberOutputButtonCaps As Short
        Friend NumberOutputValueCaps As Short
        Friend NumberOutputDataIndices As Short
        Friend NumberFeatureButtonCaps As Short
        Friend NumberFeatureValueCaps As Short
        Friend NumberFeatureDataIndices As Short
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure HIDP_VALUE_CAPS
        Friend UsagePage As Short
        Friend ReportID As Byte
        Friend IsAlias As Integer
        Friend BitField As Short
        Friend LinkCollection As Short
        Friend LinkUsage As Short
        Friend LinkUsagePage As Short
        Friend IsRange As Integer
        Friend IsStringRange As Integer
        Friend IsDesignatorRange As Integer
        Friend IsAbsolute As Integer
        Friend HasNull As Integer
        Friend Reserved As Byte
        Friend BitSize As Short
        Friend ReportCount As Short
        Friend Reserved2 As Short
        Friend Reserved3 As Short
        Friend Reserved4 As Short
        Friend Reserved5 As Short
        Friend Reserved6 As Short
        Friend LogicalMin As Integer
        Friend LogicalMax As Integer
        Friend PhysicalMin As Integer
        Friend PhysicalMax As Integer
        Friend UsageMin As Short
        Friend UsageMax As Short
        Friend StringMin As Short
        Friend StringMax As Short
        Friend DesignatorMin As Short
        Friend DesignatorMax As Short
        Friend DataIndexMin As Short
        Friend DataIndexMax As Short
    End Structure

    <DllImport("hid.dll")> _
    Friend Shared Function HidD_FlushQueue(ByVal HidDeviceObject As Integer) As Boolean
    End Function

    <DllImport("hid.dll")> _
    Friend Shared Function HidD_GetAttributes(ByVal HidDeviceObject As Integer, ByRef Attributes As HIDD_ATTRIBUTES) As Boolean
    End Function

    <DllImport("hid.dll")> _
    Friend Shared Function HidD_GetFeature(ByVal HidDeviceObject As Integer, ByRef lpReportBuffer As Byte, ByVal ReportBufferLength As Integer) As Boolean
    End Function

    <DllImport("hid.dll")> _
    Friend Shared Function HidD_GetInputReport(ByVal HidDeviceObject As Integer, ByRef lpReportBuffer As Byte, ByVal ReportBufferLength As Integer) As Boolean
    End Function

    <DllImport("hid.dll")> _
    Friend Shared Sub HidD_GetHidGuid(ByRef HidGuid As System.Guid)
    End Sub

    <DllImport("hid.dll")> _
    Friend Shared Function HidD_GetNumInputBuffers(ByVal HidDeviceObject As Integer, ByRef NumberBuffers As Integer) As Boolean
    End Function

    <DllImport("hid.dll")> _
    Friend Shared Function HidD_GetPreparsedData(ByVal HidDeviceObject As IntPtr, ByRef PreparsedData As IntPtr) As Boolean
    End Function

    <DllImport("hid.dll")> _
    Friend Shared Function HidD_FreePreparsedData(ByVal PreparsedData As IntPtr) As Boolean
    End Function

    <DllImport("hid.dll")> _
    Friend Shared Function HidD_SetFeature(ByVal HidDeviceObject As Integer, ByRef lpReportBuffer As Byte, ByVal ReportBufferLength As Integer) As Boolean
    End Function

    <DllImport("hid.dll")> _
    Friend Shared Function HidD_SetNumInputBuffers(ByVal HidDeviceObject As Integer, ByVal NumberBuffers As Integer) As Boolean
    End Function

    <DllImport("hid.dll")> _
    Friend Shared Function HidD_SetOutputReport(ByVal HidDeviceObject As Integer, ByRef lpReportBuffer As Byte, ByVal ReportBufferLength As Integer) As Boolean
    End Function

    <DllImport("hid.dll")> _
    Friend Shared Function HidP_GetCaps(ByVal PreparsedData As IntPtr, ByRef Capabilities As HIDP_CAPS) As Integer
    End Function

    <DllImport("hid.dll")> _
    Friend Shared Function HidP_GetValueCaps(ByVal ReportType As Short, ByRef ValueCaps As Byte, ByRef ValueCapsLength As Short, ByVal PreparsedData As IntPtr) As Integer
    End Function

#End Region

End Class
