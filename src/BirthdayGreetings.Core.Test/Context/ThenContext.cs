using FakeItEasy;

namespace BirthdayGreetings.Core.Test.Context
{
    internal class ThenContext
    {
        private readonly IGreetingsChannelGateway _greetingsChannelGateway;

        internal ThenContext(IGreetingsChannelGateway greetingsChannelGateway)
        {
            _greetingsChannelGateway = greetingsChannelGateway;
        }

        internal void NoBirthdayGreetingsHaveBeenSent()
        {
            A.CallTo(() => _greetingsChannelGateway.Send(null)).WithAnyArguments().MustNotHaveHappened();
        }
    }
}