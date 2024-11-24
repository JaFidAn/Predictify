namespace Prediction.Application.Dtos
{
    public record SeasonDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } 
    }
}
