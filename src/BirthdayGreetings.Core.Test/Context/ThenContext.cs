﻿using System.Collections.Generic;
using System.Linq;
using BirthdayGreetings.Core.Test.Extension;
using FakeItEasy;
using Xunit;

namespace BirthdayGreetings.Core.Test.Context
{
    internal class ThenContext
    {
        private readonly IGreetingsChannelGateway _greetingsChannelGateway;
        private readonly GivenContext _givenContext;
        private IEnumerable<GreetingDto> _greetingsSent;

        internal ThenContext(IGreetingsChannelGateway greetingsChannelGateway, GivenContext givenContext)
        {
            _greetingsChannelGateway = greetingsChannelGateway;
            _givenContext = givenContext;
        }

        internal void NotifyGreetingsSent(IEnumerable<GreetingDto> greetings)
        {
            _greetingsSent = greetings;
        }

        internal void NoBirthdayGreetingsHaveBeenSent()
        {
            A.CallTo(() => _greetingsChannelGateway.Send(null)).WithAnyArguments().MustNotHaveHappened();
        }

        internal void BirthdayGreetingsHaveBeenSentToEmployeesWithBirthdateEqualToChosenDate()
        {
            A.CallTo(() => _greetingsChannelGateway.Send(A<IEnumerable<GreetingDto>>._)).MustHaveHappened(Repeated.Exactly.Once);
            AssertThatGreetingsSentAreOnlyExpectedOnes();
        }

        private void AssertThatGreetingsSentAreOnlyExpectedOnes()
        {
            _givenContext
                .DoWithGivenEmployeesWithDateOfBirthEqualToChosenDate(employees =>
                {
                    var employeesNamesWhoShouldReceiveGreetings = employees.Select(e => e.FirstName).ToList();
                    employeesNamesWhoShouldReceiveGreetings
                        .CompareItemValuesIgnoringOrder(
                            _greetingsSent.Select(d => d.FirstName).ToList(), 
                            () => { },
                            differences => Assert.True(false, string.Format("Greetings sent are not those that were expected: {0}", differences)));
                });
        }
    }
}