using Newtonsoft.Json;
using Prediction.Application.Dtos;
using Prediction.Application.Services;

namespace Prediction.Infrastructure.Services
{
    public class CountryApiService : ICountryApiService
    {
        private readonly HttpClient _httpClient;

        public CountryApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<string>> GetCountryNamesAsync(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _httpClient.GetAsync("https://restcountries.com/v3.1/all", cancellationToken);
                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                using var reader = new StreamReader(stream);
                using var jsonReader = new JsonTextReader(reader);

                var serializer = new JsonSerializer();
                var countries = serializer.Deserialize<List<CountryApiDto>>(jsonReader);

                return countries?.Select(c => c.Name.Common) ?? Enumerable.Empty<string>();
            }
            catch (Exception ex)
            {
                // Log detailed exception for better debugging
                Console.WriteLine($"Error fetching countries: {ex.Message}");
                throw;
            }
        }
    }
}
