using MediatR;
using Microsoft.Extensions.Options;
using Prediction.Application.Configurations;

namespace Prediction.Application.Features.Imports.Commands.ImportInitialData
{
    public class ImportInitialDataCommandHandler : IRequestHandler<ImportInitialDataCommand, Unit>
    {
        private readonly IFootballDataImporter _importService;
        private readonly ILogger<ImportInitialDataCommandHandler> _logger;
        private readonly string _folderPath;

        public ImportInitialDataCommandHandler(
            IFootballDataImporter importService,
            IOptions<DataSettings> dataSettings,
            ILogger<ImportInitialDataCommandHandler> logger)
        {
            _importService = importService;
            _logger = logger;
            _folderPath = dataSettings.Value.ExternalFolderPath; // Predefined path from configuration
        }

        public async Task<Unit> Handle(ImportInitialDataCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting import of initial football data...");

            if (!Directory.Exists(_folderPath))
            {
                _logger.LogError("Folder not found: {FolderPath}", _folderPath);
                throw new DirectoryNotFoundException($"The folder path '{_folderPath}' does not exist.");
            }

            await _importService.ImportInitialDataAsync(_folderPath, cancellationToken);

            _logger.LogInformation("Import of initial football data completed successfully.");
            return Unit.Value;
        }
    }
}
