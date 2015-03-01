using System;
using System.Collections.Specialized;
using BirthdayGreetings.Core.Employees;
using BirthdayGreetings.Core.Greetings;
using FakeItEasy;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;

namespace BirthdayGreetings.Core.Test.Context
{
    internal class WhenContext
    {
        private readonly DateTime _chosenDate;
        private readonly SendBirthdayGreetingsCommandHandler _sut;

        internal WhenContext(IEmployeeGateway employeesGateway, IAmAHandlerFactory handlerFactory, DateTime chosenDate)
        {
            _chosenDate = chosenDate;
            _sut = new SendBirthdayGreetingsCommandHandler(employeesGateway, GetCommandProcessor(handlerFactory), A.Fake<ILog>());
            EmployeeGatewayException = null;
            GreetingsGatewayException = null;
        }

        private static IAmACommandProcessor GetCommandProcessor(IAmAHandlerFactory handlerFactory)
        {
            var registry = new SubscriberRegistry();
            registry.Register<SendGreetingCommand, MockSendBirthdayGreetingsByEmailCommandHandler>();
            return CommandProcessorBuilder
                .With()
                .Handlers(new HandlerConfiguration(registry, handlerFactory))
                .NoPolicy()
                .Logger(LogProvider.For<TestContext>())
                .NoTaskQueues()
                .RequestContextFactory(new InMemoryRequestContextFactory())
                .Build();
        }

        private EmployeeGatewayException EmployeeGatewayException { get; set; }
        private GreetingsGatewayException GreetingsGatewayException { get; set; }

        internal void SendingBirthdayGreetings()
        {
            try
            {
                _sut.Handle(new SendBirthdayGreetingsCommand(_chosenDate));
            }
            catch (EmployeeGatewayException employeeException)
            {
                EmployeeGatewayException = employeeException;
            }
            catch (GreetingsGatewayException greetingsChannelGatewayException)
            {
                GreetingsGatewayException = greetingsChannelGatewayException;
            }
        }
    }
}