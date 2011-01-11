module IO.Usb.Hid.Native.HidAdapter

    open System
    open IO.Usb.Hid.Native
    open IO.Usb.Hid.Native.Hid
    open System.Runtime.InteropServices
    open System.ComponentModel

    type DeviceAttributes (venderId:int, productId:int, version:int) =
        member p.VenderId = venderId
        member p.ProductId = productId
        member p.Version = version

    let HidClass = 
        let mutable hidClass = Guid.Empty
        Hid.HidD_GetHidGuid(&hidClass)
        hidClass

    let GetDeviceAttributes handle = 
        let mutable attributes = new Hid.HIDD_ATTRIBUTES()
        attributes.Init()
        let success = Hid.HidD_GetAttributes(handle, &attributes)
        if success then new DeviceAttributes((int)attributes.VendorID, (int)attributes.ProductID, (int)attributes.VersionNumber)
        else 
            let error = Marshal.GetLastWin32Error()
            raise (Win32Exception(Marshal.GetLastWin32Error()))