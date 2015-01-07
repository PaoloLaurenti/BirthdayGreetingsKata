using BirthdayGreetings.Core.Employees;

namespace BirthdayGreetings.Core.Greetings
{
    public class GreetingDtoFactory
    {
        public static GreetingDto CreateGreetingFor(EmployeeDto e)
        {
            return new GreetingDto { FirstName = e.FirstName, Email = e.Email };
        }
    }
}