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
        private const string CustomExceptionMessage1 = "Custom exception message1";
        private const string CustomExceptionMessage2 = "Custom exception message2";
        private readonly IEmailChannel _emailChannel;
        private readonly EmailGreetingsGateway _sut;
        private readonly GreetingDto _exampleGreeting1 = new GreetingDto("FirstName1", "email1@Address.com");
        private readonly GreetingDto _exampleGreeting2 = new GreetingDto("FirstName2", "email2@Address.com");
        private readonly GreetingDto _exampleGreeting3 = new GreetingDto("FirstName3", "email3@Address.com");
        private readonly GreetingDto _exampleGreeting4 = new GreetingDto("FirstName4", "email4@Address.com");

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
            _sut.Deliver(new List<GreetingDto> { _exampleGreeting1 });

            A.CallTo(() => _emailChannel
                            .Send(A<MailMessage>.That.Matches(mm => AssertMailMessageIsCoherentWithGreeting(_exampleGreeting1, mm))))
                            .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void Should_send_an_email_for_every_greeting_present_in_the_list_that_has_been_required_to_send()
        {
            var greetings = new List<GreetingDto> { _exampleGreeting1, _exampleGreeting2, _exampleGreeting3 };

            _sut.Deliver(greetings);

            A.CallTo(() => _emailChannel
                            .Send(A<MailMessage>.That.Matches(mm => AssertMailMessageIsCoherentWithGreetings(greetings, mm), "Mail sent is not coherent with any expected greeting")))
                            .MustHaveHappened(Repeated.Exactly.Times(greetings.Count));
        }

        [Fact]
        public void Should_raise_an_exception_when_it_is_unable_to_send_an_email()
        {
            var exceptionMessage = string.Format(@"Error occurred sending mail to {0}: {1}", _exampleGreeting1.Email, CustomExceptionMessage1);
            A.CallTo(() => _emailChannel.Send(null)).WithAnyArguments().Throws(new Exception(CustomExceptionMessage1));

            _sut.Invoking(x => x.Deliver(new List<GreetingDto> { _exampleGreeting1 })).ShouldThrow<GreetingsGatewayException>().WithMessage(exceptionMessage);
        }

        [Fact]
        public void Should_try_to_send_all_the_required_greetings_when_it_is_unable_to_send_one_of_them()
        {
            var exceptionMessage1 = string.Format(@"Error occurred sending mail to {0}: {1}", _exampleGreeting2.Email, CustomExceptionMessage1);
            var exceptionMessage2 = string.Format(@"Error occurred sending mail to {0}: {1}", _exampleGreeting3.Email, CustomExceptionMessage2);
            var greetings = new List<GreetingDto> { _exampleGreeting1, _exampleGreeting2, _exampleGreeting3, _exampleGreeting4 };
            A.CallTo(() => _emailChannel
                            .Send(A<MailMessage>.That.Matches(mailMessage => CheckMailMessageIsCoherentWithGreeting(_exampleGreeting2, mailMessage))))
             .Throws(new Exception(CustomExceptionMessage1));
            A.CallTo(() => _emailChannel
                            .Send(A<MailMessage>.That.Matches(mailMessage => CheckMailMessageIsCoherentWithGreeting(_exampleGreeting3, mailMessage))))
             .Throws(new Exception(CustomExceptionMessage2));

            _sut.Invoking(x => x.Deliver(greetings)).ShouldThrow<GreetingsGatewayException>().WithMessage(string.Concat(exceptionMessage1, " - ", exceptionMessage2));

            A.CallTo(() => _emailChannel
                            .Send(A<MailMessage>.That.Matches(mm => CheckMailMessageIsCoherentWithGreeting(_exampleGreeting1, mm))))
                            .MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => _emailChannel
                            .Send(A<MailMessage>.That.Matches(mm => CheckMailMessageIsCoherentWithGreeting(_exampleGreeting4, mm))))
                            .MustHaveHappened(Repeated.Exactly.Once);
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

        private static bool CheckMailMessageIsCoherentWithGreeting(GreetingDto greeting, MailMessage mailMessage)
        {
            return mailMessage.Subject == "Happy birthday!"
                    && mailMessage.Body == string.Format("Happy birthday, dear {0}!", greeting.FirstName)
                    && mailMessage.To.Single().Address == greeting.Email;
        }
    }
}
