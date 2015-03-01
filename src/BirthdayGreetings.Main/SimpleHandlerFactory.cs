using System;
using BirthdayGreetings.Core;
using BirthdayGreetings.Email;
using BirthdayGreetings.FileSystem;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;

namespace BirthdayGreetings.Main
{
    internal class SimpleHandlerFactory : IAmAHandlerFactory
    {
        private readonly string _employeeFileFullPath;
        private readonly SmtpServerSettings _smtpServerSettings;
        private readonly ILog _logger;

        public SimpleHandlerFactory(string employeeFileFullPath, SmtpServerSettings smtpServerSettings, ILog logger)
        {
            _employeeFileFullPath = employeeFileFullPath;
            _smtpServerSettings = smtpServerSettings;
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
            var registry = new SubscriberRegistry();
            registry.Register<SendGreetingCommand, SendGreetingsByEmailCommandHandler>();
            return CommandProcessorBuilder
                .With()
                .Handlers(new HandlerConfiguration(registry, handlerFactory))
                .NoPolicy()
                .Logger(_logger)
                .NoTaskQueues()
                .RequestContextFactory(new InMemoryRequestContextFactory())
                .Build();
        }

        public void Release(IHandleRequests handler)
        {
        }
    }
}