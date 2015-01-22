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

        public SendBirthdayGreetingsCommandHandler(IEmployeeGateway employeesGateway, IGreetingsGateway greetingsGateway, ILog logger)
            : base(logger)
        {
            _employeeRepository = new EmployeeRepository(employeesGateway);
            _greetingService = new GreetingService(greetingsGateway);
        }

        public override SendBirthdayGreetingsCommand Handle(SendBirthdayGreetingsCommand command)
        {
            _employeeRepository
                .FindAll()
                .Map(BirthdayGreetingsSendingStrategy.Create(command.ChosenDate).GetOnlyEmployeesToSendGreetingsTo)
                .Do(_greetingService.SendToAll);
            return base.Handle(command);
        }
    }
}