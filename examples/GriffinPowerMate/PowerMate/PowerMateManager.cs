using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HidLibrary;

// This example was written by Thomas Hammer (www.thammer.net).

namespace GriffinPowerMate
{
    /// <summary>
    /// Manages one PowerMate device.
    /// </summary>
    public class PowerMateManager : IDisposable
    {
        private const int VendorId = 0x077d;
        private const int ProductId = 0x0410;
        private HidDevice device;
        private bool attached = false;
        private bool connectedToDriver = false;
        private PowerMateState prevState;
        private bool debugPrintRawMessages = false;
        private bool disposed = false;

        /// <summary>
        /// Occurs when a PowerMate device is attached.
        /// </summary>
        public event EventHandler DeviceAttached;

        /// <summary>
        /// Occurs when a PowerMate device is removed.
        /// </summary>
        public event EventHandler DeviceRemoved;

        /// <summary>
        /// Occurs when the PowerMate button changes state from Up to Down.
        /// </summary>
        public event EventHandler<PowerMateEventArgs> ButtonDown;

        /// <summary>
        /// Occurs when the PowerMate button changes state from Down to Up.
        /// </summary>
        public event EventHandler<PowerMateEventArgs> ButtonUp;

        /// <summary>
        /// Occurs when the PowerMate is rotated.
        /// </summary>
        public event EventHandler<PowerMateEventArgs> KnobDisplacement;

        /// <summary>
        /// Initializes a new instance of the PowerMateManager class.
        /// </summary>
        public PowerMateManager()
        {
            prevState = new PowerMateState(PowerMateButtonState.Up, 0, 0, false, false, 0);
        }

        /// <summary>
        /// Attempts to connect to a PowerMate device.
        /// 
        /// After a successful connection, a DeviceAttached event will normally be sent.
        /// </summary>
        /// <returns>True if a PowerMate device is connected, False otherwise.</returns>
        public bool OpenDevice()
        {
            device = HidDevices.Enumerate(VendorId, ProductId).FirstOrDefault();

            if (device != null)
            {
                connectedToDriver = true;
                device.OpenDevice();

                device.Inserted += DeviceAttachedHandler;
                device.Removed += DeviceRemovedHandler;

                device.MonitorDeviceEvents = true;

                device.ReadReport(OnReport); 

                return true;
            }

            return false;
        }

        /// <summary>
        /// Closes the connection to the device.
        /// 
        /// FIXME: Verify that this also shuts down any thread waiting for device data. 2012-06-07 thammer.
        /// </summary>
        public void CloseDevice()
        {
            device.CloseDevice();
            connectedToDriver = false;
        }

        /// <summary>
        /// Closes the connection to the device.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Sends a message to the PowerMate device to enable pulsing of the LED.
        /// The (constant) LED brightness will be ignored. The pulse speed is set
        /// by the SetLedPulseBrightness() method.
        /// </summary>
        /// <param name="enable">True to enable pulsing, False otherwise.</param>
        public void SetLedPulseEnabled(bool enable)
        {
            if (connectedToDriver)
            {
                byte [] data = new byte[9];
                data[0] = 0x00;
                data[1] = 0x41;
                data[2] = 0x01;
                data[3] = 0x03; // command
                data[4] = 0x00;
                data[5] = enable ? (byte)0x01 : (byte)0x00;
                data[6] = 0x00;
                data[7] = 0x00;
                data[8] = 0x00;
                HidReport report = new HidReport(9, new HidDeviceData(data, HidDeviceData.ReadStatus.Success));
                device.WriteFeatureData(data);
            }
        }

        /// <summary>
        /// Sends a message to the PowerMate device to enable pulsing of the LED
        /// while the computer is sleeping.
        /// </summary>
        /// <param name="enable">True to enable pulsing during sleep, False otherwse.</param>
        public void SetLedPulseDuringSleepEnabled(bool enable)
        {
            if (connectedToDriver)
            {
                byte[] data = new byte[9];
                data[0] = 0x00;
                data[1] = 0x41;
                data[2] = 0x01; 
                data[3] = 0x02;
                data[4] = 0x00; // command
                data[5] = enable ? (byte)0x01 : (byte)0x00;
                data[6] = 0x00;
                data[7] = 0x00;
                data[8] = 0x00;
                HidReport report = new HidReport(9, new HidDeviceData(data, HidDeviceData.ReadStatus.Success));
                device.WriteFeatureData(data);
            }
        }

        /// <summary>
        /// Sets the PowerMate's LED pulse speed. The range is  [-255, 255], and the
        /// useful range seems to be approximately [-32, 64]. A value of 0 means default 
        /// pulse speed. A negative value is slower than the default, a positive value 
        /// is faster.
        /// </summary>
        /// <param name="speed"></param>
        public void SetLedPulseSpeed(int speed)
        {
            if (connectedToDriver)
            {
                byte[] data = new byte[9];
                data[0] = 0x00;
                data[1] = 0x41;
                data[2] = 0x01;
                data[3] = 0x04; // command
                data[4] = 0x00; // Table 0
                if (speed < 0)
                {
                    data[5] = 0;
                    data[6] = (byte)(-speed);
                } 
                else if (speed == 0)
                {
                    data[5] = 1;
                    data[6] = 0;
                }
                else // speed > 0
                {
                    data[5] = 2;
                    data[6] = (byte)(speed);
                }
                data[7] = 0x00;
                data[8] = 0x00;
                HidReport report = new HidReport(9, new HidDeviceData(data, HidDeviceData.ReadStatus.Success));
                device.WriteFeatureData(data);
            }
        }

        /// <summary>
        /// Sets the PowerMate's LED brightness. 
        /// </summary>
        /// <param name="brightness">The brightness of the LED. The valid range is [0, 255],
        /// with 0 being completely off and 255 being full brightness.
        /// </param>
        public void SetLedBrightness(int brightness)
        {
            if (connectedToDriver)
            {
                byte[] data = new byte[2];
                data[0] = 0;
                data[1] = (byte)brightness;
                HidReport report = new HidReport(2, new HidDeviceData(data, HidDeviceData.ReadStatus.Success));
                device.WriteReport(report);
            }
        }

        private void OnButtonDown(PowerMateState state)
        {
            var handle = ButtonDown;
            if (handle != null)
            {
                handle(this, new PowerMateEventArgs(state));
            }
        }

        private void OnButtonUp(PowerMateState state)
        {
            var handle = ButtonUp;
            if (handle != null)
            {
                handle(this, new PowerMateEventArgs(state));
            }
        }

        private void OnKnobDisplacement(PowerMateState state)
        {
            var handle = KnobDisplacement;
            if (handle != null)
            {
                handle(this, new PowerMateEventArgs(state));
            }
        }

        private void GenerateEvents(PowerMateState state)
        {
            if (state.ButtonState == PowerMateButtonState.Down && prevState.ButtonState == PowerMateButtonState.Up)
                OnButtonDown(state);
            else if (state.ButtonState == PowerMateButtonState.Up && prevState.ButtonState == PowerMateButtonState.Down)
                OnButtonUp(state);

            if (state.KnobDisplacement != 0)
                OnKnobDisplacement(state);

            prevState = state;
        }

        private void DeviceAttachedHandler()
        {
            attached = true;

            if (DeviceAttached != null)
                DeviceAttached(this, EventArgs.Empty);

            device.ReadReport(OnReport);
        }

        private void DeviceRemovedHandler()
        {
            attached = false;

            if (DeviceRemoved != null)
                DeviceRemoved(this, EventArgs.Empty);
        }

        private void OnReport(HidReport report)
        {
            if (attached == false) { return; }

            if (report.Data.Length >= 6)
            {
                PowerMateState state = ParseState(report.Data);
                if (!state.IsValid)
                {
                    System.Diagnostics.Debug.WriteLine("Invalid PowerMate state");
                }
                else
                {
                    GenerateEvents(state);

                    if (debugPrintRawMessages)
                    {
                        System.Diagnostics.Debug.Write("PowerMate raw data: ");
                        for (int i = 0; i < report.Data.Length; i++)
                        {
                            System.Diagnostics.Debug.Write(String.Format("{0:000} ", report.Data[i]));
                        }
                        System.Diagnostics.Debug.WriteLine("");
                    }
                }
            }

            device.ReadReport(OnReport);
        }

        private PowerMateState ParseState(byte[] data)
        {
            if (data.Length >= 6)
            {
                PowerMateButtonState buttonState = data[0] == 0 ? PowerMateButtonState.Up : PowerMateButtonState.Down;
                int knobDisplacement = data[1] < 128 ? data[1] : -256 + data[1];
                int ledBrightness = data[2];
                bool ledPulseEnabled = (data[4] & 0x01) == 0x01;
                bool ledPulseDuringSleepEnabled = (data[4] & 0x04) == 0x04;
                int ledPulseSpeedFlags = (data[4] & 0x30) >> 4;
                int ledPulseSpeed = 0;
                switch (ledPulseSpeedFlags) {
                    case 0: ledPulseSpeed = -data[5]; break;
                    case 1: ledPulseSpeed = 0; break;
                    case 2: ledPulseSpeed = data[5]; break;
                }

                return new PowerMateState(buttonState, knobDisplacement, ledBrightness, ledPulseEnabled, ledPulseDuringSleepEnabled, ledPulseSpeed);
            }
            else
            {
                return new PowerMateState(); // PowerMateState.Invalid() will return false
            }
        }

        /// <summary>
        /// Closes any connected devices.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if(disposing)
                {
                    CloseDevice();
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Destroys instance and frees device resources (if not freed already)
        /// </summary>
        ~PowerMateManager()
        {
            Dispose(false);
        }

    }
}
