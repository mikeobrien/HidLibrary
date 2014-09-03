namespace HidLibrary
{
    public class HidDeviceData
    {
        public enum ReadStatus
        {
            Success = 0,
            WaitTimedOut = 1,
            WaitFail = 2,
            NoDataRead = 3,
            ReadError = 4,
            NotConnected = 5
        }

        public HidDeviceData(ReadStatus status, byte[] data = null)
        {
            Data = data;
            Status = status;
        }

        public byte[] Data { get; private set; }
        public ReadStatus Status { get; private set; }
    }
}
