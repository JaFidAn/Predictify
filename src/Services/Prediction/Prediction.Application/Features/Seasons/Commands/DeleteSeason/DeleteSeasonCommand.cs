namespace Prediction.Application.Features.Seasons.Commands.DeleteSeason
{
    public record DeleteSeasonCommand(Guid SeasonId) : ICommand<DeleteSeasonResult>;
    public record DeleteSeasonResult(bool IsSuccess);

    public class DeleteSeasonCommandValidator : AbstractValidator<DeleteSeasonCommand>
    {
        public DeleteSeasonCommandValidator()
        {
            RuleFor(x => x.SeasonId)
           .NotEmpty()
           .WithMessage("Season Id is required.")
           .Must(id => id != Guid.Empty)
           .WithMessage("Season Id cannot be empty.");
        }
    }
}
