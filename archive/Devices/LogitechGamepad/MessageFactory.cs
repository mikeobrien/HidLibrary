using System;
using System.Collections.Generic;
using System.Text;

namespace LogitechGamepad
{
    internal class MessageFactory
    {
        public static IMessage CreateMessage(int productId, byte[] messageData)
        {
            switch (productId)
            {
                case Device.PRECISION_ID:
                    return new PrecisionMessage(messageData);
                case Device.DUAL_ACTION_ID:
                    return new DualActionMessage(messageData);
            }
            return null;
        }
    }
}
