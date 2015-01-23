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

            var mailMessage = new MailMessage();
            var greeting = greetings.First();
            mailMessage.To.Add(greeting.Email);
            mailMessage.Subject = "Happy birthday!";
            mailMessage.Body = string.Format("Happy birthday, dear {0}!", greeting.FirstName);
            _emailChannel.Send(mailMessage);
        }
    }
}