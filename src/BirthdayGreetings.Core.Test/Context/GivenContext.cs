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

        internal GivenContext(IEmployeeGateway employeesGateway, DateTime chosenDate)
        {
            _employeesGateway = employeesGateway;
            _chosenDate = chosenDate;
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
            var listSize = new Random(DateTime.Now.Millisecond).Next(1, 100);
            Given(GetAListOf(() => EmployeeDtoFactory.CreateRandomEmployeeDtoWithDateOfBirthDifferentFrom(_chosenDate), listSize));
        }

        private static IEnumerable<T> GetAListOf<T>(Func<T> factory, int size)
        {
            return Enumerable.Range(0, size).Select(x => factory()).ToList();
        }
    }
}