using di_sample.domain.core.Models;
using di_sample.domain.infrastrcture.Models.Db;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Linq;
using System.Runtime.CompilerServices;

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

        protected async IAsyncEnumerable<TDomainModel> QueryAsync(
            Expression<Func<TDbModel, bool>> predicate,
            QueryRequestOptions? requestOptions = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            IQueryable<TDbModel> queryable = Container
                .GetItemLinqQueryable<TDbModel>(
                    allowSynchronousQueryExecution: false,
                    requestOptions: requestOptions)
                .Where(predicate);

            FeedIterator<TDbModel> feedIterator = queryable.ToFeedIterator();

            while (feedIterator.HasMoreResults)
            {
                FeedResponse<TDbModel> response = await feedIterator.ReadNextAsync(cancellationToken);
                foreach (TDbModel dbModel in response)
                {
                    yield return ToDomainModel(dbModel);
                }
            }
        }

        protected async Task<TDomainModel?> FirstOrDefaultAsync(
            Expression<Func<TDbModel, bool>> predicate,
            QueryRequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            await foreach (TDomainModel item in QueryAsync(predicate, requestOptions, cancellationToken))
            {
                return item; // immediately return first
            }
            return null;
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
