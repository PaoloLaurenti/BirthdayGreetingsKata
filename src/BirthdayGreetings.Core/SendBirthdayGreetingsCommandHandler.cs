using System.Linq;
using Common.Logging;
using paramore.brighter.commandprocessor;

namespace BirthdayGreetings.Core
{
    public class SendBirthdayGreetingsCommandHandler : RequestHandler<SendBirthdayGreetingsCommand>
    {
        private readonly IEmployeeGateway _employeesGateway;
        private readonly IGreetingsChannelGateway _greetingsChannelGateway;

        public SendBirthdayGreetingsCommandHandler(IEmployeeGateway employeesGateway, IGreetingsChannelGateway greetingsChannelGateway, ILog logger) : base(logger)
        {
            _employeesGateway = employeesGateway;
            _greetingsChannelGateway = greetingsChannelGateway;
        }

        public override SendBirthdayGreetingsCommand Handle(SendBirthdayGreetingsCommand command)
        {
            var employees = _employeesGateway
                                .GetEmployees()
                                .Where(e => e.DateOfBirth.Month == command.DateTime.Month && e.DateOfBirth.Day == command.DateTime.Day)
                                .ToList();
            if (employees.Any())
                _greetingsChannelGateway.Send(employees.Select(e => new GreetingDto
                {
                   FirstName = e.FirstName
                }).ToList());
            return base.Handle(command);
        }
    }
}