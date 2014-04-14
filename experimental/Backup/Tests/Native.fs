namespace Tests

open System
open System.IO
open IO.Usb.Hid
open IO.Usb.Hid.Native
open NUnit.Framework
open FsUnit

[<TestFixture>] 
type ``Native method tests`` ()=
    let GetFirstHidDevice () = 
        SetupApiAdapter.GetDevices HidAdapter.HidClass true 
                     |> Seq.filter (fun device -> device.Description.Contains("Keyboard") = false && device.Description.Contains("Mouse") = false) 
                     |> Seq.head

    [<Test>] 
    member test.``SetupAPI Device Path Enumeration`` () = 
        let totalConnected = SetupApiAdapter.GetDevicePaths HidAdapter.HidClass true |> Seq.length
        let total = SetupApiAdapter.GetDevicePaths HidAdapter.HidClass false |> Seq.length
        Assert.Greater(total, 0)
        Assert.Greater(total, totalConnected)

    [<Test>] 
    member test.``SetupAPI Device Enumeration`` () = 
        let totalConnected = SetupApiAdapter.GetDevices HidAdapter.HidClass true |> Seq.length
        let total = SetupApiAdapter.GetDevices HidAdapter.HidClass false |> Seq.length
        Assert.Greater(total, 0)
        Assert.Greater(total, totalConnected)

    [<Test>] 
    member test.``Create File Handle Test`` () = 
        let device = GetFirstHidDevice ()
        let handle = Kernel32Adapter.OpenFile device.Path FileAccess.Read FileShare.ReadWrite FileMode.Open false
        handle.Close()
    
    [<Test>]
    member test.``Create Device Stream Test`` () = 
        let device = GetFirstHidDevice ()
        let stream = new DeviceStream(device.Path, FileAccess.Read, FileShare.ReadWrite, FileMode.Open, false)
        stream.Close()

    [<Test>] 
    member test.``Hid Device Id`` () = 
        new Guid("4d1e55b2-f16f-11cf-88cb-001111000030") |> should equal HidAdapter.HidClass

    [<Test>] 
    member test.``HidDevice Attributes`` () = 
        let device = GetFirstHidDevice ()
        let handle = Kernel32Adapter.OpenFile device.Path FileAccess.Read FileShare.ReadWrite FileMode.Open false
        let attributes = HidAdapter.GetDeviceAttributes(handle)
        handle.Close()
        Assert.Greater(attributes.VenderId, 0)
        Assert.Greater(attributes.ProductId, 0)
        Assert.Greater(attributes.Version, 0)