using System;
using System.Collections.Generic;
using BirthdayGreetings.Core.Greetings;
using FakeItEasy;

namespace BirthdayGreetings.Core.Test.Extension
{
    internal static class GreetingsChannelGatewayExtension
    {
        internal static void ConfigureToNotifyGreetingsSent(this IGreetingsGateway fakeGreetingsGateway, Action<IEnumerable<GreetingDto>> actionToInvoke)
        {
            A.CallTo(() => fakeGreetingsGateway.Deliver(A<IEnumerable<GreetingDto>>._)).Invokes(actionToInvoke);
        }
    }
}