using System.Collections.Generic;

namespace BirthdayGreetings.Core
{
    public interface IGreetingsChannelGateway
    {
        void Send(IEnumerable<GreetingDto> greetings);
    }
}