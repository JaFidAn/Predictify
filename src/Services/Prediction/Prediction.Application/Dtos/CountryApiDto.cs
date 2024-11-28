using Newtonsoft.Json;

namespace Prediction.Application.Dtos
{
    public record CountryApiDto
    {
        [JsonProperty("name")]
        public CountryName Name { get; init; } = default!;
    }

    public record CountryName
    {
        [JsonProperty("common")]
        public string Common { get; init; } = default!;
    }
}
