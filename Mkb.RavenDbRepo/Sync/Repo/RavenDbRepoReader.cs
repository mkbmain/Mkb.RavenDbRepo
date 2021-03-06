using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mkb.RavenDbRepo.Configs;
using Mkb.RavenDbRepo.Entities;
using Mkb.RavenDbRepo.Sync.Interfaces;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;

namespace Mkb.RavenDbRepo.Sync.Repo
{
    public class RavenDbRepoReader<T> : IRavenReaderRepo<T> where T : RavenEntity
    {
        protected readonly IDocumentStore Store;

        public RavenDbRepoReader(RavenDbConfig ravenDbConfig)
        {
            var store = new DocumentStore
            {
                Urls = ravenDbConfig.Urls,
                Database = ravenDbConfig.DataBase
            };
            Store = store.Initialize();
        }

        public RavenDbRepoReader(IDocumentStore documentStore)
        {
            Store = documentStore.Initialize();
        }

        public bool Any<TEntity>(Expression<Func<TEntity, bool>> @where, bool includeSoftDelete = false) where TEntity : T
        {
            return GenericGetQuery(Store,
                action:  f =>  f.Any(),
                @where: where,
                orderBy: null,
                orderByDescending: false,
                returnDeleted: includeSoftDelete);
        }

        public TEntity Get<TEntity>(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy = null, bool orderByDescending = false, bool includeSoftDelete = false) where TEntity : T
        {
            return Get(@where: where,
                projection: f => f,
                orderBy: orderBy,
                orderByDescending: orderByDescending,
                includeSoftDelete: includeSoftDelete);
        }

        public TOut Get<TEntity, TOut>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TOut>> projection,
            Expression<Func<TEntity, object>> orderBy = null, bool orderByDescending = false, bool includeSoftDelete = false) where TEntity : T
        {
            return GenericGetQuery(Store,
                action:  f =>  f.Select(projection).FirstOrDefault(),
                @where: where,
                orderBy: orderBy,
                orderByDescending: orderByDescending,
                returnDeleted: includeSoftDelete);
        }

        public List<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, object>> orderBy = null, bool orderByDescending = false, bool includeSoftDelete = false) where TEntity : T
        {
            return GetAll(where, f => f, orderBy: orderBy, orderByDescending: orderByDescending, includeSoftDelete: includeSoftDelete);
        }

        public List<TOut> GetAll<TEntity, TOut>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TOut>> projection,
            Expression<Func<TEntity, object>> orderBy = null, bool orderByDescending = false, bool includeSoftDelete = false) where TEntity : T
        {
            return GenericGetQuery(Store,  f =>  f.Select(projection).ToList(),
                @where: where,
                orderBy: orderBy,
                orderByDescending: orderByDescending,
                returnDeleted: includeSoftDelete);
        }

        private static TOut GenericGetQuery<TEntity, TOut>(IDocumentStore store, Func<IRavenQueryable<TEntity>, TOut> action,
            Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, object>> orderBy = null,
            bool orderByDescending = false,
            bool returnDeleted = false)
            where TEntity : RavenEntity
        {
            return InternalExecutors.Execute(store, f =>
            {
                var set = f.Query<TEntity>();
                set = where != null ? set.Where(where) : set;
                set = returnDeleted ? set : set.Where(ravenEntity => !ravenEntity.DeletedAt.HasValue);
                return orderBy == null ? action(set) : action(orderByDescending ? set.OrderByDescending(orderBy) : set.OrderBy(orderBy));
            });
        }
    }
}