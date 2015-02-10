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

        public void Deliver(GreetingDto greeting)
        {
            if (greeting == null)
                return;
            try
            {
                _emailChannel.Send(CreateMailMessageFor(greeting));
            }
            catch (System.Exception ex)
            {
                throw new GreetingsGatewayException(string.Format("Error occurred sending mail to {0}: {1}", greeting.Email, ex.Message));
            }
        }

        private static MailMessage CreateMailMessageFor(GreetingDto singleGreeting)
        {
            var mailMessage = new MailMessage
                {
                    From = new MailAddress("acme@company.com"),
                    Subject = "Happy birthday!",
                    Body = string.Format("Happy birthday, dear {0}!", singleGreeting.FirstName)
                };
            mailMessage.To.Add(singleGreeting.Email);
            return mailMessage;
        }
    }
}