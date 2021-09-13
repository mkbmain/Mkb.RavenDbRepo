using System.Linq;
using System.Threading.Tasks;
using Mkb.RavenDbRepo.Async.Interfaces;
using Mkb.RavenDbRepo.Sync.Interfaces;
using Mkb.RavenDbRepo.Tests.RavenDbRepoAsyncTests;
using Raven.TestDriver;
using Shouldly;
using Xunit;

namespace Mkb.RavenDbRepo.Tests.RavenDbRepoTests.RavenWriterRepoAsyncTests
{
    public class AddTests : RavenTestDriver
    {
        [Fact]
        public async Task Ensure_we_can_add_a_item()
        {
            var store = GetDocumentStore();
            var entity = await store.CreateEntity(add:false);

            IRavenWriterRepo<Entity> repo = RavenRepoFactory.Build<Entity>(store);
             repo.Add(entity);

            var items = await store.GetAll<Entity>();
            items.Length.ShouldBe(1);
            items.FirstOrDefault().ShouldBe(entity);
        }
    }
}