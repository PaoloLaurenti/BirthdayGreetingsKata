using System;
using System.Collections.Generic;
using BirthdayGreetings;
using NUnit.Framework;
using Rhino.Mocks;

namespace BirthdayGreetingsTests
{
    [TestFixture]
    public class BirthdayGreetingsTest
    {
        [Test]
        public void ShouldSendNoGreetingsWhenThereAreNotAnyEmployees()
        {
            var greetingsDeliverService = MockRepository.GenerateMock<GreetingsDeliveryService>();
            var peopleRepository = MockRepository.GenerateStub<PeopleRepository>();
            peopleRepository.Stub(pr => pr.GetAll()).Return(new List<PersonDTO>());
            var sut = new BirthdayGreetingsEngine(peopleRepository, greetingsDeliverService);

            sut.SendGreetingsToPeopleBornInThis(DateTime.Now);

            greetingsDeliverService.AssertWasNotCalled(gds => gds.Deliver());
        }
    }
}
