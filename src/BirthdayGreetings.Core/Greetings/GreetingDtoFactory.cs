using System.Collections.Generic;
using System.Linq;
using BirthdayGreetings.Common;
using BirthdayGreetings.Common.Extensions;
using BirthdayGreetings.Core.Employees;

namespace BirthdayGreetings.Core.Greetings
{
    public class GreetingDtoFactory
    {
        public static IMaybe<List<GreetingDto>> CreateGreetingFor(List<EmployeeDto> employees)
        {
            return employees.Select(CreateGreetingFor).ToList().ToMaybe();
        }

        public static GreetingDto CreateGreetingFor(EmployeeDto e)
        {
            return new GreetingDto (e.FirstName, e.Email);
        }
    }
}