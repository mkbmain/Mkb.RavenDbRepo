using System;
using System.Linq;
using System.Threading.Tasks;
using Mkb.RavenDbRepo;
using Mkb.RavenDbRepo.Async;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace RavenDb
{
    class Program
    {
        public class MyContext : RavenEntity
        {
        }

        class User : MyContext
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class Role : MyContext
        {
            public string Name { get; set; }
        }


        static async Task Main(string[] args)
        {
            var repo = new RavenRepoAsync<MyContext>(new RavenConfig(new[] { "http://localhost:8080" }, "Auth"));
            var users = Enumerable.Range(1, 100).Select(f => new User
            {
                Email = $"Test{f}@gmail.com",
                Password = $"fewgw{f}"
            }).ToArray();
            await repo.AddMany(users);


            var getUsers = await repo.GetAll<User>();
            foreach (var t in getUsers)
            {
                t.Email = "ToDelete";
            }

            var getUser1 = await repo.Get<User>(null, t => t.CreatedAt, true);

            await repo.UpdateMany(getUsers);


            await repo.DeleteMany(getUsers);

            await repo.HardDeleteMany(getUsers);
        }
    }
}