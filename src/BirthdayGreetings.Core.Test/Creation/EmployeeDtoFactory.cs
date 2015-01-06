using System;
using BirthdayGreetings.Core.Employees;
using BirthdayGreetings.Core.Test.Extension;

namespace BirthdayGreetings.Core.Test.Creation
{
    internal class EmployeeDtoFactory
    {
        internal static EmployeeDto CreateRandomEmployeeDtoWithDateOfBirthDifferentFrom(DateTime date)
        {
            return CreateRandomEmployeeDtoWithDateOfBirthEqualTo(date.GetANewDateWithDifferentDay());
        }

        internal static EmployeeDto CreateRandomEmployeeDtoWithDateOfBirthEqualTo(DateTime date)
        {
            var r = new Random(DateTime.Now.Millisecond);
            var lastName = string.Format("Lastname_{0}", r.Next());
            var firstName = string.Format("Firstname_{0}", r.Next());
            var email = string.Format("{0}.{1}@domain.com", firstName, lastName);
            return new EmployeeDto(lastName, firstName, date, email);
        }
    }
}