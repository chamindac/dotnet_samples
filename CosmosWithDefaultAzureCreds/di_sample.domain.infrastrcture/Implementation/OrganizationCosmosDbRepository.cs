using di_sample.domain.core.Interfaces;
using di_sample.domain.core.Models;
using di_sample.domain.infrastrcture.Mappers;
using di_sample.domain.infrastrcture.Models.Db;
using Microsoft.Azure.Cosmos;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;

namespace di_sample.domain.infrastrcture.Implementation
{
    internal class OrganizationCosmosDbRepository
        : GenericCosmosDbRepository<Organization, OrganizationCosmosDbModel>, IOrganizationDbRepository<Organization>
    {
        public OrganizationCosmosDbRepository(
            CosmosClient cosmosClient) :
            base(cosmosClient,
                "px",
                "organizations")
        {
        }

        protected override OrganizationCosmosDbModel ToDbModel(Organization domainModel)
            => domainModel.ToDbModel();

        protected override Organization ToDomainModel(OrganizationCosmosDbModel dbModel)
            => dbModel.ToDomainModel();

        public Task<Organization?> GetByNameAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            Expression<Func<OrganizationCosmosDbModel, bool>> predicate = orgDb => orgDb.Name == name;
            return QueryFirstOrDefaultAsync(predicate, null, null, cancellationToken);
        }

        public async Task<IEnumerable<Organization>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            List<Organization> result = [];

            IAsyncEnumerable<Organization> allOrganizations = QueryAllAsync(cancellationToken: cancellationToken);

            await foreach (Organization org in allOrganizations)
            {
                result.Add(org);
            }

            return result;
        }
    }
}
