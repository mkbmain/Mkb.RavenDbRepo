using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mkb.RavenDbRepo.Async.Interfaces
{
    public interface IRavenWriterRepoAsync< T> where T : RavenEntity
    {
        Task Update<TEntity>(TEntity entity) where TEntity : T;

        Task UpdateMany<TEntity>(IEnumerable<TEntity> entity) where TEntity : T;

        Task Add<TEntity>(TEntity entity) where TEntity : T;

        Task AddMany<TEntity>(IEnumerable<TEntity> entitys) where TEntity : T;
    }
}