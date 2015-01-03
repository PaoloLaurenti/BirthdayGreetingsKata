using System;
using paramore.brighter.commandprocessor;

namespace BirthdayGreetings.Core
{
    public class SendBirthdayGreetingsCommand : Command
    {
        public SendBirthdayGreetingsCommand(DateTime now) : base(Guid.NewGuid())
        {
            
        }
    }
}