using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Mkb.RavenDbRepo.Entities;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace Mkb.RavenDbRepo.Sync
{
    internal static class InternalExecutors
    {
        internal static TOut GenericGetQuery<TEntity, TOut>(IDocumentStore store, Func<IRavenQueryable<TEntity>, TOut> action,
            Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, object>> orderBy = null,
            bool orderByDescending = false,
            bool returnDeleted = false)
            where TEntity : RavenEntity
        {
            return Execute(store, f =>
            {
                var set = f.Query<TEntity>();
                set = where != null ? set.Where(where) : set;
                set = returnDeleted ? set : set.Where(ravenEntity => !ravenEntity.DeletedAt.HasValue);
                return orderBy == null ? action(set) : action(orderByDescending ? set.OrderByDescending(orderBy) : set.OrderBy(orderBy));
            });
        }

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
            using var session = store.OpenSession();
            return (action(session)); // we need to await and can't return task as the using will close at end of method block
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