namespace Prediction.Application.Data
{
    public interface IFootballDataImporter
    {
        Task ImportInitialDataAsync(string baseFolderPath, CancellationToken cancellationToken);
        Task ImportMatchesAsync(string baseFolderPath, CancellationToken cancellationToken);
    }
}
