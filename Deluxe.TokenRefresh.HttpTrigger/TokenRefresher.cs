using Deluxe.TokenRefresh.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Deluxe.TokenRefresh.EventGridTrigger
{
    public class TokenRefresher
    {
        private readonly IDeluxeTokenRefreshService _deluxeTokenRefreshService;

        public TokenRefresher(IDeluxeTokenRefreshService deluxeTokenRefreshService)
        {
            _deluxeTokenRefreshService = deluxeTokenRefreshService;
        }

        [FunctionName("TokenRefresherHttpTrigger")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiParameter(name: "secretName", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "Name of the secret key")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log, CancellationToken cancellationToken = default)
        {
            try
            {
                var secretName = req.Query["secretName"];

                if(string.IsNullOrEmpty(secretName))
                {
                    throw new ArgumentException("The secret name parameter is required.");
                }

                log.LogInformation("TokenRefresherHttpTrigger was triggered.");

                await _deluxeTokenRefreshService.RefreshToken(secretName, cancellationToken);

                string responseMessage = "The Token was refreshed successfuly.";

                return new OkObjectResult(responseMessage);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Something went wrong");
                var model = new { error = ex.Message };
                return new ObjectResult(model)
                {
                    StatusCode = 500
                };
            }
        }
    }
}
