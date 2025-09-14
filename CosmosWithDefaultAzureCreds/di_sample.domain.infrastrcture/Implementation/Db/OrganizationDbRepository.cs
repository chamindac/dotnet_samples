using di_sample.domain.core.Models.Db;
using Microsoft.Azure.Cosmos;

namespace di_sample.domain.infrastrcture.Implementation.Db
{
    internal class OrganizationDbRepository
        : GenericDbRepository<OrganizationDbModel>
    {
        public OrganizationDbRepository(
            CosmosClient cosmosClient) :
            base(cosmosClient,
                "px",
                "organizations")
        {
        }
    }
}
