using System;
using System.Linq;
using System.Threading.Tasks;
using Mkb.RavenDbRepo.Async.Interfaces;
using Raven.TestDriver;
using Shouldly;
using Xunit;

namespace Mkb.RavenDbRepo.Tests.RaveReponAsyncTests.RavenWriterRepoAsyncTests
{
    public class UpdateTests : RavenTestDriver
    {
        [Fact]
        public async Task Ensure_we_can_update_a_item()
        {
            var store = GetDocumentStore();
            var entity = await store.CreateEntity();

            await store.AddMany(new[] { entity });

            IRavenWriterRepoAsync<Entity> repo = RavenRepoAsyncFactory.Build<Entity>(store);
            entity.Email = "Test";

            await repo.Update(entity);

            var item = await store.GetAll<Entity>();

            item.Length.ShouldBe(1);
            item[0].Id.ShouldBe(entity.Id);
            item[0].Email.ShouldBe("Test");
        }
    }
}