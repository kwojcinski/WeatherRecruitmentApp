using Abstraction.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Globalization;

namespace WeatherFunctionApp.Functions
{
    public class LogQueryFunction
    {
        private readonly IStorageService _storageService;

        public LogQueryFunction(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [Function("GetLogs")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "logs")] HttpRequestData req,
            FunctionContext context)
        {
            var logger = context.GetLogger("GetLogs");

            var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            string fromStr = query["from"];
            string toStr = query["to"];

            if (!DateTime.TryParseExact(fromStr, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fromTime) ||
                !DateTime.TryParseExact(toStr, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime toTime))
            {
                var badResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                await badResponse.WriteStringAsync("Invalid dates for 'from' or 'to'.");
                return badResponse;
            }

            var logs = await _storageService.QueryLogsAsync(fromTime, toTime);
            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(logs);
            return response;
        }
    }
}
