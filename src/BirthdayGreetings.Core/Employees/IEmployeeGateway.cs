using System.Collections.Generic;

namespace BirthdayGreetings.Core.Employees
{
    public interface IEmployeeGateway
    {
        IEnumerable<EmployeeDto> GetEmployees();
    }
}