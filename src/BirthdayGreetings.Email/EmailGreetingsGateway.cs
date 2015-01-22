using System.Collections.Generic;
using BirthdayGreetings.Core.Greetings;

namespace BirthdayGreetings.Email
{
    public class EmailGreetingsGateway : IGreetingsGateway
    {
        public EmailGreetingsGateway(IEmailChannel emailChannel)
        {
            
        }

        public void Deliver(IEnumerable<GreetingDto> greetings)
        {
        }
    }
}