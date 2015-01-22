using System.Collections.Generic;

namespace BirthdayGreetings.Core.Greetings
{
    public interface IGreetingsGateway
    {
        void Deliver(IEnumerable<GreetingDto> greetings);
    }
}