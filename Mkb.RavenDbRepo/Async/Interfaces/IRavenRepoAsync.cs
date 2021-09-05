using Mkb.RavenDbRepo.Entities;

namespace Mkb.RavenDbRepo.Async.Interfaces
{
    public interface IRavenRepoAsync<in T> : IRavenReaderRepoAsync<T>, IRavenWriterRepoAsync<T>, IRavenDeleteRepoAsync<T> where T : RavenEntity
    {
    }
}