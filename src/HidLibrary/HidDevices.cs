using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using NLog;

namespace HidLibrary
{
	public class HidDevices
	{
		private static readonly Logger logger = LogManager.GetCurrentClassLogger();

		private static Guid _hidClassGuid = Guid.Empty;

		public static bool IsConnected(string devicePath)
		{
			return EnumerateHidDevicePaths().Any(x => x == devicePath);
		}

		public static HidDevice GetDevice(string devicePath)
		{
			return Enumerate(devicePath).FirstOrDefault();
		}

		public static IEnumerable<HidDevice> Enumerate()
		{
			//return EnumerateHidDevicePaths().Select(x => new HidDevice(x));
			return EnumerateHidDevices();
		}

		public static IEnumerable<HidDevice> Enumerate(string devicePath)
		{
			return EnumerateHidDevicePaths().Where(x => x == devicePath).Select(x => new HidDevice(x));
		}

		public static IEnumerable<HidDevice> Enumerate(int vendorId, params int[] productIds)
		{
			return EnumerateHidDevicePaths().Select(x => new HidDevice(x)).Where(x => x.Attributes.VendorId == vendorId &&
																				  productIds.Contains(x.Attributes.ProductId));
		}

		public static IEnumerable<HidDevice> Enumerate(int vendorId)
		{
			return EnumerateHidDevicePaths().Select(x => new HidDevice(x)).Where(x => x.Attributes.VendorId == vendorId);
		}

		private static IEnumerable<string> EnumerateHidDevicePaths()
		{
			var hidClass = HidClassGuid;
			var deviceInfoSet = NativeMethods.SetupDiGetClassDevs(ref hidClass, null, 0, NativeMethods.DIGCF_PRESENT | NativeMethods.DIGCF_DEVICEINTERFACE);

			if (deviceInfoSet.ToInt32() != NativeMethods.INVALID_HANDLE_VALUE)
			{
				var devices = new List<string>();
				var deviceInfoData = CreateDeviceInfoData();
				var deviceIndex = 0;

				while (NativeMethods.SetupDiEnumDeviceInfo(deviceInfoSet, deviceIndex, ref deviceInfoData))
				{
					deviceIndex += 1;

					var deviceInterfaceData = new NativeMethods.SP_DEVICE_INTERFACE_DATA();
					deviceInterfaceData.cbSize = Marshal.SizeOf(deviceInterfaceData);
					var deviceInterfaceIndex = 0;

					while (NativeMethods.SetupDiEnumDeviceInterfaces(deviceInfoSet, 0, ref hidClass, deviceInterfaceIndex, ref deviceInterfaceData))
					{
						deviceInterfaceIndex++;
						var devicePath = GetDevicePath(deviceInfoSet, deviceInterfaceData);

						if (devices.Any(x => x == devicePath)) continue;

						yield return devicePath;
					}
				}
				NativeMethods.SetupDiDestroyDeviceInfoList(deviceInfoSet);
			}
		}

		private static IEnumerable<HidDevice> EnumerateHidDevices()
		{
			var hidClass = HidClassGuid;
			var deviceInfoSet = NativeMethods.SetupDiGetClassDevs(ref hidClass, null, 0, NativeMethods.DIGCF_PRESENT | NativeMethods.DIGCF_DEVICEINTERFACE);

			if (deviceInfoSet.ToInt32() != NativeMethods.INVALID_HANDLE_VALUE)
			{
				var devices = new List<HidDevice>();
				var deviceInfoData = CreateDeviceInfoData();
				var deviceIndex = 0;

				while (NativeMethods.SetupDiEnumDeviceInfo(deviceInfoSet, deviceIndex, ref deviceInfoData))
				{
					deviceIndex += 1;

					var deviceInterfaceData = new NativeMethods.SP_DEVICE_INTERFACE_DATA();
					deviceInterfaceData.cbSize = Marshal.SizeOf(deviceInterfaceData);
					var deviceInterfaceIndex = 0;

					while (NativeMethods.SetupDiEnumDeviceInterfaces(deviceInfoSet, 0, ref hidClass, deviceInterfaceIndex, ref deviceInterfaceData))
					{
						deviceInterfaceIndex++;
						var devicePath = GetDevicePath(deviceInfoSet, deviceInterfaceData);

						var tempDevice = new HidDevice(devicePath, GetDeviceBusReportedDescription(deviceInfoSet, ref deviceInfoData));
						if (devices.Any(x => x.DevicePath == devicePath))
							continue;

						yield return tempDevice;
					}
				}
				NativeMethods.SetupDiDestroyDeviceInfoList(deviceInfoSet);
			}
		}

		private static NativeMethods.SP_DEVINFO_DATA CreateDeviceInfoData()
		{
			var deviceInfoData = new NativeMethods.SP_DEVINFO_DATA();

			deviceInfoData.cbSize = Marshal.SizeOf(deviceInfoData);
			deviceInfoData.DevInst = 0;
			deviceInfoData.ClassGuid = Guid.Empty;
			deviceInfoData.Reserved = IntPtr.Zero;

			return deviceInfoData;
		}

		private static string GetDevicePath(IntPtr deviceInfoSet, NativeMethods.SP_DEVICE_INTERFACE_DATA deviceInterfaceData)
		{
			var bufferSize = 0;
			var interfaceDetail = new NativeMethods.SP_DEVICE_INTERFACE_DETAIL_DATA { Size = IntPtr.Size == 4 ? 4 + Marshal.SystemDefaultCharSize : 8 };

			NativeMethods.SetupDiGetDeviceInterfaceDetailBuffer(deviceInfoSet, ref deviceInterfaceData, IntPtr.Zero, 0, ref bufferSize, IntPtr.Zero);

			return NativeMethods.SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref deviceInterfaceData, ref interfaceDetail, bufferSize, ref bufferSize, IntPtr.Zero) ?
				interfaceDetail.DevicePath : null;
		}

		private static string GetAsUTF8String(byte[] buffer)
		{
			var tempString = Encoding.UTF8.GetString(buffer);
			return tempString.Remove(tempString.IndexOf((char)0));
		}

		private static string GetAsUTF16String(byte[] buffer)
		{
			var tempString = Encoding.Unicode.GetString(buffer);
			return tempString.Remove(tempString.IndexOf((char)0));
		}

		private static string GetDeviceBusReportedDescription(IntPtr deviceInfoSet, ref NativeMethods.SP_DEVINFO_DATA devinfoData)
		{
			byte[] descriptionBuffer = new byte[1024];

			int requiredSize = 0;
			int type = 0;

			// This function is available everywhere
			NativeMethods.SetupDiGetDeviceRegistryProperty(deviceInfoSet,
															ref devinfoData,
															(int)NativeMethods.SPDRP.DEVICEDESC,
															ref type,
															descriptionBuffer,
															descriptionBuffer.Length,
															ref requiredSize);

			string deviceDescription = GetAsUTF8String(descriptionBuffer);

			string reportedDeviceDescription = string.Empty;

			// TODO: Check windows version
			try
			{
				descriptionBuffer = new byte[1024];
				ulong propertyType = 0;
				requiredSize = 0;

				// This function is only available from Vista onwards
				bool _continue = NativeMethods.SetupDiGetDeviceProperty(deviceInfoSet,
																		ref devinfoData,
																		NativeMethods.DEVPKEY_Device_BusReportedDeviceDesc,
																		ref propertyType,
																		descriptionBuffer,
																		descriptionBuffer.Length,
																		ref requiredSize,
																		0);

				if (_continue)
				{
					reportedDeviceDescription = GetAsUTF16String(descriptionBuffer);
				}
				else
				{
					int error = Marshal.GetLastWin32Error();
					logger.Fatal("BusReported: {0}, propertyType={1}, requiredSize={2}, error={4}", reportedDeviceDescription,
								 propertyType, requiredSize, error);
				}
			}
			catch (Exception e) // TODO: Catch ONLY native method not found
			{
				logger.InfoException("Likely we're running on XP", e);
			}

			if (reportedDeviceDescription != string.Empty)
				return reportedDeviceDescription;

			return deviceDescription;
		}

		private static Guid HidClassGuid
		{
			get
			{
				if (_hidClassGuid.Equals(Guid.Empty)) NativeMethods.HidD_GetHidGuid(ref _hidClassGuid);
				return _hidClassGuid;
			}
		}
	}
}