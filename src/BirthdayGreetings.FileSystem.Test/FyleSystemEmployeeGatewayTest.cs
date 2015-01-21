﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BirthdayGreetings.Core.Employees;
using FluentAssertions;
using Xunit;

namespace BirthdayGreetings.FileSystem.Test
{
    public class FyleSystemEmployeeGatewayTest
    {
        private string _employeeFileFullPath;
        private readonly FileSystemEmployeeGateway _sut;

        public FyleSystemEmployeeGatewayTest()
        {
            PrepareEmployeeFile();
            _sut = new FileSystemEmployeeGateway(_employeeFileFullPath);
        }

        [Fact]
        public void Should_provide_an_empty_list_of_employees_when_file_is_empty()
        {
            File.WriteAllText(_employeeFileFullPath, "");
            
            var employees = _sut.GetEmployees().ToList();

            employees.Any().Should().BeFalse("It should not provide anything when file is empty");
        }

        [Fact]
        public void Should_provide_a_list_with_one_employee_when_file_contains_only_one_employee_information()
        {
            var employeeOnFile = new EmployeeDto("EmployeeLastName", "EmployeeFirstName", DateTime.Now.AddYears(-30).Date, "EmployeeEmail");
            PutEmployeesInformationInEmployeesFile(new List<EmployeeDto> { employeeOnFile });

            var employees = _sut.GetEmployees().ToList(); 

            employees.Single().ShouldBeEquivalentTo(employeeOnFile, "It shoud provide the only employee listed in the employee file");
        }

        private void PrepareEmployeeFile()
        {
            _employeeFileFullPath = Path.GetTempFileName();
            File.WriteAllLines(_employeeFileFullPath, new List<string> {"last_name, first_name, date_of_birth, email"});
        }

        private void PutEmployeesInformationInEmployeesFile(IEnumerable<EmployeeDto> employeesToPutInFile)
        {
            var employeesRows = employeesToPutInFile.Select(x => string.Format("{0}, {1}, {2}, {3}", x.LastName, x.FirstName, x.DateOfBirth.ToString("yyyy/MM/dd"), x.Email));
            File.AppendAllLines(_employeeFileFullPath, employeesRows);
        }

        //TODO LIST
        // Should_provide_an_empty_list_when_employees_file_contains_only_the_header_row
        // Should_provide_a_list_with_all_employees_present_in_the_file
        // Should_raise_exception_if_it_is_unable_to_interpret_employees_information
        // Should_raise_exception_when_it_is_unable_to_access_file
    }
}
