using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// This example was written by Thomas Hammer (www.thammer.net).

namespace GriffinPowerMate
{
    /// <summary>
    /// Checks to see if a Griffin PowerMate device is connected. If it is, 
    /// read rotation and button press events and control the LED brightness
    /// and pulse speed.
    /// 
    /// This code has not been tested with multiple PowerMates connected at the same time.
    /// </summary>
    class Program
    {
        static PowerMateManager powerMateManager = new PowerMateManager();
        
        static void Main(string[] args)
        {
            if (powerMateManager.OpenDevice())
            {
                powerMateManager.DeviceAttached +=new EventHandler(powerMateManager_DeviceAttached);
                powerMateManager.DeviceRemoved +=new EventHandler(powerMateManager_DeviceRemoved);
                powerMateManager.ButtonDown += new EventHandler<PowerMateEventArgs>(powerMateManager_ButtonDown);
                powerMateManager.ButtonUp += new EventHandler<PowerMateEventArgs>(powerMateManager_ButtonUp);
                powerMateManager.KnobDisplacement += new EventHandler<PowerMateEventArgs>(powerMateManager_KnobDisplacement);

                Console.WriteLine("PowerMate found, press any key to exit.");
                Console.ReadKey();
                powerMateManager.CloseDevice();
            }
            else
            {
                Console.WriteLine("Could not find a PowerMate.");
                Console.ReadKey();
            }
        }

        static void powerMateManager_DeviceRemoved(object sender, EventArgs e)
        {
            Console.WriteLine("PowerMate removed.");
        }

        static void powerMateManager_DeviceAttached(object sender, EventArgs e)
        {
            Console.WriteLine("PowerMate attached.");
        }

        static void powerMateManager_KnobDisplacement(object sender, PowerMateEventArgs e)
        {
            Console.WriteLine("PowerMate knob displacement event");
            Console.WriteLine("PowerMate state button: {0}", (e.State.ButtonState == PowerMateButtonState.Up ? "Up" : "Down"));
            Console.WriteLine("PowerMate state knob displacement: {0}", e.State.KnobDisplacement);

            //Console.WriteLine("PowerMate led brightness: {0}", e.State.LedBrightness);
            //Console.WriteLine("PowerMate led pulse enabled: {0}", e.State.LedPulseEnabled ? "Yes" : "No");
            //Console.WriteLine("PowerMate led pulse during sleep enabled: {0}", e.State.LedPulseDuringSleepEnabled ? "Yes" : "No");
            //Console.WriteLine("PowerMate led pulse speed: {0}", e.State.LedPulseSpeed);    
        }

        static void powerMateManager_ButtonUp(object sender, PowerMateEventArgs e)
        {
            Console.WriteLine("PowerMate button up event");
            Console.WriteLine("PowerMate state button: {0}", (e.State.ButtonState == PowerMateButtonState.Up ? "Up" : "Down"));
            Console.WriteLine("PowerMate state knob displacement: {0}", e.State.KnobDisplacement);
        }

        static void powerMateManager_ButtonDown(object sender, PowerMateEventArgs e)
        {
            Console.WriteLine("PowerMate button down event");
            Console.WriteLine("PowerMate state button: {0}", (e.State.ButtonState == PowerMateButtonState.Up ? "Up" : "Down"));
            Console.WriteLine("PowerMate state knob displacement: {0}", e.State.KnobDisplacement);
        }
    }
}
