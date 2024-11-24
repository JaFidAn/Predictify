namespace Prediction.Domain.ValueObjects
{
    public record TeamLeagueSeasonId
    {
        public Guid Value { get; }

        private TeamLeagueSeasonId(Guid value) => Value = value;

        public static TeamLeagueSeasonId Of(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainException("TeamLeagueSeasonId cannot be empty.");
            }                
            return new TeamLeagueSeasonId(value);
        }
    }
}
