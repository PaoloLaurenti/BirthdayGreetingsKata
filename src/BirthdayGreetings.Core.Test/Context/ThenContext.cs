using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using KellermanSoftware.CompareNetObjects;

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

        internal void NoBirthdayGreetingsHaveBeenSent()
        {
            A.CallTo(() => _greetingsChannelGateway.Send(null)).WithAnyArguments().MustNotHaveHappened();
        }

        internal void BirthdayGreetingsHaveBeenSentToEmployeesWithBirthdateEqualToChosenDate()
        {
            A.CallTo(() => _greetingsChannelGateway.Send(A<IEnumerable<GreetingDto>>._)).MustHaveHappened(Repeated.Exactly.Once);
            _givenContext
                .DoWithGivenEmployeesWithDateOfBirthEqualToChosenDate(employees =>
                {
                    var comparisonResult = new CompareLogic(new ComparisonConfig { IgnoreCollectionOrder = true }).Compare(
                        employees.Select(e => e.FirstName).ToList(), _greetingsSent.Select(d => d.FirstName).ToList());
                    comparisonResult.AreEqual.Should().BeTrue("Because greetings should be sent only to employees with date of birth equal to chosen date. Expected greetings to send are different from those that have been sent: {0}", comparisonResult.DifferencesString);
                });
        }

        internal void NotifyGreetingsSent(IEnumerable<GreetingDto> greetings)
        {
            _greetingsSent = greetings;
        }
    }
}