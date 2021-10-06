using System;
using System.Linq;
using System.Threading.Tasks;
using Mkb.RavenDbRepo.Async.Interfaces;
using Raven.TestDriver;
using Shouldly;
using Xunit;

namespace Mkb.RavenDbRepo.Tests.RavenDbRepoAsyncTests.IRavenReaderRepoAsyncTests
{
    public class AnyTests  : RavenTestDriver
    {
        [Fact]
        public async Task Ensure_we_can_Get_items_that_exist()
        {
            var store = GetDocumentStore();
            var entities = await store.CreateManyEntities(5);
            var thirdItem = entities.Skip(2).First();

            IRavenReaderRepoAsync<Entity> repo = RavenRepoAsyncFactory.BuildReaderRepoAsync<Entity>(store);

            var item = await repo.Any<Entity>(f => f.Id == thirdItem.Id);
            item.ShouldBeTrue();
        }
        [Fact]
        public async Task Ensure_if_item_does_not_exist_we_return_false()
        {
            var store = GetDocumentStore();
            var entities = await store.CreateManyEntities(5);
            var thirdItem = entities.Skip(2).First();

            IRavenReaderRepoAsync<Entity> repo = RavenRepoAsyncFactory.BuildReaderRepoAsync<Entity>(store);

            var item = await repo.Any<Entity>(f => f.Id == Guid.NewGuid().ToString());
            item.ShouldBeFalse();
        }
        
        [Fact]
        public async Task Ensure_we_can_get_non_hard_deleted_items()
        {
            var store = GetDocumentStore();
            var entities = await store.CreateManyEntities(5);
            var thirdItem = entities.Skip(2).First();
            thirdItem.DeletedAt = DateTime.Now;
            await store.AddMany(new[] { thirdItem });

            IRavenReaderRepoAsync<Entity> repo = RavenRepoAsyncFactory.BuildReaderRepoAsync<Entity>(store);

            var item = await repo.Any<Entity>(f=> f.Id == thirdItem.Id, includeSoftDelete: true);

            item.ShouldBeTrue();
        }
        
        [Fact]
        public async Task Ensure_we_do_not_get_non_hard_deleted_items_by_deafult()
        {
            var store = GetDocumentStore();
            var entities = await store.CreateManyEntities(5);
            var thirdItem = entities.Skip(2).First();
            thirdItem.DeletedAt = DateTime.Now;
            await store.AddMany(new[] { thirdItem });

            IRavenReaderRepoAsync<Entity> repo = RavenRepoAsyncFactory.BuildReaderRepoAsync<Entity>(store);

            var item = await repo.Any<Entity>(f=> f.Id == thirdItem.Id );

            item.ShouldBeFalse();
        }

    }
}