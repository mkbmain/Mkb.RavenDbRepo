using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mkb.RavenDbRepo.Tests.RavenDbRepoAsyncTests;
using Raven.Client.Documents;

namespace Mkb.RavenDbRepo.Tests
{
    public static class DbFuncs
    {
        private static Entity[] GenerateEntites(int amount) => Enumerable.Range(1, amount).Select(t => new Entity
        {
            Name = t.ToString(),
            Email = t.ToString(),
            Dob = DateTime.Now.AddYears(-t)
        }).ToArray();

        public async static Task<Entity[]> CreateManyEntities(this IDocumentStore documentStore,int amount =10,bool add = true)
        {
            var items = GenerateEntites(amount);
            if(!add){return items;}
            await documentStore.AddMany(items);
            return items;
        }
        public async static Task<Entity> CreateEntity(this IDocumentStore documentStore,bool add = true)
        {
            var items = GenerateEntites(1).First();
            if(!add){return items;}
            await documentStore.AddMany(new [] {items});
            return items;
        }

        public async static Task<T[]> GetAll<T>(this IDocumentStore documentStore)
        {
            using var context = documentStore.OpenAsyncSession();
            return await context.Query<T>().ToArrayAsync();
        }

        public async static Task AddMany<T>(this IDocumentStore documentStore, IEnumerable<T> items)
        {
            using var context = documentStore.OpenAsyncSession();
            foreach (var item in items)
            {
                await context.StoreAsync(item);
            }

            await context.SaveChangesAsync();
        }
    }
}