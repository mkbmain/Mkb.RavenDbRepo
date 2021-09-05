using System;
using System.Linq;
using System.Threading.Tasks;
using Mkb.RavenDbRepo.Async.Interfaces;
using Raven.TestDriver;
using Shouldly;
using Xunit;

namespace Mkb.RavenDbRepo.Tests.RaveReponAsyncTests.RavenWriterRepoAsyncTests
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

            IRavenWriterRepoAsync<Entity> repo = RavenRepoAsyncFactory.Build<Entity>(store);
            await repo.UpdateMany(items);

            var item = await store.GetAll<Entity>();

            item.Length.ShouldBe(10);
            item.Select(t => t.Email).ShouldAllBe(f => f == "Email");
        }
    }
}