using System;
using BirthdayGreetings.Core;
using BirthdayGreetings.Email;
using BirthdayGreetings.FileSystem;
using Common.Logging;
using paramore.brighter.commandprocessor;

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
            var fileSystemEmployeeGateway = new FileSystemEmployeeGateway(_employeeFileFullPath);
            var emailChannel = new EmailChannel(_smtpServerSettings.HostAddress, _smtpServerSettings.Port, _smtpServerSettings.Username, _smtpServerSettings.Password);
            var emailGreetingsGateway = new EmailGreetingsGateway(emailChannel);
            return new SendBirthdayGreetingsCommandHandler(fileSystemEmployeeGateway, null, _logger);
        }

        public void Release(IHandleRequests handler)
        {
        }
    }
}