using System;
using System.Collections.Generic;
using BirthdayGreetings.Core.Greetings;
using FakeItEasy;

namespace BirthdayGreetings.Core.Test.Extension
{
    internal static class GreetingsChannelGatewayExtension
    {
        internal static void ConfigureToNotifyGreetingsSent(this IGreetingsChannelGateway fakeGreetingsChannelGateway, Action<IEnumerable<GreetingDto>> actionToInvoke)
        {
            A.CallTo(() => fakeGreetingsChannelGateway.Send(A<IEnumerable<GreetingDto>>._)).Invokes(actionToInvoke);
        }
    }
}