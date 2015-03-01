using System;
using System.Configuration;
using BirthdayGreetings.Core;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;

namespace BirthdayGreetings.Main
{
    class Program
    {
        private static readonly ILog Logger = LogProvider.For<Program>();

        static void Main(string[] args)
        {
            var registry = new SubscriberRegistry();
            registry.Register<SendBirthdayGreetingsCommand, SendBirthdayGreetingsCommandHandler>();

            var employeeFileFullPath = ConfigurationManager.AppSettings["EmployeeFileFullPath"];
            Logger.InfoFormat("EmployeeFileFullPath: {0}", employeeFileFullPath);

            var smtpHostAddress = ConfigurationManager.AppSettings["SMTPHostAddress"];
            Logger.InfoFormat("SMTPHostAddress: {0}", smtpHostAddress);

            var smtpHostPort =  ConfigurationManager.AppSettings["SMTPHostPort"];
            Logger.InfoFormat("SMTPHostPort: {0}", smtpHostPort);

            var smtpHostUsername = ConfigurationManager.AppSettings["SMTPHostUsername"];
            var smtpHostPassword = ConfigurationManager.AppSettings["SMTPHostPassword"];

            var smtpServerSettings = new SmtpServerSettings(smtpHostAddress, Int32.Parse(smtpHostPort), smtpHostUsername, smtpHostPassword);
            var simpleHandlerFactory = new SimpleHandlerFactory(employeeFileFullPath, smtpServerSettings, Logger);
            CommandProcessorBuilder
                .With()
                .Handlers(new HandlerConfiguration(registry, simpleHandlerFactory))
                .NoPolicy()
                .Logger(Logger)
                .NoTaskQueues()
                .RequestContextFactory(new InMemoryRequestContextFactory())
                .Build()
                .Send(new SendBirthdayGreetingsCommand(DateTime.Now));
        }
    }
}
