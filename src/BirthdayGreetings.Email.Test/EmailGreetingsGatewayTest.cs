using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using BirthdayGreetings.Core.Greetings;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace BirthdayGreetings.Email.Test
{
    public class EmailGreetingsGatewayTest
    {
        private readonly IEmailChannel _emailChannel;
        private readonly EmailGreetingsGateway _sut;

        public EmailGreetingsGatewayTest()
        {
            _emailChannel = A.Fake<IEmailChannel>();
            _sut = new EmailGreetingsGateway(_emailChannel);
        }

        [Fact]
        public void Should_not_send_anything_when_it_is_required_to_send_a_null_list_of_greetings()
        {
            _sut.Deliver(null);

            A.CallTo(() => _emailChannel.Send(null)).WithAnyArguments().MustNotHaveHappened();
        }

        [Fact]
        public void Should_not_send_anything_when_it_is_required_to_send_an_empty_lits_of_greetings()
        {
            _sut.Deliver(new List<GreetingDto>());

            A.CallTo(() => _emailChannel.Send(null)).WithAnyArguments().MustNotHaveHappened();
        }

        [Fact]
        public void Should_send_one_email_when_it_is_required_to_send_a_list_with_only_one_greeting()
        {
            var greeting = new GreetingDto("FirstName", "email@Address.com");

            _sut.Deliver(new List<GreetingDto> { greeting });

            A.CallTo(() => _emailChannel
                            .Send(A<MailMessage>.That.Matches(mm => AssertMailMessageIsCoherentWithGreeting(greeting, mm))))
                            .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void Should_send_an_email_for_every_greeting_present_in_the_list_that_has_been_required_to_send()
        {
            var greeting1 = new GreetingDto("FirstName1", "email1@Address.com");
            var greeting2 = new GreetingDto("FirstName2", "email2@Address.com");
            var greeting3 = new GreetingDto("FirstName3", "email3@Address.com");
            var greetings = new List<GreetingDto> { greeting1, greeting2, greeting3 };

            _sut.Deliver(greetings);

            A.CallTo(() => _emailChannel
                            .Send(A<MailMessage>.That.Matches(mm => AssertMailMessageIsCoherentWithGreetings(greetings, mm), "Mail sent is not coherent with any expected greeting")))
                            .MustHaveHappened(Repeated.Exactly.Times(greetings.Count));
        }

        private static bool AssertMailMessageIsCoherentWithGreetings(IEnumerable<GreetingDto> greetings, MailMessage mailMessage)
        {
            var greetingToCheck = greetings.FirstOrDefault(x => x.Email == mailMessage.To.Single().Address);
            return greetingToCheck != null && AssertMailMessageIsCoherentWithGreeting(greetingToCheck, mailMessage);
        }

        private static bool AssertMailMessageIsCoherentWithGreeting(GreetingDto greeting, MailMessage mailMessage)
        {
            mailMessage.Subject.ShouldBeEquivalentTo("Happy birthday!");
            mailMessage.Body.ShouldBeEquivalentTo(string.Format("Happy birthday, dear {0}!", greeting.FirstName));
            mailMessage.To.Single().Address.ShouldBeEquivalentTo(greeting.Email);
            return true;
        }

        //TODO
        // Should_send_an_email_for_every_greeting_present_in_the_list_that_has_been_required_to_send
        // Should_raise_an_exception_when_it_is_unable_to_send_an_email
        // Should_try_to_send_all_the_required_greetings_when_it_is_unable_to_send_one_of_them
    }
}
