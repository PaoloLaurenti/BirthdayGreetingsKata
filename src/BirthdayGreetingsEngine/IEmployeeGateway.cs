using System.Collections.Generic;

namespace BirthdayGreetingsEngine
{
    public interface IEmployeeGateway
    {
        IEnumerable<EmployeeDto> GetEmployees();
    }
}