using di_sample.domain.core.Interfaces;
using di_sample.domain.core.Interfaces.Db;
using di_sample.domain.core.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace di_sample.domain.core.Implementation
{
    public class OrganizationOperationService : IOrganizationOperationService
    {
        private readonly IGenericDbRepository<OrganizationDbModel> _organizationRepository;

        public OrganizationOperationService(
            IGenericDbRepository<OrganizationDbModel> organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public Task<OrganizationDbModel> CreateOrganizationAsync(string name)
        {
            OrganizationDbModel organization = new()
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = name
            };

            return _organizationRepository.CreateAsync(organization);
        }

        public Task<IEnumerable<OrganizationDbModel>> GetAllAsync()
        {
            return _organizationRepository.QueryAllAsync();
        }

        public Task<OrganizationDbModel?> GetOrganizationByNameAsync(string name)
        {
            Expression<Func<OrganizationDbModel, bool>> predicate = orgDb => orgDb.Name == name;
            return _organizationRepository.QueryFirstOrDefaultAsync(predicate);
        }
    }
}
