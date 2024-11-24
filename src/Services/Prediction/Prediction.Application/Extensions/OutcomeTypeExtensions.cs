namespace Prediction.Application.Extensions
{
    public static class OutcomeTypeExtensions
    {
        public static IEnumerable<OutcomeTypeDto> ToOutcomeTypeDtoList(this IEnumerable<OutcomeType> outcomeTypes)
        {
            return outcomeTypes.Select(outcomeType => new OutcomeTypeDto
            {
                Id = outcomeType.Id.Value,
                Name = outcomeType.Name,
                Description = outcomeType.Description
            }).ToList();
        }

        public static OutcomeTypeDto ToOutcomeTypeDto(this OutcomeType outcomeType)
        {
            return new OutcomeTypeDto
            {
                Id = outcomeType.Id.Value,
                Name = outcomeType.Name,
                Description = outcomeType.Description
            };
        }
    }
}
