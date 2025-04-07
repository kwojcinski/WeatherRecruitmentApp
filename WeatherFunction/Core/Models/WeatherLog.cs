using Azure;
using Azure.Data.Tables;
using System;

namespace Core.Models
{
    public class WeatherLog : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Custom properties
        public DateTime TimestampUtc { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
