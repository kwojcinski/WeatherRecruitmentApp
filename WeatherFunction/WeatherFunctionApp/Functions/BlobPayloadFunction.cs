using Abstraction.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace WeatherFunctionApp.Functions
{
    public class BlobPayloadFunction
    {
        private readonly IStorageService _storageService;

        public BlobPayloadFunction(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [Function("GetPayload")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "payload/{logId}")] HttpRequestData req,
            string logId,
            FunctionContext context)
        {
            var payload = await _storageService.GetPayloadAsync(logId);
            var response = req.CreateResponse(payload == null ?
                System.Net.HttpStatusCode.NotFound : System.Net.HttpStatusCode.OK);

            if (payload != null)
            {
                await response.WriteStringAsync(payload);
            }
            return response;
        }
    }
}
