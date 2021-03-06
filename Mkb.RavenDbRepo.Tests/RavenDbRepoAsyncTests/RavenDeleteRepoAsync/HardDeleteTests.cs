using System.Threading.Tasks;
using Mkb.RavenDbRepo.Async.Interfaces;
using Raven.TestDriver;
using Shouldly;
using Xunit;

namespace Mkb.RavenDbRepo.Tests.RavenDbRepoAsyncTests.RavenDeleteRepoAsync
{
    public class HardDeleteTests : RavenTestDriver
    {
        [Fact]
        public async Task Ensure_we_can_HardDelete_a_item()
        {
            var store = GetDocumentStore();
            var entity = await store.CreateEntity();

            IRavenDeleteRepoAsync<Entity> repo = RavenRepoAsyncFactory.Build<Entity>(store);
            await repo.HardDelete(entity);

            var item = await store.GetAll<Entity>();

            item.Length.ShouldBe(0);
        }
    }
}