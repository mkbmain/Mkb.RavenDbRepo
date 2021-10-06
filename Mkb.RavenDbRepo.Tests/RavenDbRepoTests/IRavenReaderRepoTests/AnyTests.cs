using System;
using System.Linq;
using System.Threading.Tasks;
using Mkb.RavenDbRepo.Async.Interfaces;
using Mkb.RavenDbRepo.Sync.Interfaces;
using Mkb.RavenDbRepo.Tests.RavenDbRepoAsyncTests;
using Raven.TestDriver;
using Shouldly;
using Xunit;

namespace Mkb.RavenDbRepo.Tests.RavenDbRepoTests.IRavenReaderRepoTests
{
    public class AnyTests : RavenTestDriver
    {
        [Fact]
        public async Task Ensure_we_can_Get_items_that_exist()
        {
            var store = GetDocumentStore();
            var entities = await store.CreateManyEntities(5);
            var thirdItem = entities.Skip(2).First();

            IRavenReaderRepo<Entity> repo = RavenRepoFactory.BuildReaderRepo<Entity>(store);

            var item = repo.Any<Entity>(f => f.Id == thirdItem.Id);
            item.ShouldBeTrue();
        }

        [Fact]
        public async Task Ensure_if_item_does_not_exist_we_return_false()
        {
            var store = GetDocumentStore();
            var entities = await store.CreateManyEntities(5);
            var thirdItem = entities.Skip(2).First();

            IRavenReaderRepo<Entity> repo = RavenRepoFactory.BuildReaderRepo<Entity>(store);

            var item = repo.Any<Entity>(f => f.Id == Guid.NewGuid().ToString());
            item.ShouldBeFalse();
        }

        [Fact]
        public async Task Ensure_we_can_get_non_hard_deleted_items()
        {
            var store = GetDocumentStore();
            var entities = await store.CreateManyEntities(5);
            var thirdItem = entities.Skip(2).First();
            thirdItem.DeletedAt = DateTime.Now;
            await store.AddMany(new[] {thirdItem});

            IRavenReaderRepo<Entity> repo = RavenRepoFactory.BuildReaderRepo<Entity>(store);

            var item = repo.Any<Entity>(f => f.Id == thirdItem.Id, includeSoftDelete: true);

            item.ShouldBeTrue();
        }

        [Fact]
        public async Task Ensure_we_do_not_get_non_hard_deleted_items_by_deafult()
        {
            var store = GetDocumentStore();
            var entities = await store.CreateManyEntities(5);
            var thirdItem = entities.Skip(2).First();
            thirdItem.DeletedAt = DateTime.Now;
            await store.AddMany(new[] {thirdItem});

            IRavenReaderRepo<Entity> repo = RavenRepoFactory.BuildReaderRepo<Entity>(store);

            var item = repo.Any<Entity>(f => f.Id == thirdItem.Id);

            item.ShouldBeFalse();
        }
    }
}