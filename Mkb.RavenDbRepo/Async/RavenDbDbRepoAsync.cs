using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mkb.RavenDbRepo.Async.Interfaces;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;

namespace Mkb.RavenDbRepo.Async
{
    public class RavenDbDbRepoAsync<T> : RavenDbRepoReaderAsync<T>, IRavenRepoAsync<T> where T : RavenEntity
    {
        public RavenDbDbRepoAsync(RavenDbConfig ravenDbConfig) : base(ravenDbConfig)
        {
        }

        public RavenDbDbRepoAsync(IDocumentStore documentStore) : base(documentStore: documentStore)
        {
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

        public Task AddMany<TEntity>(IEnumerable<TEntity> entities) where TEntity : T
        {
            return InternalAsyncExecutors.ExecuteAsync(Store,
                InternalAsyncExecutors.AsyncEnumerableWithSave(entities, async (session, item) => { await session.StoreAsync(item); }));
        }

        public Task Delete<TEntity>(TEntity entity) where TEntity : T
        {
            return DeleteMany(new[] { entity });
        }

        public Task DeleteMany<TEntity>(IEnumerable<TEntity> entities) where TEntity : T
        {
            return InternalAsyncExecutors.ExecuteAsync(Store, InternalAsyncExecutors.AsyncEnumerableWithSave(entities, async (session, item) =>
            {
                item.DeletedAt = DateTime.UtcNow;
                await session.StoreAsync(item);
            }));
        }

        public Task HardDelete<TEntity>(TEntity entity) where TEntity : T
        {
            return HardDeleteMany(new[] { entity });
        }

        public Task HardDeleteMany<TEntity>(IEnumerable<TEntity> entities) where TEntity : T
        {
            return InternalAsyncExecutors.ExecuteAsync(Store,
                InternalAsyncExecutors.AsyncEnumerableWithSave(entities, async (session, item) => { session.Delete(item.Id); }));
        }
    }
}