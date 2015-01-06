using System;
using System.Collections.Generic;
using System.Linq;
using BirthdayGreetings.Core.Test.Creation;
using FakeItEasy;

namespace BirthdayGreetings.Core.Test.Context
{
    internal class GivenContext
    {
        private readonly IEmployeeGateway _employeesGateway;
        private readonly DateTime _chosenDate;
        private readonly List<EmployeeDto> _employeesWihtBirthdateEqualToChosenDate;

        internal GivenContext(IEmployeeGateway employeesGateway, DateTime chosenDate)
        {
            _employeesGateway = employeesGateway;
            _chosenDate = chosenDate;
            _employeesWihtBirthdateEqualToChosenDate = new List<EmployeeDto>();
        }

        internal void NoEmployee()
        {
            Given(new List<EmployeeDto>());
        }

        private void Given(IEnumerable<EmployeeDto> employees)
        {
            A.CallTo(() => _employeesGateway.GetEmployees()).Returns(employees);
        }

        internal void AllEmployeesWithDateOfBirthDifferentThanChosenDate()
        {
            Given(GetAListOfEmployeesWithBirthdateDifferentFromChosenDate());
        }

        private IEnumerable<EmployeeDto> GetAListOfEmployeesWithBirthdateDifferentFromChosenDate()
        {
            return GetAListWithRandomSizeOf(() => EmployeeDtoFactory.CreateRandomEmployeeDtoWithDateOfBirthDifferentFrom(_chosenDate));
        }

        internal void OnlyOneEmployeeWithDateOfBirthEqualToTheChosenDate()
        {
            _employeesWihtBirthdateEqualToChosenDate.Add(EmployeeDtoFactory.CreateRandomEmployeeDtoWithDateOfBirthEqualTo(_chosenDate));
            Given(GetAListOfEmployeesWithBirthdateDifferentFromChosenDate().Union(_employeesWihtBirthdateEqualToChosenDate));
        }

        private static IEnumerable<T> GetAListWithRandomSizeOf<T>(Func<T> factory)
        {
            var size = new Random(DateTime.Now.Millisecond).Next(1, 100);
            return Enumerable.Range(0, size).Select(x => factory()).ToList();
        }

        internal void DoWithGivenEmployeesWithDateOfBirthEqualToChosenDate(Action<IEnumerable<EmployeeDto>> action)
        {
            action(_employeesWihtBirthdateEqualToChosenDate);
        }
    }
}