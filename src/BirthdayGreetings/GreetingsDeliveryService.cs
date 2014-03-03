using System.Collections.Generic;

namespace BirthdayGreetings
{
    public interface GreetingsDeliveryService
    {
        void Deliver(IEnumerable<GreetingDto> greetings);
    }
}