using System;
using BirthdayGreetings.Core.Greetings;
using FakeItEasy;

namespace BirthdayGreetings.Core.Test.Extension
{
    internal static class GreetingsChannelGatewayExtension
    {
        internal static void ConfigureToNotifyGreetingsSent(this IGreetingsGateway fakeGreetingsGateway, Action<GreetingDto> actionToInvoke)
        {
            A.CallTo(() => fakeGreetingsGateway.Deliver(A<GreetingDto>._)).Invokes(actionToInvoke);
        }
    }
}