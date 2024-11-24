namespace Prediction.Infrastructure.Data.Extensions
{
    internal class InitialData
    {
        public static IEnumerable<Country> Countries =>
            new List<Country>
            {
                Country.Create(CountryId.Of(new Guid("58c49479-ec65-4de2-86e7-033c546291aa")), "England"),
                Country.Create(CountryId.Of(new Guid("189dc8dc-990f-48e0-a37b-e6f2b60b9d7d")), "Germany"),
            };

        public static IEnumerable<League> Leagues =>
           new List<League>
           {
                League.Create(LeagueId.Of(new Guid("080c69a3-fb2e-4a72-a3b2-d1bf7f32aa84")), "Premier League", CountryId.Of(new Guid("58c49479-ec65-4de2-86e7-033c546291aa"))),
                League.Create(LeagueId.Of(new Guid("0beccccc-1d1c-424f-aae9-45e9d936a79d")), "Championship", CountryId.Of(new Guid("58c49479-ec65-4de2-86e7-033c546291aa"))),
                League.Create(LeagueId.Of(new Guid("e1b31d94-f641-4393-8943-2c9791002125")), "League One", CountryId.Of(new Guid("58c49479-ec65-4de2-86e7-033c546291aa"))),
                League.Create(LeagueId.Of(new Guid("702b1ce0-fe13-4d39-a38f-8d195e9c5e87")), "Bundesliga", CountryId.Of(new Guid("189dc8dc-990f-48e0-a37b-e6f2b60b9d7d"))),
                League.Create(LeagueId.Of(new Guid("1d8c4d8d-c144-4a04-bf91-7e21fe01ee34")), "Bundesliga 1", CountryId.Of(new Guid("189dc8dc-990f-48e0-a37b-e6f2b60b9d7d"))),
                League.Create(LeagueId.Of(new Guid("54d17bcb-5ad2-42c6-8c45-f1cede7e8be5")), "Bundesliga 2", CountryId.Of(new Guid("189dc8dc-990f-48e0-a37b-e6f2b60b9d7d"))),
           };

        public static IEnumerable<OutcomeType> OutcomeTypes =>
            new List<OutcomeType>
            {
                OutcomeType.Create(OutcomeTypeId.Of(new Guid("7b0db166-65e0-4c99-aaac-89bf544b47c0")), "Win", "Team 1 win"),
                OutcomeType.Create(OutcomeTypeId.Of(new Guid("7858e1e9-2979-40bf-acea-8f61f8d663c1")), "Draw", "It is a draw"),
                OutcomeType.Create(OutcomeTypeId.Of(new Guid("3cff379b-ef0a-45b8-8564-238b9edad8e9")), "Loss", "Team 2 win"),
                OutcomeType.Create(OutcomeTypeId.Of(new Guid("3b79c387-4b5d-4ba9-bba3-1d4523f82348")), "Over_2_5", "Total goals over 2.5 goals"),
                OutcomeType.Create(OutcomeTypeId.Of(new Guid("4eee3579-76c8-4fac-8bf0-a3f309f947b3")), "Under_2_5", "Total goals under 2.5 goals")
            };

        public static IEnumerable<Season> Seasons =>
        new List<Season>
        {
             Season.Create(SeasonId.Of(new Guid("5ec440cd-5aad-422f-5dcb-cd6e642eaf7e")),"2024/2025", new DateTime(2024, 7, 1), new DateTime(2025, 6, 30)),
            Season.Create(SeasonId.Of(new Guid("1fc440cd-1aad-422f-9dcb-cd6e642eaf7a")), "2023/2024", new DateTime(2023, 7, 1), new DateTime(2024, 6, 30)),
            Season.Create(SeasonId.Of(new Guid("2bc440cd-2aad-422f-8dcb-cd6e642eaf7b")), "2022/2023", new DateTime(2022, 7, 1), new DateTime(2023, 6, 30)),
            Season.Create(SeasonId.Of(new Guid("3cc440cd-3aad-422f-7dcb-cd6e642eaf7c")), "2021/2022", new DateTime(2021, 7, 1), new DateTime(2022, 6, 30)),
            Season.Create( SeasonId.Of(new Guid("4dc440cd-4aad-422f-6dcb-cd6e642eaf7d")), "2020/2021", new DateTime(2020, 7, 1), new DateTime(2021, 6, 30)),
       
        };
    }
}
