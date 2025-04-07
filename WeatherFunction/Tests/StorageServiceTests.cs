using Abstraction.Interfaces;
using Core.Models;
using Microsoft.Extensions.Configuration;
using Services.Services;
using Xunit;

namespace Tests
{
    public class StorageServiceTests
    {
        private readonly IStorageService _storageService;

        public StorageServiceTests()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"AzureWebJobsStorage", "UseDevelopmentStorage=true"}
            };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _storageService = new StorageService(configuration);
        }

        [Fact]
        public async Task InsertAndQueryLog_WorksAsExpected()
        {
            var logEntry = new WeatherLog
            {
                PartitionKey = "WeatherLog",
                RowKey = Guid.NewGuid().ToString(),
                TimestampUtc = DateTime.UtcNow,
                Success = true,
                Message = "Test log"
            };

            await _storageService.InsertLogAsync(logEntry);
            var results = await _storageService.QueryLogsAsync(DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(1));

            Assert.Contains(results, l => l.RowKey == logEntry.RowKey);
        }
    }
}
