namespace HidLibrary
{
    public class HidFastReadDevice : HidDevice
    {
        internal HidFastReadDevice(string devicePath, string description = null)
             : base(devicePath, description) { }

        // FastRead assumes that the device is connected,
        // which could cause stability issues if hardware is
        // disconnected during a read
        public HidDeviceData FastRead()
        {
            return FastRead(0);
        }

        public HidDeviceData FastRead(int timeout)
        {
            try
            {
                return ReadData(timeout);
            }
            catch
            {
                return new HidDeviceData(HidDeviceData.ReadStatus.ReadError);
            }
        }

        public void FastRead(ReadCallback callback)
        {
            var readDelegate = new ReadDelegate(FastRead);
            var asyncState = new HidAsyncState(readDelegate, callback);
            readDelegate.BeginInvoke(EndRead, asyncState);
        }

        public void FastReadReport(ReadReportCallback callback)
        {
            var readReportDelegate = new ReadReportDelegate(FastReadReport);
            var asyncState = new HidAsyncState(readReportDelegate, callback);
            readReportDelegate.BeginInvoke(EndReadReport, asyncState);
        }

        public HidReport FastReadReport(int timeout)
        {
            return new HidReport(Capabilities.InputReportByteLength, FastRead(timeout));
        }

        public HidReport FastReadReport()
        {
            return FastReadReport(0);
        }
    }
}
