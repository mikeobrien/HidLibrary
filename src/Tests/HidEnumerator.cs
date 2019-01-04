using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Should;

namespace HidLibrary.Tests
{
    public class HidEnumeratorTests
    {
        private HidEnumerator enumerator;
        private string devicePath;

        private void BeforeEach()
        {
            enumerator = new HidEnumerator();
            var firstDevice = enumerator.Enumerate().FirstOrDefault();

            if(firstDevice != null)
            {
                devicePath = firstDevice.DevicePath;
            }
            else
            {
                devicePath = "";
            }
        }

        [Fact]
        public void CanConstruct()
        {
            BeforeEach();
            enumerator.ShouldBeType(typeof(HidEnumerator));
        }

        [Fact]
        public void WrapsIsConnected()
        {
            BeforeEach();
            bool enumIsConnected = enumerator.IsConnected(devicePath);
            bool hidIsConnected = HidDevices.IsConnected(devicePath);
            enumIsConnected.ShouldEqual(hidIsConnected);
        }

        [Fact]
        public void WrapsGetDevice()
        {
            BeforeEach();
            IHidDevice enumDevice = enumerator.GetDevice(devicePath);
            IHidDevice hidDevice = HidDevices.GetDevice(devicePath);
            enumDevice.DevicePath.ShouldEqual(hidDevice.DevicePath);
        }

        [Fact]
        public void WrapsEnumerateDefault()
        {
            BeforeEach();
            IEnumerable<IHidDevice> enumDevices = enumerator.Enumerate();
            IEnumerable<IHidDevice> hidDevices = HidDevices.Enumerate().
                Select(d => d as IHidDevice);

            
            AllDevicesTheSame(enumDevices, hidDevices).ShouldBeTrue();
        }

        [Fact]
        public void WrapsEnumerateDevicePath()
        {
            BeforeEach();
            IEnumerable<IHidDevice> enumDevices =
                enumerator.Enumerate(devicePath);
            IEnumerable<IHidDevice> hidDevices =
                HidDevices.Enumerate(devicePath).
                    Select(d => d as IHidDevice);


            AllDevicesTheSame(enumDevices, hidDevices).ShouldBeTrue();
        }

        [Fact]
        public void WrapsEnumerateVendorId()
        {
            BeforeEach();
            int vid = GetVid();

            IEnumerable<IHidDevice> enumDevices =
                enumerator.Enumerate(vid);
            IEnumerable<IHidDevice> hidDevices =
                HidDevices.Enumerate(vid).
                    Select(d => d as IHidDevice);


            AllDevicesTheSame(enumDevices, hidDevices).ShouldBeTrue();
        }

        [Fact]
        public void WrapsEnumerateVendorIdProductId()
        {
            BeforeEach();
            int vid = GetVid();
            int pid = GetPid();

            IEnumerable<IHidDevice> enumDevices =
                enumerator.Enumerate(vid, pid);
            IEnumerable<IHidDevice> hidDevices =
                HidDevices.Enumerate(vid, pid).
                    Select(d => d as IHidDevice);


            AllDevicesTheSame(enumDevices, hidDevices).ShouldBeTrue();
        }

        private bool AllDevicesTheSame(IEnumerable<IHidDevice> a,
            IEnumerable<IHidDevice> b)
        {
            if(a.Count() != b.Count())
                return false;
            
            bool allSame = true;

            var aList = a.ToList();
            var bList = b.ToList();

            int numDevices = aList.Count;

            for (int i = 0; i < numDevices; i++)
            {
                if (aList[i].DevicePath !=
                    bList[i].DevicePath)
                {
                    allSame = false;
                    break;
                }
            }

            return allSame;
        }

        private int GetVid()
        {
            return GetNumberFromRegex("vid_([0-9a-f]{4})");
        }

        private int GetPid()
        {
            return GetNumberFromRegex("pid_([0-9a-f]{3,4})");
        }

        private int GetNumberFromRegex(string pattern)
        {
            var match = Regex.Match(devicePath, pattern,
                RegexOptions.IgnoreCase);

            int num = 0;

            if (match.Success)
            {
                num = int.Parse(match.Groups[1].Value,
                    System.Globalization.NumberStyles.HexNumber);
            }

            return num;
        }
    }
}
