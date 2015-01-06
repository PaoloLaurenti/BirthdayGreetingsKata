using System;

namespace BirthdayGreetings.Core.Employees
{
    internal class Employee
    {
        private readonly string _lastName;
        private readonly string _firstName;
        private readonly DateTime _dateOfBirth;
        private readonly string _email;

        internal static Employee Create(EmployeeDto employeeDto)
        {
            return new Employee(employeeDto.LastName, employeeDto.FirstName, employeeDto.DateOfBirth, employeeDto.Email);            
        }

        private Employee(string lastName, string firstName, DateTime dateOfBirth, string email)
        {
            _lastName = lastName;
            _firstName = firstName;
            _dateOfBirth = dateOfBirth;
            _email = email;
        }

        internal void DoOnBirthday(DateTime date, Action<EmployeeDto> toDo)
        {
            if (_dateOfBirth.Month == date.Month && _dateOfBirth.Day == date.Day)
                toDo(ToDto());
        }

        private EmployeeDto ToDto()
        {
            return new EmployeeDto(_lastName, _firstName, _dateOfBirth, _email);
        }
    }
}