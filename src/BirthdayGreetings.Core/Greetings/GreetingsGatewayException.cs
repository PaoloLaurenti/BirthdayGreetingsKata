using System;
using System.Runtime.Serialization;

namespace BirthdayGreetings.Core.Greetings
{
    public class GreetingsGatewayException : Exception
    {
        public GreetingsGatewayException()
        {
            
        }

        public GreetingsGatewayException(string message)
            : base(message)
        {
        }

        public GreetingsGatewayException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public GreetingsGatewayException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}