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
            var repo = new RavenRepoAsync<MyContext>(new RavenDbConfig(new[] { "http://localhost:8080" }, "Auth"));
            var users = Enumerable.Range(1, 100).Select(f => new User
            {
                Email = $"Test{f}@gmail.com",
                Password = $"fewgw{f}"
            }).ToArray();

            await repo.Add(users.FirstOrDefault());

            await repo.AddMany(users);


            var getUsers = await repo.GetAll<User>(f=> f.CreatedAt < DateTime.Now.AddDays(-55));
            foreach (var t in getUsers)
            {
                t.Email = "ToDelete";
            }

            await repo.DeleteMany(getUsers);

            await repo.HardDeleteMany(getUsers);
        }
    }
}