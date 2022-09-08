using System.Threading.Tasks;

namespace HidLibrary
{
    public class HidFastReadWriteDevice : HidFastReadDevice
    {
        internal HidFastReadWriteDevice(string devicePath, string description = null)
            : base(devicePath, description) { }

        // FastWrite assumes that the device is connected,
        // which could cause stability issues if hardware is
        // disconnected during a write
        public bool FastWrite(byte[] data)
        {
            return FastWrite(data, 0);
        }

        public bool FastWrite(byte[] data, int timeout)
        {
            try
            {
                return WriteData(data, timeout);
            }
            catch
            {
                return false;
            }
        }

        public void FastWrite(byte[] data, WriteCallback callback)
        {
            FastWrite(data, callback, 0);
        }

        public void FastWrite(byte[] data, WriteCallback callback, int timeout)
        {
            var writeDelegate = new WriteDelegate(WriteData);
            var asyncState = new HidAsyncState(writeDelegate, callback);
            writeDelegate.BeginInvoke(data, timeout, EndWrite, asyncState);
        }

        public async Task<bool> FastWriteAsync(byte[] data, int timeout = 0)
        {
            var writeDelegate = new WriteDelegate(FastWrite);
#if NET20 || NET35 || NET5_0_OR_GREATER
            return await Task<bool>.Factory.StartNew(() => writeDelegate.Invoke(data, timeout));
#else
            return await Task<bool>.Factory.FromAsync(writeDelegate.BeginInvoke, writeDelegate.EndInvoke, data, timeout, null);
#endif
        }

        public bool FastWriteReport(HidReport report)
        {
            return FastWriteReport(report, 0);
        }

        public bool FastWriteReport(HidReport report, int timeout)
        {
            return FastWrite(report.GetBytes(), timeout);
        }

        public void FastWriteReport(HidReport report, WriteCallback callback)
        {
            FastWriteReport(report, callback, 0);
        }

        public void FastWriteReport(HidReport report, WriteCallback callback, int timeout)
        {
            var writeReportDelegate = new WriteReportDelegate(FastWriteReport);
            var asyncState = new HidAsyncState(writeReportDelegate, callback);
            writeReportDelegate.BeginInvoke(report, timeout, EndWriteReport, asyncState);
        }

        public async Task<bool> FastWriteReportAsync(HidReport report, int timeout = 0)
        {
            var writeReportDelegate = new WriteReportDelegate(FastWriteReport);
#if NET20 || NET35 || NET5_0_OR_GREATER
            return await Task<bool>.Factory.StartNew(() => writeReportDelegate.Invoke(report, timeout));
#else
            return await Task<bool>.Factory.FromAsync(writeReportDelegate.BeginInvoke, writeReportDelegate.EndInvoke, report, timeout, null);
#endif
        }    }
}
