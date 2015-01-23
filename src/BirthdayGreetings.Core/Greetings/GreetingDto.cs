namespace BirthdayGreetings.Core.Greetings
{
    public class GreetingDto
    {
        public GreetingDto(string firstName, string email)
        {
            FirstName = firstName;
            Email = email;
        }

        public string Email { get; set; }
        public string FirstName { get; set; }
    }
}