using System;
using System.Collections.Generic;
using Mkb.RavenDbRepo.Configs;
using Mkb.RavenDbRepo.Entities;
using Mkb.RavenDbRepo.Sync.Interfaces;
using Raven.Client.Documents;

namespace Mkb.RavenDbRepo.Sync.Repo
{
    public class RavenDbDbRepo<T> : RavenDbRepoReader<T>, IRavenRepo<T> where T : RavenEntity
    {
        public RavenDbDbRepo(RavenDbConfig ravenDbConfig) : base(ravenDbConfig)
        {
        }

        public RavenDbDbRepo(IDocumentStore documentStore) : base(documentStore: documentStore)
        {
        }

        public void Update<TEntity>(TEntity entity) where TEntity : T
        {
            UpdateMany(new[] { entity });
        }

        public void UpdateMany<TEntity>(IEnumerable<TEntity> entity) where TEntity : T
        {
            AddMany(entity);
        }

        public void Add<TEntity>(TEntity entity) where TEntity : T
        {
            AddMany(new[] { entity });
        }

        public void AddMany<TEntity>(IEnumerable<TEntity> entities) where TEntity : T
        {
            InternalExecutors.ExecuteVoid(Store,
                InternalExecutors.EnumerableWithSave(entities, (session, item) => { session.Store(item); }));
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : T
        {
            DeleteMany(new[] { entity });
        }

        public void DeleteMany<TEntity>(IEnumerable<TEntity> entities) where TEntity : T
        {
            InternalExecutors.ExecuteVoid(Store, InternalExecutors.EnumerableWithSave(entities, (session, item) =>
            {
                item.DeletedAt = DateTime.UtcNow;
                session.Store(item);
            }));
        }

        public void HardDelete<TEntity>(TEntity entity) where TEntity : T
        {
            HardDeleteMany(new[] { entity });
        }

        public void HardDeleteMany<TEntity>(IEnumerable<TEntity> entities) where TEntity : T
        {
            InternalExecutors.ExecuteVoid(Store,
                InternalExecutors.EnumerableWithSave(entities, (session, item) =>
                {
                    session.Delete(session.Load<TEntity>(item.Id));
                }));
        }
    }
}