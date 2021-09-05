using System.Linq;
using System.Threading.Tasks;
using Mkb.RavenDbRepo.Async.Interfaces;
using Raven.TestDriver;
using Shouldly;
using Xunit;

namespace Mkb.RavenDbRepo.Tests.RaveReponAsyncTests.RavenWriterRepoAsyncTests
{
    public class AddTests : RavenTestDriver
    {
        [Fact]
        public async Task Ensure_we_can_add_a_item()
        {
            var store = GetDocumentStore();
            var entity = await store.CreateEntity(add:false);

            IRavenWriterRepoAsync<Entity> repo = RavenRepoAsyncFactory.Build<Entity>(store);
            await repo.Add(entity);

            var items = await store.GetAll<Entity>();
            items.Length.ShouldBe(1);
            items.FirstOrDefault().ShouldBe(entity);
        }
    }
}