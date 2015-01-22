using System.Net.Mail;

namespace BirthdayGreetings.Email
{
    public interface IEmailChannel
    {
        void Send(MailMessage email);
    }
}