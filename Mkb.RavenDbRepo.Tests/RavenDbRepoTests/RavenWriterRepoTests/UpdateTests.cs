using System.Threading.Tasks;
using Mkb.RavenDbRepo.Sync.Interfaces;
using Mkb.RavenDbRepo.Tests.RavenDbRepoAsyncTests;
using Raven.TestDriver;
using Shouldly;
using Xunit;

namespace Mkb.RavenDbRepo.Tests.RavenDbRepoTests.RavenWriterRepoTests
{
    public class UpdateTests : RavenTestDriver
    {
        [Fact]
        public async Task Ensure_we_can_update_a_item()
        {
            var store = GetDocumentStore();
            var entity = await store.CreateEntity();

            await store.AddMany(new[] { entity });

            IRavenWriterRepo<Entity> repo = RavenRepoFactory.Build<Entity>(store);
            entity.Email = "Test";

             repo.Update(entity);

            var item = await store.GetAll<Entity>();

            item.Length.ShouldBe(1);
            item[0].Id.ShouldBe(entity.Id);
            item[0].Email.ShouldBe("Test");
        }
    }
}