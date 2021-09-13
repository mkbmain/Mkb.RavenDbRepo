using System.Threading.Tasks;
using Mkb.RavenDbRepo.Sync.Interfaces;
using Mkb.RavenDbRepo.Tests.RavenDbRepoAsyncTests;
using Raven.TestDriver;
using Shouldly;
using Xunit;

namespace Mkb.RavenDbRepo.Tests.RavenDbRepoTests.RavenWriterRepoAsyncTests
{
    public class AddManyTests : RavenTestDriver
    {
        [Fact]
        public async Task Ensure_we_can_add_multiple_items()
        {
            var store = GetDocumentStore();

            IRavenWriterRepo<Entity> repo = RavenRepoFactory.Build<Entity>(store);
            var entities =  await store.CreateManyEntities(2, false);

             repo.AddMany(entities);

            var items = await store.GetAll<Entity>();
            items.Length.ShouldBe(entities.Length);
            items.ShouldContain(entities[0]);
            items.ShouldContain(entities[1]);
        }
    }
}