using System.Collections.Generic;
using BlimpBot.Constants;
using BlimpBot.Database.Models;
using BlimpBot.Interfaces;
using BlimpBot.Models;
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
        private Mock<IWeatherServices> weatherRepositoryMock;
        private Mock<IExchangeRateService> exchangeRateServiceMock;
        private Mock<IChatBotRepository> chatRepositoryMock;
        private Mock<IReviewRepository> reviewRepositoryMock;
        private Mock<ICryptoServices> cryptoServicesMock;
        private Mock<IMinorApiServices> minorApiServiceMock;

        [SetUp]
        public void SetUp()
        {

            telegramRepositoryMock = new Mock<ITelegramRepository>();
            weatherRepositoryMock = new Mock<IWeatherServices>();
            exchangeRateServiceMock = new Mock<IExchangeRateService>();
            chatRepositoryMock = new Mock<IChatBotRepository>();
            reviewRepositoryMock = new Mock<IReviewRepository>();
            cryptoServicesMock = new Mock<ICryptoServices>();
            minorApiServiceMock = new Mock<IMinorApiServices>();
            
            messageParserServices = new MessageParserServices(weatherRepositoryMock.Object,
                                                              exchangeRateServiceMock.Object,
                                                              telegramRepositoryMock.Object,
                                                              chatRepositoryMock.Object,
                                                              reviewRepositoryMock.Object,
                                                              cryptoServicesMock.Object,
                                                              minorApiServiceMock.Object
                                                            );
        }

        [Test]
        public void ShouldReturnEmptyIfMessagesIsTargetedAtAnotherBot()
        {
            OurChatResponse result = messageParserServices.GetChatResponse("message@ADifferentBot");
            Assert.AreEqual(string.Empty, result.Text);
        }

        [Test]
        public void ShouldReturnNotEmptyIsMessageIsTargetedAtBlimpBot()
        {
            OurChatResponse result = messageParserServices.GetChatResponse("message@blimpbot");
            Assert.AreNotEqual(string.Empty, result.Text);
            result = messageParserServices.GetChatResponse("/command@blimpbot");
            Assert.AreNotEqual(string.Empty, result.Text);
            result = messageParserServices.GetChatResponse("/command@blImpBoT");
            Assert.AreNotEqual(string.Empty, result.Text);
            result = messageParserServices.GetChatResponse("Hi blimpbot");
            Assert.AreNotEqual(string.Empty, result.Text);
            result = messageParserServices.GetChatResponse("How are you blimpbot");
            Assert.AreNotEqual(string.Empty, result.Text);
        }
        [Test]
        public void ShouldReturnEmptyIfMessageIsGeneral()
        {
            OurChatResponse result = messageParserServices.GetChatResponse("some random message not for blimpy");
            Assert.AreEqual(string.Empty, result.Text);
            result = messageParserServices.GetChatResponse("some random message that blimpbot doesn't understand");
            Assert.AreEqual(string.Empty, result.Text);
        }

        [Test]
        public void ShouldReturnWeatherResult()
        {
            //Not sure I like having to mock the virtual method
            weatherRepositoryMock.Setup(w => w.GetChatResponse(It.IsAny<List<string>>(), MinorApiType.None))
                                 .Returns(new OurChatResponse{Text = "Some response from the weather API"});

            OurChatResponse result = messageParserServices.GetChatResponse("/weather@blimpbot");
            Assert.AreNotEqual(string.Empty, result.Text);
            result = messageParserServices.GetChatResponse("/weather");
            Assert.AreNotEqual(string.Empty, result.Text);
        }

        [Test]
        public void ShouldReturnDuckResult()
        {
            minorApiServiceMock.Setup(d => d.GetChatResponse(It.IsAny<List<string>>(), MinorApiType.Duck))
                               .Returns(new OurChatResponse
                               {
                                   PhotoUrl = "someurl.gif",
                                   IsPhotoMessage = true,
                               });
            var result = messageParserServices.GetChatResponse("/duck");
            Assert.IsTrue(result.IsPhotoMessage);
            result = messageParserServices.GetChatResponse("/duck@BlimpBot");
            Assert.IsTrue(result.IsPhotoMessage);
            result = messageParserServices.GetChatResponse("/quack");
            Assert.IsTrue(result.IsPhotoMessage);
        }

        [Test]
        public void ShouldRunAddingChatToDatabase()
        {
            chatRepositoryMock.Setup(r => r.AddChat(It.IsAny<Chat>())).Returns(true);
            chatRepositoryMock.Setup(r=>r.CheckIfChatExistsByTelegramChatId(It.IsAny<string>())).Returns(false);


            telegramRepositoryMock.Setup(r => r.GetChatMemberCount(It.IsAny<long>()))
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
            chatRepositoryMock.Setup(r => r.CheckIfChatExistsByTelegramChatId(It.IsAny<string>())).Returns(true);

            chatRepositoryMock.Setup(r => r.GetChatByTelegramChatId(It.IsAny<string>()))
                              .Returns(new Chat());

            telegramRepositoryMock.Setup(r => r.GetChatMemberCount(It.IsAny<long>()))
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
