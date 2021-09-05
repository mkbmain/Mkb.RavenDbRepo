using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mkb.RavenDbRepo.Async.Interfaces;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;

namespace Mkb.RavenDbRepo.Async
{
    public class RavenRepoAsync<T> : RavenRepoAsyncBase, IRavenRepoAsync<T> where T : RavenEntity
    {
        public RavenRepoAsync(RavenDbConfig ravenDbConfig) : base(ravenDbConfig)
        {
        }

        public RavenRepoAsync(IDocumentStore documentStore) : base(documentStore: documentStore)
        {
        }

        public Task<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy = null, bool orderByDescending = false, bool includeSoftDelete = false) where TEntity : T
        {
            return Get(where: where,
                projection: f => f,
                orderBy: orderBy,
                orderByDescending: orderByDescending,
                includeSoftDelete: includeSoftDelete);
        }

        public Task<TOut> Get<TEntity, TOut>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TOut>> projection,
            Expression<Func<TEntity, object>> orderBy = null, bool orderByDescending = false, bool includeSoftDelete = false) where TEntity : T
        {
            return GenericGetQueryAsync(
                action: async f => await f.Select(projection).FirstOrDefaultAsync(),
                where: where,
                orderBy: orderBy,
                orderByDescending: orderByDescending,
                returnDeleted: includeSoftDelete);
        }

        public Task<List<TEntity>> GetAll<TEntity>(Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, object>> orderBy = null, bool orderByDescending = false, bool includeSoftDelete = false) where TEntity : T
        {
            return GetAll(where, f => f, orderBy: orderBy, orderByDescending: orderByDescending, includeSoftDelete: includeSoftDelete);
        }

        public Task<List<TOut>> GetAll<TEntity, TOut>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TOut>> projection,
            Expression<Func<TEntity, object>> orderBy = null, bool orderByDescending = false, bool includeSoftDelete = false) where TEntity : T
        {
            return GenericGetQueryAsync(async f => await f.Select(projection).ToListAsync(),
                where: where,
                orderBy: orderBy,
                orderByDescending: orderByDescending,
                returnDeleted: includeSoftDelete);
        }

        public Task Update<TEntity>(TEntity entity) where TEntity : T
        {
            return UpdateMany(new[] { entity });
        }

        public Task UpdateMany<TEntity>(IEnumerable<TEntity> entity) where TEntity : T
        {
            return AddMany(entity);
        }

        public Task Add<TEntity>(TEntity entity) where TEntity : T
        {
            return AddMany(new[] { entity });
        }

        public Task AddMany<TEntity>(IEnumerable<TEntity> entitys) where TEntity : T
        {
            return ExecuteAsync(AsyncEnumerableWithSave(entitys, async (session, item) => { await session.StoreAsync(item); }));
        }

        public Task Delete<TEntity>(TEntity entity) where TEntity : T
        {
            return DeleteMany(new[] { entity });
        }

        public Task DeleteMany<TEntity>(IEnumerable<TEntity> entitys) where TEntity : T
        {
            return ExecuteAsync(AsyncEnumerableWithSave(entitys, async (session, item) =>
            {
                item.DeletedAt = DateTime.UtcNow;
                await session.StoreAsync(item);
            }));
        }

        public Task HardDelete<TEntity>(TEntity entity) where TEntity : T
        {
            return HardDeleteMany(new[] { entity });
        }

        public Task HardDeleteMany<TEntity>(IEnumerable<TEntity> entitys) where TEntity : T
        {
            return ExecuteAsync(AsyncEnumerableWithSave(entitys, async (session, item) => { session.Delete(item.Id); }));
        }
    }
}