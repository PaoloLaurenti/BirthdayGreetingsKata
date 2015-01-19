using System;
using System.Collections.Generic;
using System.Linq;
using BirthdayGreetings.Core.Greetings;
using BirthdayGreetings.Core.Test.Extension;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace BirthdayGreetings.Core.Test.Context
{
    internal class ThenContext
    {
        private readonly IGreetingsChannelGateway _greetingsChannelGateway;
        private readonly GivenContext _givenContext;
        private readonly WhenContext _whenContext;
        private IEnumerable<GreetingDto> _greetingsSent;

        internal ThenContext(IGreetingsChannelGateway greetingsChannelGateway, GivenContext givenContext, WhenContext whenContext)
        {
            _greetingsChannelGateway = greetingsChannelGateway;
            _givenContext = givenContext;
            _whenContext = whenContext;
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
            A.CallTo(() => _greetingsChannelGateway.Send(A<IEnumerable<GreetingDto>>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
            AssertThatGreetingsSentAreOnlyExpectedOnes();
        }

        private void AssertThatGreetingsSentAreOnlyExpectedOnes()
        {
            _givenContext
                .DoWithGivenEmployeesWithDateOfBirthEqualToChosenDate(employees =>
                {
                    var employeesNamesWhoShouldReceiveGreetings = employees.Select(GreetingDtoFactory.CreateGreetingFor).Sort().ToList();
                    employeesNamesWhoShouldReceiveGreetings
                        .CompareAlreadySortedItems(
                            _greetingsSent.Sort().ToList(), 
                            () => { },
                            differences => Assert.True(false, string.Format("Greetings sent are not those that were expected: {0}", differences)));
                });
        }

        internal void AnExceptionHasBeenThrownAs<T>() where T : Exception
        {
            _whenContext.EmployeeException.Should().NotBeNull(string.Format("Exception of type {0} has not been thrown as expected", typeof(T)));
            _whenContext.EmployeeException.Message.Should().NotBeNullOrWhiteSpace("Exception message should not be empty");
        }
    }
}