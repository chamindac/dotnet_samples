using di_sample.domain.core.Interfaces;
using di_sample.domain.infrastrcture.Models.Db;
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
            //OrganizationDbModel organization = await _organizationOperationService.CreateOrganizationAsync("Third Org");

            //_logger.LogInformation("Created organization with ID: {OrganizationId}, Name: {OrganizationName}, CreatedTime: {CreatedTime}",
            //    organization.Id,
            //    organization.Name,
            //    organization.CreatedTimeUtc);

            //OrganizationDbModel? organization = await _organizationOperationService.GetOrganizationByNameAsync("Test Organization");

            //_logger.LogInformation("Received organization with ID: {OrganizationId}, Name: {OrganizationName}, CreatedTime: {CreatedTime}",
            //    organization?.Id,
            //    organization?.Name,
            //    organization?.CreatedTimeUtc);

            IEnumerable<OrganizationDbModel> organizations = await _organizationOperationService.GetAllAsync();

            foreach (OrganizationDbModel org in organizations)
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
