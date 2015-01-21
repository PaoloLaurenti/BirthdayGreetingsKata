using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using BirthdayGreetings.Core.Employees;

namespace BirthdayGreetings.FileSystem
{
    public class FileSystemEmployeeGateway : IEmployeeGateway
    {
        private readonly string _employeeFileFullPath;

        public FileSystemEmployeeGateway(string employeeFileFullPath)
        {
            _employeeFileFullPath = employeeFileFullPath;
        }

        public IEnumerable<EmployeeDto> GetEmployees()
        {
            var employeesOnFile = new List<EmployeeDto>();
            var employeesRows = File.ReadAllLines(_employeeFileFullPath).Skip(1).ToList();
            if (employeesRows.Any())
                employeesOnFile.AddRange(employeesRows.Select(CreateEmployeeDtoBy));
            return employeesOnFile;
        }

        private static EmployeeDto CreateEmployeeDtoBy(string employeeRow)
        {
            var employeeData = employeeRow.Split(',').Select(x => x.Trim()).ToList();
            return new EmployeeDto(employeeData[0], employeeData[1], GetDateOfBirth(employeeData), employeeData[3]);
        }

        private static DateTime GetDateOfBirth(IReadOnlyList<string> employeeData)
        {
            return DateTime.ParseExact(employeeData[2], "yyyy/MM/dd", CultureInfo.InvariantCulture);
        }
    }
}