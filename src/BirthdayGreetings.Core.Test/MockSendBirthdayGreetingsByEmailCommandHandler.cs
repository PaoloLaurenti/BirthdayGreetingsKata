using System;
using System.Collections.Generic;
using System.Linq;
using BirthdayGreetings.Core.Greetings;
using BirthdayGreetings.Core.Test.Extension;
using Common.Logging;
using FluentAssertions;
using Xunit;
using paramore.brighter.commandprocessor;

namespace BirthdayGreetings.Core.Test
{
    public class MockSendBirthdayGreetingsByEmailCommandHandler : RequestHandler<SendGreetingCommand>
    {
        private readonly List<SendGreetingCommand> _handledCommands;
        private GreetingsGatewayException _exceptionToThrowSendingGreetings;

        public MockSendBirthdayGreetingsByEmailCommandHandler(ILog logger)
            : base(logger)
        {
            _handledCommands = new List<SendGreetingCommand>();
        }

        public override SendGreetingCommand Handle(SendGreetingCommand command)
        {
            _handledCommands.Add(command);
            if (_exceptionToThrowSendingGreetings != null)
                throw _exceptionToThrowSendingGreetings;
            return base.Handle(command);
        }

        public void AssertNoBirthdayGreetingsHaveBeenSent()
        {
            var greetingsSentMessagePart = string.Join(Environment.NewLine, _handledCommands.Select(hc => string.Format("{0} - {1}", hc.GreetingDto.FirstName, hc.GreetingDto.Email)));
            var reasonMsg = string.Format("No greetings should be sent. Instead, following greetings have been sent:{0}", greetingsSentMessagePart);
            _handledCommands.Any().Should().BeFalse(reasonMsg);

        }

        public void AssertThatGreetingsSentAre(List<GreetingDto> expectedGreetings)
        {
            var greetingsCountAssertFailureMessage = string.Format("Expected greetings count is {0}, actual is {1}", expectedGreetings.Count, _handledCommands.Count);
            Assert.True(expectedGreetings.Count == _handledCommands.Count, greetingsCountAssertFailureMessage);

            expectedGreetings.Should().HaveSameCount(_handledCommands, greetingsCountAssertFailureMessage);

            var sentGreetings = _handledCommands.Select(hc => hc.GreetingDto).ToList();
            var firstPart = string.Format("Expected sent greetings are:{0}{1}", Environment.NewLine, string.Join(Environment.NewLine, expectedGreetings.Select(eg => eg.ConvertToString()).ToList()));
            var secondPart = string.Format("Actual sent greetings are:{0}{1}", Environment.NewLine, string.Join(Environment.NewLine, sentGreetings.Select(sg => sg.ConvertToString()).ToList()));
            var greetingsEqualityAssertFailureMessage = string.Join(firstPart, Environment.NewLine, secondPart);
            expectedGreetings.All(eg => sentGreetings.Any(sg => sg.FirstName == eg.FirstName && sg.Email == eg.Email)).Should().BeTrue(greetingsEqualityAssertFailureMessage);
        }

        public void ThrowSendingGreetings(GreetingsGatewayException exception)
        {
            _exceptionToThrowSendingGreetings = exception;
        }
    }
}