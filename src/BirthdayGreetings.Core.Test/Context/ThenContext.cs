using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BirthdayGreetings.Core.Greetings;
using BirthdayGreetings.Core.Test.Extension;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace BirthdayGreetings.Core.Test.Context
{
    internal class ThenContext
    {
        private readonly IGreetingsGateway _greetingsGateway;
        private readonly GivenContext _givenContext;
        private readonly WhenContext _whenContext;
        private IEnumerable<GreetingDto> _greetingsSent;

        internal ThenContext(IGreetingsGateway greetingsGateway, GivenContext givenContext, WhenContext whenContext)
        {
            _greetingsGateway = greetingsGateway;
            _givenContext = givenContext;
            _whenContext = whenContext;
        }

        internal void NotifyGreetingsSent(IEnumerable<GreetingDto> greetings)
        {
            _greetingsSent = greetings;
        }

        internal void NoBirthdayGreetingsHaveBeenSent()
        {
            A.CallTo(() => _greetingsGateway.Deliver(null)).WithAnyArguments().MustNotHaveHappened();
        }

        internal void BirthdayGreetingsHaveBeenSentToEmployeesWithBirthdateEqualToChosenDate()
        {
            A.CallTo(() => _greetingsGateway.Deliver(A<IEnumerable<GreetingDto>>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
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
            var thrownException = (T) _whenContext
                                        .GetType()
                                        .GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                                        .Single(x => typeof(T).IsAssignableFrom(x.PropertyType))
                                        .GetValue(_whenContext);

            thrownException.Should().NotBeNull(string.Format("An exception must be thrown f type {0}", typeof(T)));
            thrownException.Should().BeOfType<T>(string.Format("Thrown exception should be of type {0} - Actual {1}", typeof(T), thrownException.GetType()));
            thrownException.Message.Should().NotBeNullOrWhiteSpace("Exception message should not be empty");
        }
    }
}