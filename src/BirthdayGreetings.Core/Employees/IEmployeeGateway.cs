using System.Collections.Generic;
using BirthdayGreetings.Common;

namespace BirthdayGreetings.Core.Employees
{
    public interface IEmployeeGateway
    {
        IMaybe<IEnumerable<EmployeeDto>> GetEmployees();
    }
}