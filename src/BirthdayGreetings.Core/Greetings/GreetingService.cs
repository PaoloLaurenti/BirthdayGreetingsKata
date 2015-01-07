using System.Collections.Generic;
using System.Linq;
using BirthdayGreetings.Core.Employees;

namespace BirthdayGreetings.Core.Greetings
{
    internal class GreetingService
    {
        private readonly IGreetingsChannelGateway _greetingsChannelGateway;
        private readonly List<EmployeeDto> _employeesToSendGreetingsTo;

        internal GreetingService(IGreetingsChannelGateway greetingsChannelGateway)
        {
            _greetingsChannelGateway = greetingsChannelGateway;
            _employeesToSendGreetingsTo = new List<EmployeeDto>();
        }

        internal void Collect(EmployeeDto employee)
        {
            _employeesToSendGreetingsTo.Add(employee);
        }

        public void SendAll()
        {
            var greetings = _employeesToSendGreetingsTo.Select(GreetingDtoFactory.CreateGreetingFor).ToList();
            if (greetings.Any())
                _greetingsChannelGateway.Send(greetings);
        }
    }
}