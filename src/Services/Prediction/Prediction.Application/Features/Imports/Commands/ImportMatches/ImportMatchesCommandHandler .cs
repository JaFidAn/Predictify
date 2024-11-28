using MediatR;
using Microsoft.Extensions.Options;
using Prediction.Application.Configurations;

namespace Prediction.Application.Features.Imports.Commands.ImportMatches
{
    public class ImportMatchesCommandHandler : IRequestHandler<ImportMatchesCommand, Unit>
    {
        private readonly IFootballDataImporter _importService;
        private readonly ILogger<ImportMatchesCommandHandler> _logger;
        private readonly string _folderPath;

        public ImportMatchesCommandHandler(
            IFootballDataImporter importService,
            IOptions<DataSettings> dataSettings,
            ILogger<ImportMatchesCommandHandler> logger)
        {
            _importService = importService;
            _logger = logger;
            _folderPath = dataSettings.Value.ExternalFolderPath; // Predefined path from configuration
        }

        public async Task<Unit> Handle(ImportMatchesCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting import of football matches...");

            if (!Directory.Exists(_folderPath))
            {
                _logger.LogError("Folder not found: {FolderPath}", _folderPath);
                throw new DirectoryNotFoundException($"The folder path '{_folderPath}' does not exist.");
            }

            await _importService.ImportMatchesAsync(_folderPath, cancellationToken);

            _logger.LogInformation("Import of football matches completed successfully.");
            return Unit.Value;
        }
    }
}
