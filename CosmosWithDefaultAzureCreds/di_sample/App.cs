using di_sample.domain.core.Interfaces;
using di_sample.domain.core.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace di_sample
{
    public class App
    {
        private readonly IOrganizationOperationService _organizationOperationService;
        private readonly ILogger<App> _logger;

        public App(
            IOrganizationOperationService organizationOperationService,
            ILogger<App> logger)
        {
            _organizationOperationService = organizationOperationService;
            _logger = logger;
        }

        public async Task RunAsync()
        {
            _logger.LogInformation("Application started");
            Organization organization = await _organizationOperationService.CreateOrganizationAsync("Test Organization");

            _logger.LogInformation("Created organization with ID: {OrganizationId}, Name: {OrganizationName}, CreatedTime: {CreatedTime}", 
                organization.Id, 
                organization.Name,
                organization.CreatedTimeUtc);
            _logger.LogInformation("Application finished");
        }
    }
}
