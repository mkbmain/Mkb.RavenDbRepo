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
            return InternalExecutors.GenericGetQuery(Store,
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
            return InternalExecutors.GenericGetQuery(Store,  f =>  f.Select(projection).ToList(),
                @where: where,
                orderBy: orderBy,
                orderByDescending: orderByDescending,
                returnDeleted: includeSoftDelete);
        }
    }
}