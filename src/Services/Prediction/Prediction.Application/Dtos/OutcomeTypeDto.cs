namespace Prediction.Application.Dtos
{
    public record OutcomeTypeDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public string? Description { get; init; }
    }
}
