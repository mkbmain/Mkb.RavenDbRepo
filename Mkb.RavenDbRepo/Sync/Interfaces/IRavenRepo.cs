using Mkb.RavenDbRepo.Entities;

namespace Mkb.RavenDbRepo.Sync.Interfaces
{
    public interface IRavenRepo<in T> : IRavenReaderRepo<T>, IRavenWriterRepo<T>, IRavenDeleteRepo<T> where T : RavenEntity
    {
    }
}