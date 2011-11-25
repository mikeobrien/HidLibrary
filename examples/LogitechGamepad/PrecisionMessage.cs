using System;

namespace LogitechGamepad
{
    public class PrecisionMessage : IMessage
    {
        private const int DepressMask = 65343;

        private const int PressUpMask = 16;
        private const int PressDownMask = 32;
        private const int PressLeftMask = 4;
        private const int PressRightMask = 8;
                
        private const int Press1Mask = 256;
        private const int Press2Mask = 512;
        private const int Press3Mask = 1024;
        private const int Press4Mask = 2048;
        private const int Press5Mask = 4096;
        private const int Press6Mask = 8192;
        private const int Press7Mask = 16384;
        private const int Press8Mask = 32768;
        private const int Press9Mask = 1;
        private const int Press10Mask = 2;

        private readonly uint _message;
        private readonly byte _buttonsPressed;

        public PrecisionMessage(uint message)
        {
            _message = message;
        }

        public PrecisionMessage(byte[] message)
        {
            if (message != null && message.Length == 4)
            {
                Array.Reverse(message);
                _message = BitConverter.ToUInt32(message, 0);
            }
            else throw new InvalidCastException("Cannot convert gamepad message to 32 bit integer.");
            _buttonsPressed = GetButtonsPressed(this);
        }

        public byte TotalPressed { get { return _buttonsPressed; } }
        public bool MultiplePressed { get { return (_buttonsPressed > 1); } }
        public bool Depress { get { return (DepressMask & _message) == 0; } }
        public bool UpPressed { get { return (PressUpMask & _message) != 0; } }
        public bool DownPressed { get { return (PressDownMask & _message) != 0; } }
        public bool LeftPressed { get { return (PressLeftMask & _message) != 0; } }
        public bool RightPressed { get { return (PressRightMask & _message) != 0; } }
        public bool Button1Pressed { get { return (Press1Mask & _message) != 0; } }
        public bool Button2Pressed { get { return (Press2Mask & _message) != 0; } }
        public bool Button3Pressed { get { return (Press3Mask & _message) != 0; } }
        public bool Button4Pressed { get { return (Press4Mask & _message) != 0; } }
        public bool Button5Pressed { get { return (Press5Mask & _message) != 0; } }
        public bool Button6Pressed { get { return (Press6Mask & _message) != 0; } }
        public bool Button7Pressed { get { return (Press7Mask & _message) != 0; } }
        public bool Button8Pressed { get { return (Press8Mask & _message) != 0; } }
        public bool Button9Pressed { get { return (Press9Mask & _message) != 0; } }
        public bool Button10Pressed { get { return (Press10Mask & _message) != 0; } }

        private static byte GetButtonsPressed(IMessage message)
        {
            byte buttonsPressed = 0;

            if (message.UpPressed) { buttonsPressed++; }
            if (message.DownPressed) { buttonsPressed++; }
            if (message.LeftPressed) { buttonsPressed++; }
            if (message.RightPressed) { buttonsPressed++; }
            if (message.Button1Pressed) { buttonsPressed++; }
            if (message.Button2Pressed) { buttonsPressed++; }
            if (message.Button3Pressed) { buttonsPressed++; }
            if (message.Button4Pressed) { buttonsPressed++; }
            if (message.Button5Pressed) { buttonsPressed++; }
            if (message.Button6Pressed) { buttonsPressed++; }
            if (message.Button7Pressed) { buttonsPressed++; }
            if (message.Button8Pressed) { buttonsPressed++; }
            if (message.Button9Pressed) { buttonsPressed++; }
            if (message.Button10Pressed) { buttonsPressed++; }

            return buttonsPressed;
        }
    }
}
