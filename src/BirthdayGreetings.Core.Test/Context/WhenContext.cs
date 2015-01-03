using System;

namespace BirthdayGreetings.Core.Test.Context
{
    internal class WhenContext
    {
        private readonly SendBirthdayGreetingsCommandHandler _sut;

        internal WhenContext(SendBirthdayGreetingsCommandHandler sut)
        {
            _sut = sut;
        }

        internal void WhenSendingBirthdayGreetings()
        {
            _sut.Handle(new SendBirthdayGreetingsCommand(DateTime.Now));
        }
    }
}