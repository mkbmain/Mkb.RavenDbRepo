using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace Mkb.RavenDbRepo.Async
{
    public abstract class RavenRepoAsyncBase
    {
        protected readonly IDocumentStore _store;

        public RavenRepoAsyncBase(RavenDbConfig ravenDbConfig)
        {
            var store = new DocumentStore
            {
                Urls = ravenDbConfig.Urls,
                Database = ravenDbConfig.DataBase
            };
            _store = store.Initialize();
        }

        public RavenRepoAsyncBase(IDocumentStore documentStore)
        {

            _store = documentStore.Initialize();
        }

        protected Task<Tout> GenericGetQueryAsync<TEntity, Tout>(Func<IRavenQueryable<TEntity>, Task<Tout>> action, Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, object>> orderBy = null,
            bool orderByDescending = false,
            bool returnDeleted = false)
            where TEntity : RavenEntity
        {
            return ExecuteAsync(async f =>
            {
                var set = f.Query<TEntity>();
                if (where != null)
                {
                    set = set.Where(where);
                }

                if (!returnDeleted)
                {
                    set = set.Where(ravenEntity => !ravenEntity.DeletedAt.HasValue);
                }

                if (orderBy != null)
                {
                    set = orderByDescending ? set.OrderByDescending(orderBy) : set.OrderBy(orderBy);
                }

                return await action(set);
            });
        }

        protected static Func<IAsyncDocumentSession, Task<bool>> AsyncEnumerableWithSave<TEntity>(IEnumerable<TEntity> items, Func<IAsyncDocumentSession, TEntity, Task> action) =>
            async session =>
            {
                foreach (var item in items)
                {
                    await action(session, item);
                }

                await session.SaveChangesAsync();
                return true;
            };

        protected async Task<T> ExecuteAsync<T>(Func<IAsyncDocumentSession, Task<T>> action)
        {
            using var session = _store.OpenAsyncSession();
            return (await action(session)); // we need to await and can't return task as the using will close at end of method block
        }
    }
}