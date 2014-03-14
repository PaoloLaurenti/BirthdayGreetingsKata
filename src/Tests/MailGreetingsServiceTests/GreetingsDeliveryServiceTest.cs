using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BirthdayGreetings;
using MailGreetingsService;
using NUnit.Framework;
using SharpTestsEx;

namespace MailGreetingsServiceTests
{
    [TestFixture]
    public class GreetingsDeliveryServiceTest
    {
        private readonly string _mailFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Tmp");

        [SetUp]
        public void Init()
        {
            if (Directory.Exists(_mailFolderPath))
                Directory.Delete(_mailFolderPath, true);

            Directory.CreateDirectory(_mailFolderPath);
        }

        [Test]
        public void ShuldSendNoGreetingsWhenThereAreNoGreetingsToSend()
        {
            var sut = new MailGreetingsDeliveryService();

            sut.Deliver(new List<GreetingDto>());

            GetMailSent().Count().Should().Be.EqualTo(0);
        }

        private IEnumerable<FileStream> GetMailSent()
        {
            return Directory.GetFiles(_mailFolderPath).Select(File.OpenRead).ToList();
        }
    }
}
