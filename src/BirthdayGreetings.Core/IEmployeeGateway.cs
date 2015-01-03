using System.Collections.Generic;

namespace BirthdayGreetings.Core
{
    public interface IEmployeeGateway
    {
        IEnumerable<EmployeeDto> GetEmployees();
    }
}