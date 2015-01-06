using System;
using BirthdayGreetings.Core.Employees;
using BirthdayGreetings.Core.Greetings;
using Common.Logging;
using FakeItEasy;

namespace BirthdayGreetings.Core.Test.Context
{
    internal class WhenContext
    {
        private readonly DateTime _chosenDate;
        private readonly SendBirthdayGreetingsCommandHandler _sut;

        internal WhenContext(IEmployeeGateway employeesGateway, IGreetingsChannelGateway greetingsChannelGateway, DateTime chosenDate)
        {
            _chosenDate = chosenDate;
            _sut = new SendBirthdayGreetingsCommandHandler(employeesGateway, greetingsChannelGateway, A.Fake<ILog>());
        }

        internal void WhenSendingBirthdayGreetings()
        {
            _sut.Handle(new SendBirthdayGreetingsCommand(_chosenDate));
        }
    }
}