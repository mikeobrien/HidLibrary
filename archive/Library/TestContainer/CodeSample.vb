Imports HIDLibrary

Public Class CodeSample

    Public Shared Sub Main()

        Dim HidDeviceList As HIDLibrary.HidDevice()
        Dim HidDevice As HidDevice

        HidDeviceList = HidDevices.Enumerate(1334, 519)

        If HidDeviceList.Length > 0 Then

            HidDevice = HidDeviceList(0)

            '--> Check if connected...

            Debug.WriteLine("Connected: " & HidDevice.IsConnected)

            Dim OutData As Byte()

            ReDim OutData(HidDevice.Capabilities.OutputReportByteLength - 1)

            '--> Send a report to initiate an error sound

            OutData(0) = &H4
            OutData(1) = &H20

            HidDevice.Write(OutData)

            '--> Send a report to initiate an success sound

            OutData(1) = &H40

            HidDevice.Write(OutData)

            '--> Send a report to start a scan

            OutData(1) = &H4

            HidDevice.Write(OutData)

            Threading.Thread.Sleep(2000)

            '--> Send a report to stop the scan

            OutData(1) = &H1

            HidDevice.Write(OutData)

            '--> Blocking read of report

            Dim InData As HidDeviceData
            Dim Text As String

            InData = HidDevice.Read

            Text = System.Text.ASCIIEncoding.ASCII.GetString(InData.Data)

            Debug.WriteLine(Text)

        End If

    End Sub

End Class
