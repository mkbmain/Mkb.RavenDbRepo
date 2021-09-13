using System.Threading.Tasks;
using Mkb.RavenDbRepo.Async.Interfaces;
using Mkb.RavenDbRepo.Sync.Interfaces;
using Mkb.RavenDbRepo.Tests.RavenDbRepoAsyncTests;
using Raven.TestDriver;
using Shouldly;
using Xunit;

namespace Mkb.RavenDbRepo.Tests.RavenDbRepoTests.RavenDeleteRepoAsync
{
    public class HardDeleteTests : RavenTestDriver
    {
        [Fact]
        public async Task Ensure_we_can_HardDelete_a_item()
        {
            var store = GetDocumentStore();
            var entity = await store.CreateEntity();

            IRavenDeleteRepo<Entity> repo = RavenRepoFactory.Build<Entity>(store);
             repo.HardDelete(entity);

            var item = await store.GetAll<Entity>();

            item.Length.ShouldBe(0);
        }
    }
}