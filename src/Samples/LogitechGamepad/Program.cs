using System;
using System.Linq;
using HidLibrary;

namespace LogitechGamepad
{
    class Program
    {
        private const int VendorId = 0x046D;
        private static readonly int[] ProductIds = new[] { MessageFactory.PrecisionId, MessageFactory.DualActionId };
        private static int _currentProductId;
        private static HidDevice _device;
        private static bool _attached;

        static void Main()
        {
            foreach (var productId in ProductIds)
            {
                _device = HidDevices.Enumerate(VendorId, productId).FirstOrDefault();

                if (_device == null) continue;

                _currentProductId = productId;

                _device.OpenDevice();

                _device.Inserted += DeviceAttachedHandler;
                _device.Removed += DeviceRemovedHandler;

                _device.MonitorDeviceEvents = true;

                _device.ReadReport(OnReport);
                break;
            }

            if (_device != null)
            {
                Console.WriteLine("Gamepad found, press any key to exit.");
                Console.ReadKey();
                _device.CloseDevice();
            }
            else
            {
                Console.WriteLine("Could not find a gamepad.");
                Console.ReadKey();
            }
        }

        private static void OnReport(HidReport report)
        {
            if (_attached == false) { return; }

            if (report.Data.Length >= 4)
            {
                var message = MessageFactory.CreateMessage(_currentProductId, report.Data);
                if (message.Depress) { KeyDepressed(); }
                else if (message.MultiplePressed == false)
                {
                    if (message.UpPressed) { KeyPressed(1); }
                    if (message.DownPressed) { KeyPressed(2); }
                    if (message.LeftPressed) { KeyPressed(3); }
                    if (message.RightPressed) { KeyPressed(4); }
                    if (message.Button1Pressed) { KeyPressed(5); }
                    if (message.Button2Pressed) { KeyPressed(6); }
                    if (message.Button3Pressed) { KeyPressed(7); }
                    if (message.Button4Pressed) { KeyPressed(8); }
                    if (message.Button5Pressed) { KeyPressed(9); }
                    if (message.Button6Pressed) { KeyPressed(10); }
                    if (message.Button7Pressed) { KeyPressed(11); }
                    if (message.Button8Pressed) { KeyPressed(12); }
                    if (message.Button9Pressed) { KeyPressed(13); }
                    if (message.Button10Pressed) { KeyPressed(14); }
                }
            }

            _device.ReadReport(OnReport);
        }

        private static void KeyPressed(int value)
        {
            Console.WriteLine("Button {0} pressed.", value);
        }

        private static void KeyDepressed()
        {
            Console.WriteLine("Button depressed.");
        }

        private static void DeviceAttachedHandler()
        {
            _attached = true;
            Console.WriteLine("Gamepad attached.");
            _device.ReadReport(OnReport);
        }

        private static void DeviceRemovedHandler()
        {
            _attached = false;
            Console.WriteLine("Gamepad removed.");
        }
    }
}
