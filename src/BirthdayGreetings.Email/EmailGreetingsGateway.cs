using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using BirthdayGreetings.Core.Greetings;

namespace BirthdayGreetings.Email
{
    public class EmailGreetingsGateway : IGreetingsGateway
    {
        private readonly IEmailChannel _emailChannel;

        public EmailGreetingsGateway(IEmailChannel emailChannel)
        {
            _emailChannel = emailChannel;
        }

        public void Deliver(IEnumerable<GreetingDto> greetings)
        {
            if (greetings == null || !greetings.Any())
                return;
            greetings
                .ToList()
                .ForEach(singleGreeting => _emailChannel.Send(CreateMailMessageFor(singleGreeting)));
        }

        private static MailMessage CreateMailMessageFor(GreetingDto singleGreeting)
        {
            var mailMessage = new MailMessage();
            mailMessage.To.Add(singleGreeting.Email);
            mailMessage.Subject = "Happy birthday!";
            mailMessage.Body = string.Format("Happy birthday, dear {0}!", singleGreeting.FirstName);
            return mailMessage;
        }
    }
}