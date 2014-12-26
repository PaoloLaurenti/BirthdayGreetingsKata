﻿using Common.Logging;
using paramore.brighter.commandprocessor;

namespace BirthdayGreetingsEngine
{
    public class SendBirthdayGreetingsCommandHandler : RequestHandler<SendBirthdayGreetingsCommand>
    {
        public SendBirthdayGreetingsCommandHandler(IEmployeeGateway employeesGateway, IGreetingsChannelGateway greetingsChannelGateway, ILog logger) : base(logger)
        {
        }

        public override SendBirthdayGreetingsCommand Handle(SendBirthdayGreetingsCommand command)
        {
            return base.Handle(command);
        }
    }
}