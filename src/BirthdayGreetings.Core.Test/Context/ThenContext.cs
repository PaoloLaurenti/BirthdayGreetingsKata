using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BirthdayGreetings.Core.Greetings;
using BirthdayGreetings.Core.Test.Extension;
using FakeItEasy;
using FluentAssertions;

namespace BirthdayGreetings.Core.Test.Context
{
    internal class ThenContext
    {
        private readonly GivenContext _givenContext;
        private readonly WhenContext _whenContext;
        private readonly MockSendBirthdayGreetingsByEmailCommandHandler _mockSendBirthdayGreetingsByEmailCommandHandler;

        internal ThenContext(GivenContext givenContext, WhenContext whenContext, MockSendBirthdayGreetingsByEmailCommandHandler mockSendBirthdayGreetingsByEmailCommandHandler)
        {
            _givenContext = givenContext;
            _whenContext = whenContext;
            _mockSendBirthdayGreetingsByEmailCommandHandler = mockSendBirthdayGreetingsByEmailCommandHandler;
        }

        internal void NotifyGreetingsSent(IEnumerable<GreetingDto> greetings)
        {
        }

        internal void NoBirthdayGreetingsHaveBeenSent()
        {
            _mockSendBirthdayGreetingsByEmailCommandHandler.AssertNoBirthdayGreetingsHaveBeenSent();
        }

        internal void BirthdayGreetingsHaveBeenSentToEmployeesWithBirthdateEqualToChosenDate()
        {
            _givenContext
                .DoWithGivenEmployeesWithDateOfBirthEqualToChosenDate(employees =>
                    {
                        var expectedGreetings = employees.Select(GreetingDtoFactory.CreateGreetingFor).Sort().ToList();
                        _mockSendBirthdayGreetingsByEmailCommandHandler.AssertThatGreetingsSentAre(expectedGreetings);
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