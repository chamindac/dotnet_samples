using di_sample.domain.core.Models;
using di_sample.domain.infrastrcture.Models.Db;
using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;

namespace di_sample.domain.infrastrcture.Implementation
{
    internal abstract class GenericCosmosDbRepository<TDomainModel, TDbModel>: IDisposable
        where TDomainModel : BaseModel
        where TDbModel : BaseCosmosDbModel
    {
        private readonly string _databaseName;
        private readonly string _containerName;

        private bool _disposed;

        protected CosmosClient CosmosClient { get; }

        protected Container Container => CosmosClient.GetContainer(_databaseName, _containerName);

        protected GenericCosmosDbRepository(
            CosmosClient cosmosClient,
            string databaseName,
            string containerName)
        {
            CosmosClient = cosmosClient;
            _databaseName = databaseName;
            _containerName = containerName;
        }

        protected abstract TDbModel ToDbModel(TDomainModel domainModel);
        protected abstract TDomainModel ToDomainModel(TDbModel dbModel);

        public async Task<TDomainModel> CreateAsync(TDomainModel domainModel)
        {
            domainModel.CreatedTimeUtc = DateTime.UtcNow;
            TDbModel dbModel = ToDbModel(domainModel);
            dbModel = await Container.CreateItemAsync(dbModel);

            return ToDomainModel(dbModel);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                CosmosClient.Dispose();
                GC.SuppressFinalize(this);
            }
        }
    }
}
