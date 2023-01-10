using Deluxe.TokenRefresh.Domain;
using Deluxe.TokenRefresh.Domain.Configuration;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Deluxe.TokenRefresh.Infrastructure.Deluxe;

public class DeluxeDPXApi : IDeluxeDPXApi
{
    private readonly HttpClient _httpClient;
    private readonly DeluxeVendorSettings _settings;

    public DeluxeDPXApi(HttpClient httpClient, DeluxeVendorSettings deluxeVendorSettings)
    {
        _httpClient = httpClient;
        _settings = deluxeVendorSettings;
    }

    public async Task<Token?> GetRequestTokenAsync(string email, CancellationToken cancellationToken = default)
    {
        var requestToken = new RequestToken { Email = email };
        var message = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(_settings.DeluxeApiUrl),
            $"auth/request_grant?client_id={_settings.ClientId}&client_secret={_settings.ClientSecret}"))
        {
            Content = new StringContent(JsonSerializer.Serialize(requestToken), Encoding.UTF8, "application/json")
        };

        var response = await _httpClient.SendAsync(message, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Token>(cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        return null;
    }

    public async Task<Token?> ResolveRequestTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var message = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(_settings.DeluxeApiUrl),
            $"auth/resolve?client_id={_settings.ClientId}&client_secret={_settings.ClientSecret}&token={token}"));

        var response = await _httpClient.SendAsync(message, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Token>(cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        return null;
    }

    public async Task<Token?> RefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var message = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(_settings.DeluxeApiUrl),
            $"auth/refresh?client_id={_settings.ClientId}&client_secret={_settings.ClientSecret}&token={token}"));

        var response = await _httpClient.SendAsync(message, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Token>(cancellationToken: cancellationToken).ConfigureAwait(false);
        }
        else if (response != null)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: cancellationToken).ConfigureAwait(false);
            throw new TaskCanceledException($"RefreshToken - {ErrorService.BuildError(errorResponse)}");
        }

        return null;
    }
}
