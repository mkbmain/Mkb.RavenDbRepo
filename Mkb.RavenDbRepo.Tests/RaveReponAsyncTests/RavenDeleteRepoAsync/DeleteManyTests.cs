using System.Linq;
using System.Threading.Tasks;
using Mkb.RavenDbRepo.Async.Interfaces;
using Raven.TestDriver;
using Shouldly;
using Xunit;

namespace Mkb.RavenDbRepo.Tests.RaveReponAsyncTests.RavenDeleteRepoAsync
{
    public class DeleteManyTests : RavenTestDriver
    {
        [Fact]
        public async Task Ensure_we_can_Delete_Multiple_items()
        {
            var store = GetDocumentStore();
            var entities = await store.CreateManyEntities(2);

            IRavenDeleteRepoAsync<Entity> repo = RavenRepoAsyncFactory.Build<Entity>(store);

            await repo.DeleteMany(entities);

            var item = await store.GetAll<Entity>();

            item.Length.ShouldBe(entities.Length);
            item.All(t => t.DeletedAt != null).ShouldBeTrue();
        }
    }
}