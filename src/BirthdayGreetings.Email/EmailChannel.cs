using System.Net;
using System.Net.Mail;

namespace BirthdayGreetings.Email
{
    public class EmailChannel : IEmailChannel
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;

        public EmailChannel(string host, int port, string username, string password)
        {
            _host = host;
            _port = port;
            _username = username;
            _password = password;
        }

        public void Send(MailMessage email)
        {
            using (var client = new SmtpClient(_host, _port))
            {
                client.Credentials = new NetworkCredential(_username, _password);
                client.EnableSsl = true;
                client.Send(email); 
            }
        }
    }
}