using System.Collections.Generic;
using Mkb.RavenDbRepo.Entities;

namespace Mkb.RavenDbRepo.Sync.Interfaces
{
    public interface IRavenDeleteRepo<in T> where T : RavenEntity
    {
        void Delete<TEntity>(TEntity entity) where TEntity : T;

        void DeleteMany<TEntity>(IEnumerable<TEntity> entities) where TEntity : T;

        void  HardDelete<TEntity>(TEntity entity) where TEntity : T;

        void  HardDeleteMany<TEntity>(IEnumerable<TEntity> entities) where TEntity : T;
    }
}