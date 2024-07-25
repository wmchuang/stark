using System.Linq.Expressions;

namespace Stark.Starter.Mongodb.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> selector);
        Task<T> GetAsync(Expression<Func<T, bool>> selector);
        Task<T> AddAsync(T item);
        Task AddManyAsync(List<T> documents);
        Task<bool> RemoveAsync(T model);
        Task<bool> RemoveRangeAsync(Expression<Func<T, bool>> selector);
        Task<bool> UpdateAsync(T model);
        Task<bool> UpdateRangeAsync(IEnumerable<T> models);
        Task<bool> RemoveAllAsync();
    }
}