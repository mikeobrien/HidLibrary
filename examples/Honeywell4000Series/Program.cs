using System;
using System.Linq;
using System.Text;

namespace Honeywell4000Series
{
    class Program
    {
        private static readonly int[] SupportedDevices = new [] { 0x207, 0x1C7 };
        private static Scanner _scanner;

        static void Main()
        {
            _scanner = Scanner.Enumerate(SupportedDevices).FirstOrDefault();

            if (_scanner != null)
            {
                _scanner.Inserted += ScannerInserted;
                _scanner.DataRecieved += ScannerDataRecieved;
                _scanner.Removed += ScannerRemoved;
                _scanner.StartListen();

                Console.WriteLine("Connected, press any key to quit.");
                Console.ReadKey();

                _scanner.StopListen();
                _scanner.Dispose();
            }
            else
            {
                Console.WriteLine("No scanner found.");
                Console.ReadKey();
            }
        }

        private static void ScannerDataRecieved(byte[] data)
        {
            Console.WriteLine(Encoding.ASCII.GetString(data));
        }

        private static void ScannerInserted()
        {
            Console.WriteLine("Scanner attached.");
        }

        private static void ScannerRemoved()
        {
            Console.WriteLine("Scanner detached.");
        }
    }
}
