using System.Collections.Generic;
using System.Linq;
using BirthdayGreetings.Common;
using BirthdayGreetings.Core.Employees;

namespace BirthdayGreetings.Core.Greetings
{
    internal class GreetingService
    {
        private readonly IGreetingsChannelGateway _greetingsChannelGateway;

        internal GreetingService(IGreetingsChannelGateway greetingsChannelGateway)
        {
            _greetingsChannelGateway = greetingsChannelGateway;
        }

        internal void SendToAll(IMaybe<List<EmployeeDto>> employees)
        {
            employees
                .Map(GreetingDtoFactory.CreateGreetingFor)
                .DoIf(greetings => greetings.Any(), _greetingsChannelGateway.Send);
        }
    }
}