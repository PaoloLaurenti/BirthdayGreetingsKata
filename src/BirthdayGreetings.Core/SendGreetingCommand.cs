using System;
using BirthdayGreetings.Core.Greetings;
using paramore.brighter.commandprocessor;

namespace BirthdayGreetings.Core
{
    public class SendGreetingCommand : Command
    {
        private readonly GreetingDto _greetingDto;

        public SendGreetingCommand(GreetingDto greetingDto) : base(Guid.NewGuid())
        {
            _greetingDto = greetingDto;
        }

        public GreetingDto GreetingDto
        {
            get { return _greetingDto; }
        }
    }
}