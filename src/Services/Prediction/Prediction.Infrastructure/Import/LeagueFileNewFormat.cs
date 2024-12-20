﻿namespace Prediction.Infrastructure.Import
{
    public class LeagueFileNewFormat
    {
        public string Name { get; set; } = default!;
        public List<MatchJsonNew> Matches { get; set; } = new();
    }

    public class MatchJsonNew
    {
        public string Round { get; set; } = default!;
        public string Date { get; set; } = default!;
        public string Time { get; set; } = default!;
        public string Team1 { get; set; } = default!;
        public string Team2 { get; set; } = default!;
        public Score Score { get; set; } = new();
    }

    public class Score
    {
        public List<int> Ht { get; set; } = new();
        public List<int> Ft { get; set; } = new();
    }
}
