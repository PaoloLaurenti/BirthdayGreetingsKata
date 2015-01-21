using System.Linq;
using FluentAssertions;
using Xunit;

namespace BirthdayGreetings.FileSystem.Test
{
    public class FyleSystemEmployeeGatewayTest
    {
        [Fact]
        public void Should_provide_an_empty_list_of_employees_when_file_is_empty()
        {
            var sut = new FileSystemEmployeeGateway();

            var employees = sut.GetEmployees().ToList();

            employees.Any().Should().BeFalse("It should not provide anything when file is empty");
        }

        //TODO LIST
        // Should_provide_a_list_with_one_employee_when_file_contains_only_one_employee_information()
        // Should_provide_a_list_with_all_employees_present_in_the_file
        // Should_raise_exception_if_it_is_unable_to_interpret_employees_information
        // Should_raise_exception_when_it_is_unable_to_access_file
    }
}
