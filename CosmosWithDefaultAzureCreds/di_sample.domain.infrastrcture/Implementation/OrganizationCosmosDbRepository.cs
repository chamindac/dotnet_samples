using di_sample.domain.core.Interfaces;
using di_sample.domain.core.Models;
using di_sample.domain.infrastrcture.Mappers;
using di_sample.domain.infrastrcture.Models.Db;
using Microsoft.Azure.Cosmos;

namespace di_sample.domain.infrastrcture.Implementation
{
    internal class OrganizationCosmosDbRepository 
        : GenericCosmosDbRepository<Organization,OrganizationCosmosDbModel>, IGenericDbRepository<Organization>
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
    }
}
