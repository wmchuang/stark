using System.Linq.Expressions;
using MongoDB.Driver;
using Stark.Starter.Mongodb.Context;
using Stark.Starter.Mongodb.Domain;

namespace Stark.Starter.Mongodb.Repository
{
    public class MongoRepository<TCollection> : IMongoRepository<TCollection> where TCollection : DataCollection
    {
        private readonly IMongoCollection<TCollection> _collection;

        public MongoRepository(IMongoContext context)
        {
            _collection = context.Db.GetCollection<TCollection>(typeof(TCollection).Name);
        }

        public async Task<TCollection> AddAsync(TCollection item)
        {
            await _collection.InsertOneAsync(item);
            return await GetAsync(item.Id);
        }

        public async Task AddManyAsync(List<TCollection> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public async Task<TCollection> GetAsync(string id)
        {
            return await _collection.Find(item => item.Id == id)?.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TCollection>> GetAllAsync()
        {
            var result = await _collection.FindAsync(item => true);
            return result?.ToList() ?? new List<TCollection>();
        }

        public async Task<TCollection> GetAsync(Expression<Func<TCollection, bool>> selector)
        {
            // ReSharper disable once PossibleNullReferenceException
            return await _collection.Find(selector)?.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TCollection>> GetListAsync(Expression<Func<TCollection, bool>> selector)
        {
            var result = await _collection.FindAsync(selector);
            return result?.ToList() ?? new List<TCollection>();
        }

        public async Task<IEnumerable<TCollection>> GetFilterListAsync(
            Expression<Func<TCollection, bool>>[] filters)
        {
            var builder =
                Builders<TCollection>.Filter.Empty;
            if (filters.Length > 0)
            {
                builder =
                    Builders<TCollection>.Filter.And(filters.Select(ex => Builders<TCollection>.Filter.Where(ex)));
            }

            var result = await _collection.FindAsync(builder);
            return result?.ToList() ?? new List<TCollection>();
        }

        public async Task<long> CountAsync(Expression<Func<TCollection, bool>> filter)
        {
            return await _collection.CountDocumentsAsync(filter);
        }

        public async Task<long> CountAsync(Expression<Func<TCollection, bool>>[] filters)
        {
            var builder =
                Builders<TCollection>.Filter.Empty;
            if (filters.Length > 0)
            {
                builder =
                    Builders<TCollection>.Filter.And(filters.Select(ex => Builders<TCollection>.Filter.Where(ex)));
            }

            return await _collection.CountDocumentsAsync(builder);
        }

        /// <returns></returns>
        public async Task<IEnumerable<TCollection>> GetFilterPagedBySortAsync(
            Expression<Func<TCollection, bool>>[] filters,
            Expression<Func<TCollection, object>> sortBy, int pageSize, int pageIndex, bool sortDec = false)
        {
            if (sortBy == null)
                return await GetFilterPagedAsync(filters, pageSize, pageIndex);

            if (pageIndex == 0)
                return new List<TCollection>();

            var builder =
                Builders<TCollection>.Filter.Empty;
            if (filters.Length > 0)
            {
                builder =
                    Builders<TCollection>
                        .Filter
                        .And(filters.Select(ex => Builders<TCollection>.Filter.Where(ex)));
            }
            if (sortDec)
            {
                return await _collection.Find(builder)
               .SortByDescending(sortBy)
               .Skip(pageSize * (pageIndex - 1))
               .Limit(pageSize)
               .ToListAsync();
            }
            return await _collection.Find(builder)
               .SortBy(sortBy)
               .Skip(pageSize * (pageIndex - 1))
               .Limit(pageSize)
               .ToListAsync();
        }

        public async Task<IEnumerable<TCollection>> GetFilterPagedBySortAsync(
            Expression<Func<TCollection, bool>>[] filters,
            Expression<Func<TCollection, object>>[] sortBys, int pageSize, int pageIndex)
        {
            if (sortBys == null)
                return await GetFilterPagedAsync(filters, pageSize, pageIndex);

            if (pageIndex == 0)
                return new List<TCollection>();

            var builder =
                Builders<TCollection>.Filter.Empty;
            if (filters.Length > 0)
            {
                builder =
                    Builders<TCollection>
                        .Filter
                        .And(filters.Select(ex => Builders<TCollection>.Filter.Where(ex)));
            }

            var filter = _collection.Find(builder)
                .SortBy(sortBys[0]);

            for (int i = 1; i < sortBys.Length; i++)
            {
                filter.ThenBy(sortBys[i]);
            }

            return await filter.Skip(pageSize * (pageIndex - 1))
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<TCollection>> GetFilterPagedBySortDescendingAsync(
            Expression<Func<TCollection, bool>>[] filters,
            Expression<Func<TCollection, object>> sortBy, int pageSize, int pageIndex)
        {
            if (sortBy == null)
                return await GetFilterPagedAsync(filters, pageSize, pageIndex);

            if (pageIndex == 0)
                return new List<TCollection>();

            var builder =
                Builders<TCollection>.Filter.Empty;
            if (filters.Length > 0)
            {
                builder =
                    Builders<TCollection>
                        .Filter
                        .And(filters.Select(ex => Builders<TCollection>.Filter.Where(ex)));
            }

            return await _collection.Find(builder)
                .SortByDescending(sortBy)
                .Skip(pageSize * (pageIndex - 1))
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<TCollection>> GetFilterPagedBySortDescendingAsync(
            Expression<Func<TCollection, bool>>[] filters,
            Expression<Func<TCollection, object>>[] sortBys, int pageSize, int pageIndex)
        {
            if (sortBys == null)
                return await GetFilterPagedAsync(filters, pageSize, pageIndex);

            if (pageIndex == 0)
                return new List<TCollection>();

            var builder =
                Builders<TCollection>.Filter.Empty;
            if (filters.Length > 0)
            {
                builder =
                    Builders<TCollection>
                        .Filter
                        .And(filters.Select(ex => Builders<TCollection>.Filter.Where(ex)));
            }

            var filter = _collection.Find(builder)
                .SortByDescending(sortBys[0]);

            for (int i = 1; i < sortBys.Length; i++)
            {
                filter.ThenByDescending(sortBys[i]);
            }

            return await filter.Skip(pageSize * (pageIndex - 1))
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<TCollection>> GetFilterPagedAsync(Expression<Func<TCollection, bool>>[] filters,
            int pageSize, int pageIndex)
        {
            if (pageIndex == 0)
                return new List<TCollection>();

            var builder =
                Builders<TCollection>.Filter.Empty;
            if (filters.Length > 0)
            {
                builder =
                    Builders<TCollection>
                        .Filter
                        .And(filters.Select(ex => Builders<TCollection>.Filter.Where(ex)));
            }

            return await _collection.Find(builder)
                .Skip(pageSize * (pageIndex - 1))
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task<bool> RemoveAllAsync()
        {
            var result = await _collection.DeleteManyAsync(item => true);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<bool> RemoveAsync(TCollection model)
        {
            var result = await _collection.DeleteOneAsync(item => model.Id == item.Id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<bool> RemoveRangeAsync(Expression<Func<TCollection, bool>> selector)
        {
            var result = await _collection.DeleteManyAsync(selector);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<bool> UpdateAsync(TCollection model)
        {
            var result = await _collection.ReplaceOneAsync(item => item.Id == model.Id, model);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> UpdateRangeAsync(IEnumerable<TCollection> models)
        {
            var result = true;
            foreach (var model in models)
            {
                result &= await UpdateAsync(model);
            }

            return result;
        }
    }
}