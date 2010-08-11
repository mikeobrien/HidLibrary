using System;
using System.Collections.Generic;
using System.Text;
using HIDLibrary;
using System.Collections;
using System.Collections.Specialized;

namespace MagtekCardReader
{
    public class Device
    {
        #region Private Constants

            private int _VendorId = 0x0801;
            private int _ProductId = 0x0002;

        #endregion

        #region Events

            public event Action<byte[]> DataArrived;
            public event Action DeviceAttached;
            public event Action DeviceRemoved;

        #endregion

        #region Private Fields

            private bool _DeviceFound = false;
            private HidDevice _CardReader;

        #endregion

        #region Properties

            public bool DeviceFound
            {
                get { return _DeviceFound; }
            }

        #endregion

        #region Methods

            public void Open()
            {
                try
                {
                    HidDevice[] Devices = HidDevices.Enumerate(_VendorId, _ProductId);

                    if (Devices != null && Devices.Length > 0)
                    {
                        _DeviceFound = true;
                        _CardReader = Devices[0];

                        _CardReader.Open();

                        _CardReader.Inserted += DeviceAttachedHandler;
                        _CardReader.Removed += DeviceRemovedHandler;

                        _CardReader.ReadReport(OnReport);
                    }
                }
                catch {}
            }

            public void Close()
            {
                _CardReader.Inserted -= DeviceAttachedHandler;
                _CardReader.Removed -= DeviceRemovedHandler;
                _CardReader.Close();
            }

        #endregion

        #region Private Methods 

            private void OnReport(HidReport Report)
            {
                bool Connected = _CardReader.IsConnected;
                if (Connected == false) { return; }
                try
                {
                    Data CardData = new Data(Report.Data);

                    if (CardData.Error == false)
                    {
                        if (DataArrived != null)
                        {
                            DataArrived.Invoke(Report.Data);
                        }
                    }
                    else
                    {
                        if (DataArrived != null) { throw new Exception(CardData.ErrorMessage); }
                    }
                }
                catch
                {
                    if (DataArrived != null) throw;
                }

                if (Connected == true) { _CardReader.ReadReport(OnReport); }
            }

            private void DeviceAttachedHandler()
            {
                if (DeviceAttached != null) { DeviceAttached.Invoke(); }
                _CardReader.ReadReport(OnReport);
            }

            private void DeviceRemovedHandler()
            {
                if (DeviceRemoved != null) { DeviceRemoved.Invoke(); }
            }

        #endregion

    }
}
