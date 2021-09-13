using Mkb.RavenDbRepo.Entities;
using Mkb.RavenDbRepo.Sync.Interfaces;
using Mkb.RavenDbRepo.Sync.Repo;
using Raven.Client.Documents;

namespace Mkb.RavenDbRepo.Tests.RavenDbRepoTests
{
    public static class RavenRepoFactory
    {
        public static IRavenRepo<T> Build<T>(IDocumentStore documentStore) where T : RavenEntity
        {
            return new RavenDbDbRepo<T>(documentStore);
        }

        public static IRavenReaderRepo<T> BuildReaderRepo<T>(IDocumentStore documentStore) where T : RavenEntity
        {
            return new RavenDbRepoReader<T>(documentStore);
        }
    }
}