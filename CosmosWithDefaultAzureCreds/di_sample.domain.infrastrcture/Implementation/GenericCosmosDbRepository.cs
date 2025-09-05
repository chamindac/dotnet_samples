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
    internal abstract class GenericCosmosDbRepository<TDomainModel, TDbModel> : IDisposable
        where TDomainModel : BaseModel
        where TDbModel : BaseCosmosDbModel
    {
        private const int DefaultMaxItemCount = 100;

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
            Expression<Func<TDbModel, TDbModel>>? selectExpression = null,
            Expression<Func<TDbModel, IComparable>>? orderBy = null,
            bool orderByAscending = true,
            string? partitionKeyValue = null,
            int maxItemCount = DefaultMaxItemCount)
        {
            FeedIterator<TDbModel> feedIterator = BuildFeedIterator(
                                                    predicate, 
                                                    selectExpression, 
                                                    orderBy, 
                                                    orderByAscending, 
                                                    partitionKeyValue, 
                                                    maxItemCount);

            while (feedIterator.HasMoreResults)
            {
                FeedResponse<TDbModel> response = await feedIterator.ReadNextAsync();
                foreach (TDbModel dbModel in response)
                {
                    yield return ToDomainModel(dbModel);
                }
            }
        }

        protected IAsyncEnumerable<TDomainModel> QueryAllAsync(
            Expression<Func<TDbModel, TDbModel>>? selectExpression = null,
            Expression<Func<TDbModel, IComparable>>? orderBy = null,
            bool orderByAscending = true,
            string? partitionKeyValue = null,
            int maxItemCount = DefaultMaxItemCount)
        {
            // Use a predicate that matches everything
            Expression<Func<TDbModel, bool>> allPredicate = _ => true;

            // Reuse QueryAsync
            return QueryAsync(
                allPredicate,
                selectExpression,
                orderBy,
                orderByAscending,
                partitionKeyValue,
                maxItemCount);
        }

        protected async Task<TDomainModel?> QueryFirstOrDefaultAsync(
            Expression<Func<TDbModel, bool>> predicate,
            Expression<Func<TDbModel, TDbModel>>? selectExpression = null,
            Expression<Func<TDbModel, IComparable>>? orderBy = null,
            bool orderByAscending = true,
            string? partitionKeyValue = null)
        {
            FeedIterator<TDbModel> feedIterator = BuildFeedIterator(
                                                    predicate, 
                                                    selectExpression, 
                                                    orderBy, 
                                                    orderByAscending, 
                                                    partitionKeyValue, 
                                                    1);
            
            if (feedIterator.HasMoreResults)
            {
                FeedResponse<TDbModel> response = await feedIterator.ReadNextAsync();
                TDbModel? dbModel = response.FirstOrDefault();
                return dbModel != null ? ToDomainModel(dbModel) : null;
            }

            return null; // no matches
        }

        private FeedIterator<TDbModel> BuildFeedIterator(
            Expression<Func<TDbModel, bool>> predicate, 
            Expression<Func<TDbModel, TDbModel>>? selectExpression, 
            Expression<Func<TDbModel, IComparable>>? orderBy, 
            bool orderByAscending, 
            string? partitionKeyValue, 
            int maxItemCount)
        {
            QueryRequestOptions queryRequestOptions = new()
            {
                PartitionKey = string.IsNullOrWhiteSpace(partitionKeyValue) ? null : new PartitionKey(partitionKeyValue),
                MaxItemCount = maxItemCount
            };

            IQueryable<TDbModel> queryable = Container
                .GetItemLinqQueryable<TDbModel>(
                    allowSynchronousQueryExecution: false,
                    requestOptions: queryRequestOptions)
                .Where(predicate);

            if (orderBy != null)
            {
                queryable = orderByAscending
                    ? queryable.OrderBy(orderBy)
                    : queryable.OrderByDescending(orderBy);
            }

            if (selectExpression != null)
            {
                queryable = queryable.Select(selectExpression);
            }

            FeedIterator<TDbModel> feedIterator = queryable.ToFeedIterator();
            return feedIterator;
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
