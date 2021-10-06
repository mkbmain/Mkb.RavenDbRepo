using System.Linq;
using System.Threading.Tasks;
using Mkb.RavenDbRepo.Sync.Interfaces;
using Mkb.RavenDbRepo.Tests.RavenDbRepoAsyncTests;
using Raven.TestDriver;
using Shouldly;
using Xunit;

namespace Mkb.RavenDbRepo.Tests.RavenDbRepoTests.RavenDeleteRepo
{
    public class DeleteManyTests : RavenTestDriver
    {
        [Fact]
        public async Task Ensure_we_can_Delete_Multiple_items()
        {
            var store = GetDocumentStore();
            var entities = await store.CreateManyEntities(2);

            IRavenDeleteRepo<Entity> repo = RavenRepoFactory.Build<Entity>(store);

             repo.DeleteMany(entities);

            var item = await store.GetAll<Entity>();

            item.Length.ShouldBe(entities.Length);
            item.All(t => t.DeletedAt != null).ShouldBeTrue();
        }
    }
}