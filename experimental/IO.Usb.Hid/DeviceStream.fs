namespace IO.Usb.Hid

    open System
    open System.IO
    open IO.Usb.Hid.Native

    type public DeviceStream(path, access:FileAccess, share:FileShare, mode:FileMode, isAsync:bool) = 
        inherit System.IO.FileStream(Kernel32Adapter.OpenFile path access share mode isAsync, 
                                     access, 
                                     0x1000, 
                                     isAsync) 