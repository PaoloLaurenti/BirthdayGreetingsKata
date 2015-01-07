using System.Collections.Generic;
using System.Linq;

namespace BirthdayGreetings.Core.Employees
{
    internal class EmployeeRepository
    {
        private readonly IEmployeeGateway _employeesGateway;

        internal EmployeeRepository(IEmployeeGateway employeesGateway)
        {
            _employeesGateway = employeesGateway;
        }

        internal List<Employee> FindAll()
        {
            var employeeDtos = _employeesGateway.GetEmployees() ?? new List<EmployeeDto>();
            return employeeDtos.Select(Employee.Create).ToList();
        }
    }
}