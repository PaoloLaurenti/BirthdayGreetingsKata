using System;
using System.Collections.Generic;
using System.Linq;
using BirthdayGreetings;
using Rhino.Mocks;

namespace BirthdayGreetingsTests
{
    public partial class BirthdayGreetingsTest
    {
        readonly List<string> _firstNames = new List<string> { "Pino", "Lucia", "Paolo", "Marco", "Elena" };
        readonly List<string> _lastNames = new List<string> { "Graziani", "Menti", "Piccioni", "Vanti", "Arcetti" };

        private void GivenNoPeopleToSendGreetingsTo()
        {
            _peopleRepository.Stub(pr => pr.GetAll()).Return(new List<PersonDto>());
        }

        private void GivenAllPeopleWithBirthdayDifferentThanToday()
        {
            _peopleRepository.Stub(pr => pr.GetAll()).Return(new List<PersonDto> { CreatePersonNotBornToday() });
        }

        private void GivenAllPeopleWithBirthdayEqualToToday()
        {
            _peopleRepository.Stub(pr => pr.GetAll()).Return(_allPeopleWithBirthdayEqualToToday);
        }

        private void GivenHalfPeopleWithBirthdayEqualToToday()
        {
            _peopleRepository.Stub(pr => pr.GetAll()).Return(_halfThePeopleWithBirthdayEqualToToday);
        }

        private void WhenItSendsGreetingsToPeopleBornOn(DateTime today)
        {
            _sut.SendGreetingsToPeopleBornInThis(today);
        }

        private void ThenNoGreetingsAreSent()
        {
            _greetingsDeliverService.AssertWasNotCalled(gds => gds.Deliver(null), mo => mo.IgnoreArguments());
        }

        private void ThenGreetingsHaveBeenSentToAllPeople()
        {
            var expectedGreetings = _allPeopleWithBirthdayEqualToToday
                                        .Select(p => CreateGreeting(p.FirstName, p.LastName, p.Email))
                                        .ToList();
            _greetingsDeliverService.AssertWasCalled(gds => gds.Deliver(Arg<IEnumerable<GreetingDto>>.Matches(en => CheckGreetingsAreEqual(expectedGreetings, en))));
        }

        private void ThenGreetingsHaveBeenSentToHalfThePeople()
        {
            var expectedGreetings = _peopleWithBirthdayEqualToToday
                                        .Select(p => CreateGreeting(p.FirstName, p.LastName, p.Email))
                                        .ToList();
            _greetingsDeliverService.AssertWasCalled(gds => gds.Deliver(Arg<IEnumerable<GreetingDto>>.Matches(en => CheckGreetingsAreEqual(expectedGreetings, en))));
        }

        private static GreetingDto CreateGreeting(string firstName, string lastName, string email)
        {
            var text = string.Format("Dear {0} {1}, happy birthday!", firstName, lastName);
            return new GreetingDto { Email = email, Text = text };
        }

        private PersonDto CreatePersonNotBornToday()
        {
            var now = DateTime.Now;
            var month = now.Month == 12 ? 1 : now.Month + 1;
            var day = now.Day == DateTime.DaysInMonth(1982, now.Month) ? 1 : now.Day + 1;
            return new PersonDto { FirstName = GetRandomFirstName(), LastName = GetRandomLastName(), Birthday = new DateTime(1982, month, day) };
        }

        private PersonDto CreatePersonBornToday()
        {
            var today = DateTime.Now;
            var rand = new Random(DateTime.Now.Millisecond);
            var firstName = GetRandomFirstName();
            var lastName = GetRandomLastName();
            return new PersonDto
            {
                FirstName = firstName,
                LastName = lastName,
                Birthday = new DateTime(rand.Next(1950, 2000), today.Month, today.Day),
                Email = string.Format("{0}{1}@gmail.com", firstName, lastName)
            };
        }

        private string GetRandomFirstName()
        {
            var rand = new Random(DateTime.Now.Millisecond);
            var nameIndex = rand.Next(0, _firstNames.Count - 1);
            return _firstNames[nameIndex];
        }

        private string GetRandomLastName()
        {
            var rand = new Random(DateTime.Now.Millisecond);
            var nameIndex = rand.Next(0, _lastNames.Count - 1);
            return _lastNames[nameIndex];
        }

        private static bool CheckGreetingsAreEqual(IEnumerable<GreetingDto> greetings1, IEnumerable<GreetingDto> greetings2)
        {
            var greetingDtoComparer = new GreetingDtoComparer();
            return greetings1.ToList().All(g1 => greetings2.Contains(g1, greetingDtoComparer));
        }

        private class GreetingDtoComparer : IEqualityComparer<GreetingDto>
        {
            public bool Equals(GreetingDto x, GreetingDto y)
            {
                return x.Email == y.Email && x.Text == y.Text;
            }

            public int GetHashCode(GreetingDto obj)
            {
                return string.Format("{0}-{1}", obj.Email, obj.Text).GetHashCode();
            }
        }
    }
}
