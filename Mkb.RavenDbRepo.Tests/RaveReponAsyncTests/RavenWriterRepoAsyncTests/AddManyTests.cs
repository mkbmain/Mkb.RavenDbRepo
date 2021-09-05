using System;
using System.Threading.Tasks;
using Mkb.RavenDbRepo.Async.Interfaces;
using Raven.TestDriver;
using Shouldly;
using Xunit;

namespace Mkb.RavenDbRepo.Tests.RaveReponAsyncTests.RavenWriterRepoAsyncTests
{
    public class AddManyTests : RavenTestDriver
    {
        [Fact]
        public async Task Ensure_we_can_add_multiple_items()
        {
            var store = GetDocumentStore();
            IRavenWriterRepoAsync<Entity> repo = RavenRepoAsyncFactory.Build<Entity>(store);
            var entities = await store.CreateManyEntities(2, false);

            await repo.AddMany(entities);

            var items = await store.GetAll<Entity>();
            items.Length.ShouldBe(entities.Length);
            items.ShouldContain(entities[0]);
            items.ShouldContain(entities[1]);
        }
    }
}