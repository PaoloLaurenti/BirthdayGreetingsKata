using BirthdayGreetings;
using NUnit.Framework;
using Rhino.Mocks;

namespace BirthdayGreetingsTests
{
    [TestFixture]
    public class BirthdayGreetingsTest
    {
        [Test]
        public void ShouldSendNoGreetingsWhenThereAreNotAnyEmployees()
        {
            var greetingsDeliverService = MockRepository.GenerateMock<GreetingsDeliveryService>();
            var sut = new BirthdayGreetingsEngine(greetingsDeliverService);

            sut.SendGreetings();

            greetingsDeliverService.AssertWasNotCalled(gds => gds.Deliver());
        }
    }
}
