using System.Collections.Generic;
using System.Threading.Tasks;
using Mkb.RavenDbRepo.Entities;

namespace Mkb.RavenDbRepo.Async.Interfaces
{
    public interface IRavenDeleteRepoAsync<in T> where T : RavenEntity
    {
        Task Delete<TEntity>(TEntity entity) where TEntity : T;

        Task DeleteMany<TEntity>(IEnumerable<TEntity> entities) where TEntity : T;

        Task HardDelete<TEntity>(TEntity entity) where TEntity : T;

        Task HardDeleteMany<TEntity>(IEnumerable<TEntity> entities) where TEntity : T;
    }
}