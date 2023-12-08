using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoaringAPI.Interface;
using RoaringAPI.ModelRoaring;
using System.Text;

namespace RoaringAPI.Service
{
    public class RoaringApiService : IRoaringApiService
    {
        private readonly HttpClient _client = new HttpClient();
        private string _accessToken;

        private readonly IConfiguration _configuration;
        private readonly IExceptionHandlingService _exceptionHandlingService;
        private readonly string _companyFinancialRecordUrl;
        public RoaringApiService(IConfiguration configuration, IExceptionHandlingService exceptionHandlingService)
        {
            _configuration = configuration;
            _exceptionHandlingService = exceptionHandlingService;
            
            Task.Run(() => GetAccessTokenAsync()).Wait();
        }

        private async Task<JObject> SendRequestAsync(string url)
        {
            try
            {
                Console.WriteLine("Sending request to: " + url);
                HttpResponseMessage response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Data retrieved successfully.");
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

        private async Task GetAccessTokenAsync()
        {
            Console.WriteLine("Attempting to retrieve access token...");
            string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_configuration["RoaringApiCredentials:ClientId"]}:{_configuration["RoaringApiCredentials:ClientSecret"]}"));
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

            var content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage response = await _client.PostAsync(_configuration["RoaringApiUrls:TokenUrl"], content);

            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseJson);
                _accessToken = jsonResponse["access_token"].ToString();
                Console.WriteLine("Access token retrieved successfully.");
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);
            }
            else
            {
                Console.WriteLine("Error retrieving access token.");
            }
        }

        public async Task<FinancialRecordApiResponse> FetchCompanyFinancialRecordAsync(string companyId)
        {
            // Correctly construct the URL using the specific configuration setting
            string url = $"{_configuration["RoaringApiUrls:CompanyFinancialRecordUrl"]}{companyId}";
            Console.WriteLine($"Constructed URL for FetchCompanyFinancialRecordAsync: {url}"); // Log the URL for debugging

            var response = await SendRequestAsync(url);

            if (response != null)
            {
                Console.WriteLine("Deserializing the response...");
                var jsonResponse = response.ToString();
                Console.WriteLine($"Raw JSON response: {jsonResponse}");

                try
                {
                    var deserializedResponse = JsonConvert.DeserializeObject<FinancialRecordApiResponse>(jsonResponse);
                    if (deserializedResponse?.Records == null)
                    {
                        Console.WriteLine("Deserialization successful but FinancialRecord is null.");
                    }
                    return deserializedResponse;
                }
                catch (JsonException je)
                {
                    Console.WriteLine($"JSON deserialization error: {je.Message}");
                }
            }
            else
            {
                Console.WriteLine($"No data returned for company ID: {companyId} with URL: {url}");
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
                Console.WriteLine("Deserializing the response...");
                return JsonConvert.DeserializeObject<RoaringApiResponse>(response.ToString());
            }
            else
            {
                Console.WriteLine($"No data returned for company ID: {companyId}");
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
                Console.WriteLine("Deserializing the response...");
                return JsonConvert.DeserializeObject<RoaringApiResponse>(response.ToString());
            }
            else
            {
                Console.WriteLine($"No data returned for company ID: {companyId}");
            }
            return null;
        }

        public async Task<CompanyRatingApiRespons> FetchCompanyRatingAsync(string companyId)
        {
            string url = $"{_configuration["RoaringApiUrls:RatingUrl"]}{companyId}";
            var response = await SendRequestAsync(url);

            if (response != null)
            {
                Console.WriteLine("Deserializing the response...");
                var jsonResponse = response.ToString();
                Console.WriteLine($"Raw JSON response: {jsonResponse}");

                try
                {
                    var deserializedResponse = JsonConvert.DeserializeObject<CompanyRatingApiRespons>(jsonResponse);
                    return deserializedResponse;
                }
                catch (JsonException je)
                {
                    Console.WriteLine($"JSON deserialization error: {je.Message}");
                }
            }
            else
            {
                Console.WriteLine($"No data returned for company ID: {companyId}");
            }
            return null;
        }

    }
}
