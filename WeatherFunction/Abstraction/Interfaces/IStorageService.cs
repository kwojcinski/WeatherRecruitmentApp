using Core.Models;

namespace Abstraction.Interfaces
{
    public interface IStorageService
    {
        Task InsertLogAsync(WeatherLog log);
        Task<IEnumerable<WeatherLog>> QueryLogsAsync(DateTime from, DateTime to);
        Task UploadPayloadAsync(string logId, string payload);
        Task<string> GetPayloadAsync(string logId);
    }
}
