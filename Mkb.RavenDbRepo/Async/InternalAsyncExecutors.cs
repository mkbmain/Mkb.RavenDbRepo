using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Mkb.RavenDbRepo.Async
{
    internal static class InternalAsyncExecutors
    {
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
            using (var session = store.OpenAsyncSession())
            {
                return (await action(session)); // we need to await and can't return task as the using will close at end of method block
            }
        }
    }
}