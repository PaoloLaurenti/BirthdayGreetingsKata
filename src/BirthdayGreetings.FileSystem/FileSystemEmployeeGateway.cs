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
            var employeesRows = ReadAllFileLines().Skip(1).ToList();
            if (employeesRows.Any())
                employeesOnFile.AddRange(employeesRows.Select(CreateEmployeeDtoBy));
            return employeesOnFile;
        }

        private IEnumerable<string> ReadAllFileLines()
        {
            try
            {
                return File.ReadAllLines(_employeeFileFullPath);
            }
            catch (Exception ex)
            {
                throw new EmployeeGatewayException(string.Format("Error occurred accessing file located to {0}: {1}", _employeeFileFullPath, ex.Message), ex);
            }
        }

        private static EmployeeDto CreateEmployeeDtoBy(string employeeRow)
        {
            try
            {
                var employeeData = employeeRow.Split(',').Select(x => x.Trim()).ToList();
                return new EmployeeDto(employeeData[0], employeeData[1], GetDateOfBirth(employeeData), employeeData[3]);
            }
            catch (Exception ex)
            {
                throw new EmployeeGatewayException(string.Format("Error occurred interpreting employee data of this row {0}: {1}", employeeRow, ex.Message), ex);
            }
        }

        private static DateTime GetDateOfBirth(IReadOnlyList<string> employeeData)
        {
            return DateTime.ParseExact(employeeData[2], "yyyy/MM/dd", CultureInfo.InvariantCulture);
        }
    }
}