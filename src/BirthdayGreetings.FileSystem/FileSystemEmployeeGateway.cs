using System.Collections.Generic;
using BirthdayGreetings.Core.Employees;

namespace BirthdayGreetings.FileSystem
{
    public class FileSystemEmployeeGateway : IEmployeeGateway
    {
        public IEnumerable<EmployeeDto> GetEmployees()
        {
            return new List<EmployeeDto>();
        }
    }
}