using Abstraction.Interfaces;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class StorageService : IStorageService
    {
        private readonly TableClient _tableClient;
        private readonly BlobContainerClient _blobContainerClient;

        public StorageService(IConfiguration configuration)
        {
            // Read connection strings from configuration
            var storageConnectionString = configuration["AzureWebJobsStorage"];
            var tableName = "WeatherLogs";
            var blobContainerName = "weatherpayloads";

            _tableClient = new TableClient(storageConnectionString, tableName);
            _tableClient.CreateIfNotExists();

            var blobServiceClient = new BlobServiceClient(storageConnectionString);
            _blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
            _blobContainerClient.CreateIfNotExists();
        }

        public async Task InsertLogAsync(WeatherLog log)
        {
            await _tableClient.AddEntityAsync(log);
        }

        public async Task<IEnumerable<WeatherLog>> QueryLogsAsync(DateTime from, DateTime to)
        {
            // Query logs by TimestampUtc.
            // Note: Table storage queries on non-key properties require filtering on a string; adjust accordingly.
            string filter = TableClient.CreateQueryFilter<WeatherLog>(log =>
                log.TimestampUtc >= from && log.TimestampUtc <= to);

            var queryResult = _tableClient.QueryAsync<WeatherLog>(filter);
            List<WeatherLog> results = new List<WeatherLog>();
            await foreach (var entity in queryResult)
            {
                results.Add(entity);
            }
            return results;
        }

        public async Task UploadPayloadAsync(string logId, string payload)
        {
            var blobClient = _blobContainerClient.GetBlobClient($"{logId}.json");
            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(payload));
            await blobClient.UploadAsync(stream, overwrite: true);
        }

        public async Task<string> GetPayloadAsync(string logId)
        {
            try
            {
                var blobClient = _blobContainerClient.GetBlobClient($"{logId}.json");
                var downloadResponse = await blobClient.DownloadContentAsync();
                return downloadResponse.Value.Content.ToString();
            }
            catch (Azure.RequestFailedException)
            {
                return null;
            }
        }
    }
}
