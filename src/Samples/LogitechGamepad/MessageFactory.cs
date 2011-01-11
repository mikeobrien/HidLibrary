namespace LogitechGamepad
{
    internal class MessageFactory
    {
        internal const int PrecisionId = 0xC21A;
        internal const int DualActionId = 0xC216;

        public static IMessage CreateMessage(int productId, byte[] messageData)
        {
            switch (productId)
            {
                case PrecisionId: return new PrecisionMessage(messageData);
                case DualActionId: return new DualActionMessage(messageData);
            }
            return null;
        }
    }
}
