using System;
using System.Configuration;
using BirthdayGreetings.Core;
using Polly;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
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

            var smtpHostPort = ConfigurationManager.AppSettings["SMTPHostPort"];
            logger.InfoFormat("SMTPHostPort: {0}", smtpHostPort);

            var smtpHostUsername = ConfigurationManager.AppSettings["SMTPHostUsername"];
            var smtpHostPassword = ConfigurationManager.AppSettings["SMTPHostPassword"];

            var factory = new MsSqlMessageStoreFactory(brighterMssqlDBConnectionString, logger);

            var smtpServerSettings = new SmtpServerSettings(smtpHostAddress, Int32.Parse(smtpHostPort), smtpHostUsername, smtpHostPassword);
            var simpleHandlerFactory = new SimpleHandlerFactory(employeeFileFullPath, smtpServerSettings, factory, logger);

            var messageMapperRegistry = new MessageMapperRegistry(new SimpleMessageMapperFactory())
                {
                    {
                        typeof (SendBirthdayGreetingsCommand), typeof (SendBirthdayGreetingCommandMessageMapper)
                    }
                };
            var messagingConfiguration = new MessagingConfiguration(factory.GetMsSqlMessageStore("SendBirthdayGreetings"),
                                                                    new RmqMessageProducer(logger),
                                                                    messageMapperRegistry);

            var retryPolicy = Policy.Handle<Exception>().WaitAndRetry(new[] { TimeSpan.FromMilliseconds(50), TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(150) });
            var circuitBreakerPolicy = Policy.Handle<Exception>().CircuitBreaker(1, TimeSpan.FromMilliseconds(500));
            var policyRegistry = new PolicyRegistry { { CommandProcessor.RETRYPOLICY, retryPolicy }, { CommandProcessor.CIRCUITBREAKER, circuitBreakerPolicy } };

            CommandProcessorBuilder
                .With()
                .Handlers(new HandlerConfiguration(registry, simpleHandlerFactory))
                .Policies(policyRegistry)
                .Logger(logger)
                .TaskQueues(messagingConfiguration)
                .RequestContextFactory(new InMemoryRequestContextFactory())
                .Build()
                .Post(new SendBirthdayGreetingsCommand(new DateTime(2015, 7, 6)));

            Console.WriteLine("Press a button to terminate...");
            Console.ReadKey();
        }
    }
}
