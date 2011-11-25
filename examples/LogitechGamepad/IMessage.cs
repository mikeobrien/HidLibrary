namespace LogitechGamepad
{
    interface IMessage
    {
        byte TotalPressed { get; }
        bool MultiplePressed { get; }
        bool Depress { get; }
        bool UpPressed { get; }
        bool DownPressed { get; }
        bool LeftPressed { get; }
        bool RightPressed { get; }
        bool Button1Pressed { get; }
        bool Button2Pressed { get; }
        bool Button3Pressed { get; }
        bool Button4Pressed { get; }
        bool Button5Pressed { get; }
        bool Button6Pressed { get; }
        bool Button7Pressed { get; }
        bool Button8Pressed { get; }
        bool Button9Pressed { get; }
        bool Button10Pressed { get; }
    }
}
