using System;
using paramore.brighter.commandprocessor;

namespace BirthdayGreetings.Core
{
    public class SendBirthdayGreetingsCommand : Command
    {
        private readonly DateTime _dateTime;

        public SendBirthdayGreetingsCommand(DateTime dateTime) : base(Guid.NewGuid())
        {
            _dateTime = dateTime;
        }

        public DateTime DateTime
        {
            get { return _dateTime; }
        }
    }
}