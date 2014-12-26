using System;
using paramore.brighter.commandprocessor;

namespace BirthdayGreetingsEngine
{
    public class SendBirthdayGreetingsCommand : Command
    {
        public SendBirthdayGreetingsCommand(DateTime now) : base(Guid.NewGuid())
        {
            
        }
    }
}