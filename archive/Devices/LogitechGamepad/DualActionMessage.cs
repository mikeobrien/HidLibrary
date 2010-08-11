using System;
using System.Collections.Generic;
using System.Text;

namespace LogitechGamepad
{
    internal class DualActionMessage : IMessage
    {
        #region Constants

            private const int DEPRESS = 0x0800;

            private const int PRESS_UP = 0x0000;
            private const int PRESS_DOWN = 0x0400;
            private const int PRESS_LEFT = 0x0600;
            private const int PRESS_RIGHT = 0x0200;
                
            private const int PRESS_1 = 0x1800;
            private const int PRESS_2 = 0x2800;
            private const int PRESS_3 = 0x4800;
            private const int PRESS_4 = 0x8800;
            private const int PRESS_5 = 0x0801;
            private const int PRESS_6 = 0x0802;
            private const int PRESS_7 = 0x0804;
            private const int PRESS_8 = 0x0808;
            private const int PRESS_9 = 0x0810;
            private const int PRESS_10 = 0x0820;

        #endregion

        #region Private Fields

            private ushort _Message;
            private byte _ButtonsPressed;

        #endregion

        #region Constructor

            public DualActionMessage(byte[] Message)
            {
                if (Message != null && Message.Length == 8)
                {
                    byte[] value = new byte[2];
                    Array.Copy(Message, 4, value, 0, 2);
                    _Message = BitConverter.ToUInt16(value, 0);
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
            { get { return (DEPRESS & _Message) == 0; } }

            public bool UpPressed
            { get { return (PRESS_UP & _Message) != 0; } }

            public bool DownPressed
            { get { return (PRESS_DOWN & _Message) != 0; } }

            public bool LeftPressed
            { get { return (PRESS_LEFT & _Message) != 0; } }

            public bool RightPressed
            { get { return (PRESS_RIGHT & _Message) != 0; } }

            public bool Button1Pressed
            { get { return (PRESS_1 & _Message) != 0; } }

            public bool Button2Pressed
            { get { return (PRESS_2 & _Message) != 0; } }

            public bool Button3Pressed
            { get { return (PRESS_3 & _Message) != 0; } }

            public bool Button4Pressed
            { get { return (PRESS_4 & _Message) != 0; } }

            public bool Button5Pressed
            { get { return (PRESS_5 & _Message) != 0; } }

            public bool Button6Pressed
            { get { return (PRESS_6 & _Message) != 0; } }

            public bool Button7Pressed
            { get { return (PRESS_7 & _Message) != 0; } }

            public bool Button8Pressed
            { get { return (PRESS_8 & _Message) != 0; } }

            public bool Button9Pressed
            { get { return (PRESS_9 & _Message) != 0; } }

            public bool Button10Pressed
            { get { return (PRESS_10 & _Message) != 0; } }

        #endregion

        #region Private Methods

            private static byte GetButtonsPressed(DualActionMessage Message)
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
