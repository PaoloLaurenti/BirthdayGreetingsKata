using System.Collections.Generic;
using System.Linq;
using BirthdayGreetings.Common;
using BirthdayGreetings.Common.Extensions;

namespace BirthdayGreetings.Core.Employees
{
    internal class EmployeeRepository
    {
        private readonly IEmployeeGateway _employeesGateway;

        internal EmployeeRepository(IEmployeeGateway employeesGateway)
        {
            _employeesGateway = employeesGateway;
        }

        internal IMaybe<List<Employee>> FindAll()
        {
            return _employeesGateway
                    .GetEmployees()
                    .Map(TransformDtosToEmployees);
        }

        private static IMaybe<List<Employee>> TransformDtosToEmployees(IEnumerable<EmployeeDto> employees)
        {
            return employees.Select(Employee.Create).ToList().ToMaybe();
        }
    }
}