using di_sample.domain.core.Interfaces;
using di_sample.domain.core.Models;
using System;
using System.Threading.Tasks;

namespace di_sample.domain.core.Implementation
{
    public class OrganizationOperationService : IOrganizationOperationService
    {
        private readonly IGenericDbRepository<Organization> _organizationRepository;

        public OrganizationOperationService(
            IGenericDbRepository<Organization> organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public Task<Organization> CreateOrganizationAsync(string name)
        {
            Organization organization = new()
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = name
            };

            return _organizationRepository.CreateAsync(organization);
        }
    }
}
