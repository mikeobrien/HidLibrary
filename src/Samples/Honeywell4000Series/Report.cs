using System;
using HidLibrary;

namespace Honeywell4000Series
{
    public class Report
    {
        private readonly byte[] _data;
        private readonly HidDeviceData.ReadStatus _status;

        public Report(HidReport hidReport)
        {
            _status = hidReport.ReadStatus;
            ReportId = hidReport.ReportId;
            Exists = hidReport.Exists;

            if (hidReport.Data.Length > 0) Length = hidReport.Data[0];
            if (hidReport.Data.Length > 1) AimSymbologyId0 = hidReport.Data[1];
            if (hidReport.Data.Length > 2) AimSymbologyId1 = hidReport.Data[2];
            if (hidReport.Data.Length > 3) AimSymbologyId2 = hidReport.Data[3];

            if (hidReport.Data.Length > Length + 3)
            {
                Array.Resize(ref _data, Length);
                Array.Copy(hidReport.Data, 4, _data, 0, Length);
            }

            if (hidReport.Data.Length > 60) HhpSymbologyId = hidReport.Data[60];
            if (hidReport.Data.Length > 61) Reserved = hidReport.Data[61];
            if (hidReport.Data.Length > 62) MoreData = hidReport.Data[62] == 1;
        }

        public HidDeviceData.ReadStatus ReadStatus { get { return _status; } }
        public byte[] Data { get { return _data; } }
        public bool Exists { get; private set; }
        public byte ReportId { get; private set; }
        public byte Length { get; private set; }
        public byte AimSymbologyId0 { get; private set; }
        public byte AimSymbologyId1 { get; private set; }
        public byte AimSymbologyId2 { get; private set; }
        public byte HhpSymbologyId { get; private set; }
        public byte Reserved { get; private set; }
        public bool MoreData { get; private set; }
    }
}
