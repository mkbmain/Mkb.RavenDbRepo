using System.Threading.Tasks;
using Mkb.RavenDbRepo.Async.Interfaces;
using Mkb.RavenDbRepo.Sync.Interfaces;
using Mkb.RavenDbRepo.Tests.RavenDbRepoAsyncTests;
using Raven.TestDriver;
using Shouldly;
using Xunit;

namespace Mkb.RavenDbRepo.Tests.RavenDbRepoTests.RavenDeleteRepoAsync
{
    public class HardDeleteManyTests : RavenTestDriver
    {
        [Fact]
        public async Task Ensure_we_can_HardDelete_Multiple_items()
        {
            var store = GetDocumentStore();
            var entity = await store.CreateManyEntities(5);

            IRavenDeleteRepo<Entity> repo = RavenRepoFactory.Build<Entity>(store);
             repo.HardDeleteMany(entity);

            var item = await store.GetAll<Entity>();

            item.Length.ShouldBe(0);
        }
    }
}