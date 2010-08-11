using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HIDLibrary;
using System.Diagnostics;

namespace TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<HidDevices.DeviceInfo> _deviceInfo = HidDevices.EnumerateTest();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            IEnumerable<HidDevices.DeviceInfo> _deviceInfo2 = HidDevices.EnumerateTest();
            stopwatch.Stop();
            Console.Write(stopwatch.ElapsedTicks);

            Console.ReadKey();
        }
    }
}
