using System.Collections.Generic;
using System.Linq;
using BirthdayGreetings.Common;
using BirthdayGreetings.Core.Employees;
using paramore.brighter.commandprocessor;

namespace BirthdayGreetings.Core.Greetings
{
    internal class GreetingService
    {
        private readonly IAmACommandProcessor _commandProcessor;

        internal GreetingService(IAmACommandProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor;
        }

        internal void SendToAll(IMaybe<List<EmployeeDto>> employees)
        {
            employees
                .Map(GreetingDtoFactory.CreateGreetingFor)
                .DoIf(greetings => greetings.Any(), greetingsDtos => greetingsDtos.ForEach(g => _commandProcessor.Send(new SendGreetingCommand(g))));
        }
    }
}