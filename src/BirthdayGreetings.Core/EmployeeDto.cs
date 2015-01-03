using System;

namespace BirthdayGreetings.Core
{
    public class EmployeeDto
    {
        public EmployeeDto(string lastName, string firstName, DateTime dateOfBirth, string email)
        {
            LastName = lastName;
            FirstName = firstName;
            DateOfBirth = dateOfBirth;
            Email = email;
        }

        public string LastName { get; private set; }
        public string FirstName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string Email { get; private set; }
    }
}