using System;
using System.Collections.Generic;
using BirthdayGreetings;
using Rhino.Mocks;

namespace BirthdayGreetingsTests
{
    public partial class BirthdayGreetingsTest
    {
        private void GivenNoPeopleToSendGreetingsTo()
        {
            _peopleRepository.Stub(pr => pr.GetAll()).Return(new List<PersonDTO>());
        }

        private void GivenAllPeopleWithBirthdayDifferentThanToday()
        {
            _peopleRepository.Stub(pr => pr.GetAll()).Return(new List<PersonDTO> { CreatePersonNotBornToday() });
        }

        private void WhenGreetingsAreSentOnlyToPeopleBorn(DateTime today)
        {
            _sut.SendGreetingsToPeopleBornInThis(today);
        }

        private void ThenNoGreetingsAreSent()
        {
            _greetingsDeliverService.AssertWasNotCalled(gds => gds.Deliver());
        }
    }
}
