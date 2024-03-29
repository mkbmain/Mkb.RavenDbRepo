﻿using System.Linq;
using System.Threading.Tasks;
using Mkb.RavenDbRepo.Async.Repo;
using Mkb.RavenDbRepo.Configs;
using Mkb.RavenDbRepo.Entities;

namespace ExampleProgram
{
    class Example
    {
        public class MyContext : RavenEntity
        {
        }

        class User : MyContext
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class UserRoles : MyContext
        {
            public string UserId { get; set; }
            public string[] RoleIds { get; set; }
        }

        public class Role : MyContext
        {
            public string Name { get; set; }
        }


        static async Task Main(string[] args)
        {
            var readOnlyRepo = new RavenDbRepoReaderAsync<MyContext>(new RavenDbConfig(new[] {"http://localhost:8080"}, "Auth"));


            var repo = new RavenDbDbRepoAsync<MyContext>(new RavenDbConfig(new[] {"http://localhost:8080"}, "Auth"));

            if (!(await readOnlyRepo.Any<Role>(null)))
            {
                await repo.Add(new Role {Name = "Customer"});
            }

            var users = Enumerable.Range(1, 100).Select(f => new User
            {
                Email = $"Test{f}@gmail.com",
                Password = $"fewgw{f}"
            }).ToArray();

            await repo.Add(users.FirstOrDefault());

            await repo.AddMany(users);


            var getUsers = await readOnlyRepo.GetAll<User>(null);
            foreach (var t in getUsers)
            {
                t.Email = "ToDelete";
            }

            await repo.DeleteMany(getUsers);

            await repo.HardDeleteMany(getUsers);
        }
    }
}