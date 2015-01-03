using System;

namespace BirthdayGreetings.Core.Test.Context
{
    internal class WhenContext
    {
        private readonly DateTime _chosenDate;
        private readonly SendBirthdayGreetingsCommandHandler _sut;

        internal WhenContext(DateTime chosenDate, SendBirthdayGreetingsCommandHandler sut)
        {
            _chosenDate = chosenDate;
            _sut = sut;
        }

        internal void WhenSendingBirthdayGreetings()
        {
            _sut.Handle(new SendBirthdayGreetingsCommand(_chosenDate));
        }
    }
}