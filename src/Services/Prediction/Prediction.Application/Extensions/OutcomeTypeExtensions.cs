using Prediction.Domain.Exceptions;

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

        public static async Task<Dictionary<string, OutcomeTypeId>> GetOutcomeTypeMapAsync(this IApplicationDbContext context, CancellationToken cancellationToken)
        {
            // Fetch outcome types dynamically
            var outcomeTypes = await context.OutcomeTypes.ToListAsync(cancellationToken);

            // Map outcome names to their IDs
            var outcomeTypeMap = outcomeTypes.ToDictionary(
                ot => ot.Name,
                ot => OutcomeTypeId.Of(ot.Id.Value)
            );

            // Validate required outcome types
            var requiredKeys = new[] { "Win", "Loss", "Draw", "Over_2_5", "Under_2_5" };
            foreach (var key in requiredKeys)
            {
                if (!outcomeTypeMap.ContainsKey(key))
                    throw new DomainException($"Outcome type '{key}' is missing from the database.");
            }

            return outcomeTypeMap;
        }
    }
}
