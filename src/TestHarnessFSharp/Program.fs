open System
open System.IO
open IO.Usb.Hid
open System.Runtime.InteropServices
open IO.Usb.Hid.Native
open System.Diagnostics

let phidgetVid = 0x06c2
let phidgetPid = 0x0045


Console.WriteLine()
Console.WriteLine("Press any key to continue...")
Console.ReadKey() |> ignore


        