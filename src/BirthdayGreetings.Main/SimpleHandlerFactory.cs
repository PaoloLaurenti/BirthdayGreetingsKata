using System;
using BirthdayGreetings.Core;
using BirthdayGreetings.Email;
using BirthdayGreetings.FileSystem;
using Polly;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using paramore.brighter.commandprocessor.messaginggateway.rmq;

namespace BirthdayGreetings.Main
{
    internal class SimpleHandlerFactory : IAmAHandlerFactory
    {
        private readonly string _employeeFileFullPath;
        private readonly SmtpServerSettings _smtpServerSettings;
        private readonly MsSqlMessageStoreFactory _factory;
        private readonly ILog _logger;

        public SimpleHandlerFactory(string employeeFileFullPath, SmtpServerSettings smtpServerSettings, MsSqlMessageStoreFactory factory, ILog logger)
        {
            _employeeFileFullPath = employeeFileFullPath;
            _smtpServerSettings = smtpServerSettings;
            _factory = factory;
            _logger = logger;
        }

        public IHandleRequests Create(Type handlerType)
        {
            return Instanciate(handlerType);
        }

        private IHandleRequests Instanciate(Type handlerType)
        {
            return handlerType == typeof (SendGreetingsByEmailCommandHandler)
                    ? GetSendGreetingsByEmailCommandHandler()
                    : GetSendBirthdayGreetingsCommandHandler();
        }

        private IHandleRequests GetSendBirthdayGreetingsCommandHandler()
        {
            var fileSystemEmployeeGateway = new FileSystemEmployeeGateway(_employeeFileFullPath);
            return new SendBirthdayGreetingsCommandHandler(fileSystemEmployeeGateway, GetSendGreetingsByEmailCommandProcessor(this), _logger);
        }

        private IHandleRequests GetSendGreetingsByEmailCommandHandler()
        {
            var emailChannel = new EmailChannel(_smtpServerSettings.HostAddress, 
                                                _smtpServerSettings.Port,
                                                _smtpServerSettings.Username, 
                                                _smtpServerSettings.Password);
            var emailGreetingsGateway = new EmailGreetingsGateway(emailChannel);
            return new SendGreetingsByEmailCommandHandler(emailGreetingsGateway, _logger);
        }

        private IAmACommandProcessor GetSendGreetingsByEmailCommandProcessor(IAmAHandlerFactory handlerFactory)
        {
            var messageMapperRegistry = new MessageMapperRegistry(new SimpleMessageMapperFactory())
                {
                    {
                        typeof (SendGreetingCommand), typeof (SendGreetingCommandMessageMapper)
                    }
                };
            var messagingConfiguration = new MessagingConfiguration(_factory.GetMsSqlMessageStore("SendGreetingsByEmail"),
                                                                    new RmqMessageProducer(_logger),
                                                                    messageMapperRegistry);
            var registry = new SubscriberRegistry();
            registry.Register<SendGreetingCommand, SendGreetingsByEmailCommandHandler>();

            var retryPolicy = Policy.Handle<Exception>().WaitAndRetry(new[] { TimeSpan.FromMilliseconds(50), TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(150) });
            var circuitBreakerPolicy = Policy.Handle<Exception>().CircuitBreaker(1, TimeSpan.FromMilliseconds(500));
            var policyRegistry = new PolicyRegistry { { CommandProcessor.RETRYPOLICY, retryPolicy }, { CommandProcessor.CIRCUITBREAKER, circuitBreakerPolicy } };

            return CommandProcessorBuilder
                    .With()
                    .Handlers(new HandlerConfiguration(registry, handlerFactory))
                    .Policies(policyRegistry)
                    .Logger(_logger)
                    .TaskQueues(messagingConfiguration)
                    .RequestContextFactory(new InMemoryRequestContextFactory())
                    .Build();
        }

        public void Release(IHandleRequests handler)
        {
        }
    }
}