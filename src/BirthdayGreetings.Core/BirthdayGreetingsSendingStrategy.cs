using System;
using System.Collections.Generic;
using BirthdayGreetings.Common;
using BirthdayGreetings.Common.Extensions;
using BirthdayGreetings.Core.Employees;

namespace BirthdayGreetings.Core
{
    internal class BirthdayGreetingsSendingStrategy
    {
        private readonly DateTime _chosenDate;

        private BirthdayGreetingsSendingStrategy(DateTime chosenDate)
        {
            _chosenDate = chosenDate;
        }

        internal static BirthdayGreetingsSendingStrategy Create(DateTime chosenDate)
        {
            return new BirthdayGreetingsSendingStrategy(chosenDate);
        }

        internal IMaybe<List<EmployeeDto>> GetOnlyEmployeesToSendGreetingsTo(List<Employee> allEmployees)
        {
            var employeesToSendGreetingsTo = new List<EmployeeDto>();
            allEmployees.ForEach(singleEmployee => singleEmployee.DoOnBirthday(_chosenDate, employeesToSendGreetingsTo.Add));
            return employeesToSendGreetingsTo.ToMaybe();
        }
    }
}