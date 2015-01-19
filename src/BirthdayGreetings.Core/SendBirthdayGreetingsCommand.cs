using System;
using paramore.brighter.commandprocessor;

namespace BirthdayGreetings.Core
{
    public class SendBirthdayGreetingsCommand : Command
    {
        private readonly DateTime _chosenDate;

        public SendBirthdayGreetingsCommand(DateTime chosenDate) : base(Guid.NewGuid())
        {
            _chosenDate = chosenDate;
        }

        public DateTime ChosenDate
        {
            get { return _chosenDate; }
        }
    }
}