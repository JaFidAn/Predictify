namespace Prediction.Infrastructure.Import
{
    public class LeagueFileOldFormat
    {
        public string Name { get; set; } = default!;
        public List<Round> Rounds { get; set; } = new();
    }

    public class Round
    {
        public string Name { get; set; } = default!;
        public List<MatchJsonOld> Matches { get; set; } = new();
    }

    public class MatchJsonOld
    {
        public string Date { get; set; } = default!;
        public string Team1 { get; set; } = default!;
        public string Team2 { get; set; } = default!;
        public Score Score { get; set; } = new();
    }
}
