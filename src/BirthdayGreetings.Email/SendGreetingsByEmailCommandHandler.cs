using BirthdayGreetings.Core;
using BirthdayGreetings.Core.Greetings;
using Common.Logging;
using paramore.brighter.commandprocessor;

namespace BirthdayGreetings.Email
{
    public class SendGreetingsByEmailCommandHandler : RequestHandler<SendGreetingCommand>
    {
        private readonly IGreetingsGateway _greetingsGateway;

        public SendGreetingsByEmailCommandHandler(IGreetingsGateway greetingsGateway, ILog logger) : base(logger)
        {
            _greetingsGateway = greetingsGateway;
        }

        public override SendGreetingCommand Handle(SendGreetingCommand command)
        {
            _greetingsGateway.Deliver(command.GreetingDto);
            return base.Handle(command);
        }
    }
}