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
            return _employeesGateway.GetEmployees().Select(Employee.Create).ToList();
        }
    }
}