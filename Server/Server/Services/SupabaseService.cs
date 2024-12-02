using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Server.Services
{
    public class SupabaseService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public SupabaseService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["Supabase:Uri"]);
            _httpClient.DefaultRequestHeaders.Add("apikey", _configuration["Supabase:ApiKey"]);
        }

        public async Task<IEnumerable<dynamic>?> FetchUsersAsync()
        {
            var response = await _httpClient.GetAsync("/rest/v1/Users");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<dynamic>>(content);
        }
    }
}