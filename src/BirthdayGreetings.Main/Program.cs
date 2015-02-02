using System;
using System.Configuration;
using BirthdayGreetings.Core;
using Common.Logging;
using Common.Logging.Configuration;
using Common.Logging.Simple;
using paramore.brighter.commandprocessor;

namespace BirthdayGreetings.Main
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = GetLogger();
            var registry = new SubscriberRegistry();
            registry.Register<SendBirthdayGreetingsCommand, SendBirthdayGreetingsCommandHandler>();

            var employeeFileFullPath = ConfigurationManager.AppSettings["EmployeeFileFullPath"];
            logger.InfoFormat("EmployeeFileFullPath: {0}", employeeFileFullPath);

            var smtpHostAddress = ConfigurationManager.AppSettings["SMTPHostAddress"];
            logger.InfoFormat("SMTPHostAddress: {0}", smtpHostAddress);

            var smtpHostPort =  ConfigurationManager.AppSettings["SMTPHostPort"];
            logger.InfoFormat("SMTPHostPort: {0}", smtpHostPort);

            var smtpHostUsername = ConfigurationManager.AppSettings["SMTPHostUsername"];
            var smtpHostPassword = ConfigurationManager.AppSettings["SMTPHostPassword"];

            var smtpServerSettings = new SmtpServerSettings(smtpHostAddress, Int32.Parse(smtpHostPort), smtpHostUsername, smtpHostPassword);
            var simpleHandlerFactory = new SimpleHandlerFactory(employeeFileFullPath, smtpServerSettings, logger);
            var builder = CommandProcessorBuilder
                            .With()
                            .Handlers(new HandlerConfiguration(registry, simpleHandlerFactory))
                            .NoPolicy()
                            .Logger(logger)
                            .NoTaskQueues()
                            .RequestContextFactory(new InMemoryRequestContextFactory());
            builder
                .Build()
                .Send(new SendBirthdayGreetingsCommand(DateTime.Now));
        }

        private static ILog GetLogger()
        {
            var properties = new NameValueCollection();
            properties["showDateTime"] = "true";
            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter(properties);
            return LogManager.GetLogger(typeof (Program));
        }
    }
}
