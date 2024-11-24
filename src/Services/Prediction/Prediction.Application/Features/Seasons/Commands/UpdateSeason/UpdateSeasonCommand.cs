namespace Prediction.Application.Features.Seasons.Commands.UpdateSeason
{
    public record UpdateSeasonCommand(SeasonDto Season) : ICommand<UpdateSeasonResult>;
    public record UpdateSeasonResult(bool IsSuccess);

    public class UpdateSeasonCommandValidator : AbstractValidator<UpdateSeasonCommand>
    {
        public UpdateSeasonCommandValidator()
        {
            RuleFor(x => x.Season.Id)
                .NotEmpty()
                .WithMessage("Season Id is required.")
                .Must(id => id != Guid.Empty)
                .WithMessage("Season Id cannot be empty.");

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
