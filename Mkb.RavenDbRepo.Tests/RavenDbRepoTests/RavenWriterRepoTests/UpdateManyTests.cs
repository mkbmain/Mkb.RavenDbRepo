using System.Linq;
using System.Threading.Tasks;
using Mkb.RavenDbRepo.Sync.Interfaces;
using Mkb.RavenDbRepo.Tests.RavenDbRepoAsyncTests;
using Raven.TestDriver;
using Shouldly;
using Xunit;

namespace Mkb.RavenDbRepo.Tests.RavenDbRepoTests.RavenWriterRepoTests
{
    public class UpdateManyTests : RavenTestDriver
    {
        [Fact]
        public async Task Ensure_we_can_update_multiple_items()
        {
            var store = GetDocumentStore();
            var items = await store.CreateManyEntities();

            foreach (var t in items)
            {
                t.Email = "Email";
            }

            IRavenWriterRepo<Entity> repo = RavenRepoFactory.Build<Entity>(store);
             repo.UpdateMany(items);

            var item = await store.GetAll<Entity>();

            item.Length.ShouldBe(10);
            item.Select(t => t.Email).ShouldAllBe(f => f == "Email");
        }
    }
}