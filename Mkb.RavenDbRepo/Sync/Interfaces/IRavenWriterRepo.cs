using System.Collections.Generic;
using Mkb.RavenDbRepo.Entities;

namespace Mkb.RavenDbRepo.Sync.Interfaces
{
    public interface IRavenWriterRepo<in T> where T : RavenEntity
    {
        void Update<TEntity>(TEntity entity) where TEntity : T;

        void UpdateMany<TEntity>(IEnumerable<TEntity> entity) where TEntity : T;

        void Add<TEntity>(TEntity entity) where TEntity : T;

        void AddMany<TEntity>(IEnumerable<TEntity> entities) where TEntity : T;
    }
}