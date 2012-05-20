using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// This example was written by Thomas Hammer (www.thammer.net).

namespace GriffinPowerMate
{
    /// <summary>
    /// Provides data for PowerMate events.
    /// </summary>
    public class PowerMateEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the PowerMateEventArgs class.
        /// </summary>
        /// <param name="state"></param>
        public PowerMateEventArgs(PowerMateState state)
        {
            State = state;
        }

        /// <summary>
        /// Gets the current PowerMate state.
        /// </summary>
        public PowerMateState State { get; private set; }
    }
}
