namespace Prediction.Application.Features.Seasons.Commands.CreateSeason
{
    public record CreateSeasonCommand(SeasonDto Season) : ICommand<CreateSeasonResult>;
    public record CreateSeasonResult(Guid Id);

    public class CreateSeasonCommandValidator : AbstractValidator<CreateSeasonCommand>
    {
        public CreateSeasonCommandValidator()
        {           
            RuleFor(x => x.Season.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("Season name is required")
                .MaximumLength(50)
                .WithMessage("Season name cannot exceed 50 characters.");
                                    
            RuleFor(x => x.Season.EndDate)
                .GreaterThan(x => x.Season.StartDate)
                .WithMessage("Season end date must be after the start date.");
                       
            RuleFor(x => x.Season)
                .Must(season => (season.EndDate - season.StartDate).TotalDays <= 366)
                .WithMessage("Season duration cannot exceed one year.");
        }
    }
}
