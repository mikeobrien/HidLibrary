using System;
using System.Linq;

namespace HidLibrary
{
    public class HidReport
    {
        private byte _reportId;
        private byte[] _data;

        private readonly HidDeviceData.ReadStatus _status;

        public HidReport(int reportSize)
        {
            _data = new byte[reportSize - 1];
        }

        public HidReport(int reportSize, HidDeviceData deviceData)
            : this(reportSize)
        {
            _status = deviceData.Status;

            if (deviceData.Data != null && deviceData.Data.Any())
            {
                ReportId = deviceData.Data.First();
                deviceData.Data.Skip(1).ToArray().CopyTo(_data, 0);
            }
        }

        public bool Exists { get; private set; }
        public HidDeviceData.ReadStatus ReadStatus { get { return _status; } }

        public byte ReportId
        {
            get { return _reportId; }
            set
            {
                _reportId = value;
                Exists = true;
            }
        }

        public byte[] Data
        {
            get { return _data; }
            set
            {
                _data = value;
                Exists = true;
            }
        }

        public byte[] GetBytes()
        {
            return new[] { _reportId }.Concat(_data).ToArray();
        }
    }
}
