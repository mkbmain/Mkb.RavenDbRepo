using Mkb.RavenDbRepo.Async;
using Mkb.RavenDbRepo.Async.Interfaces;
using Raven.Client.Documents;

namespace Mkb.RavenDbRepo.Tests.RaveReponAsyncTests
{
    public static class RavenRepoAsyncFactory
    {
        public static IRavenRepoAsync<T> Build<T>(IDocumentStore documentStore) where T : RavenEntity
        {
            return new RavenRepoAsync<T>(documentStore);
        }
    }
}