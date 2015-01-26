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
                .ForEach(singleGreeting =>
                    {
                        try
                        {
                            _emailChannel.Send(CreateMailMessageFor(singleGreeting));
                        }
                        catch (System.Exception ex)
                        {
                            throw new GreetingsGatewayException(string.Format("Error occurred sending mail to {0}: {1}", singleGreeting.Email, ex.Message), ex);
                        }
                    });
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