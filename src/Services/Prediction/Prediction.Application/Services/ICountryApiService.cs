namespace Prediction.Application.Services
{
    public interface ICountryApiService
    {
        Task<IEnumerable<string>> GetCountryNamesAsync(CancellationToken cancellationToken);
    }
}
