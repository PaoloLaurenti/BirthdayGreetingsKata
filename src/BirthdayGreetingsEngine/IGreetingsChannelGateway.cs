using System.Collections;
using System.Collections.Generic;

namespace BirthdayGreetingsEngine
{
    public interface IGreetingsChannelGateway
    {
        void Send(IEnumerable<GreetingDto> greetings);
    }
}