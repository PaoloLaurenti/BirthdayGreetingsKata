namespace BirthdayGreetings.Main
{
    internal class SmtpServerSettings
    {
        internal SmtpServerSettings(string hostAddress, int port, string username, string password)
        {
            HostAddress = hostAddress;
            Port = port;
            Username = username;
            Password = password;
        }

        internal string HostAddress { get; private set; }
        internal int Port { get; private set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}