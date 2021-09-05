using System.Threading.Tasks;
using Mkb.RavenDbRepo.Async.Interfaces;
using Raven.TestDriver;
using Shouldly;
using Xunit;

namespace Mkb.RavenDbRepo.Tests.RaveReponAsyncTests.RavenDeleteRepoAsync
{
    public class HardDeleteManyTests : RavenTestDriver
    {
        [Fact]
        public async Task Ensure_we_can_HardDelete_Multiple_items()
        {
            var store = GetDocumentStore();
            var entity = await store.CreateManyEntities(5);

            IRavenDeleteRepoAsync<Entity> repo = RavenRepoAsyncFactory.Build<Entity>(store);
            await repo.HardDeleteMany(entity);

            var item = await store.GetAll<Entity>();

            item.Length.ShouldBe(0);
        }
    }
}