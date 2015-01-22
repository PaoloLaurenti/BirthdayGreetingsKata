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

        internal WhenContext(IEmployeeGateway employeesGateway, IGreetingsGateway greetingsGateway, DateTime chosenDate)
        {
            _chosenDate = chosenDate;
            _sut = new SendBirthdayGreetingsCommandHandler(employeesGateway, greetingsGateway, A.Fake<ILog>());
            EmployeeGatewayException = null;
            GreetingsChannelGatewayException = null;
        }

        private EmployeeGatewayException EmployeeGatewayException { get; set; }
        private GreetingsChannelGatewayException GreetingsChannelGatewayException { get; set; }

        internal void SendingBirthdayGreetings()
        {
            try
            {
                _sut.Handle(new SendBirthdayGreetingsCommand(_chosenDate));
            }
            catch (EmployeeGatewayException employeeException)
            {
                EmployeeGatewayException = employeeException;
            }
            catch (GreetingsChannelGatewayException greetingsChannelGatewayException)
            {
                GreetingsChannelGatewayException = greetingsChannelGatewayException;
            }
        }
    }
}