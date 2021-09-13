using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mkb.RavenDbRepo.Entities;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace Mkb.RavenDbRepo.Async
{
    internal static class InternalAsyncExecutors
    {
        internal static Task<TOut> GenericGetQueryAsync<TEntity, TOut>(IDocumentStore store, Func<IRavenQueryable<TEntity>, Task<TOut>> action,
            Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, object>> orderBy = null,
            bool orderByDescending = false,
            bool returnDeleted = false)
            where TEntity : RavenEntity
        {
            return ExecuteAsync(store, async f =>
            {
                var set = f.Query<TEntity>();
                set = where != null ? set.Where(where) : set;
                set = returnDeleted ? set : set.Where(ravenEntity => !ravenEntity.DeletedAt.HasValue);
                return orderBy == null ? await action(set) : await action(orderByDescending ? set.OrderByDescending(orderBy) : set.OrderBy(orderBy));
            });
        }

        internal static Func<IAsyncDocumentSession, Task<bool>> AsyncEnumerableWithSave<TEntity>(IEnumerable<TEntity> items, Func<IAsyncDocumentSession, TEntity, Task> action) =>
            async session =>
            {
                foreach (var item in items)
                {
                    await action(session, item);
                }

                await session.SaveChangesAsync(); // we need to await and can't return task as the using will close at end of method block
                return true;
            };

        internal static async Task<T> ExecuteAsync<T>(IDocumentStore store, Func<IAsyncDocumentSession, Task<T>> action)
        {
            using var session = store.OpenAsyncSession();
            return (await action(session)); // we need to await and can't return task as the using will close at end of method block
        }
    }
}