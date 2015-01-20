using BirthdayGreetings.Core.Employees;
using BirthdayGreetings.Core.Greetings;
using BirthdayGreetings.Core.Test.Context;
using Xunit;

namespace BirthdayGreetings.Core.Test
{
    public class SendBirthdayGreetingsScenarioTest
    {
        private readonly TestContext _context;

        public SendBirthdayGreetingsScenarioTest()
        {
            _context = new TestContext();
        }

        [Fact]
        public void Should_not_send_greetings_if_no_employee_has_been_retrieved()
        {
            _context
                .Given(x => x.NoEmployee())
                .When(x => x.SendingBirthdayGreetings())
                .Then(x => x.NoBirthdayGreetingsHaveBeenSent());
        }

        [Fact]
        public void Should_not_send_greetings_if_all_employees_have_their_birthdays_different_than_chosen_date()
        {
            _context
                .Given(x => x.AllEmployeesWithDateOfBirthDifferentThanChosenDate())
                .When(x => x.SendingBirthdayGreetings())
                .Then(x => x.NoBirthdayGreetingsHaveBeenSent());
        }

        [Fact]
        public void Should_send_one_greeting_to_one_employee_when_only_one_employee_has_his_birthday_equal_to_the_chosen_date()
        {
            _context
                .Given(x => x.OnlyOneEmployeeWithDateOfBirthEqualToTheChosenDate())
                .When(x => x.SendingBirthdayGreetings())
                .Then(x => x.BirthdayGreetingsHaveBeenSentToEmployeesWithBirthdateEqualToChosenDate());
        }

        [Fact]
        public void Should_send_many_greetings_to_all_employees_that_have_their_birthdays_equal_to_today()
        {
            _context
                .Given(x => x.ManyEmployeesWithDateOfBirthEqualToTheChosenDate())
                .When(x => x.SendingBirthdayGreetings())
                .Then(x => x.BirthdayGreetingsHaveBeenSentToEmployeesWithBirthdateEqualToChosenDate());
        }

        [Fact]
        public void Should_not_send_greetings_if_null_employees_list_has_been_retrieved()
        {
            _context
                .Given(x => x.NullEmployee())
                .When(x => x.SendingBirthdayGreetings())
                .Then(x => x.NoBirthdayGreetingsHaveBeenSent());
        }

        [Fact]
        public void Should_raise_employee_gateway_exception_when_it_is_unable_to_retrieve_employees()
        {
            _context
                .Given(x => x.EmployeeGatewayExceptionRetrievingEmployees())
                .When(x => x.SendingBirthdayGreetings())
                .Then(x => x.NoBirthdayGreetingsHaveBeenSent())
                .Then(x => x.AnExceptionHasBeenThrownAs<EmployeeGatewayException>());
        }

        [Fact]
        public void Should_raise_greetings_channel_gateway_exception_when_it_is_unable_to_send_greetings()
        {
            _context
                .Given(x => x.ManyEmployeesWithDateOfBirthEqualToTheChosenDate())
                .Given(x => x.GreetingsChannelGatewayExceptionSendingGreetings())
                .When(x => x.SendingBirthdayGreetings())
                .Then(x => x.AnExceptionHasBeenThrownAs<GreetingsChannelGatewayException>());
        }

        //TODO LIST
        // - Should_send_greetings_on_Februrary_29th_to_employees_with_birthday_equal_to_Februrary_29th_during_leap_years
        // - Should_send_greetings_on_Februrary_28th_to_employees_with_birthday_equal_to_Februrary_29th_during_not_leap_years
    }
}
