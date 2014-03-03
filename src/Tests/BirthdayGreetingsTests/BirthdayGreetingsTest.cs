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
        private List<PersonDto> _allPeopleWithBirthdayEqualToToday;

        [SetUp]
        public void Init()
        {
            _greetingsDeliverService = MockRepository.GenerateMock<GreetingsDeliveryService>();
            _peopleRepository = MockRepository.GenerateStub<PeopleRepository>();
            _sut = new BirthdayGreetingsEngine(_peopleRepository, _greetingsDeliverService);
            _allPeopleWithBirthdayEqualToToday = new List<PersonDto> { CreatePersonBornToday(), CreatePersonBornToday() };
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
    }
}
