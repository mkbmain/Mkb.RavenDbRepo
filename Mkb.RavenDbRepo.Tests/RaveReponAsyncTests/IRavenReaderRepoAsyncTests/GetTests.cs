using System;
using System.Linq;
using System.Threading.Tasks;
using Mkb.RavenDbRepo.Async.Interfaces;
using Raven.TestDriver;
using Shouldly;
using Xunit;

namespace Mkb.RavenDbRepo.Tests.RaveReponAsyncTests.IRavenReaderRepoAsyncTests
{
    public class GetTests : RavenTestDriver
    {
        [Fact]
        public async Task Ensure_we_can_Get_a_item()
        {
            var store = GetDocumentStore();
            var entities = await store.CreateManyEntities(5);
            var thirdItem = entities.Skip(2).First();

            IRavenReaderRepoAsync<Entity> repo = RavenRepoAsyncFactory.Build<Entity>(store);

            var item = await repo.Get<Entity>(f => f.Id == thirdItem.Id);

            item.ShouldBe(thirdItem);
        }

        [Fact]
        public async Task Ensure_we_can_order_by()
        {
            var store = GetDocumentStore();
            var entities = await store.CreateManyEntities(5);
            var thirdItem = entities.First(); // we want this one do something weird to make it fit order by
            thirdItem.Dob = DateTime.Now.AddYears(-55);
            await store.AddMany(new[] { thirdItem });

            IRavenReaderRepoAsync<Entity> repo = RavenRepoAsyncFactory.Build<Entity>(store);

            var item = await repo.Get<Entity>(null, f => f.Dob);

            item.ShouldBe(thirdItem);
        }

        [Fact]
        public async Task Ensure_we_can_order_by_desc()
        {
            var store = GetDocumentStore();
            var entities = await store.CreateManyEntities(5);
            var thirdItem = entities.First(); // we want this one do something weird to make it fit order by
            thirdItem.Dob = DateTime.Now.AddYears(55);
            await store.AddMany(new[] { thirdItem });

            IRavenReaderRepoAsync<Entity> repo = RavenRepoAsyncFactory.Build<Entity>(store);

            var item = await repo.Get<Entity>(null, f => f.Dob, true);

            item.ShouldBe(thirdItem);
        }

        [Fact]
        public async Task Ensure_we_can_get_non_hard_deleted_items()
        {
            var store = GetDocumentStore();
            var entities = await store.CreateManyEntities(5);
            var thirdItem = entities.Skip(2).First();
            thirdItem.DeletedAt = DateTime.Now;
            await store.AddMany(new[] { thirdItem });

            IRavenReaderRepoAsync<Entity> repo = RavenRepoAsyncFactory.Build<Entity>(store);

            var item = await repo.Get<Entity>(f => f.Id == thirdItem.Id, includeSoftDelete: true);

            item.ShouldBe(thirdItem);
        }

        [Fact]
        public async Task Ensure_we_can_get_item_with_projection()
        {
            var store = GetDocumentStore();
            var entities = await store.CreateManyEntities(5);
            var thirdItem = entities.Skip(2).First();

            IRavenReaderRepoAsync<Entity> repo = RavenRepoAsyncFactory.Build<Entity>(store);

            var item = await repo.Get<Entity, DateTime>(f => f.Id == thirdItem.Id, f => f.Dob);

            item.ShouldBe(thirdItem.Dob);
        }

        [Fact]
        public async Task Ensure_we_do_not_Get_a_non_hard_deleted_item_back_by_defualt()
        {
            var store = GetDocumentStore();
            var entities = await store.CreateManyEntities(5);
            var thirdItem = entities.Skip(2).First();
            thirdItem.DeletedAt = DateTime.Now;
            await store.AddMany(new[] { thirdItem });

            IRavenReaderRepoAsync<Entity> repo = RavenRepoAsyncFactory.Build<Entity>(store);

            var item = await repo.Get<Entity>(f => f.Id == thirdItem.Id);

            item.ShouldBeNull();
        }
    }
}