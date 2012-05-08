using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GriffinPowerMate;

// This example was written by Thomas Hammer (www.thammer.net).

namespace GriffinPowerMateWindowsForms
{
    /// <summary>
    /// Displays a window indicating the current state of the PowerMate and 
    /// allows the user to control the LED.
    /// 
    /// The window will indicate:
    /// - if a PowerMate device is connected
    /// - the PowerMate button state
    /// - the last PowerMate rotation direction and magnitude
    /// - the PowerMate LED brightness (constant, when not pulsing)
    /// - if pulsing of the PowerMate is enabled
    /// - the LED pulse speed
    /// 
    /// The user can control the PowerMate LED brightness
    /// by rotating the PowerMate when the LED is not pulsing.
    /// 
    /// LED pulsing is started and stopped by clicking the PowerMate button.
    /// 
    /// The user can control the PowerMate LED pulse speed
    /// by rotating the PowerMate when the LED is pulsing.
    /// </summary>
    public partial class PowerMateViewer : Form
    {
        public PowerMateViewer()
        {
            InitializeComponent();

            connectionTimer.Interval = 500;
            connectionTimer.Tick += new EventHandler(connectionTimer_Tick);
            connectionTimer.Start();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                powerMateManager.CloseDevice();
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void connectionTimer_Tick(object sender, EventArgs e)
        {
            bool connected = ConnectToPowerMate();
            if (connected)
            {
                connectionTimer.Stop();
                statusLabel.Text = "Connected";
            }
        }

        private bool ConnectToPowerMate()
        {
            if (powerMateManager.OpenDevice())
            {
                powerMateManager.DeviceAttached += new EventHandler(powerMateManager_DeviceAttached);
                powerMateManager.DeviceRemoved += new EventHandler(powerMateManager_DeviceRemoved);
                powerMateManager.ButtonDown += new EventHandler<PowerMateEventArgs>(powerMateManager_ButtonDown);
                powerMateManager.ButtonUp += new EventHandler<PowerMateEventArgs>(powerMateManager_ButtonUp);
                powerMateManager.KnobDisplacement += new EventHandler<PowerMateEventArgs>(powerMateManager_KnobDisplacement);

                System.Diagnostics.Debug.WriteLine("PowerMate found");
                return true;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Could not find a PowerMate.");
                return false;
            }
        }

        private void powerMateManager_DeviceRemoved(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {   // Event can be received on a separate thread, so we need to push the message
                // back on the GUI thread before we execute.
                BeginInvoke(new Action<object, EventArgs>(powerMateManager_DeviceRemoved), sender, e);
                return;
            }

            statusLabel.Text = "Disconnected";
            System.Diagnostics.Debug.WriteLine("PowerMate removed.");
        }

        private void powerMateManager_DeviceAttached(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {   // Event can be received on a separate thread, so we need to push the message
                // back on the GUI thread before we execute.
                BeginInvoke(new Action<object, EventArgs>(powerMateManager_DeviceAttached), sender, e);
                return;
            }

            statusLabel.Text = "Connected";
            System.Diagnostics.Debug.WriteLine("PowerMate attached.");
        }

        private void powerMateManager_KnobDisplacement(object sender, PowerMateEventArgs e)
        {
            if (InvokeRequired)
            {   // Event can be received on a separate thread, so we need to push the message
                // back on the GUI thread before we execute.
                BeginInvoke(new Action<object, PowerMateEventArgs>(powerMateManager_KnobDisplacement), sender, e);
                return;
            }

            UpdateGUIFromState(e.State);

            if (!e.State.LedPulseEnabled)
            {   // Set LED brightness
                int range = rotationProgressBar.Maximum - rotationProgressBar.Minimum + 1;
                int value = (rotationProgressBar.Value + e.State.KnobDisplacement - rotationProgressBar.Minimum) % range;
                if (value < 0)
                    value = rotationProgressBar.Maximum + value + 1;
                else
                    value = rotationProgressBar.Minimum + value;
                rotationProgressBar.Value = value;

                int brightness = value * 255 / range;
                powerMateManager.SetLedBrightness(brightness);
                ledBrightnessLabel.Text = brightness.ToString();

            }
            else
            {   // Set pulse speed
                int speed = e.State.LedPulseSpeed + e.State.KnobDisplacement;
                speed = speed < -32 ? -32 : speed > 32 ? 32 : speed;
                powerMateManager.SetLedPulseSpeed(speed);
                ledPulseSpeedLabel.Text = speed.ToString();
            }

            System.Diagnostics.Debug.WriteLine("PowerMate knob displacement event");
            System.Diagnostics.Debug.WriteLine(String.Format("PowerMate state button: {0}", (e.State.ButtonState == PowerMateButtonState.Up ? "Up" : "Down")));
            System.Diagnostics.Debug.WriteLine(String.Format("PowerMate state knob displacement: {0}", e.State.KnobDisplacement));

            //System.Diagnostics.Debug.WriteLine("PowerMate led brightness: {0}", e.State.LedBrightness);
            //System.Diagnostics.Debug.WriteLine("PowerMate led pulse enabled: {0}", e.State.LedPulseEnabled ? "Yes" : "No");
            //System.Diagnostics.Debug.WriteLine("PowerMate led pulse during sleep enabled: {0}", e.State.LedPulseDuringSleepEnabled ? "Yes" : "No");
            //System.Diagnostics.Debug.WriteLine("PowerMate led pulse speed: {0}", e.State.LedPulseSpeed);    
        }

        private void powerMateManager_ButtonUp(object sender, PowerMateEventArgs e)
        {
            if (InvokeRequired)
            {   // Event can be received on a separate thread, so we need to push the message
                // back on the GUI thread before we execute.
                BeginInvoke(new Action<object, PowerMateEventArgs>(powerMateManager_ButtonUp), sender, e);
                return;
            }

            UpdateGUIFromState(e.State);
            ledPulseLabel.Text = !e.State.LedPulseEnabled ? "On" : "Off";

            powerMateManager.SetLedPulseEnabled(!e.State.LedPulseEnabled);

            System.Diagnostics.Debug.WriteLine("PowerMate button up event");
            System.Diagnostics.Debug.WriteLine(String.Format("PowerMate state button: {0}", (e.State.ButtonState == PowerMateButtonState.Up ? "Up" : "Down")));
            System.Diagnostics.Debug.WriteLine(String.Format("PowerMate state knob displacement: {0}", e.State.KnobDisplacement));
        }

        private void powerMateManager_ButtonDown(object sender, PowerMateEventArgs e)
        {
            if (InvokeRequired)
            {   // Event can be received on a separate thread, so we need to push the message
                // back on the GUI thread before we execute.
                BeginInvoke(new Action<object, PowerMateEventArgs>(powerMateManager_ButtonDown), sender, e);
                return;
            }

            UpdateGUIFromState(e.State);

            System.Diagnostics.Debug.WriteLine("PowerMate button down event");
            System.Diagnostics.Debug.WriteLine(String.Format("PowerMate state button: {0}", (e.State.ButtonState == PowerMateButtonState.Up ? "Up" : "Down")));
            System.Diagnostics.Debug.WriteLine(String.Format("PowerMate state knob displacement: {0}", e.State.KnobDisplacement));
        }

        private void UpdateGUIFromState(PowerMateState state)
        {
            buttonLabel.Text = state.ButtonState == PowerMateButtonState.Up ? "Up" : "Down";
            rotationLabel.Text = state.KnobDisplacement.ToString();
            ledBrightnessLabel.Text = state.LedBrightness.ToString();
            ledPulseLabel.Text = state.LedPulseEnabled ? "On" : "Off";
            ledPulseSpeedLabel.Text = state.LedPulseSpeed.ToString();
        }

        private PowerMateManager powerMateManager = new PowerMateManager();
        private Timer connectionTimer = new Timer();
    }
}
