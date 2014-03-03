using System;
using System.Linq;

namespace BirthdayGreetings
{
    public class BirthdayGreetingsEngine
    {
        private readonly PeopleRepository _peopleRepository;
        private readonly GreetingsDeliveryService _greetingsDeliverService;

        public BirthdayGreetingsEngine(PeopleRepository peopleRepository, GreetingsDeliveryService greetingsDeliverService)
        {
            _peopleRepository = peopleRepository;
            _greetingsDeliverService = greetingsDeliverService;
        }

        public void SendGreetingsToPeopleBornInThis(DateTime date)
        {
            var people = _peopleRepository.GetAll().ToList();
            if (!people.Any())
                return;

            var recipients = people.Where(p => p.Birthday.Month == date.Month && p.Birthday.Day == date.Day).ToList();
            if (!recipients.Any())
                return;

            var greetings = recipients.Select(CreateGreetingDto);
            _greetingsDeliverService.Deliver(greetings);
        }

        private static GreetingDto CreateGreetingDto(PersonDto p)
        {
            return new GreetingDto
                {
                    Address = p.Address,
                    Email = p.Email,
                    Text = string.Format("Dear {0} {1}, happy birthday!", p.FirstName, p.LastName)
                };
        }
    }
}