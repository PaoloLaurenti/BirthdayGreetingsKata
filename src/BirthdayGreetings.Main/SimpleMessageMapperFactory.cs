using System;
using paramore.brighter.commandprocessor;

namespace BirthdayGreetings.Main
{
    internal class SimpleMessageMapperFactory : IAmAMessageMapperFactory
    {
        public IAmAMessageMapper Create(Type messageMapperType)
        {
            return new SendGreetingCommandMessageMapper();
        }
    }
}