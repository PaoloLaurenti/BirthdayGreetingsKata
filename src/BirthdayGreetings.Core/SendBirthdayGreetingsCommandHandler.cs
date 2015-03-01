using BirthdayGreetings.Core.Employees;
using BirthdayGreetings.Core.Greetings;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;

namespace BirthdayGreetings.Core
{
    public class SendBirthdayGreetingsCommandHandler : RequestHandler<SendBirthdayGreetingsCommand>
    {
        private readonly IAmACommandProcessor _commandProcessor;
        private readonly EmployeeRepository _employeeRepository;
        private readonly GreetingService _greetingService;

        public SendBirthdayGreetingsCommandHandler(IEmployeeGateway employeesGateway, IAmACommandProcessor commandProcessor, ILog logger)
            : base(logger)
        {
            _commandProcessor = commandProcessor;
            _employeeRepository = new EmployeeRepository(employeesGateway);
            _greetingService = new GreetingService(_commandProcessor);
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