# Mkb.RavenDbRepo
First draft Add test and check it works  next tests:) 

## Suggested Use


## Contexts and entities
```
       public class MyContext : RavenEntity     // create a context that inherits from RavenEntity, this could be abstract
        {
        }
        
        
        class User : MyContext                  // every entity you create will then inherit from that context
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
```

by creating a context in this way and allowing your entites to inherit from it will allow multiple repo's connect to multiple databases and will stop user error from adding wrong entities to wrong db


```
     var repo = new RavenRepoAsync<MyContext>(new RavenDbConfig(new[] { "http://localhost:8080" }, "Auth")); 
       // or setup via DI
```
Essential a context is now bound to a set of urls and database name meaning RavenRepoAsync<MyContext> can also have RavenRepoAsync<MyContext2> etc.. and entities will not be added to the wrong by mistake. (this is handled at repo level)

## Supported functions at glance

  ### add
  ```
            var users = Enumerable.Range(1, 100).Select(f => new User
            {
                Email = $"Test{f}@gmail.com",
                Password = $"fewgw{f}"
            }).ToArray();

            await repo.Add(users.FirstOrDefault());     // to add single
            
            await repo.AddMany(users);                  // to add many
  ```
  
  ### get \ getall
  
  ```

        var getUsers = await repo.GetAll<User>();   // simple get all
  
        // both get and get all have support the same params
        var getUsers = await repo.GetAll<User,string>(where:f=> f.CreatedAt < DateTime.Now.AddDays(-55),projection:f=> f.Email , orderBy:f=> f.CreatedAt,orderByDescending:true, includeSoftDelete: true  ); 
       // full possiblities with projections order bys and ability to include soft deletes
  
  ```
  
  ### update
  
  ```
            var user = await repo.Get<User>(f => f.Email == email);
            user.Password = newPassword;
            await repo.Update(user);                 
            // there is also a update many if you have a collection
  ```
  
  
  ### delete
  
  ```
  
            var oldUser = await repo.GetAll<User>(f=> f.CreatedAt < DateTime.Now.AddDays(-155));
            await repo.DeleteMany(getUsers);            
            // will soft delete updating a deleted at value in the RavenEntity to be marked as removed
                                                                                                
            // of course there is a delete singular and hard delete singular
                                                                                                
            await repo.HardDeleteMany(getUsers);      
            // will hard delete removing this record from the db
  ```
  
  
