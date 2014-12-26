using System;
using System.Collections.Generic;
using Common.Logging;
using FakeItEasy;
using Xunit;

namespace BirthdayGreetingsEngine.Test
{
    public class SendBirthdayGreetingsUseCaseTest
    {
        [Fact]
        public void Should_not_send_greetings_if_no_employee_has_been_retrieved()
        {
            var log = A.Fake<ILog>();
            var greetingsChannelGateway = A.Fake<IGreetingsChannelGateway>();
            var employeesGateway = A.Fake<IEmployeeGateway>();
            A.CallTo(() => employeesGateway.GetEmployees()).Returns(new List<EmployeeDto>());
            var sut = new SendBirthdayGreetingsCommandHandler(employeesGateway, greetingsChannelGateway, log);
            var command = new SendBirthdayGreetingsCommand(DateTime.Now);
            
            sut.Handle(command);

            A.CallTo(() => greetingsChannelGateway.Send(null)).WithAnyArguments().MustNotHaveHappened();
        }

        //TODO LIST
        // - Should_not_send_greetings_if_null_employees_list_has_been_retrieved
        // - Should_not_send_greetings_if_all_employees_have_their_birthdays_different_than_today
        // - Should_send_one_greetings_to_one_employee_when_only_one_employee_has_his_birthday_equal_to_today
        // - Should_send_many_greetings_to_all_employees_that_have_their_birthdays_equal_to_today
        // - Should_raise_exception_when_it_is_unable_to_retrieve_employees
        // - Should_notify_skipped_employees_that_have_no_name
        // - Should_notify_skipped_employees_that_have_no_birthday_date
        // - Should_raise_exception_when_it_is_unable_to_send_greetings
        // - Should_send_greetings_on_Februrary_29th_to_employee_with_birthday_equal_to_Februrary_29th_during_leap_years
        // - Should_send_greetings_on_Februrary_28th_to_employee_with_birthday_equal_to_Februrary_29th_during_not_leap_years
    }
}
