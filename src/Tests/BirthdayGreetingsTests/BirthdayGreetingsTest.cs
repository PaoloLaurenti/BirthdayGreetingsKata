using System;
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

        private static PersonDTO CreatePersonNotBornToday()
        {
            var now = DateTime.Now;
            var month = now.Month == 12 ? 1 : now.Month + 1;
            var day = now.Day == DateTime.DaysInMonth(1982, now.Month) ? 1 : now.Day + 1;
            return new PersonDTO { Birthday = new DateTime(1982, month, day) };
        }

        [SetUp]
        public void Init()
        {
            _greetingsDeliverService = MockRepository.GenerateMock<GreetingsDeliveryService>();
            _peopleRepository = MockRepository.GenerateStub<PeopleRepository>();
            _sut = new BirthdayGreetingsEngine(_peopleRepository, _greetingsDeliverService);
        }

        [Test]
        public void ShouldSendNoGreetingsWhenThereAreNotAnyEmployees()
        {
            GivenNoPeopleToSendGreetingsTo();

            WhenGreetingsAreSentOnlyToPeopleBorn(DateTime.Now);

            ThenNoGreetingsAreSent();
        }

        [Test]
        public void ShouldSendNoGreetingsWhenThereAreEmployeesWithBirthdayDifferentThanToday()
        {
            GivenAllPeopleWithBirthdayDifferentThanToday();

            WhenGreetingsAreSentOnlyToPeopleBorn(DateTime.Now);

            ThenNoGreetingsAreSent();
        }
    }
}
