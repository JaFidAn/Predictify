namespace Prediction.Application.Dtos
{
    public record CountryDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
    }
}
