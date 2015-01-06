using System.Collections.Generic;

namespace BirthdayGreetings.Core.Greetings
{
    public interface IGreetingsChannelGateway
    {
        void Send(IEnumerable<GreetingDto> greetings);
    }
}