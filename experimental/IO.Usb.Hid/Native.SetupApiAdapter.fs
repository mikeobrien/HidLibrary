module IO.Usb.Hid.Native.SetupApiAdapter

    open System
    open System.Text
    open IO.Usb.Hid.Native
    open IO.Usb.Hid.Native.SetupApi
    open System.Runtime.InteropServices
    open System.ComponentModel
    open System.Collections
    open System.Collections.Generic

    // ---------------------------- Private Functions ----------------------------

    let private GetDeviceInterface handle index deviceInfo deviceClass =
        let mutable deviceInterface = new SetupApi.SP_DEVICE_INTERFACE_DATA()
        let mutable classFilter = deviceClass
        let mutable info = deviceInfo
        deviceInterface.Init ()
        let success = SetupApi.SetupDiEnumDeviceInterfaces(handle, &info, &classFilter, index, &deviceInterface)
        if not success then
            let error = Marshal.GetLastWin32Error()
            match error with
            | SetupApi.ERROR_NO_MORE_ITEMS -> (deviceInterface,false)
            | SetupApi.SUCCESS -> (deviceInterface,false)
            | _ -> raise (Win32Exception(Marshal.GetLastWin32Error()))
        else (deviceInterface,true)

    type private DeviceInterfaces(infoSetHandle, deviceInfo, deviceClass) = 
        member private c.ToSeq = 
            Seq.unfold (fun index -> 
                                let info, more = GetDeviceInterface infoSetHandle index deviceInfo deviceClass
                                match more with
                                | true -> Some (info, index + 1)
                                | _ -> None
                        ) 0
        interface IEnumerable with
            member c.GetEnumerator () = c.ToSeq.GetEnumerator() :> IEnumerator
        interface IEnumerable<SP_DEVICE_INTERFACE_DATA> with
            member c.GetEnumerator () = c.ToSeq.GetEnumerator()

    let private GetDeviceInfoSet deviceClass connectedOnly = 
        let mutable classFilter = deviceClass
        let flags = match connectedOnly with
                    | true -> DeviceInterfaceGetClassFlags.DEVICEINTERFACE ||| 
                              DeviceInterfaceGetClassFlags.PRESENT
                    | false -> DeviceInterfaceGetClassFlags.DEVICEINTERFACE
        let handle = SetupApi.SetupDiGetClassDevs(&classFilter, null, IntPtr.Zero, flags)
        let error = Marshal.GetLastWin32Error()
        match error with
        | 0 -> handle
        | _ -> raise (Win32Exception(Marshal.GetLastWin32Error()))

    let private GetDeviceInfo handle index =
        let mutable deviceInfo = new SetupApi.SP_DEVINFO_DATA()
        deviceInfo.Init ()
        let success = SetupApi.SetupDiEnumDeviceInfo(handle, index, &deviceInfo)
        if not success then
            let error = Marshal.GetLastWin32Error()
            match error with
            | SetupApi.ERROR_NO_MORE_ITEMS -> (deviceInfo,false)
            | _ -> raise (Win32Exception(Marshal.GetLastWin32Error()))
        else (deviceInfo,true)

    let private GetDevicePath handle deviceInterface = 
        let mutable deviceInterfaceInfo = deviceInterface
        let mutable deviceInterfaceDetail = new SetupApi.SP_DEVICE_INTERFACE_DETAIL_DATA()
        let mutable requiredSize = 0
        deviceInterfaceDetail.Init()
        SetupApi.SetupDiGetDeviceInterfaceDetailBufferCheck(handle, &deviceInterfaceInfo, IntPtr.Zero, 0, &requiredSize, IntPtr.Zero) |> ignore
        let success = SetupApi.SetupDiGetDeviceInterfaceDetail(handle, &deviceInterfaceInfo, &deviceInterfaceDetail, requiredSize, &requiredSize, IntPtr.Zero)
        if not success then
            let error = Marshal.GetLastWin32Error()
            raise (Win32Exception(Marshal.GetLastWin32Error()))
        else deviceInterfaceDetail.DevicePath

    let private GetDeviceProperty handle deviceInfo property = 
        let mutable info = deviceInfo
        let mutable valueType = 0
        let mutable description = new StringBuilder(256)
        let mutable size = 0
        let success = SetupApi.SetupDiGetDeviceRegistryProperty(handle, &info, property, &valueType, description, 255, &size)
        match success with
        | true -> description.ToString()
        | false -> String.Empty

    // ---------------------------- Nested Types ----------------------------

    type DeviceInfo (path:string, description:string, manufacturer:string, connected:bool) =
        member p.Path = path
        member p.Description = description
        member p.Manufacturer = manufacturer
        member p.Connected = connected

    type private Devices(deviceClass:Guid, connectedOnly:bool) = 
        let handle = GetDeviceInfoSet deviceClass connectedOnly
        member private c.ToSeq = 
            Seq.unfold (fun index -> 
                                let info, more = GetDeviceInfo handle index
                                match more with
                                | true ->   let interfaces = new DeviceInterfaces(handle, info, deviceClass)
                                            let defaultInterface = Seq.head interfaces
                                            let path = GetDevicePath handle defaultInterface
                                            Some((handle, defaultInterface, info, path), index + 1)
                                | _ -> None
                        ) 0
        interface IEnumerable with
            member c.GetEnumerator () = c.ToSeq.GetEnumerator() :> IEnumerator
        interface IEnumerable<SetupApi.DeviceInfoSetSafeHandle * SP_DEVICE_INTERFACE_DATA * SetupApi.SP_DEVINFO_DATA * string> with
            member c.GetEnumerator () = c.ToSeq.GetEnumerator()
        interface IDisposable with
            member c.Dispose () = handle.Dispose()

    // ---------------------------- Public Functions ----------------------------

    let GetDevicePaths deviceClass connectedOnly = 
        use devices = new Devices(deviceClass, connectedOnly)
        devices |> Seq.map (fun (handle, inf, info, path) -> path) |> Seq.toList

    let GetDevices deviceClass connectedOnly = 
        use devices = new Devices(deviceClass, connectedOnly)
        devices 
        |> Seq.map (fun (handle, inf, info, path) -> 
                let description = GetDeviceProperty handle info SetupApi.SetupDeviceRegistryProperty.DEVICEDESC
                let manufacturer = GetDeviceProperty handle info SetupApi.SetupDeviceRegistryProperty.MFG
                let connected = (inf.Flags &&& SetupApi.SPINT_ACTIVE) = SetupApi.SPINT_ACTIVE
                new DeviceInfo(path, description, manufacturer, connected)) 
        |> Seq.toList
         