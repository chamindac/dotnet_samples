using di_sample.domain.core.Interfaces;
using di_sample.domain.core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace di_sample.domain.core.Implementation
{
    public class OrganizationOperationService : IOrganizationOperationService
    {
        private readonly IOrganizationDbRepository<Organization> _organizationRepository;

        public OrganizationOperationService(
            IOrganizationDbRepository<Organization> organizationRepository)
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

        public Task<IEnumerable<Organization>> GetAllAsync()
        {
            return _organizationRepository.GetAllAsync();
        }

        public Task<Organization?> GetOrganizationByNameAsync(string name)
        {
            return _organizationRepository.GetByNameAsync(name);
        }
    }
}
