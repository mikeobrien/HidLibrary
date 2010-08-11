module IO.Usb.Hid.Native.Kernel32Adapter

    open System
    open System.IO
    open IO.Usb.Hid.Native
    open System.Runtime.InteropServices

    let OpenFile path (access:FileAccess) (share:FileShare) (mode:FileMode) isAsync = 
        Kernel32.CreateFileW(path, 
                             access, 
                             share, 
                             IntPtr.Zero, 
                             mode, 
                             (if isAsync then FileOptions.Asynchronous else FileOptions.None), 
                             IntPtr.Zero)