using di_sample.domain.core.Constants;
using di_sample.domain.core.Interfaces.Db;
using di_sample.domain.infrastrcture.Models.Db;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace di_sample.domain.infrastrcture.Implementation.Db
{
    internal abstract class GenericDbRepository<TDbModel> : IGenericDbRepository<TDbModel>, IDisposable
        where TDbModel : BaseDbModel
    {
        private readonly string _databaseName;
        private readonly string _containerName;

        private bool _disposed;

        protected CosmosClient CosmosClient { get; }

        protected Container Container => CosmosClient.GetContainer(_databaseName, _containerName);

        protected GenericDbRepository(
            CosmosClient cosmosClient,
            string databaseName,
            string containerName)
        {
            CosmosClient = cosmosClient;
            _databaseName = databaseName;
            _containerName = containerName;
        }

        
        public async Task<TDbModel> CreateAsync(TDbModel dbModel)
        {
            dbModel.CreatedTimeUtc = DateTime.UtcNow;
            dbModel = await Container.CreateItemAsync(dbModel);

            return dbModel;
        }

        public async Task<IEnumerable<TDbModel>> QueryAsync(
            Expression<Func<TDbModel, bool>> predicate,
            Expression<Func<TDbModel, TDbModel>>? selectExpression = null,
            Expression<Func<TDbModel, IComparable>>? orderBy = null,
            bool orderByAscending = true,
            string? partitionKeyValue = null,
            int pageSize = DbConstants.DefaultPageSize)
        {
            List<TDbModel> results = [];

            using (FeedIterator<TDbModel> feedIterator = BuildFeedIterator(
                                                    predicate,
                                                    selectExpression,
                                                    orderBy,
                                                    orderByAscending,
                                                    partitionKeyValue,
                                                    pageSize))
            { 

                while (feedIterator.HasMoreResults)
                {
                    FeedResponse<TDbModel> response = await feedIterator.ReadNextAsync();
                    results.AddRange(response);
                }
            }

            return results; 
        }

        public Task<IEnumerable<TDbModel>> QueryAllAsync(
            Expression<Func<TDbModel, TDbModel>>? selectExpression = null,
            Expression<Func<TDbModel, IComparable>>? orderBy = null,
            bool orderByAscending = true,
            string? partitionKeyValue = null,
            int pageSize = DbConstants.DefaultPageSize)
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
                pageSize);
        }

        public async Task<TDbModel?> QueryFirstOrDefaultAsync(
            Expression<Func<TDbModel, bool>> predicate,
            Expression<Func<TDbModel, TDbModel>>? selectExpression = null,
            Expression<Func<TDbModel, IComparable>>? orderBy = null,
            bool orderByAscending = true,
            string? partitionKeyValue = null)
        {
            using (FeedIterator<TDbModel> feedIterator = BuildFeedIterator(
                                                    predicate,
                                                    selectExpression,
                                                    orderBy,
                                                    orderByAscending,
                                                    partitionKeyValue,
                                                    1,
                                                    true))
            {
                if (feedIterator.HasMoreResults)
                {
                    FeedResponse<TDbModel> response = await feedIterator.ReadNextAsync();
                    TDbModel? dbModel = response.FirstOrDefault();
                    return dbModel ?? null;
                }
            }

            return null; // no matches
        }

        private FeedIterator<TDbModel> BuildFeedIterator(
            Expression<Func<TDbModel, bool>> predicate, 
            Expression<Func<TDbModel, TDbModel>>? selectExpression, 
            Expression<Func<TDbModel, IComparable>>? orderBy, 
            bool orderByAscending, 
            string? partitionKeyValue, 
            int pageSize,
            bool takeOne = false)
        {
            QueryRequestOptions queryRequestOptions = new()
            {
                PartitionKey = string.IsNullOrWhiteSpace(partitionKeyValue) ? null : new PartitionKey(partitionKeyValue),
                MaxItemCount = pageSize
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

            if (takeOne)
            {
                queryable = queryable.Take(1); // 👈 enforced at Cosmos SQL level
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
