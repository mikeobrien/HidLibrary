module internal IO.Usb.Hid.Native.Hid

    open System
    open Microsoft.Win32.SafeHandles
    open System.Runtime.InteropServices
    
    [<Struct; StructLayoutAttribute(LayoutKind.Sequential)>]
    type HIDD_ATTRIBUTES =
        val mutable Size:int
        val mutable VendorID:uint16
        val mutable ProductID:uint16
        val mutable VersionNumber:uint16
        member x.Init () =  
            x.Size <- sizeof<HIDD_ATTRIBUTES>
            x.VendorID <- 0us
            x.ProductID <- 0us
            x.VersionNumber <- 0us

    [<DllImport("hid.dll")>]
    extern void public HidD_GetHidGuid(Guid& hidGuid)

    [<DllImportAttribute("hid.dll", EntryPoint="HidD_GetAttributes")>]
    extern bool HidD_GetAttributes(SafeFileHandle HidDeviceObject, HIDD_ATTRIBUTES& attributes)
