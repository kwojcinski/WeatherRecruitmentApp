using Abstraction.Interfaces.Services;
using Core.Models;
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
        public async Task GetWeatherDataAsync_ReturnsValidRecord()
        {
            string fakeJson = "{\"main\":{\"temp\":25, \"temp_min\":23, \"temp_max\":27}}";

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
                {"WeatherApiKey", "TEST_API_KEY"}
            };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var apiClient = new WeatherApiClient(httpClient, configuration);

            WeatherRecord result = await apiClient.GetWeatherDataAsync("TestCountry", "TestCity");

            Assert.NotNull(result);
            Assert.Equal("TestCountry", result.Country);
            Assert.Equal("TestCity", result.City);
            Assert.Equal(25, result.Temperature);
            Assert.Equal(23, result.MinTemperature);
            Assert.Equal(27, result.MaxTemperature);
            // Check that LastUpdate is set roughly to current time (within a small margin).
            Assert.True(result.LastUpdate <= DateTime.UtcNow && result.LastUpdate >= DateTime.UtcNow.AddSeconds(-10));

            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Once(),
               ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
               ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}
