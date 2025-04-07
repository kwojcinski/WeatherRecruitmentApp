using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Services.Services;
using System.Net;
using Xunit;

namespace Tests
{
    public class WeatherServiceTests
    {
        [Fact]
        public async Task FetchWeatherAsync_ReturnsWeatherResponse_WhenSuccessful()
        {
            var fakeJson = "{\"weather\":\"sunny\"}";
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(fakeJson),
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);

            var inMemorySettings = new Dictionary<string, string>
            {
                {"WeatherMapApiKey", "TEST_API_KEY"}
            };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var weatherService = new WeatherService(httpClient, configuration);

            var result = await weatherService.FetchWeatherAsync();

            Assert.Equal(fakeJson, result.RawJson);
        }
    }
}
