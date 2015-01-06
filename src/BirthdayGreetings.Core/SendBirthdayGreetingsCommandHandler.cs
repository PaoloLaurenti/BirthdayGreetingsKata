using BirthdayGreetings.Core.Employees;
using BirthdayGreetings.Core.Greetings;
using Common.Logging;
using paramore.brighter.commandprocessor;

namespace BirthdayGreetings.Core
{
    public class SendBirthdayGreetingsCommandHandler : RequestHandler<SendBirthdayGreetingsCommand>
    {
        private readonly EmployeeRepository _employeeRepository;
        private readonly GreetingService _greetingService;

        public SendBirthdayGreetingsCommandHandler(IEmployeeGateway employeesGateway, IGreetingsChannelGateway greetingsChannelGateway, ILog logger)
            : base(logger)
        {
            _employeeRepository = new EmployeeRepository(employeesGateway);
            _greetingService = new GreetingService(greetingsChannelGateway);
        }

        public override SendBirthdayGreetingsCommand Handle(SendBirthdayGreetingsCommand command)
        {
            _employeeRepository
                .FindAll()
                .ForEach(e => e.DoOnBirthday(command.DateTime, employee => _greetingService.Collect(employee)));
            _greetingService.SendAll();

            return base.Handle(command);
        }
    }
}