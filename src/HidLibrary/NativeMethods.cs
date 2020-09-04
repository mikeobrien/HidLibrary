using System;
using System.Runtime.InteropServices;

[module: DefaultCharSet(CharSet.Unicode)]

namespace HidLibrary
{
    internal static class NativeMethods
    {
	    internal const int FILE_FLAG_OVERLAPPED = 0x40000000;
	    internal const short FILE_SHARE_READ = 0x1;
	    internal const short FILE_SHARE_WRITE = 0x2;
	    internal const uint GENERIC_READ = 0x80000000;
	    internal const uint GENERIC_WRITE = 0x40000000;
	    internal const int ACCESS_NONE = 0;
	    internal const int INVALID_HANDLE_VALUE = -1;
	    internal const short OPEN_EXISTING = 3;
	    internal const int WAIT_TIMEOUT = 0x102;
	    internal const uint WAIT_OBJECT_0 = 0;
	    internal const uint WAIT_FAILED = 0xffffffff;

        internal const int WAIT_INFINITE = -1;

	    [StructLayout(LayoutKind.Sequential)]
	    internal struct SECURITY_ATTRIBUTES
	    {
		    public int nLength;
		    public IntPtr lpSecurityDescriptor;
		    public bool bInheritHandle;
	    }

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static internal extern bool CancelIoEx(IntPtr hFile, IntPtr lpOverlapped);

	    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
	    static internal extern bool CloseHandle(IntPtr hObject);

	    [DllImport("kernel32.dll")]
	    static internal extern IntPtr CreateEvent(ref SECURITY_ATTRIBUTES securityAttributes, int bManualReset, int bInitialState, string lpName);

	    [DllImport("kernel32.dll", SetLastError = true)]
	    static internal extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, int dwShareMode, ref SECURITY_ATTRIBUTES lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        static internal extern bool ReadFile(IntPtr hFile, IntPtr lpBuffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, [In] ref System.Threading.NativeOverlapped lpOverlapped);

	    [DllImport("kernel32.dll")]
	    static internal extern uint WaitForSingleObject(IntPtr hHandle, int dwMilliseconds);
		
        [DllImport("kernel32.dll", SetLastError = true)]
        static internal extern bool GetOverlappedResult(IntPtr hFile, [In] ref System.Threading.NativeOverlapped lpOverlapped, out uint lpNumberOfBytesTransferred, bool bWait);

        [DllImport("kernel32.dll")]
        static internal extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, [In] ref System.Threading.NativeOverlapped lpOverlapped);

	    internal const short DIGCF_PRESENT = 0x2;
	    internal const short DIGCF_DEVICEINTERFACE = 0x10;
	    internal const int DIGCF_ALLCLASSES = 0x4;

	    internal const int SPDRP_ADDRESS = 0x1c;
	    internal const int SPDRP_BUSNUMBER = 0x15;
	    internal const int SPDRP_BUSTYPEGUID = 0x13;
	    internal const int SPDRP_CAPABILITIES = 0xf;
	    internal const int SPDRP_CHARACTERISTICS = 0x1b;
	    internal const int SPDRP_CLASS = 7;
	    internal const int SPDRP_CLASSGUID = 8;
	    internal const int SPDRP_COMPATIBLEIDS = 2;
	    internal const int SPDRP_CONFIGFLAGS = 0xa;
	    internal const int SPDRP_DEVICE_POWER_DATA = 0x1e;
	    internal const int SPDRP_DEVICEDESC = 0;
	    internal const int SPDRP_DEVTYPE = 0x19;
	    internal const int SPDRP_DRIVER = 9;
	    internal const int SPDRP_ENUMERATOR_NAME = 0x16;
	    internal const int SPDRP_EXCLUSIVE = 0x1a;
	    internal const int SPDRP_FRIENDLYNAME = 0xc;
	    internal const int SPDRP_HARDWAREID = 1;
	    internal const int SPDRP_LEGACYBUSTYPE = 0x14;
	    internal const int SPDRP_LOCATION_INFORMATION = 0xd;
	    internal const int SPDRP_LOWERFILTERS = 0x12;
	    internal const int SPDRP_MFG = 0xb;
	    internal const int SPDRP_PHYSICAL_DEVICE_OBJECT_NAME = 0xe;
	    internal const int SPDRP_REMOVAL_POLICY = 0x1f;
	    internal const int SPDRP_REMOVAL_POLICY_HW_DEFAULT = 0x20;
	    internal const int SPDRP_REMOVAL_POLICY_OVERRIDE = 0x21;
	    internal const int SPDRP_SECURITY = 0x17;
	    internal const int SPDRP_SECURITY_SDS = 0x18;
	    internal const int SPDRP_SERVICE = 4;
	    internal const int SPDRP_UI_NUMBER = 0x10;
	    internal const int SPDRP_UI_NUMBER_DESC_FORMAT = 0x1d;

        internal const int SPDRP_UPPERFILTERS = 0x11;

	    [StructLayout(LayoutKind.Sequential)]
	    internal struct SP_DEVICE_INTERFACE_DATA
	    {
		    internal int cbSize;
		    internal System.Guid InterfaceClassGuid;
		    internal int Flags;
		    internal IntPtr Reserved;
	    }

	    [StructLayout(LayoutKind.Sequential)]
	    internal struct SP_DEVINFO_DATA
	    {
		    internal int cbSize;
		    internal Guid ClassGuid;
		    internal int DevInst;
		    internal IntPtr Reserved;
	    }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            internal int Size;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            internal string DevicePath;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DEVPROPKEY
        {
            public Guid fmtid;
            public uint pid;
        }

        internal static DEVPROPKEY DEVPKEY_Device_BusReportedDeviceDesc = 
            new DEVPROPKEY { fmtid = new Guid(0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2), pid = 4 };

	    [DllImport("setupapi.dll")]
        public static extern unsafe bool SetupDiGetDeviceRegistryProperty(IntPtr deviceInfoSet, ref SP_DEVINFO_DATA deviceInfoData, int propertyVal, ref int propertyRegDataType, void* propertyBuffer, int propertyBufferSize, ref int requiredSize);
	
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern unsafe bool SetupDiGetDeviceProperty(IntPtr deviceInfo, ref SP_DEVINFO_DATA deviceInfoData, ref DEVPROPKEY propkey, ref uint propertyDataType, void* propertyBuffer, int propertyBufferSize, ref int requiredSize, uint flags);

	    [DllImport("setupapi.dll")]
	    static internal extern bool SetupDiEnumDeviceInfo(IntPtr deviceInfoSet, int memberIndex, ref SP_DEVINFO_DATA deviceInfoData);

	    [DllImport("setupapi.dll")]
	    static internal extern int SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

	    [DllImport("setupapi.dll")]
        static internal extern bool SetupDiEnumDeviceInterfaces(IntPtr deviceInfoSet, ref SP_DEVINFO_DATA deviceInfoData, ref Guid interfaceClassGuid, int memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

	    [DllImport("setupapi.dll")]
        static internal extern IntPtr SetupDiGetClassDevs(ref System.Guid classGuid, string enumerator, IntPtr hwndParent, int flags);

        [DllImport("setupapi.dll")]
        static internal extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, ref int requiredSize, IntPtr deviceInfoData);

	    [DllImport("setupapi.dll")]
	    static internal extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, ref int requiredSize, IntPtr deviceInfoData);

	    [StructLayout(LayoutKind.Sequential)]
	    internal struct HIDD_ATTRIBUTES
	    {
		    internal int Size;
		    internal ushort VendorID;
		    internal ushort ProductID;
		    internal short VersionNumber;
	    }

	    [StructLayout(LayoutKind.Sequential)]
	    internal struct HIDP_CAPS
	    {
		    internal short Usage;
		    internal short UsagePage;
		    internal short InputReportByteLength;
		    internal short OutputReportByteLength;
		    internal short FeatureReportByteLength;
		    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
		    internal short[] Reserved;
		    internal short NumberLinkCollectionNodes;
		    internal short NumberInputButtonCaps;
		    internal short NumberInputValueCaps;
		    internal short NumberInputDataIndices;
		    internal short NumberOutputButtonCaps;
		    internal short NumberOutputValueCaps;
		    internal short NumberOutputDataIndices;
		    internal short NumberFeatureButtonCaps;
		    internal short NumberFeatureValueCaps;
		    internal short NumberFeatureDataIndices;
	    }

	    [DllImport("hid.dll")]
	    static internal extern bool HidD_GetAttributes(IntPtr hidDeviceObject, ref HIDD_ATTRIBUTES attributes);

	    [DllImport("hid.dll")]
	    static internal extern bool HidD_GetFeature(IntPtr hidDeviceObject, byte[] lpReportBuffer, int reportBufferLength);

	    [DllImport("hid.dll")]
	    static internal extern bool HidD_GetInputReport(IntPtr hidDeviceObject, byte[] lpReportBuffer, int reportBufferLength);

	    [DllImport("hid.dll")]
	    static internal extern void HidD_GetHidGuid(ref Guid hidGuid);

	    [DllImport("hid.dll")]
	    static internal extern bool HidD_GetPreparsedData(IntPtr hidDeviceObject, ref IntPtr preparsedData);

	    [DllImport("hid.dll")]
	    static internal extern bool HidD_FreePreparsedData(IntPtr preparsedData);

	    [DllImport("hid.dll")]
        static internal extern bool HidD_SetFeature(IntPtr hidDeviceObject, byte[] lpReportBuffer, int reportBufferLength);

	    [DllImport("hid.dll")]
        static internal extern bool HidD_SetOutputReport(IntPtr hidDeviceObject, byte[] lpReportBuffer, int reportBufferLength);

	    [DllImport("hid.dll")]
	    static internal extern int HidP_GetCaps(IntPtr preparsedData, ref HIDP_CAPS capabilities);

        [DllImport("hid.dll")]
        internal static extern bool HidD_GetProductString(IntPtr hidDeviceObject, ref byte lpReportBuffer, int ReportBufferLength);

        [DllImport("hid.dll")]
        internal static extern bool HidD_GetManufacturerString(IntPtr hidDeviceObject, ref byte lpReportBuffer, int ReportBufferLength);

        [DllImport("hid.dll")]
        internal static extern bool HidD_GetSerialNumberString(IntPtr hidDeviceObject, ref byte lpReportBuffer, int reportBufferLength);
    }
}
