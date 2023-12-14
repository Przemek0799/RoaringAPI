using LazyCache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoaringAPI.Interface;
using RoaringAPI.ModelRoaring;
using Serilog.Core;
using System.Net.Http.Headers;
using System.Text;

namespace RoaringAPI.Service
{
    public class RoaringApiService : IRoaringApiService
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly IConfiguration _configuration;
        private readonly IExceptionHandlingService _exceptionHandlingService;
        private readonly IAppCache _cache;
        private readonly ILogger<RoaringApiService> _logger;

        public RoaringApiService(IConfiguration configuration, IExceptionHandlingService exceptionHandlingService, IAppCache cache, ILogger<RoaringApiService> logger)
        {
            _configuration = configuration;
            _exceptionHandlingService = exceptionHandlingService;
            _cache = cache;
            _logger = logger;

        }

        private async Task<JObject> SendRequestAsync(string url)
        {
            try
            {
                string accessToken = await GetAccessTokenAsync();
                if (string.IsNullOrEmpty(accessToken))
                {
                    throw new InvalidOperationException("Access token is not available.");
                }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                _logger.LogInformation("Sending request to: {Url}", url);
                HttpResponseMessage response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Data retrieved successfully.");
                return JObject.Parse(responseBody);
            }
            catch (HttpRequestException e)
            {
                await _exceptionHandlingService.HandleExceptionAsync(e, "HttpRequestException occurred");
                return null;
            }
            catch (Exception ex)
            {
                await _exceptionHandlingService.HandleExceptionAsync(ex, "An unexpected exception occurred");
                return null;
            }
        }



        private async Task<string> GetAccessTokenAsync()
        {
            try
            {
                // Try to get the token from cache
                string cachedToken = _cache.Get<string>("AccessToken");
                if (!string.IsNullOrEmpty(cachedToken))
                {
                    _logger.LogInformation("Using cached access token.");
                    return cachedToken;
                }

                _logger.LogInformation("Attempting to retrieve access token...");
                string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_configuration["RoaringApiCredentials:ClientId"]}:{_configuration["RoaringApiCredentials:ClientSecret"]}"));
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                var content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");
                HttpResponseMessage response = await _client.PostAsync(_configuration["RoaringApiUrls:TokenUrl"], content);

                if (response.IsSuccessStatusCode)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JObject.Parse(responseJson);
                    string accessToken = jsonResponse["access_token"].ToString();
                    int expiresIn = jsonResponse["expires_in"].ToObject<int>();

                    // Cache the token
                    _cache.Add("AccessToken", accessToken, TimeSpan.FromSeconds(expiresIn));

                    _logger.LogInformation("Access token retrieved and cached successfully.");
                    return accessToken;
                }
                else
                {
                    _logger.LogInformation ("Error retrieving access token.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                await _exceptionHandlingService.HandleExceptionAsync(ex, "An error occurred while retrieving the access token");
                return null;
            }
        }

        public async Task<FinancialRecordApiResponse> FetchCompanyFinancialRecordAsync(string companyId)
        {
            // Correctly construct the URL using the specific configuration setting
            string url = $"{_configuration["RoaringApiUrls:CompanyFinancialRecordUrl"]}{companyId}";
            _logger.LogInformation($"Constructed URL for FetchCompanyFinancialRecordAsync: {url}"); // Log the URL for debugging

            var response = await SendRequestAsync(url);

            if (response != null)
            {
                _logger.LogInformation("Deserializing the response...");
                var jsonResponse = response.ToString();
                _logger.LogInformation($"Raw JSON response: {jsonResponse}");

                try
                {
                    var deserializedResponse = JsonConvert.DeserializeObject<FinancialRecordApiResponse>(jsonResponse);
                    if (deserializedResponse?.Records == null)
                    {
                        _logger.LogInformation("Deserialization successful but FinancialRecord is null.");
                    }
                    return deserializedResponse;
                }
                catch (JsonException je)
                {
                    _logger.LogInformation($"JSON deserialization error: {je.Message}");
                }
            }
            else
            {
                _logger.LogInformation($"No data returned for company ID: {companyId} with URL: {url}");
            }
            return null;
        }

        public async Task<RoaringApiResponse> FetchDataAsync(string companyId)
        {
            // Use the configuration to get the API URL
            string url = $"{_configuration["RoaringApiUrls:FetchDataUrl"]}{companyId}";
            var response = await SendRequestAsync(url);

            if (response != null)
            {
                _logger.LogInformation("Deserializing the response...");
                return JsonConvert.DeserializeObject<RoaringApiResponse>(response.ToString());
            }
            else
            {
                _logger.LogInformation($"No data returned for company ID: {companyId}");
            }
            return null;
        }

        public async Task<RoaringApiResponse> FetchCompanyGroupStructureAsync(string companyId)
        {
            // Use the configuration to get the API URL
            string url = $"{_configuration["RoaringApiUrls:CompanyGroupStructureUrl"]}{companyId}";
            var response = await SendRequestAsync(url);

            if (response != null)
            {
                _logger.LogInformation("Deserializing the response...");
                return JsonConvert.DeserializeObject<RoaringApiResponse>(response.ToString());
            }
            else
            {
                _logger.LogInformation($"No data returned for company ID: {companyId}");
            }
            return null;
        }

        public async Task<CompanyRatingApiRespons> FetchCompanyRatingAsync(string companyId)
        {
            string url = $"{_configuration["RoaringApiUrls:RatingUrl"]}{companyId}";
            var response = await SendRequestAsync(url);

            if (response != null)
            {
                _logger.LogInformation("Deserializing the response...");
                var jsonResponse = response.ToString();
                _logger.LogInformation($"Raw JSON response: {jsonResponse}");

                try
                {
                    var deserializedResponse = JsonConvert.DeserializeObject<CompanyRatingApiRespons>(jsonResponse);
                    return deserializedResponse;
                }
                catch (JsonException je)
                {
                    _logger.LogInformation($"JSON deserialization error: {je.Message}");
                }
            }
            else
            {
                _logger.LogInformation($"No data returned for company ID: {companyId}");
            }
            return null;
        }

        public async Task<RoaringSearchResult> FetchCompanySearchAsync(Dictionary<string, string> searchParams)
        {
            var queryParams = string.Join("&", searchParams.Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));
            string url = $"{_configuration["RoaringApiUrls:SearchUrl"]}?{queryParams}";
            _logger.LogInformation($"Constructed URL for FetchCompanySearchAsync: {url}");

            var response = await SendRequestAsync(url);

            if (response != null)
            {
                _logger.LogInformation("Deserializing the response...");
                return JsonConvert.DeserializeObject<RoaringSearchResult>(response.ToString());
            }
            else
            {
                _logger.LogInformation($"No data returned for query: {queryParams}");
            }
            return null;
        }

    }
}
