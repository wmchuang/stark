using System.Linq.Expressions;
using Stark.Starter.Mongodb.Domain;

namespace Stark.Starter.Mongodb.Repository
{
    public interface IMongoRepository<TCollection> : IRepository<TCollection> where TCollection : DataCollection
    {
        Task<TCollection> GetAsync(string id);
        
        Task<IEnumerable<TCollection>> GetFilterListAsync(Expression<Func<TCollection, bool>>[] filters);

        Task<long> CountAsync(Expression<Func<TCollection, bool>> filter);

        Task<long> CountAsync(Expression<Func<TCollection, bool>>[] filters);

        Task<IEnumerable<TCollection>> GetFilterPagedBySortAsync(Expression<Func<TCollection, bool>>[] filters,
            Expression<Func<TCollection, object>> sortBy, int pageSize, int pageIndex, bool sortDec = false);

        Task<IEnumerable<TCollection>>  GetFilterPagedBySortAsync(
            Expression<Func<TCollection, bool>>[] filters,
            Expression<Func<TCollection, object>>[] sortBys, int pageSize, int pageIndex);

        Task<IEnumerable<TCollection>> GetFilterPagedBySortDescendingAsync(
            Expression<Func<TCollection, bool>>[] filters,
            Expression<Func<TCollection, object>> sortBy, int pageSize, int pageIndex);

        Task<IEnumerable<TCollection>> GetFilterPagedBySortDescendingAsync(
            Expression<Func<TCollection, bool>>[] filters,
            Expression<Func<TCollection, object>>[] sortBys, int pageSize, int pageIndex);

        Task<IEnumerable<TCollection>> GetFilterPagedAsync(Expression<Func<TCollection, bool>>[] filters,
            int pageSize, int pageIndex);
    }
}