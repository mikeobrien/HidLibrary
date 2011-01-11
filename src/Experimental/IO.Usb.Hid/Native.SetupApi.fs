module internal IO.Usb.Hid.Native.SetupApi

    open System
    open System.Text
    open System.Runtime.InteropServices
    open Microsoft.Win32.SafeHandles

    [<Literal>] 
    let ERROR_NO_MORE_ITEMS = 259

    [<Literal>] 
    let SUCCESS = 0
    
    [<Literal>] 
    let SPINT_ACTIVE = 0x1
    [<Literal>] 
    let SPINT_DEFAULT = 0x2
    [<Literal>] 
    let SPINT_REMOVED = 0x4 

    type SetupDeviceRegistryProperty = 
        | ADDRESS = 0x1C
        | BUSNUMBER = 0x15
        | BUSTYPEGUID = 0x13
        | CAPABILITIES = 0xF
        | CHARACTERISTICS = 0x1B
        | CLASS = 7
        | CLASSGUID = 8
        | COMPATIBLEIDS = 2
        | CONFIGFLAGS = 0xA
        | DEVICE_POWER_DATA = 0x1E
        | DEVICEDESC = 0
        | DEVTYPE = 0x19
        | DRIVER = 9
        | ENUMERATOR_NAME = 0x16
        | EXCLUSIVE = 0x1A
        | FRIENDLYNAME = 0xC
        | HARDWAREID = 1
        | LEGACYBUSTYPE = 0x14
        | LOCATION_INFORMATION = 0xD
        | LOWERFILTERS = 0x12
        | MFG = 0xB
        | PHYSICAL_DEVICE_OBJECT_NAME = 0xE
        | REMOVAL_POLICY = 0x1F
        | REMOVAL_POLICY_HW_DEFAULT = 0x20
        | REMOVAL_POLICY_OVERRIDE = 0x21
        | SECURITY = 0x17
        | SECURITY_SDS = 0x18
        | SERVICE = 4
        | UI_NUMBER = 0x10
        | UI_NUMBER_DESC_FORMAT = 0x1D
        | UPPERFILTERS = 0x11 

    type DeviceInterfaceGetClassFlags = 
        | PRESENT = 0x2
        | DEVICEINTERFACE = 0x10
        | ALLCLASSES = 0x4  

    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type SP_DEVINFO_DATA =
        val mutable Size:int
        val mutable ClassGuid:Guid
        val mutable DevInst:int
        val mutable Reserved:IntPtr
        member x.Init () =  
            x.Size <- sizeof<SP_DEVINFO_DATA>
            x.ClassGuid <- Guid.Empty
            x.DevInst <- 0
            x.Reserved <- IntPtr.Zero
 
     [<Struct; StructLayout(LayoutKind.Sequential)>]
     type SP_DEVICE_INTERFACE_DATA =
        val mutable Size:int
        val mutable InterfaceClassGuid:Guid
        val mutable Flags:int
        val mutable Reserved:IntPtr
        member x.Init () =  
            x.Size <- sizeof<SP_DEVICE_INTERFACE_DATA>
            x.InterfaceClassGuid <- Guid.Empty
            x.Flags <- 0
            x.Reserved <- IntPtr.Zero
    
    [<Struct; StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)>]
    type SP_DEVICE_INTERFACE_DETAIL_DATA = 
        val mutable Size:int
        [<MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)>]
        val mutable DevicePath:string
        member x.Init () =  
            match IntPtr.Size with
            | 4 -> x.Size <- 4 + Marshal.SystemDefaultCharSize
            | 8 -> x.Size <- 8

    type DeviceInfoSetSafeHandle() = 
        inherit SafeHandleMinusOneIsInvalid(true)
        [<DllImport("setupapi.dll")>]
        static extern bool SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet)
        override s.ReleaseHandle() = SetupDiDestroyDeviceInfoList(base.DangerousGetHandle())

    [<DllImport("setupapi.dll", SetLastError=true)>]
    extern DeviceInfoSetSafeHandle SetupDiGetClassDevs(Guid& classGuid, 
                                                       string enumerator, 
                                                       IntPtr hwndParent, 
                                                       DeviceInterfaceGetClassFlags flags)

    [<DllImport("setupapi.dll", SetLastError=true)>]
    extern bool SetupDiEnumDeviceInfo(DeviceInfoSetSafeHandle deviceInfoSet, 
                                      int index, 
                                      SP_DEVINFO_DATA& deviceInfoData)

    [<DllImport("setupapi.dll")>]
    extern bool SetupDiEnumDeviceInterfaces(DeviceInfoSetSafeHandle deviceInfoSet, 
                                            SP_DEVINFO_DATA& deviceInfoData, 
                                            Guid& interfaceClassGuid, 
                                            int memberIndex, 
                                            SP_DEVICE_INTERFACE_DATA& deviceInterfaceData)

    [<DllImport("setupapi.dll", EntryPoint="SetupDiGetDeviceInterfaceDetail")>]
    extern bool SetupDiGetDeviceInterfaceDetailBufferCheck(DeviceInfoSetSafeHandle deviceInfoSet, 
                                                           SP_DEVICE_INTERFACE_DATA& deviceInterfaceData, 
                                                           IntPtr deviceInterfaceDetailData, 
                                                           int deviceInterfaceDetailDataSize, 
                                                           int& requiredSize, 
                                                           IntPtr deviceInfoData)

    [<DllImport("setupapi.dll", SetLastError=true)>]
    extern bool SetupDiGetDeviceInterfaceDetail(DeviceInfoSetSafeHandle deviceInfoSet, 
                                                SP_DEVICE_INTERFACE_DATA& deviceInterfaceData, 
                                                SP_DEVICE_INTERFACE_DETAIL_DATA& deviceInterfaceDetailData, 
                                                int deviceInterfaceDetailDataSize, 
                                                int& requiredSize, 
                                                IntPtr deviceInfoData)
    
    [<DllImport("setupapi.dll")>]
    extern bool SetupDiGetDeviceRegistryProperty(DeviceInfoSetSafeHandle deviceInfoSet, 
                                                 SP_DEVINFO_DATA& deviceInfoData, 
                                                 SetupDeviceRegistryProperty propertyVal, 
                                                 int& propertyRegDataType, 
                                                 StringBuilder propertyBuffer, 
                                                 int propertyBufferSize, 
                                                 int& requiredSize)