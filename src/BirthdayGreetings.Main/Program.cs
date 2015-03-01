using System;
using System.Configuration;
using BirthdayGreetings.Core;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using paramore.brighter.commandprocessor.messagestore.mssql;
using paramore.brighter.commandprocessor.messaginggateway.rmq;

namespace BirthdayGreetings.Main
{
    class Program
    {

        static void Main(string[] args)
        {
            var logger = LogProvider.For<Program>();

            var registry = new SubscriberRegistry();
            registry.Register<SendBirthdayGreetingsCommand, SendBirthdayGreetingsCommandHandler>();

            var employeeFileFullPath = ConfigurationManager.AppSettings["EmployeeFileFullPath"];
            logger.InfoFormat("EmployeeFileFullPath: {0}", employeeFileFullPath);

            var brighterMssqlDBConnectionString = ConfigurationManager.ConnectionStrings["BirthdayGreetings_Brighter"].ConnectionString;
            logger.InfoFormat("BrighterMssqlDBConnectionString: {0}", brighterMssqlDBConnectionString);

            var smtpHostAddress = ConfigurationManager.AppSettings["SMTPHostAddress"];
            logger.InfoFormat("SMTPHostAddress: {0}", smtpHostAddress);

            var smtpHostPort =  ConfigurationManager.AppSettings["SMTPHostPort"];
            logger.InfoFormat("SMTPHostPort: {0}", smtpHostPort);

            var smtpHostUsername = ConfigurationManager.AppSettings["SMTPHostUsername"];
            var smtpHostPassword = ConfigurationManager.AppSettings["SMTPHostPassword"];

            var smtpServerSettings = new SmtpServerSettings(smtpHostAddress, Int32.Parse(smtpHostPort), smtpHostUsername, smtpHostPassword);
            var simpleHandlerFactory = new SimpleHandlerFactory(employeeFileFullPath, smtpServerSettings, logger);

            var messagingConfiguration = new MessagingConfiguration(GetMsSqlMessageStore(brighterMssqlDBConnectionString, logger), 
                                                                    new RmqMessageProducer(logger),
                                                                    new MessageMapperRegistry(new SimpleMessageMapperFactory()));
            CommandProcessorBuilder
                .With()
                .Handlers(new HandlerConfiguration(registry, simpleHandlerFactory))
                .NoPolicy()
                .Logger(logger)
                .TaskQueues(messagingConfiguration)
                .RequestContextFactory(new InMemoryRequestContextFactory())
                .Build()
                .Send(new SendBirthdayGreetingsCommand(DateTime.Now));
        }

        private static MsSqlMessageStore GetMsSqlMessageStore(string brighterMssqlDBConnectionString, ILog logger)
        {
            var msSqlMessageStoreConfiguration = new MsSqlMessageStoreConfiguration(brighterMssqlDBConnectionString,
                                                                                    "BirthdayGreetingsSend",
                                                                                    MsSqlMessageStoreConfiguration.DatabaseType.MsSqlServer);
            return new MsSqlMessageStore(msSqlMessageStoreConfiguration, logger);
        }
    }
}
