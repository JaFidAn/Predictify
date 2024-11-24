namespace Prediction.Application.Features.Matches.Commands.CreateMatch
{
    public record CreateMatchCommand(MatchDto Match) : ICommand<CreateMatchResult>;
    public record CreateMatchResult(Guid Id);

    public class CreateMatchCommandValidator : AbstractValidator<CreateMatchCommand>
    {
        public CreateMatchCommandValidator()
        {
            RuleFor(x => x.Match.Team1Id)
                .NotEmpty().WithMessage("Team1Id is required.");

            RuleFor(x => x.Match.Team2Id)
                .NotEmpty().WithMessage("Team2Id is required.")
                .NotEqual(x => x.Match.Team1Id).WithMessage("A team cannot play against itself.");

            RuleFor(x => x.Match.Date)
                .GreaterThan(DateTime.MinValue).WithMessage("Date is required.");

            RuleFor(x => x.Match.Team1Goals)
                .GreaterThanOrEqualTo(0).WithMessage("Team1Goals must be 0 or greater.")
                .When(x => x.Match.Team1Goals.HasValue);

            RuleFor(x => x.Match.Team2Goals)
                .GreaterThanOrEqualTo(0).WithMessage("Team2Goals must be 0 or greater.")
                .When(x => x.Match.Team2Goals.HasValue);
        }
    }
}
