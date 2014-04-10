using System;

namespace MagtekCardReader
{
    public class Data
    {
        private string _errorMessage = string.Empty;
        private readonly byte[] _data;

        public Data(byte[] data)
        {
            _data = GetCardData(data);
        }

        public string ErrorMessage { get { return _errorMessage; } }
        public bool Error { get; private set; }
        public byte[] CardData { get { return _data; } }

        private byte[] GetCardData(byte[] data)
        {
            if (data != null && data.Length == 337)
            {
                if (data[0] == 1 || data[1] == 1 || data[2] == 1)
                {
                    Error = true;
                    _errorMessage = "Error reading data";
                    return null;
                }
                var cardData = new byte[data[3] + data[4] + data[5]];
                Array.Copy(data, 7, cardData, 0, data[3]);
                Array.Copy(data, 117, cardData, data[3], data[4]);
                Array.Copy(data, 227, cardData, data[3] + data[4], data[5]);
                return cardData;
            }
            Error = true;
            _errorMessage = "Data length is invalid";
            return null;
        }
    }
}
