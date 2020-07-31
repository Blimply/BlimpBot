using System.Collections.Generic;
using BlimpBot.Data.Models;
using BlimpBot.Interfaces;
using BlimpBot.Models.TelegramResponseModels;
using BlimpBot.Services;
using Moq;
using NUnit.Framework;

namespace BlimpBot.Tests
{
    [TestFixture]
    public class MessageParserServicesTests
    {
        private MessageParserServices messageParserServices;
        private Mock<ITelegramRepository> telegramRepositoryMock;
        private Mock<IWeatherRepository> weatherRepositoryMock;
        private Mock<IExchangeRateRepository> exchangeRateRepositoryMock;
        private Mock<IChatRepository> chatRepositoryMock;

        [SetUp]
        public void SetUp()
        {

            telegramRepositoryMock = new Mock<ITelegramRepository>();
            weatherRepositoryMock = new Mock<IWeatherRepository>();
            exchangeRateRepositoryMock = new Mock<IExchangeRateRepository>();
            chatRepositoryMock = new Mock<IChatRepository>();
            
            messageParserServices = new MessageParserServices(weatherRepositoryMock.Object,
                                                              exchangeRateRepositoryMock.Object,
                                                              telegramRepositoryMock.Object,
                                                              chatRepositoryMock.Object);
        }

        [Test]
        public void ShouldReturnEmptyIfMessagesIsTargetedAtAnotherBot()
        {
            string result = messageParserServices.GetResponse("message@ADifferentBot");
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void ShouldReturnNotEmptyIsMessageIsTargetedAtBlimpBot()
        {
            string result = messageParserServices.GetResponse("message@blimpbot");
            Assert.AreNotEqual(string.Empty, result);
            result = messageParserServices.GetResponse("/command@blimpbot");
            Assert.AreNotEqual(string.Empty, result);
            result = messageParserServices.GetResponse("/command@blImpBoT");
            Assert.AreNotEqual(string.Empty, result);
            result = messageParserServices.GetResponse("Hi blimpbot");
            Assert.AreNotEqual(string.Empty, result);
            result = messageParserServices.GetResponse("How are you blimpbot");
            Assert.AreNotEqual(string.Empty, result);
        }
        [Test]
        public void ShouldReturnEmptyIfMessageIsGeneral()
        {
            string result = messageParserServices.GetResponse("some random message not for blimpy");
            Assert.AreEqual(string.Empty, result);
            result = messageParserServices.GetResponse("some random message that blimpbot doesn't understand");
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void ShouldReturnWeatherResult()
        {
            weatherRepositoryMock.Setup(w => w.GetWeatherString(It.IsAny<List<string>>())).Returns("Some response from the weather API");
            string result = messageParserServices.GetResponse("/weather@blimpbot");
            Assert.AreNotEqual(string.Empty, result);
            result = messageParserServices.GetResponse("/weather");
            Assert.AreNotEqual(string.Empty, result);
        }

        [Test]
        public void ShouldRunAddingChatToDatabase()
        {
            chatRepositoryMock.Setup(r => r.AddChat(It.IsAny<Chat>())).Returns(true);
            chatRepositoryMock.Setup(r=>r.CheckIfChatExistsByTelegramChatId(It.IsAny<int>())).Returns(false);


            telegramRepositoryMock.Setup(r => r.GetChatMemberCount(It.IsAny<int>()))
                                  .Returns(3);

            var testChat = new TelegramChat
            {
                Id=329,
                Title = "TestChat",
            };

            messageParserServices.AddUpdateChatListing(testChat);
        }

        [Test]
        public void ShouldRunUpdatingChatInDatabase()
        {
            chatRepositoryMock.Setup(r => r.CheckIfChatExistsByTelegramChatId(It.IsAny<int>())).Returns(true);

            chatRepositoryMock.Setup(r => r.GetChatByTelegramChatId(It.IsAny<int>()))
                              .Returns(new Chat());

            telegramRepositoryMock.Setup(r => r.GetChatMemberCount(It.IsAny<int>()))
                                  .Returns(3);

            var testChat = new TelegramChat
            {
                Id = 329,
                Title = "TestChat",
            };

            messageParserServices.AddUpdateChatListing(testChat);

        }
    }
}
