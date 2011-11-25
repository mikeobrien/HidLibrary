using System;
using System.Collections.Generic;
using System.Threading;
using HidLibrary;
using System.Linq;

namespace Honeywell4000Series
{
    public class Scanner : IDisposable
    {
	    private const int HoneywellVendorId = 0x536;
	    private const int HidReportId = 0x4;
	    private const int ErrorBeepMessage = 0x20;
	    private const int SuccessBeepMessage = 0x40;
	    private const int StartScanMessage = 0x4;
	    private const int EndScanMessage = 0x1;

	    public event InsertedEventHandler Inserted;
	    public event RemovedEventHandler Removed;
	    public event DataRecievedEventHandler DataRecieved;	
        
        public delegate void InsertedEventHandler();
	    public delegate void RemovedEventHandler();
	    public delegate void DataRecievedEventHandler(byte[] data);

	    private readonly HidDevice _scanner;
        private int _isReading;

	    public Scanner(string devicePath) : this(HidDevices.GetDevice(devicePath)) { }

	    public Scanner(HidDevice hidDevice)
	    {
		    _scanner = hidDevice;

	        _scanner.Inserted += ScannerInserted;
            _scanner.Removed += ScannerRemoved;

		    if (!_scanner.IsOpen) _scanner.OpenDevice();
		    _scanner.MonitorDeviceEvents = true;

		    BeginReadReport();
	    }

	    public string DevicePath { get { return _scanner.DevicePath; } }
        public bool IsListening { get; private set; }
        public bool IsConnected { get { return _scanner.IsConnected; } }

	    public static IEnumerable<Scanner> Enumerate(int[] productIds)
	    {
            return HidDevices.Enumerate(HoneywellVendorId, productIds).Select(x => new Scanner(x));
	    }

	    public void ErrorBeep()
	    {
		    var report = _scanner.CreateReport();

		    report.ReportId = HidReportId;
		    report.Data[0] = ErrorBeepMessage;

		    _scanner.WriteReport(report);
	    }

	    public void SuccessBeep()
	    {
		    var report = _scanner.CreateReport();

		    report.ReportId = HidReportId;
		    report.Data[0] = SuccessBeepMessage;

		    _scanner.WriteReport(report);
	    }

	    public void StartScan()
	    {
		    var report = _scanner.CreateReport();

		    report.ReportId = HidReportId;
		    report.Data[0] = StartScanMessage;

		    _scanner.WriteReport(report);
	    }

	    public void EndScan()
	    {
		    var report = _scanner.CreateReport();

		    report.ReportId = HidReportId;
            report.Data[0] = EndScanMessage;

		    _scanner.WriteReport(report);
	    }

	    public void StartListen() { IsListening = true; }
	    public void StopListen() { IsListening = false; }

	    private void BeginReadReport()
	    {
		    if (Interlocked.CompareExchange(ref _isReading, 1, 0) == 1) return;
		    _scanner.ReadReport(ReadReport);
	    }

	    private void ReadReport(HidReport report)
	    {
		    var scannerReport = new Report(report);
            var data = new byte[] { };
		    var currentPosition = 0;
		    var readRequired = false;

		    if (scannerReport.Length > 0 & scannerReport.ReadStatus == HidDeviceData.ReadStatus.Success) 
            {
			    do 
                {
				    if (scannerReport.MoreData && readRequired) scannerReport = new Report(_scanner.ReadReport());
				    else readRequired = true;

                    if (!scannerReport.Exists) continue;

                    Array.Resize(ref data, data.GetUpperBound(0) + scannerReport.Data.Length + 1);
                    Array.Copy(scannerReport.Data, 0, data, currentPosition, scannerReport.Data.Length);
                    currentPosition += scannerReport.Data.Length;

                } while (scannerReport.MoreData && scannerReport.Exists);

			    if (IsListening && data.Length > 0 && DataRecieved != null) DataRecieved(data);
		    }

		    if (scannerReport.ReadStatus != HidDeviceData.ReadStatus.NotConnected) _scanner.ReadReport(ReadReport);
            else _isReading = 0;
	    }

	    private void ScannerInserted()
	    {
		    BeginReadReport();
		    if (Inserted != null) Inserted();
	    }

	    private void ScannerRemoved()
	    {
		    if (Removed != null) Removed();
	    }

        public void Dispose()
        {
            _scanner.CloseDevice();
            GC.SuppressFinalize(this);
        }

        ~Scanner()
        {
            Dispose();
        }
    }
}
