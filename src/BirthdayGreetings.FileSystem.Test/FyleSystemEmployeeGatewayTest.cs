using System;
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
            EmptyEmployeeFile();
            
            var employees = _sut.GetEmployees().ToList();

            employees.Any().Should().BeFalse("It should not provide anything when employee file is empty");
        }

        [Fact]
        public void Should_provide_an_empty_list_getting_employees_when_employees_file_contains_only_the_header_row()
        {
            var employees = _sut.GetEmployees().ToList();

            employees.Any().Should().BeFalse("It should not provide anything when employee file contains only header row");
        }

        [Fact]
        public void Should_provide_a_list_with_one_employee_getting_employees_when_file_contains_only_one_employee_informations()
        {
            var employeeOnFile = new EmployeeDto("EmployeeLastName", "EmployeeFirstName", DateTime.Now.AddYears(-30).Date, "EmployeeEmail");
            PutEmployeesInformationInEmployeesFile(new List<EmployeeDto> { employeeOnFile });

            var employees = _sut.GetEmployees().ToList(); 

            employees.Single().ShouldBeEquivalentTo(employeeOnFile, "It shoud provide the only employee listed in the employee file");
        }

        [Fact]
        public void Should_provide_a_list_with_all_employees_present_in_the_file_when_getting_employees()
        {
            var firstEmployeeOnFile = new EmployeeDto("FirstEmployeeLastName", "FirstEmployeeFirstName", DateTime.Now.AddYears(-31).Date, "FirstEmployeeEmail");
            var secondEmployeeOnFile = new EmployeeDto("SecondEmployeeLastName", "SecondEmployeeFirstName", DateTime.Now.AddYears(-32).Date, "SecondEmployeeEmail");
            var thirdEmployeeOnFile = new EmployeeDto("ThirdEmployeeLastName", "ThirdEmployeeFirstName", DateTime.Now.AddYears(-33).Date, "ThirdEmployeeEmail");
            var employeesToPutInFile = new List<EmployeeDto> { firstEmployeeOnFile, secondEmployeeOnFile, thirdEmployeeOnFile };
            PutEmployeesInformationInEmployeesFile(employeesToPutInFile);

            var employees = _sut.GetEmployees().ToList();

            employees.Count.Should().Be(employeesToPutInFile.Count);
            employeesToPutInFile
                .ForEach(e => employees
                                .First(x => x.LastName == e.LastName)
                                .ShouldBeEquivalentTo(e));
        }

        [Fact]
        public void Shoud_raise_exception_when_it_is_unable_to_access_the_employee_file_getting_employees()
        {
            using (File.OpenWrite(_employeeFileFullPath))
            {
                _sut.Invoking(x => x.GetEmployees()).ShouldThrow<EmployeeGatewayException>();
            }
        }

        [Fact]
        public void Shoud_raise_exception_getting_employees_when_employees_file_does_not_exist()
        {
            File.Delete(_employeeFileFullPath);
            _sut.Invoking(x => x.GetEmployees()).ShouldThrow<EmployeeGatewayException>();            
        }

        [Fact]
        public void Should_raise_exception_if_it_is_unable_to_interpret_employees_information_getting_employees()
        {
            File.AppendAllLines(_employeeFileFullPath, new [] {"LastName, FirstName, NotADate, Email"});

            _sut.Invoking(x => x.GetEmployees()).ShouldThrow<EmployeeGatewayException>();            
        }

        private void PrepareEmployeeFile()
        {
            _employeeFileFullPath = Path.GetTempFileName();
            File.WriteAllLines(_employeeFileFullPath, new List<string> {"last_name, first_name, date_of_birth, email"});
        }

        private void EmptyEmployeeFile()
        {
            File.WriteAllText(_employeeFileFullPath, "");
        }

        private void PutEmployeesInformationInEmployeesFile(IEnumerable<EmployeeDto> employeesToPutInFile)
        {
            var employeesRows = employeesToPutInFile.Select(x => string.Format("{0}, {1}, {2}, {3}", x.LastName, x.FirstName, x.DateOfBirth.ToString("yyyy/MM/dd"), x.Email));
            File.AppendAllLines(_employeeFileFullPath, employeesRows);
        }
    }
}
