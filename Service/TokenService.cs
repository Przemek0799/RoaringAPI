using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using RoaringAPI.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using LazyCache;

namespace RoaringAPI.Service
{
    public class TokenService : ITokenService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly ICacheService _cacheService;
        private readonly ILogger<TokenService> _logger;
        private readonly IExceptionHandlingService _exceptionHandlingService;

        public TokenService(HttpClient client, IConfiguration configuration, ICacheService cacheService, ILogger<TokenService> logger, IExceptionHandlingService exceptionHandlingService)
        {
            _client = client;
            _configuration = configuration;
            _cacheService = cacheService;
            _logger = logger;
            _exceptionHandlingService = exceptionHandlingService;

        }

        public async Task<string> GetAccessTokenAsync()
        {
            try
            {
                if (!IsTokenExpired())
                {
                    _logger.LogInformation("Using cached access token.");
                    return _cacheService.Get<(string Token, DateTime Expiration)>("AccessToken").Token;
                }
                _logger.LogInformation("Access token expired. Attempting to retrieve new access token...");

                // Try to get the token from cache
                string cachedToken = _cacheService.Get<string>("AccessToken");
                if (!string.IsNullOrEmpty(cachedToken))
                {
                    _logger.LogInformation("Using cached access token.");
                    return cachedToken;
                }

                _logger.LogInformation("Attempting to retrieve access token...");
                string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_configuration["RoaringClientId"]}:{_configuration["RoaringClientSecret"]}"));
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                var content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");
                HttpResponseMessage response = await _client.PostAsync(_configuration["RoaringApiUrls:TokenUrl"], content);
                if (response.IsSuccessStatusCode)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JObject.Parse(responseJson);
                    string accessToken = jsonResponse["access_token"].ToString();
                    int expiresIn = jsonResponse["expires_in"].ToObject<int>();

                    // Calculate the expiration time of the token
                    DateTime expiration = DateTime.UtcNow.AddSeconds(expiresIn);

                    // Cache the token with its expiration time
                    _cacheService.Add("AccessToken", (accessToken, expiration), TimeSpan.FromSeconds(expiresIn));

                    _logger.LogInformation("New access token retrieved and cached successfully.");
                    return accessToken;
                }
                else
                {
                    _logger.LogInformation("Error retrieving new access token.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                await _exceptionHandlingService.HandleExceptionAsync(ex, "An error occurred while retrieving the access token");
                return null;
            }
        }
        private bool IsTokenExpired()
        {
            if (_cacheService.TryGetValue<(string Token, DateTime Expiration)>("AccessToken", out var tokenCache)) // Use _cacheService
            {
                return tokenCache.Expiration <= DateTime.UtcNow;
            }
            return true;
        }
    }
}
