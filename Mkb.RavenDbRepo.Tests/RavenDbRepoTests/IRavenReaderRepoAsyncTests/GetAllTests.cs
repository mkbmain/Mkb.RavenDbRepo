using System;
using System.Linq;
using System.Threading.Tasks;
using Mkb.RavenDbRepo.Sync.Interfaces;
using Mkb.RavenDbRepo.Tests.RavenDbRepoAsyncTests;
using Raven.TestDriver;
using Shouldly;
using Xunit;

namespace Mkb.RavenDbRepo.Tests.RavenDbRepoTests.IRavenReaderRepoAsyncTests
{
    public class GetAllTests : RavenTestDriver
    {
        [Fact]
        public async Task Ensure_we_can_Get_a_item()
        {
            var store = GetDocumentStore();
            var entities = await store.CreateManyEntities(5);
            var thirdItem = entities.Skip(2).First();

            IRavenReaderRepo<Entity> repo = RavenRepoFactory.BuildReaderRepo<Entity>(store);

            var item =  repo.GetAll<Entity>(f => f.Id == thirdItem.Id);
            item.Count.ShouldBe(1);
            item[0].ShouldBe(thirdItem);
        }

        [Fact]
        public async Task Ensure_we_can_order_by()
        {
            var store = GetDocumentStore();
            var entities = await store.CreateManyEntities(5);
            var thirdItem = entities.First(); // we want this one do something weird to make it fit order by
            thirdItem.Dob = DateTime.Now.AddYears(-55);
            await store.AddMany(new[] { thirdItem });

            IRavenReaderRepo<Entity> repo = RavenRepoFactory.BuildReaderRepo<Entity>(store);

            var item =  repo.GetAll<Entity>(null, f => f.Dob);
            item.Count.ShouldBe(entities.Length);
            item[0].ShouldBe(thirdItem);
        }

        [Fact]
        public async Task Ensure_we_can_order_by_desc()
        {
            var store = GetDocumentStore();
            var entities = await store.CreateManyEntities(5);
            var thirdItem = entities.First(); // we want this one do something weird to make it fit order by
            thirdItem.Dob = DateTime.Now.AddYears(55);
            await store.AddMany(new[] { thirdItem });

            IRavenReaderRepo<Entity> repo = RavenRepoFactory.BuildReaderRepo<Entity>(store);

            var item =  repo.GetAll<Entity>(null, f => f.Dob, true);

            item.Count.ShouldBe(entities.Length);
            item[0].ShouldBe(thirdItem);
        }

        [Fact]
        public async Task Ensure_we_can_get_non_hard_deleted_items()
        {
            var store = GetDocumentStore();
            var entities = await store.CreateManyEntities(5);
            var thirdItem = entities.Skip(2).First();
            thirdItem.DeletedAt = DateTime.Now;
            await store.AddMany(new[] { thirdItem });

            IRavenReaderRepo<Entity> repo = RavenRepoFactory.BuildReaderRepo<Entity>(store);

            var item =  repo.GetAll<Entity>(null, includeSoftDelete: true);

            item.ShouldContain(thirdItem);
        }

        [Fact]
        public async Task Ensure_we_can_get_item_with_projection()
        {
            var store = GetDocumentStore();
            var entities = await store.CreateManyEntities(5);
            var thirdItem = entities.Skip(2).First();

            IRavenReaderRepo<Entity> repo = RavenRepoFactory.BuildReaderRepo<Entity>(store);

            var item =  repo.GetAll<Entity, DateTime>(null, f => f.Dob);
            item.Count.ShouldBe(entities.Length);
            item.ShouldContain(thirdItem.Dob);
        }

        [Fact]
        public async Task Ensure_we_do_not_Get_a_non_hard_deleted_item_back_by_defualt()
        {
            var store = GetDocumentStore();
            var entities = await store.CreateManyEntities(5);
            var thirdItem = entities.Skip(2).First();
            thirdItem.DeletedAt = DateTime.Now;
            await store.AddMany(new[] { thirdItem });

            IRavenReaderRepo<Entity> repo = RavenRepoFactory.BuildReaderRepo<Entity>(store);

            var item =  repo.GetAll<Entity>(null);

            item.Count().ShouldBe(entities.Length - 1);
        }
    }
}