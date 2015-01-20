using System;
using System.Runtime.Serialization;

namespace BirthdayGreetings.Core.Greetings
{
    public class GreetingsChannelGatewayException : Exception
    {
        public GreetingsChannelGatewayException()
        {
            
        }

        public GreetingsChannelGatewayException(string message)
            : base(message)
        {
        }

        public GreetingsChannelGatewayException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public GreetingsChannelGatewayException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}