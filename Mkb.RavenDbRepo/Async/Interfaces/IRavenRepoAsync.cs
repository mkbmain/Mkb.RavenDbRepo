namespace Mkb.RavenDbRepo.Async.Interfaces
{
    public interface IRavenRepoAsync< T> : IRavenReaderRepoAsync<T>, IRavenWriterRepoAsync<T>, IRavenDeleteRepoAsync<T> where T : RavenEntity
    {
    }
}