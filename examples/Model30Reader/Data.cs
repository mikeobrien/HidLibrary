using System;
using System.Text;

namespace Model30Reader
{
    public class Data
    {
        private string _errorMessage = string.Empty;
        private readonly byte[] _data;

        public Data(byte[] data)
        {
            _data = GetText(data);
        }

        public Data(String text)
        {
            _data = GetBytes(text);
        }

        public string ErrorMessage { get { return _errorMessage; } }
        public bool Error { get; private set; }
        public byte[] Bytes { get { return _data; } }

        public String Text { get { return Encoding.ASCII.GetString(_data); } }

        private byte[] GetText(byte[] data)
        {
            if (data == null)
            {
                _errorMessage = "Null data";
            }
            else if(data[0] == 0x3F)
            {
                Error = false;
                var text = new byte[data[1]];
                Array.Copy(data, 2, text, 0, data[1]);
                return text;
            }
            Error = true;
            return null;
        }

        private byte[] GetBytes(String text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text.Trim().ToCharArray());
            byte[] sendData = new byte[text.Length + 2];
            sendData[0] = 0x3F;
            sendData[1] = (byte)data.Length;
            Array.Copy(data, 0, sendData, 2, data.Length);
            return sendData;
        }
    }
}
