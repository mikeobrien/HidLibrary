using System;
using System.Linq;
using System.Windows.Forms;
using HidLibrary;

namespace TestHarness
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private HidDevice[] _deviceList;
        private HidDevice _selectedDevice;
        public delegate void ReadHandlerDelegate(HidReport report);
        
        private void Main_Load(object sender, EventArgs e)
        {
            RefreshDevices();
        }

        private void Read_Click(object sender, EventArgs e)
        {
            if (Devices.SelectedIndex <= -1) return;

            var device = _deviceList[Devices.SelectedIndex];

            device.OpenDevice();
            device.MonitorDeviceEvents = true;

            device.ReadReport(ReadProcess);
        }

        private void Scan_MouseDown(object sender, MouseEventArgs e)
        {
            var outData = _deviceList[Devices.SelectedIndex].CreateReport();
            if (outData == null) return;

            outData.ReportId = 0x4;
            outData.Data[0] = 0x4;

            _deviceList[Devices.SelectedIndex].WriteReport(outData);
        }

        private void Scan_MouseUp(object sender, MouseEventArgs e)
        {
            var outData = _deviceList[Devices.SelectedIndex].CreateReport();
            if (outData == null) return;

            outData.ReportId = 0x4;
            outData.Data[0] = 0x1;

            _deviceList[Devices.SelectedIndex].WriteReport(outData);
        }

        private void ErrorBeep_Click(object sender, EventArgs e)
        {
            var report = _deviceList[Devices.SelectedIndex].CreateReport();
            if (report == null) return;

            report.ReportId = 0x4;
            report.Data[0] = 0x20;

            _deviceList[Devices.SelectedIndex].WriteReport(report);
        }

        private void SuccessBeep_Click(object sender, EventArgs e)
        {
            var outData = _deviceList[Devices.SelectedIndex].CreateReport();
            if (outData == null) return;

            outData.ReportId = 0x4;
            outData.Data[0] = 0x40;

            _deviceList[Devices.SelectedIndex].WriteReport(outData, WriteResult);
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            RefreshDevices();
        }

        private void Devices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((_selectedDevice != null)) _selectedDevice.CloseDevice();
            _selectedDevice = _deviceList[Devices.SelectedIndex];
            _selectedDevice.OpenDevice();
            _selectedDevice.MonitorDeviceEvents = true;
            _selectedDevice.Inserted += Device_Inserted;
            _selectedDevice.Removed += Device_Removed;
        }

        private void Devices_DoubleClick(object sender, EventArgs e)
        {
            if (Devices.SelectedIndex > -1) MessageBox.Show("Connected: " + _deviceList[Devices.SelectedIndex].IsConnected);
        }

        private void Device_Inserted()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(Device_Inserted));
                return;
            }
            Output.AppendText("Connected\r\n");
        }

        private void Device_Removed()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(Device_Removed));
                return;
            }
            Output.AppendText("Disconnected\r\n");
        }

        private void ReadProcess(HidReport report)
        {
            BeginInvoke(new ReadHandlerDelegate(ReadHandler), new object[] { report });
        }

        private void ReadHandler(HidReport report)
        {
            Output.Clear();
            Output.AppendText(String.Join(" ", report.Data.Select(d => d.ToString("X2"))));

            _deviceList[Devices.SelectedIndex].ReadReport(ReadProcess);
        }

        private static void WriteResult(bool success)
        {
            MessageBox.Show("Write result: " + success);
        }

        private void RefreshDevices()
        {
            _deviceList = HidDevices.Enumerate().ToArray();
            //_deviceList = HidDevices.Enumerate(0x536, 0x207, 0x1c7).ToArray();
            Devices.DisplayMember = "Description";
            Devices.DataSource = _deviceList;
            if (_deviceList.Length > 0) _selectedDevice = _deviceList[0];
        }
    }
}
