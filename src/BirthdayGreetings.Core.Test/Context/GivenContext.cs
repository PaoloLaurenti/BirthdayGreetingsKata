using System;
using System.Collections.Generic;
using System.Linq;
using BirthdayGreetings.Core.Employees;
using BirthdayGreetings.Core.Greetings;
using BirthdayGreetings.Core.Test.Creation;
using FakeItEasy;

namespace BirthdayGreetings.Core.Test.Context
{
    internal class GivenContext
    {
        private readonly IEmployeeGateway _employeesGateway;
        private readonly MockSendBirthdayGreetingsByEmailCommandHandler _sendBirthdayGreetingsByEmailCommandHandler;
        private readonly DateTime _chosenDate;
        private readonly List<EmployeeDto> _employeesWihtBirthdateEqualToChosenDate;

        internal GivenContext(IEmployeeGateway employeesGateway, MockSendBirthdayGreetingsByEmailCommandHandler sendBirthdayGreetingsByEmailCommandHandler, DateTime chosenDate)
        {
            _employeesGateway = employeesGateway;
            _sendBirthdayGreetingsByEmailCommandHandler = sendBirthdayGreetingsByEmailCommandHandler;
            _chosenDate = chosenDate;
            _employeesWihtBirthdateEqualToChosenDate = new List<EmployeeDto>();
        }

        internal void NoEmployee()
        {
            Given(new List<EmployeeDto>());
        }

        internal void AllEmployeesWithDateOfBirthDifferentThanChosenDate()
        {
            Given(GetARandomListOfEmployeesWithBirthdateDifferentFromChosenDate());
        }

        private void Given(IEnumerable<EmployeeDto> employees)
        {
            A.CallTo(() => _employeesGateway.GetEmployees()).Returns(employees);
        }

        internal void OnlyOneEmployeeWithDateOfBirthEqualToTheChosenDate()
        {
            _employeesWihtBirthdateEqualToChosenDate.Add(EmployeeDtoFactory.CreateRandomEmployeeDtoWithDateOfBirthEqualTo(_chosenDate));
            GivenEmployees();
        }

        internal void ManyEmployeesWithDateOfBirthEqualToTheChosenDate()
        {
            Enumerable
                .Range(0, 3)
                .ToList()
                .ForEach(x => _employeesWihtBirthdateEqualToChosenDate.Add(EmployeeDtoFactory.CreateRandomEmployeeDtoWithDateOfBirthEqualTo(_chosenDate)));
            GivenEmployees();
        }

        internal void NullEmployee()
        {
            Given(null);
        }

        private void GivenEmployees()
        {
            Given(GetARandomListOfEmployeesWithBirthdateDifferentFromChosenDate().Union(_employeesWihtBirthdateEqualToChosenDate));
        }

        private IEnumerable<EmployeeDto> GetARandomListOfEmployeesWithBirthdateDifferentFromChosenDate()
        {
            return GetAListWithRandomSizeOf(() => EmployeeDtoFactory.CreateRandomEmployeeDtoWithDateOfBirthDifferentFrom(_chosenDate));
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

        internal void EmployeeGatewayExceptionRetrievingEmployees()
        {
            A.CallTo(() => _employeesGateway.GetEmployees()).Throws(x => new EmployeeGatewayException("Exception retrieving employees"));
        }

        internal void GreetingsChannelGatewayExceptionSendingGreetings()
        {
            _sendBirthdayGreetingsByEmailCommandHandler.ThrowSendingGreetings(new GreetingsGatewayException("Exception sending greetings"));
        }
    }
}