using NUnit.Framework;
using HidLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Should;

namespace Tests
{
    [TestFixture]
    public class HidEnumeratorTests
    {
        private HidEnumerator enumerator;
        private string devicePath;

        [SetUp]
        public void beforeEach()
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

        [Test]
        public void CanConstruct()
        {
            enumerator.ShouldBeType(typeof(HidEnumerator));
        }

        [Test]
        public void WrapsIsConnected()
        {
            bool enumIsConnected = enumerator.IsConnected(devicePath);
            bool hidIsConnected = HidDevices.IsConnected(devicePath);
            enumIsConnected.ShouldEqual(hidIsConnected);
        }

        [Test]
        public void WrapsGetDevice()
        {
            IHidDevice enumDevice = enumerator.GetDevice(devicePath);
            IHidDevice hidDevice = HidDevices.GetDevice(devicePath);
            enumDevice.DevicePath.ShouldEqual(hidDevice.DevicePath);
        }

        [Test]
        public void WrapsEnumerateDefault()
        {
            IEnumerable<IHidDevice> enumDevices = enumerator.Enumerate();
            IEnumerable<IHidDevice> hidDevices = HidDevices.Enumerate().
                Select(d => d as IHidDevice);

            
            allDevicesTheSame(enumDevices, hidDevices).ShouldBeTrue();
        }

        [Test]
        public void WrapsEnumerateDevicePath()
        {
            IEnumerable<IHidDevice> enumDevices =
                enumerator.Enumerate(devicePath);
            IEnumerable<IHidDevice> hidDevices =
                HidDevices.Enumerate(devicePath).
                    Select(d => d as IHidDevice);


            allDevicesTheSame(enumDevices, hidDevices).ShouldBeTrue();
        }

        [Test]
        public void WrapsEnumerateVendorId()
        {
            int vid = getVid();

            IEnumerable<IHidDevice> enumDevices =
                enumerator.Enumerate(vid);
            IEnumerable<IHidDevice> hidDevices =
                HidDevices.Enumerate(vid).
                    Select(d => d as IHidDevice);


            allDevicesTheSame(enumDevices, hidDevices).ShouldBeTrue();
        }

        [Test]
        public void WrapsEnumerateVendorIdProductId()
        {
            int vid = getVid();
            int pid = getPid();

            IEnumerable<IHidDevice> enumDevices =
                enumerator.Enumerate(vid, pid);
            IEnumerable<IHidDevice> hidDevices =
                HidDevices.Enumerate(vid, pid).
                    Select(d => d as IHidDevice);


            allDevicesTheSame(enumDevices, hidDevices).ShouldBeTrue();
        }


        private bool allDevicesTheSame(IEnumerable<IHidDevice> a,
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

        private int getVid()
        {
            return getNumberFromRegex("vid_([0-9a-f]{4})");
        }

        private int getPid()
        {
            return getNumberFromRegex("pid_([0-9a-f]{3,4})");
        }

        private int getNumberFromRegex(string pattern)
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
