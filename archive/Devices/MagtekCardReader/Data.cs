using System;
using System.Collections.Generic;
using System.Text;

namespace MagtekCardReader
{
    class Data
    {
        #region Private Fields

            private bool _Error = false;
            private string _ErrorMessage = string.Empty;
            private byte[] _Data;

        #endregion

        #region Constructor

            public Data(byte[] Data)
            {
                _Data = GetCardData(Data);
            }

        #endregion

        #region Public Properties

            public string ErrorMessage { get { return _ErrorMessage; } }
            public bool Error { get { return _Error; } }
            public byte[] CardData { get { return _Data; } }

        #endregion
        
        #region Private Methods

            private byte[] GetCardData(byte[] Data)
            {
                if (Data != null && Data.Length == 337)
                {
                    if (Data[0] == 1 || Data[1] == 1 || Data[2] == 1)
                    {
                        _Error = true;
                        _ErrorMessage = "Error reading data";
                        return null;
                    }
                    else
                    {
                        byte[] CardData = new byte[Data[3] + Data[4] + Data[5]];
                        Array.Copy(Data, 7, CardData, 0, Data[3]);
                        Array.Copy(Data, 117, CardData, Data[3], Data[4]);
                        Array.Copy(Data, 227, CardData, Data[3] + Data[4], Data[5]);
                        return CardData;
                    }
                }
                else
                {
                    _Error = true;
                    _ErrorMessage = "Data length is invalid";
                    return null;
                }
            }

        #endregion
    }
}
