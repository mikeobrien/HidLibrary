module internal IO.Usb.Hid.Native.Kernel32

    open System
    open System.IO
    open Microsoft.Win32.SafeHandles
    open System.Runtime.InteropServices

    [<DllImportAttribute("kernel32.dll", EntryPoint="CreateFileW")>]
    extern SafeFileHandle CreateFileW([<InAttribute()>] 
                                      [<MarshalAsAttribute(UnmanagedType.LPWStr)>] 
                                      string lpFileName, 
                                      FileAccess dwDesiredAccess, 
                                      FileShare dwShareMode, 
                                      [<InAttribute()>] 
                                      System.IntPtr lpSecurityAttributes, 
                                      FileMode dwCreationDisposition, 
                                      FileOptions dwFlagsAndAttributes, 
                                      [<InAttribute()>] 
                                      IntPtr hTemplateFile)