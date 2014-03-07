using System;
using System.Collections.Generic;
using BirthdayGreetings;
using NUnit.Framework;
using Rhino.Mocks;

namespace BirthdayGreetingsTests
{
    [TestFixture]
    public partial class BirthdayGreetingsTest
    {
        private GreetingsDeliveryService _greetingsDeliverService;
        private PeopleRepository _peopleRepository;
        private BirthdayGreetingsEngine _sut;
        private List<PersonDto> _peopleWithBirthdayEqualToToday;
        private List<PersonDto> _peopleWithBirthdayNotEqualToToday;
        private List<PersonDto> _allPeopleWithBirthdayEqualToToday;
        private List<PersonDto> _halfThePeopleWithBirthdayEqualToToday;

        [SetUp]
        public void Init()
        {
            _greetingsDeliverService = MockRepository.GenerateMock<GreetingsDeliveryService>();
            _peopleRepository = MockRepository.GenerateStub<PeopleRepository>();
            _sut = new BirthdayGreetingsEngine(_peopleRepository, _greetingsDeliverService);
            _peopleWithBirthdayEqualToToday = new List<PersonDto> { CreatePersonBornToday(), CreatePersonBornToday() };
            _peopleWithBirthdayNotEqualToToday = new List<PersonDto> { CreatePersonNotBornToday(), CreatePersonNotBornToday() };
            _allPeopleWithBirthdayEqualToToday = new List<PersonDto>(_peopleWithBirthdayEqualToToday);

            _halfThePeopleWithBirthdayEqualToToday = new List<PersonDto>();
            _halfThePeopleWithBirthdayEqualToToday.AddRange(_peopleWithBirthdayNotEqualToToday);
            _halfThePeopleWithBirthdayEqualToToday.AddRange(_peopleWithBirthdayEqualToToday);
        }

        [Test]
        public void ShouldSendNoGreetingsWhenThereAreNotAnyEmployees()
        {
            GivenNoPeopleToSendGreetingsTo();

            WhenItSendsGreetingsToPeopleBornOn(DateTime.Now);

            ThenNoGreetingsAreSent();
        }

        [Test]
        public void ShouldSendNoGreetingsWhenAllEmployeesHaveBirthdayDifferentThanToday()
        {
            GivenAllPeopleWithBirthdayDifferentThanToday();

            WhenItSendsGreetingsToPeopleBornOn(DateTime.Now);

            ThenNoGreetingsAreSent();
        }

        [Test]
        public void ShouldSendGreetingsToAllPeopleWhenAllPeopleHaveBirthdayEqualToToday()
        {
            GivenAllPeopleWithBirthdayEqualToToday();

            WhenItSendsGreetingsToPeopleBornOn(DateTime.Now);

            ThenGreetingsHaveBeenSentToAllPeople();
        }

        [Test]
        public void ShouldSendGreetingsOnlyToPeopleWithBirthdayEqualToToday()
        {
            GivenHalfPeopleWithBirthdayEqualToToday();

            WhenItSendsGreetingsToPeopleBornOn(DateTime.Now);

            ThenGreetingsHaveBeenSentToHalfThePeople();
        }
    }
}
