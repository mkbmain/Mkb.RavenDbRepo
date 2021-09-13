using Mkb.RavenDbRepo.Async.Interfaces;
using Mkb.RavenDbRepo.Async.Repo;
using Mkb.RavenDbRepo.Entities;
using Raven.Client.Documents;

namespace Mkb.RavenDbRepo.Tests.RavenDbRepoTests
{
    public static class RavenRepoAsyncFactory
    {
        public static IRavenRepoAsync<T> Build<T>(IDocumentStore documentStore) where T : RavenEntity
        {
            return new RavenDbDbRepoAsync<T>(documentStore);
        }

        public static IRavenReaderRepoAsync<T> BuildReaderRepoAsync<T>(IDocumentStore documentStore) where T : RavenEntity
        {
            return new RavenDbRepoReaderAsync<T>(documentStore);
        }
    }
}