// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using Azure.Messaging.EventGrid;
using Deluxe.TokenRefresh.Application;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Deluxe.TokenRefresh.EventGridTrigger;

public class TokenRefresher
{
    private readonly IDeluxeTokenRefreshService _deluxeTokenRefreshService;

    public TokenRefresher(IDeluxeTokenRefreshService deluxeTokenRefreshService)
    {
        _deluxeTokenRefreshService = deluxeTokenRefreshService;
    }

    [FunctionName("TokenRefresherEventGrid")]
    public async Task Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log, CancellationToken cancellationToken = default)
    {
        log.LogInformation("TokenRefresherEventGrid was triggered");
        try
        {
            var data = JsonConvert.DeserializeObject<SecretNewVersionData>(eventGridEvent.Data.ToString());
            await _deluxeTokenRefreshService.RefreshToken(data.VaultName, cancellationToken);
        }
        catch (Exception ex)
        {
            log.LogError(ex, ex.Message);
            throw;
        }
    }
}
