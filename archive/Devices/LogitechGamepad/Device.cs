using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Collections;
using System.IO.Ports;
using HIDLibrary;

namespace LogitechGamepad
{
    public class Device
    {
        #region Private Constants

            internal const int PRECISION_ID = 0xC21A;
            internal const int DUAL_ACTION_ID = 0xC216;
            private int _VendorId = 0x046D;
            private int[] _ProductIds = new int[] { PRECISION_ID, DUAL_ACTION_ID };

        #endregion

        #region Events

            public event Action<IMessage> DataArrived;
            public event Action DeviceAttached;
            public event Action DeviceRemoved;

        #endregion

        #region Private Fields

            private bool _DeviceFound = false;
            private bool _Attached = false;

            private HidDevice _Gamepad;

            private int _CurrentProductId;

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
                    foreach (int productId in _ProductIds)
                    {
                        HidDevice[] Devices = HidDevices.Enumerate(_VendorId, productId);

                        if (Devices != null && Devices.Length > 0)
                        {
                            _CurrentProductId = productId;
                            _DeviceFound = true;
                            _Gamepad = Devices[0];

                            _Gamepad.Open();

                            _Gamepad.Inserted += DeviceAttachedHandler;
                            _Gamepad.Removed += DeviceRemovedHandler;

                            _Gamepad.ReadReport(OnReport);
                            return;
                        }
                    }
                }
                catch {}
            }

            public void Close()
            {
                _Gamepad.Inserted -= DeviceAttachedHandler; 
                _Gamepad.Removed -= DeviceRemovedHandler;
                _Gamepad.Close();
            }

        #endregion

        #region Private Methods 

            private void OnReport(HidReport Report)
            {
                if (_Attached == false) { return; }
                try
                {
                    if (Report.Data.Length >= 4)
                    {
                        IMessage Message = MessageFactory.CreateMessage(_CurrentProductId, Report.Data);
                        DataArrived.Invoke(Message);
                    }

                }
                catch { }

                if (_Attached == true) { _Gamepad.ReadReport(OnReport); }
            }

            private void DeviceAttachedHandler()
            {
                _Attached = true;
                if (DeviceAttached != null) { DeviceAttached.Invoke(); }
                _Gamepad.ReadReport(OnReport);
            }

            private void DeviceRemovedHandler()
            {
                _Attached = false;
                if (DeviceRemoved != null) { DeviceRemoved.Invoke(); }
            }

        #endregion

    }
}
