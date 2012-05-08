using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// This example was written by Thomas Hammer (www.thammer.net).

namespace GriffinPowerMate
{
    /// <summary>
    /// State for the PowerMate button - Up or Down
    /// </summary>
    public enum PowerMateButtonState
    {
        Up=0, Down=1
    }

    /// <summary>
    /// Holds the state for the PowerMate: button state, knob displacement,
    /// led brightness and pulse settings.
    /// 
    /// The struct is immutable.
    /// </summary>
    public struct PowerMateState
    {
        private PowerMateButtonState buttonState; // byte 0
        private int knobDisplacement; // byte 1
        private int ledBrightness; // byte 3
        private bool ledPulseEnabled; // byte 4, bit 1 set
        private bool ledPulseDuringSleepEnabled; // byte 4, bit 3 set
        private int ledPulseSpeed; // byte 5
        private bool valid;

        /// <summary>
        /// Initializes a new instance (valid) of the PowerMate class.
        /// </summary>
        /// <param name="buttonState"></param>
        /// <param name="knobDisplacement"></param>
        /// <param name="ledBrightness"></param>
        /// <param name="ledPulseEnabled"></param>
        /// <param name="ledPulseDuringSleepEnabled"></param>
        /// <param name="ledPulseSpeed"></param>
        public PowerMateState(PowerMateButtonState buttonState,
                              int knobDisplacement,
                              int ledBrightness,
                              bool ledPulseEnabled,
                              bool ledPulseDuringSleepEnabled,
                              int ledPulseSpeed)
        {
            this.buttonState = buttonState;
            this.knobDisplacement = knobDisplacement;
            this.ledBrightness = ledBrightness;
            this.ledPulseEnabled = ledPulseEnabled;
            this.ledPulseDuringSleepEnabled = ledPulseDuringSleepEnabled;
            this.ledPulseSpeed = ledPulseSpeed;
            this.valid = true;
        }

        /// <summary>
        /// Gets the PowerMate's button state.
        /// </summary>
        public PowerMateButtonState ButtonState
        {
            get { return buttonState; }
        }

        /// <summary>
        /// Gets the PowerMate's knob displacement. The valid range is [-127, +128],
        /// although most values are single digit values close to zero. A positive
        /// value indicates a clockwise rotation.
        /// </summary>
        public int KnobDisplacement
        {
            get { return knobDisplacement; }
        }

        /// <summary>
        /// Gets the PowerMate's LED brightness. The valid range is [0, 255],
        /// with 0 being completely off and 255 being full brightness.
        /// </summary>
        public int LedBrightness
        {
            get { return ledBrightness; }
        }

        /// <summary>
        /// Gets a value indicating if the PowerMate's LED is pulsing or constant
        /// brightness. Returns True if the LED is pulsing, and False if the LED
        /// has constant brightness.
        /// </summary>
        public bool LedPulseEnabled
        {
            get { return ledPulseEnabled; }
        }

        /// <summary>
        /// Gets a value indicating if the PowerMate's LED will pulse when the
        /// computer is in sleep mode.
        /// </summary>
        public bool LedPulseDuringSleepEnabled
        {
            get { return ledPulseDuringSleepEnabled; }
        }

        /// <summary>
        /// Gets the PowerMate's LED pulse speed. The range is  [-255, 255], and the
        /// useful range seems to be approximately [-32, 64]. A value of 0 means default 
        /// pulse speed. A negative value is slower than the default, a positive value 
        /// is faster.
        /// </summary>
        public int LedPulseSpeed
        {
            get { return ledPulseSpeed; }
        }

        /// <summary>
        /// Gets a value indicating in the PowerMateState instance is valid.
        /// </summary>
        public bool IsValid
        {
            get { return valid; }
        }
    }
}
