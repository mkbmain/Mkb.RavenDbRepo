using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mkb.RavenDbRepo.Async.Interfaces;
using Mkb.RavenDbRepo.Configs;
using Mkb.RavenDbRepo.Entities;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;

namespace Mkb.RavenDbRepo.Async.Repo
{
    public class RavenDbRepoReaderAsync<T> : IRavenReaderRepoAsync<T> where T : RavenEntity
    {
        protected readonly IDocumentStore Store;

        public RavenDbRepoReaderAsync(RavenDbConfig ravenDbConfig)
        {
            var store = new DocumentStore
            {
                Urls = ravenDbConfig.Urls,
                Database = ravenDbConfig.DataBase
            };
            Store = store.Initialize();
        }

        public RavenDbRepoReaderAsync(IDocumentStore documentStore)
        {
            Store = documentStore.Initialize();
        }

        public Task<bool> Any<TEntity>(Expression<Func<TEntity, bool>> where,bool includeSoftDelete =false) where TEntity : T
        {
            return GenericGetQueryAsync(Store, async f => await f.AnyAsync(),
                @where: where,
                orderBy: null,
                orderByDescending: false,
                returnDeleted: includeSoftDelete);
        }
        
        public Task<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy = null, bool orderByDescending = false, bool includeSoftDelete = false) where TEntity : T
        {
            return Get(@where: where,
                projection: f => f,
                orderBy: orderBy,
                orderByDescending: orderByDescending,
                includeSoftDelete: includeSoftDelete);
        }

        public Task<TOut> Get<TEntity, TOut>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TOut>> projection,
            Expression<Func<TEntity, object>> orderBy = null, bool orderByDescending = false, bool includeSoftDelete = false) where TEntity : T
        {
            return GenericGetQueryAsync(Store,
                action: async f => await f.Select(projection).FirstOrDefaultAsync(),
                @where: where,
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
            return GenericGetQueryAsync(Store, async f => await f.Select(projection).ToListAsync(),
                @where: where,
                orderBy: orderBy,
                orderByDescending: orderByDescending,
                returnDeleted: includeSoftDelete);
        }

        private static Task<TOut> GenericGetQueryAsync<TEntity, TOut>(IDocumentStore store, Func<IRavenQueryable<TEntity>, Task<TOut>> action,
            Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, object>> orderBy = null,
            bool orderByDescending = false,
            bool returnDeleted = false)
            where TEntity : RavenEntity
        {
            return InternalAsyncExecutors.ExecuteAsync(store, async f =>
            {
                var set = f.Query<TEntity>();
                set = where != null ? set.Where(where) : set;
                set = returnDeleted ? set : set.Where(ravenEntity => !ravenEntity.DeletedAt.HasValue);
                return orderBy == null ? await action(set) : await action(orderByDescending ? set.OrderByDescending(orderBy) : set.OrderBy(orderBy));
            });
        }
    }
}