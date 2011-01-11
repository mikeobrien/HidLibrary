using System;

namespace LogitechGamepad
{
    public class DualActionMessage : IMessage
    {
        private const int DepressAction = 0x0800;

        private const int PressUp = 0x0000;
        private const int PressDown = 0x0400;
        private const int PressLeft = 0x0600;
        private const int PressRight = 0x0200;
                
        private const int Press1 = 0x1800;
        private const int Press2 = 0x2800;
        private const int Press3 = 0x4800;
        private const int Press4 = 0x8800;
        private const int Press5 = 0x0801;
        private const int Press6 = 0x0802;
        private const int Press7 = 0x0804;
        private const int Press8 = 0x0808;
        private const int Press9 = 0x0810;
        private const int Press10 = 0x0820;

        private readonly ushort _message;
        private readonly byte _buttonsPressed;

        public DualActionMessage(byte[] message)
        {
            if (message != null && message.Length == 8)
            {
                var value = new byte[2];
                Array.Copy(message, 4, value, 0, 2);
                _message = BitConverter.ToUInt16(value, 0);
            }
            else  throw new InvalidCastException("Cannot convert gamepad message to 32 bit integer.");
            _buttonsPressed = GetButtonsPressed(this);
        }

        public byte TotalPressed { get { return _buttonsPressed; } }
        public bool MultiplePressed { get { return (_buttonsPressed > 1); } }
        public bool Depress { get { return (DepressAction & _message) == 0; } }
        public bool UpPressed { get { return (PressUp & _message) != 0; } }
        public bool DownPressed { get { return (PressDown & _message) != 0; } }
        public bool LeftPressed { get { return (PressLeft & _message) != 0; } }
        public bool RightPressed { get { return (PressRight & _message) != 0; } }
        public bool Button1Pressed { get { return (Press1 & _message) != 0; } }
        public bool Button2Pressed { get { return (Press2 & _message) != 0; } }
        public bool Button3Pressed { get { return (Press3 & _message) != 0; } }
        public bool Button4Pressed { get { return (Press4 & _message) != 0; } }
        public bool Button5Pressed { get { return (Press5 & _message) != 0; } }
        public bool Button6Pressed { get { return (Press6 & _message) != 0; } }
        public bool Button7Pressed { get { return (Press7 & _message) != 0; } }
        public bool Button8Pressed { get { return (Press8 & _message) != 0; } }
        public bool Button9Pressed { get { return (Press9 & _message) != 0; } }
        public bool Button10Pressed { get { return (Press10 & _message) != 0; } }

        private static byte GetButtonsPressed(DualActionMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
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
