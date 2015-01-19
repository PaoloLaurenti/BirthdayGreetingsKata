using System;
using System.Collections.Generic;
using BirthdayGreetings.Common;
using BirthdayGreetings.Common.Extensions;
using BirthdayGreetings.Core.Employees;

namespace BirthdayGreetings.Core
{
    internal class SendGreetingsStrategy
    {
        internal static IMaybe<List<EmployeeDto>> GetOnlyEmployeesToSendGreetingsTo(List<Employee> allEmployees, DateTime chosenDate)
        {
            var employeesToSendGreetingsTo = new List<EmployeeDto>();
            allEmployees.ForEach(singleEmployee => singleEmployee.DoOnBirthday(chosenDate, employeesToSendGreetingsTo.Add));
            return employeesToSendGreetingsTo.ToMaybe();
        }
    }
}