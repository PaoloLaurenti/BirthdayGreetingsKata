using System.Collections.Generic;
using System.Linq;
using BirthdayGreetings.Common;
using BirthdayGreetings.Core.Employees;

namespace BirthdayGreetings.Core.Greetings
{
    internal class GreetingService
    {
        private readonly IGreetingsGateway _greetingsGateway;

        internal GreetingService(IGreetingsGateway greetingsGateway)
        {
            _greetingsGateway = greetingsGateway;
        }

        internal void SendToAll(IMaybe<List<EmployeeDto>> employees)
        {
            employees
                .Map(GreetingDtoFactory.CreateGreetingFor)
                .DoIf(greetings => greetings.Any(), _greetingsGateway.Deliver);
        }
    }
}