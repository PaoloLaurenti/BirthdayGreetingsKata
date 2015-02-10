using System;
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
        private readonly IEmailChannel _emailChannel;
        private readonly EmailGreetingsGateway _sut;
        private readonly GreetingDto _exampleGreeting1 = new GreetingDto("FirstName1", "email1@Address.com");

        public EmailGreetingsGatewayTest()
        {
            _emailChannel = A.Fake<IEmailChannel>();
            _sut = new EmailGreetingsGateway(_emailChannel);
        }

        [Fact]
        public void Should_not_send_anything_when_it_is_required_to_send_a_null_greeting()
        {
            _sut.Deliver(null);

            A.CallTo(() => _emailChannel.Send(null)).WithAnyArguments().MustNotHaveHappened();
        }

        [Fact]
        public void Should_send_one_email_when_it_is_required_to_send_a_greeting()
        {
            _sut.Deliver( _exampleGreeting1);

            A.CallTo(() => _emailChannel
                            .Send(A<MailMessage>.That.Matches(mm => AssertMailMessageIsCoherentWithGreeting(_exampleGreeting1, mm))))
                            .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void Should_raise_an_exception_when_it_is_unable_to_send_an_email()
        {
            var exceptionMessage = string.Format(@"Error occurred sending mail to {0}: {1}", _exampleGreeting1.Email, CustomExceptionMessage1);
            A.CallTo(() => _emailChannel.Send(null)).WithAnyArguments().Throws(new Exception(CustomExceptionMessage1));

            _sut.Invoking(x => x.Deliver(_exampleGreeting1)).ShouldThrow<GreetingsGatewayException>().WithMessage(exceptionMessage);
        }

        private static bool AssertMailMessageIsCoherentWithGreeting(GreetingDto greeting, MailMessage mailMessage)
        {
            mailMessage.Subject.ShouldBeEquivalentTo("Happy birthday!");
            mailMessage.Body.ShouldBeEquivalentTo(string.Format("Happy birthday, dear {0}!", greeting.FirstName));
            mailMessage.To.Single().Address.ShouldBeEquivalentTo(greeting.Email);
            return true;
        }
    }
}
