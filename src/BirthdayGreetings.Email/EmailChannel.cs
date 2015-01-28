using System.Net.Mail;

namespace BirthdayGreetings.Email
{
    public class EmailChannel : IEmailChannel
    {
        private readonly string _host;
        private readonly int _port;

        public EmailChannel(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public void Send(MailMessage email)
        {
            using (var client = new SmtpClient(_host, _port))
            {
                client.Send(email); 
            }
        }
    }
}