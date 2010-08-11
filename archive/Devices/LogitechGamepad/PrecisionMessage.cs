using System;
using System.Collections.Generic;
using System.Text;

namespace LogitechGamepad
{
    internal class PrecisionMessage : IMessage
    {
        #region Constants

            private const int DEPRESS_MASK = 65343;

            private const int PRESS_UP_MASK = 16;
            private const int PRESS_DOWN_MASK = 32;
            private const int PRESS_LEFT_MASK = 4;
            private const int PRESS_RIGHT_MASK = 8;
                
            private const int PRESS_1_MASK = 256;
            private const int PRESS_2_MASK = 512;
            private const int PRESS_3_MASK = 1024;
            private const int PRESS_4_MASK = 2048;
            private const int PRESS_5_MASK = 4096;
            private const int PRESS_6_MASK = 8192;
            private const int PRESS_7_MASK = 16384;
            private const int PRESS_8_MASK = 32768;
            private const int PRESS_9_MASK = 1;
            private const int PRESS_10_MASK = 2;

        #endregion

        #region Private Fields

            private uint _Message;
            private byte _ButtonsPressed;

        #endregion

        #region Constructor

            public PrecisionMessage(uint Message)
            {
                _Message = Message;
            }

            public PrecisionMessage(byte[] Message)
            {
                if (Message != null && Message.Length == 4)
                {
                    Array.Reverse(Message);
                    _Message = BitConverter.ToUInt32(Message, 0);
                }
                else
                {
                    throw new InvalidCastException("Cannot convert gamepad message to 32 bit integer.");
                }
                _ButtonsPressed = GetButtonsPressed(this);
            }

        #endregion

        #region Properties

            public byte TotalPressed
            { get { return _ButtonsPressed; } }

            public bool MultiplePressed
            { get { return (_ButtonsPressed > 1); } }

            public bool Depress
            { get { return (DEPRESS_MASK & _Message) == 0; } }

            public bool UpPressed
            { get { return (PRESS_UP_MASK & _Message) != 0; } }

            public bool DownPressed
            { get { return (PRESS_DOWN_MASK & _Message) != 0; } }

            public bool LeftPressed
            { get { return (PRESS_LEFT_MASK & _Message) != 0; } }

            public bool RightPressed
            { get { return (PRESS_RIGHT_MASK & _Message) != 0; } }

            public bool Button1Pressed
            { get { return (PRESS_1_MASK & _Message) != 0; } }

            public bool Button2Pressed
            { get { return (PRESS_2_MASK & _Message) != 0; } }

            public bool Button3Pressed
            { get { return (PRESS_3_MASK & _Message) != 0; } }

            public bool Button4Pressed
            { get { return (PRESS_4_MASK & _Message) != 0; } }

            public bool Button5Pressed
            { get { return (PRESS_5_MASK & _Message) != 0; } }

            public bool Button6Pressed
            { get { return (PRESS_6_MASK & _Message) != 0; } }

            public bool Button7Pressed
            { get { return (PRESS_7_MASK & _Message) != 0; } }

            public bool Button8Pressed
            { get { return (PRESS_8_MASK & _Message) != 0; } }

            public bool Button9Pressed
            { get { return (PRESS_9_MASK & _Message) != 0; } }

            public bool Button10Pressed
            { get { return (PRESS_10_MASK & _Message) != 0; } }

        #endregion

        #region Private Methods

            private static byte GetButtonsPressed(PrecisionMessage Message)
            {
                byte ButtonsPressed = 0;

                if (Message.UpPressed == true) { ButtonsPressed++; }
                if (Message.DownPressed == true) { ButtonsPressed++; }
                if (Message.LeftPressed == true) { ButtonsPressed++; }
                if (Message.RightPressed == true) { ButtonsPressed++; }
                if (Message.Button1Pressed == true) { ButtonsPressed++; }
                if (Message.Button2Pressed == true) { ButtonsPressed++; }
                if (Message.Button3Pressed == true) { ButtonsPressed++; }
                if (Message.Button4Pressed == true) { ButtonsPressed++; }
                if (Message.Button5Pressed == true) { ButtonsPressed++; }
                if (Message.Button6Pressed == true) { ButtonsPressed++; }
                if (Message.Button7Pressed == true) { ButtonsPressed++; }
                if (Message.Button8Pressed == true) { ButtonsPressed++; }
                if (Message.Button9Pressed == true) { ButtonsPressed++; }
                if (Message.Button10Pressed == true) { ButtonsPressed++; }

                return ButtonsPressed;
            }

        #endregion
    }
}
