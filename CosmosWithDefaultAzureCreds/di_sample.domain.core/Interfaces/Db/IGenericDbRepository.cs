using di_sample.domain.core.Constants;
using di_sample.domain.infrastrcture.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace di_sample.domain.core.Interfaces.Db
{
    public interface IGenericDbRepository<TDbModel> where TDbModel : BaseDbModel
    {
        Task<TDbModel> CreateAsync(TDbModel organization);

        Task<IEnumerable<TDbModel>> QueryAsync(
            Expression<Func<TDbModel, bool>> predicate,
            Expression<Func<TDbModel, TDbModel>>? selectExpression = null,
            Expression<Func<TDbModel, IComparable>>? orderBy = null,
            bool orderByAscending = true,
            string? partitionKeyValue = null,
            int maxItemCount = DbConstants.DefaultMaxItemCount);

        Task<IEnumerable<TDbModel>> QueryAllAsync(
            Expression<Func<TDbModel, TDbModel>>? selectExpression = null,
            Expression<Func<TDbModel, IComparable>>? orderBy = null,
            bool orderByAscending = true,
            string? partitionKeyValue = null,
            int maxItemCount = DbConstants.DefaultMaxItemCount);

        Task<TDbModel?> QueryFirstOrDefaultAsync(
            Expression<Func<TDbModel, bool>> predicate,
            Expression<Func<TDbModel, TDbModel>>? selectExpression = null,
            Expression<Func<TDbModel, IComparable>>? orderBy = null,
            bool orderByAscending = true,
            string? partitionKeyValue = null);
    }
}
