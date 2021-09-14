using System;
using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Mkb.RavenDbRepo.Sync
{
    internal static class InternalExecutors
    {
        internal static Action<IDocumentSession> EnumerableWithSave<TEntity>(IEnumerable<TEntity> items, Action<IDocumentSession, TEntity> action) =>
            session =>
            {
                foreach (var item in items)
                {
                    action(session, item);
                }

                session.SaveChanges(); // we need to await and can't return task as the using will close at end of method block
            };

        internal static T Execute<T>(IDocumentStore store, Func<IDocumentSession, T> action)
        {
            using (var session = store.OpenSession())
            {
                return (action(session)); // we need to await and can't return task as the using will close at end of method block
            }
        }

        internal static void ExecuteVoid(IDocumentStore store, Action<IDocumentSession> action)
        {
            Execute(store, t =>
            {
                action(t);
                return true;
            });
        }
    }
}