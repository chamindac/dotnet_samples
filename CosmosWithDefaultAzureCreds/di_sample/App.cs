using di_sample.domain.core.Interfaces;
using di_sample.domain.core.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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
            //Organization organization = await _organizationOperationService.CreateOrganizationAsync("Second Org");

            //_logger.LogInformation("Created organization with ID: {OrganizationId}, Name: {OrganizationName}, CreatedTime: {CreatedTime}",
            //    organization.Id,
            //    organization.Name,
            //    organization.CreatedTimeUtc);

            //Organization? organization = await _organizationOperationService.GetOrganizationByNameAsync("Test Organization");

            //_logger.LogInformation("Received organization with ID: {OrganizationId}, Name: {OrganizationName}, CreatedTime: {CreatedTime}",
            //    organization?.Id,
            //    organization?.Name,
            //    organization?.CreatedTimeUtc);

            IEnumerable<Organization> organizations = await _organizationOperationService.GetAllAsync();

            foreach (Organization org in organizations)
            {
                _logger.LogInformation("Received organization with ID: {OrganizationId}, Name: {OrganizationName}, CreatedTime: {CreatedTime}",
                    org.Id,
                    org.Name,
                    org.CreatedTimeUtc);
            }

            _logger.LogInformation("Application finished");
        }
    }
}
