using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mkb.RavenDbRepo.Async.Interfaces
{
    public interface IRavenDeleteRepoAsync< T> where T : RavenEntity
    {
        Task Delete<TEntity>(TEntity entity) where TEntity : T;

        Task DeleteMany<TEntity>(IEnumerable<TEntity> entitys) where TEntity : T;

        Task HardDelete<TEntity>(TEntity entity) where TEntity : T;

        Task HardDeleteMany<TEntity>(IEnumerable<TEntity> entitys) where TEntity : T;
    }
}