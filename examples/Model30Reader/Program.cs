using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using HidLibrary;

namespace Model30Reader
{
    class Program
    {
        private const int VendorId = 0x2047;
        private const int ProductId = 0x0301;

        private static HidDevice _device;
        private static string command;
        private static string last_input;
        private static List<string> inputs = new List<string>();
        private static bool busy = false;

        static void Main()
        {
            bool deviceFound = false;
            while (!deviceFound)
            {
                _device = HidDevices.Enumerate(VendorId, ProductId).FirstOrDefault();

                if (_device != null)
                {
                    deviceFound = true;
                    _device.OpenDevice();

                    _device.Inserted += DeviceAttachedHandler;
                    _device.Removed += DeviceRemovedHandler;

                    _device.MonitorDeviceEvents = true;
                    
                    _device.Read(OnRead);
                    
                    byte[] product;
                    _device.ReadProduct(out product);
                    int l = 0;
                    for (; product != null && product[l] << 8 + product[l + 1] != (short)0; l += 2);
                    
                    Console.WriteLine(Encoding.Unicode.GetString(product, 0, l) + " found, enter command:");

                    CommandLoop();
                }
                else
                {
                    Console.WriteLine("Could not find reader. Press any key to rescan:");
                    Console.ReadKey();
                }
            }
        }

        private static void CommandLoop()
        {
            while (command == null || !command.Contains("q;"))
            {
                if (!busy)
                {
                    Console.WriteLine(last_input);
                    command = Console.ReadLine();
                    if (command.Contains("q;"))
                    {
                        continue;
                    }
                    else if(command.Contains("inputs;"))
                    {
                        inputs.ForEach(input => Console.Write(input + "\n\n"));
                    }
                    else if(command.Contains("reset;"))
                    {
                        inputs.Clear();
                        Console.WriteLine("Inputs cleared.");
                    }
                    else
                    {
                        ExecuteCommand(command);
                    }
                }
            }

            //_device.CloseDevice();
            Environment.Exit(0);
        }

        private static void ExecuteCommand(string commandText)
        {
            commandText = commandText.Trim();
            int l = commandText.Length, chunk = 62;
            for (int i = 0; i < l; i += chunk)
            {
                if (i + chunk >= l) chunk = l - i;
                Data sendData = new Data(commandText.Substring(i, chunk));
                _device.Write(sendData.Bytes);
            }
        }

        private static void OnRead(HidDeviceData data)
        {
            if (!_device.IsConnected) { return; }

            var text = new Data(data.Data);

            if (!text.Error)
            {
                if (!busy)
                {
                    busy = true;
                    last_input = "";
                }
                last_input += text.Text;

                if(last_input.Contains("ACK"))
                {
                    inputs.Add(new String(last_input.ToCharArray()));
                    busy = false;
                }
            }

            _device.Read(OnRead);
        }

        private static void DeviceAttachedHandler()
        {
            Console.WriteLine("Device attached.");
            _device.Read(OnRead);
        }

        private static void DeviceRemovedHandler()
        {
            Console.WriteLine("Device removed.");
            Console.WriteLine("Last message received: " + last_input);
            last_input = "";
            inputs.Clear();
            busy = false;
        }
    }
}
