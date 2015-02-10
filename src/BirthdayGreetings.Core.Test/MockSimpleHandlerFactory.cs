using System;
using paramore.brighter.commandprocessor;

namespace BirthdayGreetings.Core.Test
{
    internal class MockSimpleHandlerFactory : IAmAHandlerFactory
    {
        private readonly MockSendBirthdayGreetingsByEmailCommandHandler _mockSendBirthdayGreetingsByEmailCommandHandler;
        
        public MockSimpleHandlerFactory(MockSendBirthdayGreetingsByEmailCommandHandler mockSendBirthdayGreetingsByEmailCommandHandler)
        {
            _mockSendBirthdayGreetingsByEmailCommandHandler = mockSendBirthdayGreetingsByEmailCommandHandler;
        }

        public IHandleRequests Create(Type handlerType)
        {
            return _mockSendBirthdayGreetingsByEmailCommandHandler;
        }

        public void Release(IHandleRequests handler)
        {
        }
    }
}